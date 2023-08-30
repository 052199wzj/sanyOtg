package com.mes.aps.server.controller.oms;

import java.util.Calendar;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

import javax.servlet.http.HttpServletRequest;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.mes.aps.server.controller.BaseController;
import com.mes.aps.server.service.OMSService;
import com.mes.aps.server.service.po.ServiceResult;
import com.mes.aps.server.service.po.bms.BMSEmployee;
import com.mes.aps.server.service.po.oms.OMSCommand;
import com.mes.aps.server.service.utils.CloneTool;
import com.mes.aps.server.service.utils.StringUtils;
import com.mes.aps.server.utils.RetCode;

/**
 * 
 * @author PengYouWang
 * @CreateTime 2020年1月2日11:40:32
 * @LastEditTime 2019年12月27日16:39:20
 */
@RestController
@RequestMapping("/api/OMSCommand")
public class OMSCommandController extends BaseController {
	private static Logger logger = LoggerFactory.getLogger(OMSCommandController.class);

	@Autowired
	OMSService wOMSService;

	/**
	 * 查单条
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/Info")
	public Object Info(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = this.GetSession(request);

			int wID = StringUtils.parseInt(request.getParameter("ID"));

			ServiceResult<OMSCommand> wServiceResult = wOMSService.OMS_QueryCommand(wLoginUser, wID);

			if (StringUtils.isEmpty(wServiceResult.getFaultCode())) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServiceResult.Result);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.getFaultCode());
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 条件查询
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/All")
	public Object All(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = this.GetSession(request);

			int wCustomerID = StringUtils.parseInt(request.getParameter("CustomerID"));
			int wFactoryID = StringUtils.parseInt(request.getParameter("FactoryID"));
			int wBusinessUnitID = StringUtils.parseInt(request.getParameter("BusinessUnitID"));
			int wWorkShopID = StringUtils.parseInt(request.getParameter("WorkShopID"));
			int wActive = StringUtils.parseInt(request.getParameter("Active"));
			String wWBSNo = StringUtils.parseString(request.getParameter("WBSNo"));
			Calendar wStartTime = StringUtils.parseCalendar(request.getParameter("StartTime"));
			Calendar wEndTime = StringUtils.parseCalendar(request.getParameter("EndTime"));

			ServiceResult<List<OMSCommand>> wServiceResult = wOMSService.OMS_QueryCommandList(wLoginUser, wWBSNo,
					wFactoryID, wBusinessUnitID, wWorkShopID, wCustomerID, wActive, wStartTime, wEndTime);

			if (StringUtils.isEmpty(wServiceResult.getFaultCode())) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.getFaultCode());
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 新增或更新
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/Update")
	public Object Update(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = this.GetSession(request);

			if (!wParam.containsKey("data")) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			OMSCommand wOMSCommand = CloneTool.Clone(wParam.get("data"), OMSCommand.class);
			if (wOMSCommand == null) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			if (wOMSCommand.ID <= 0) {
				wOMSCommand.CreatorID = wLoginUser.ID;
				wOMSCommand.CreateTime = Calendar.getInstance();
			} else {
				wOMSCommand.EditorID = wLoginUser.ID;
				wOMSCommand.EditTime = Calendar.getInstance();
			}

			ServiceResult<Integer> wServiceResult = wOMSService.OMS_UpdateCommand(wLoginUser, wOMSCommand);

			if (StringUtils.isEmpty(wServiceResult.getFaultCode())) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServiceResult.Result);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.getFaultCode());
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 新增或更新
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/Delete")
	public Object Delete(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = this.GetSession(request);

			if (!wParam.containsKey("data")) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			List<OMSCommand> wOMSCommandList = CloneTool.CloneArray(wParam.get("data"), OMSCommand.class);
			if (wOMSCommandList == null || wOMSCommandList.size() <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			ServiceResult<Integer> wServiceResult = wOMSService.OMS_DeleteCommandList(wLoginUser, wOMSCommandList);

			if (StringUtils.isEmpty(wServiceResult.getFaultCode())) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "");
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.getFaultCode());
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 批量激活或禁用
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/Active")
	public Object Active(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = this.GetSession(request);

			if (!wParam.containsKey("Active")) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}
			int wActive = StringUtils.parseInt(wParam.get("Active"));

			if (!wParam.containsKey("data")) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			List<OMSCommand> wList = CloneTool.CloneArray(wParam.get("data"), OMSCommand.class);
			if (wList == null || wList.size() <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			List<Integer> wIDList = wList.stream().map(p -> p.getID()).collect(Collectors.toList());

			wOMSService.OMS_ActiveCommandList(wLoginUser, wIDList, wActive);

			wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, null);
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}
}
