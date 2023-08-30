package com.mes.aps.server.controller.oms;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;
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
import com.mes.aps.server.service.APSService;
import com.mes.aps.server.service.CoreService;
import com.mes.aps.server.service.OMSService;
import com.mes.aps.server.service.mesenum.OMSOrderPriority;
import com.mes.aps.server.service.mesenum.WDWReportCheckTypes;
import com.mes.aps.server.service.po.ServiceResult;
import com.mes.aps.server.service.po.aps.APSCondition;
import com.mes.aps.server.service.po.bms.BMSEmployee;
import com.mes.aps.server.service.po.oms.OMSCheckMaterial;
import com.mes.aps.server.service.po.oms.OMSCheckMsg;
import com.mes.aps.server.service.po.oms.OMSOrder;
import com.mes.aps.server.service.utils.CloneTool;
import com.mes.aps.server.service.utils.StringUtils;
import com.mes.aps.server.utils.RetCode;

/**
 * 
 * @author PengYouWang
 * @CreateTime 2019年12月26日22:46:00
 * @LastEditTime 2020-6-6 13:18:45
 */
@RestController
@RequestMapping("/api/OMSOrder")
public class OMSOrderController extends BaseController {
	private static Logger logger = LoggerFactory.getLogger(OMSOrderController.class);

	@Autowired
	OMSService wOMSService;

	@Autowired
	APSService wAPSService;

	@Autowired
	CoreService wCoreService;

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

			int wID = StringUtils.parseInt(request.getParameter("ID"));

			String wCode = StringUtils.parseString(request.getParameter("Code"));

			BMSEmployee wLoginUser = this.GetSession(request);

			ServiceResult<OMSOrder> wServiceResult = new ServiceResult<OMSOrder>();

			if (wID > 0) {
				wServiceResult = wOMSService.OMS_QueryOMSOrder(wLoginUser, wID);
			} else if (StringUtils.isNotEmpty(wCode)) {
				wServiceResult = wOMSService.OMS_QueryOMSOrderByNo(wLoginUser, wCode);
			}

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

			int wCommandID = StringUtils.parseInt(request.getParameter("CommandID"));
			int wWorkShopID = StringUtils.parseInt(request.getParameter("WorkShopID"));
			int wLineID = StringUtils.parseInt(request.getParameter("LineID"));
			int wProductID = StringUtils.parseInt(request.getParameter("ProductID"));
			int wMaterialID = StringUtils.parseInt(request.getParameter("MaterialID"));
			int wCustomerID = StringUtils.parseInt(request.getParameter("CustomerID"));
			String wPartNo = StringUtils.parseString(request.getParameter("PartNo"));
			String wBOMNo = StringUtils.parseString(request.getParameter("BOMNo"));
			int wActive = StringUtils.parseInt(request.getParameter("Active"));

			Calendar wPreStartTime = StringUtils.parseCalendar(request.getParameter("PreStartTime"));
			Calendar wPreEndTime = StringUtils.parseCalendar(request.getParameter("PreEndTime"));
			Calendar wRelStartTime = StringUtils.parseCalendar(request.getParameter("RelStartTime"));
			Calendar wRelEndTime = StringUtils.parseCalendar(request.getParameter("RelEndTime"));

			BMSEmployee wLoginUser = this.GetSession(request);

			ServiceResult<List<OMSOrder>> wServiceResult = wOMSService.OMS_QueryOMSOrderList(wLoginUser, wCommandID,
					wWorkShopID, wLineID, wProductID, wMaterialID, wCustomerID, wPartNo, wBOMNo, wActive, wPreStartTime,
					wPreEndTime, wRelStartTime, wRelEndTime, null);

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
	 * 根据客户 时间段 产线 产品规格 车号查询订单集合
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/RFOrderList")
	public Object RFOrderList(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			int wCustomerID = StringUtils.parseInt(request.getParameter("CustomerID"));
			int wWorkShopID = StringUtils.parseInt(request.getParameter("WorkShopID"));
			int wLineID = StringUtils.parseInt(request.getParameter("LineID"));
			int wProductID = StringUtils.parseInt(request.getParameter("ProductID"));
			String wPartNo = StringUtils.parseString(request.getParameter("PartNo"));
			Calendar wStartTime = StringUtils.parseCalendar(request.getParameter("StartTime"));
			Calendar wEndTime = StringUtils.parseCalendar(request.getParameter("EndTime"));

			BMSEmployee wLoginUser = this.GetSession(request);

			ServiceResult<List<OMSOrder>> wServiceResult = wOMSService.OMS_QueryRFOrderList(wLoginUser, wCustomerID,
					wWorkShopID, wLineID, wProductID, wPartNo, wStartTime, wEndTime, 1);

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
	 * 状态查询订单
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/StatusAll")
	public Object StatusAll(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}
			List<Integer> wStatusList = StringUtils.parseIntList(request.getParameter("StatusList"), ",");

			int wWorkShopID = StringUtils.parseInt(request.getParameter("WorkShopID"));
			int wLineID = StringUtils.parseInt(request.getParameter("LineID"));
			BMSEmployee wLoginUser = this.GetSession(request);

			ServiceResult<List<OMSOrder>> wServiceResult = wOMSService.OMS_QueryOMSOrderList(wLoginUser, -1,
					wWorkShopID, wLineID, -1, -1, -1, "", "", 1, null, null, null, null, wStatusList);

			if (StringUtils.isEmpty(wServiceResult.getFaultCode())) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_IN);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 删除
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

			List<OMSOrder> wOMSOrderList = CloneTool.CloneArray(wParam.get("data"), OMSOrder.class);
			if (wOMSOrderList == null || wOMSOrderList.size() <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			ServiceResult<Integer> wServiceResult = wOMSService.OMS_DeleteOrderList(wLoginUser, wOMSOrderList);

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

			if (!wParam.containsKey("data"))
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);

			OMSOrder wOMSOrder = CloneTool.Clone(wParam.get("data"), OMSOrder.class);
			if (wOMSOrder == null)
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);

			if (wOMSOrder.ID <= 0) {
				wOMSOrder.CreateID = wOMSOrder.ID;
				wOMSOrder.CreateTime = Calendar.getInstance();
			} else {
				wOMSOrder.EditID = wLoginUser.ID;
				wOMSOrder.EditTime = Calendar.getInstance();
			}

			ServiceResult<Long> wServiceResult = wOMSService.OMS_UpdateOMSOrder(wLoginUser, wOMSOrder);

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
	@PostMapping("/ImportOrder")
	public Object ImportOrder(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = this.GetSession(request);

			if (!wParam.containsKey("data"))
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);

			List<OMSOrder> wOMSOrderList = CloneTool.CloneArray(wParam.get("data"), OMSOrder.class);
			if (wOMSOrderList == null || wOMSOrderList.size() <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			ServiceResult<Long> wServiceResult = new ServiceResult<Long>();

			for (OMSOrder omsOrder : wOMSOrderList) {
				wServiceResult = wOMSService.OMS_UpdateOMSOrder(wLoginUser, omsOrder);
				if (StringUtils.isNotEmpty(wServiceResult.getFaultCode()))
					break;
			}

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
	 * 更新完成数
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/UpdatePD")
	public Object UpdatePD(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = this.GetSession(request);

			if (!wParam.containsKey("PDNum") || !wParam.containsKey("OrderID") || !wParam.containsKey("ReportType")) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			int wOrderID = StringUtils.parseInt(wParam.get("OrderID"));
			double wPDNum = StringUtils.parseDouble(wParam.get("PDNum"));
			double wPDBadNum = StringUtils.parseDouble(wParam.get("PDBadNum"));
			int wReportType = StringUtils.parseInt(wParam.get("ReportType"));
			if (wOrderID <= 0 || wReportType <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			ServiceResult<OMSOrder> wOMSOrderServiceResult = wOMSService.OMS_QueryOMSOrder(wLoginUser, wOrderID);
			if (StringUtils.isNotEmpty(wOMSOrderServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wOMSOrderServiceResult.FaultCode);
				return wResult;
			}
			OMSOrder wOMSOrder = wOMSOrderServiceResult.getResult();
			if (wOMSOrder == null || wOMSOrder.ID <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, StringUtils.Format("ID为{0}的Order不存在!", wOrderID));
				return wResult;
			}

			switch (WDWReportCheckTypes.getEnumType(wReportType)) {
			case Report:
				wOMSOrder.FQTYActual += wPDNum;
				wOMSOrder.FQTYBad += wPDBadNum;
				break;
			case FHReport:
				wOMSOrder.FQTYSent += wPDNum;
				break;
			case Scrap:
				wOMSOrder.FQTYScrap = +wPDNum;
				break;
			default:
				wResult = GetResult(RetCode.SERVER_CODE_ERR, StringUtils.Format("ReportType参数错误 : {0}!", wReportType));
				return wResult;
			}

			ServiceResult<Long> wServiceResult = wOMSService.OMS_UpdateOMSOrder(wLoginUser, wOMSOrder);

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

			if (!wParam.containsKey("Active"))
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
			int wActive = StringUtils.parseInt(wParam.get("Active"));

			if (!wParam.containsKey("data"))
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);

			List<OMSOrder> wList = CloneTool.CloneArray(wParam.get("data"), OMSOrder.class);
			if (wList == null || wList.size() <= 0)
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);

			List<Integer> wIDList = wList.stream().map(p -> p.getID()).collect(Collectors.toList());

			wOMSService.OMS_ActiveOMSOrderList(wLoginUser, wIDList, wActive);

			wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, null);
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 设置订单优先级
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/OrderPriority")
	public Object OrderPriorty(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = this.GetSession(request);

			@SuppressWarnings("unchecked")
			Map<String, Object> wDataParam = CloneTool.Clone(wParam.get("data"), Map.class);

			// 订单列表
			if (!wDataParam.containsKey("OrderList"))
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
			// 优先级集合列表
			if (!wDataParam.containsKey("OMSOrderPriorityList"))
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
			// 产线订单集合
			if (!wDataParam.containsKey("LineOrders"))
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
			// 顾客订单集合
			if (!wDataParam.containsKey("CustomerOrders"))
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
			// 产品规格ID集合
			if (!wDataParam.containsKey("ProductIDs"))
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
			// 跨天限制时长
			if (!wDataParam.containsKey("LimitMinutes"))
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
			// 排程性质
			if (!wDataParam.containsKey("ShiftPeriod"))
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
			// 冗余天数
			if (!wDataParam.containsKey("RedundantDays"))
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);

			List<OMSOrder> wOrderList = CloneTool.CloneArray(wDataParam.get("OrderList"), OMSOrder.class);
			List<Integer> wPriorityIntList = CloneTool.CloneArray(wDataParam.get("OMSOrderPriorityList"),
					Integer.class);
			List<OMSOrderPriority> wOrderPriorityList = new ArrayList<OMSOrderPriority>();
			for (Integer wItem : wPriorityIntList) {
				wOrderPriorityList.add(OMSOrderPriority.getEnumType(wItem));
			}
			List<Integer> wLineOrders = CloneTool.CloneArray(wDataParam.get("LineOrders"), Integer.class);
			List<Integer> wCustomerOrders = CloneTool.CloneArray(wDataParam.get("CustomerOrders"), Integer.class);
			List<Integer> wProductIDs = CloneTool.CloneArray(wDataParam.get("ProductIDs"), Integer.class);
			int wLimitMinutes = StringUtils.parseInt(wDataParam.get("LimitMinutes"));
			int wShiftPeriod = StringUtils.parseInt(wDataParam.get("ShiftPeriod"));
			int wRedundantDays = StringUtils.parseInt(wDataParam.get("RedundantDays"));

			ServiceResult<List<OMSOrder>> wServerRst = wAPSService.APS_SetOrderPriority(wLoginUser, wOrderList,
					wOrderPriorityList, wLineOrders, wCustomerOrders, wProductIDs);
			if (StringUtils.isNotEmpty(wServerRst.getFaultCode())) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode());
				return wResult;
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.Result, null);
			}

			// 保存排程条件
			ServiceResult<List<APSCondition>> wListRst = wAPSService.APS_QueryConditionList(wLoginUser, -1,
					wShiftPeriod);
			APSCondition wAPSCondition = new APSCondition();
			if (wListRst.Result == null || wListRst.Result.size() <= 0) {
				wAPSCondition.ID = 0;
			} else {
				wAPSCondition.ID = wListRst.Result.get(0).ID;
			}
			wAPSCondition.CustomerOrders = wCustomerOrders;
			wAPSCondition.EditID = wLoginUser.ID;
			wAPSCondition.EditTime = Calendar.getInstance();
			wAPSCondition.LimitMinutes = wLimitMinutes;
			wAPSCondition.LineOrders = wLineOrders;
			wAPSCondition.OMSOrderPrioritys = wPriorityIntList;
			wAPSCondition.ProductIDs = wProductIDs;
			wAPSCondition.ShiftPeriod = wShiftPeriod;
			wAPSCondition.RedundantDays = wRedundantDays;
			wAPSService.APS_UpdateCondition(wLoginUser, wAPSCondition);

		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 查询计划订单列表(排程用)
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/OrderList")
	public Object OrderList(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = this.GetSession(request);

			// 周计划、月计划
			Calendar wStartTime = StringUtils.parseCalendar(request.getParameter("StartTime"));
			Calendar wEndTime = StringUtils.parseCalendar(request.getParameter("EndTime"));

			int wWorkShopID = StringUtils.parseInt(request.getParameter("WorkShopID"));
			int wAPSShiftPeriod = StringUtils.parseInt(request.getParameter("APSShiftPeriod"));

			ServiceResult<List<OMSOrder>> wServiceResult = wOMSService.OMS_QueryHistoryOrderList(wLoginUser,
					wWorkShopID, wStartTime, wEndTime, wAPSShiftPeriod);

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
	 * 查询排程条件
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/ScheduleCondition")
	public Object ScheduleCondition(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (this.CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = this.GetSession(request);

			// 排程性质
			int wShiftPeriod = StringUtils.parseInt(request.getParameter("ShiftPeriod"));

			ServiceResult<List<APSCondition>> wServiceResult = wAPSService.APS_QueryConditionList(wLoginUser, -1,
					wShiftPeriod);

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
	 * 获取待竣工确认的订单列表
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/CompleteList")
	public Object CompleteList(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			ServiceResult<List<OMSOrder>> wServiceResult = wAPSService.APS_QueryCompleteOrderList(wLoginUser);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 竣工确认
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/CompleteConfirm")
	public Object CompleteConfirm(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Map<String, Object> wResult = new HashMap<String, Object>();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			// 【竣工确认】权限控制
			if (!wCoreService.BMS_CheckPowerByAuthorityID(wLoginUser.CompanyID, wLoginUser.ID, 500905, 0, 0)
					.Info(Boolean.class)) {
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_CODE_UNROLE);
			}

			// 获取参数
			int wOrderID = StringUtils.parseInt(wParam.get("OrderID"));
			List<String> wImagePathList = CloneTool.CloneArray(wParam.get("ImagePathList"), String.class);
			String wRemark = StringUtils.parseString(wParam.get("Remark"));

			ServiceResult<Integer> wServiceResult = wAPSService.APS_CompleteConfirm(wLoginUser, wOrderID,
					wImagePathList, wRemark);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServiceResult.Result);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 获取待出厂申请的订单列表
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/OutApplyList")
	public Object OutApplyList(HttpServletRequest request) {
		Map<String, Object> wResult = new HashMap<String, Object>();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			ServiceResult<List<OMSOrder>> wServiceResult = wAPSService.APS_QueryOutApplyOrderList(wLoginUser);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
				this.SetResult(wResult, "OrderMap", wServiceResult.CustomResult.get("OrderMap"));
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 出厂申请
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/OutApply")
	public Object OutApply(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Map<String, Object> wResult = new HashMap<String, Object>();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			// 【出厂申请】权限控制
			if (!wCoreService.BMS_CheckPowerByAuthorityID(wLoginUser.CompanyID, wLoginUser.ID, 500904, 0, 0)
					.Info(Boolean.class)) {
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_CODE_UNROLE);
			}

			// 获取参数
			int wOrderID = StringUtils.parseInt(wParam.get("OrderID"));
			List<String> wImagePathList = CloneTool.CloneArray(wParam.get("ImagePathList"), String.class);
			String wRemark = StringUtils.parseString(wParam.get("Remark"));

			ServiceResult<Integer> wServiceResult = wAPSService.APS_OutApply(wLoginUser, wOrderID, wImagePathList,
					wRemark);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServiceResult.Result);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 出厂确认
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/OutConfirm")
	public Object OutConfirm(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Map<String, Object> wResult = new HashMap<String, Object>();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			// 【出厂确认】权限控制
			if (!wCoreService.BMS_CheckPowerByAuthorityID(wLoginUser.CompanyID, wLoginUser.ID, 500906, 0, 0)
					.Info(Boolean.class)) {
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_CODE_UNROLE);
			}

			// 获取参数
			int wOrderID = StringUtils.parseInt(wParam.get("OrderID"));

			ServiceResult<Integer> wServiceResult = wAPSService.APS_OutConfirm(wLoginUser, wOrderID);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServiceResult.Result);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 进厂确认单列表
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/InPlantList")
	public Object InPlantList(HttpServletRequest request) {
		Map<String, Object> wResult = new HashMap<String, Object>();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			ServiceResult<List<OMSOrder>> wServiceResult = wAPSService.OMS_QueryInPlantList(wLoginUser);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
				this.SetResult(wResult, "OrderMap", wServiceResult.CustomResult.get("OrderMap"));
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 进厂确认
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/InPlantConfirm")
	public Object InPlantConfirm(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Map<String, Object> wResult = new HashMap<String, Object>();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			// 获取参数
			int wOrderID = StringUtils.parseInt(wParam.get("OrderID"));
			List<String> wImagePathList = CloneTool.CloneArray(wParam.get("ImagePathList"), String.class);
			String wRemark = StringUtils.parseString(wParam.get("Remark"));

			ServiceResult<Integer> wServiceResult = wAPSService.OMS_InPlantConfirm(wLoginUser, wOrderID, wImagePathList,
					wRemark);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServiceResult.Result);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 修改订单的RouteID
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/ChangeRoute")
	public Object ChangeRoute(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Map<String, Object> wResult = new HashMap<String, Object>();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			// 【订单工艺路线】权限控制
			if (!wCoreService.BMS_CheckPowerByAuthorityID(wLoginUser.CompanyID, wLoginUser.ID, 501904, 0, 0)
					.Info(Boolean.class)) {
				return GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_CODE_UNROLE);
			}

			// 获取参数
			OMSOrder wOrder = CloneTool.Clone(wParam.get("data"), OMSOrder.class);
			Integer wRouteID = StringUtils.parseInt(wParam.get("RouteID"));
			// 参数验证
			if (wOrder == null || wOrder.ID <= 0 || wOrder.RouteID == wRouteID) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			ServiceResult<Integer> wServiceResult = wOMSService.OMS_ChangeRoute(wLoginUser, wOrder, wRouteID);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServiceResult.Result);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	@GetMapping({ "/ConditionAll" })
	public Object ConditionAll(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			int wProductID = StringUtils.parseInt(request.getParameter("ProductID")).intValue();
			int wWorkShopID = StringUtils.parseInt(request.getParameter("WorkShopID"));
			int wLine = StringUtils.parseInt(request.getParameter("Line")).intValue();
			int wCustomerID = StringUtils.parseInt(request.getParameter("CustomerID")).intValue();
			String wWBSNo = StringUtils.parseString(request.getParameter("WBSNo"));
			Calendar wStartTime = StringUtils.parseCalendar(request.getParameter("StartTime"));
			Calendar wEndTime = StringUtils.parseCalendar(request.getParameter("EndTime"));
			int wStatus = StringUtils.parseInt(request.getParameter("Status")).intValue();

			ServiceResult<List<OMSOrder>> wServiceResult = this.wOMSService.OMS_QueryConditionAll(wLoginUser,
					wProductID, wWorkShopID, wLine, wCustomerID, wWBSNo, wStartTime, wEndTime, wStatus);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	@PostMapping({ "/SaveList" })
	public Object SaveTaskItemList(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Map<String, Object> wResult = new HashMap<>();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			List<OMSOrder> wOrderList = CloneTool.CloneArray(wParam.get("data"), OMSOrder.class);
			if (wOrderList == null || wOrderList.size() <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			ServiceResult<Integer> wServiceResult = this.wOMSService.OMS_SaveList(wLoginUser, wOrderList);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServiceResult.Result);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	@PostMapping({ "/IDList" })
	public Object IDList(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Map<String, Object> wResult = new HashMap<>();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			List<Integer> wOrderIDList = CloneTool.CloneArray(wParam.get("data"), Integer.class);
			if (wOrderIDList == null || wOrderIDList.size() <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			ServiceResult<List<OMSOrder>> wServiceResult = this.wOMSService.OMS_QueryOrderListByIDList(wLoginUser,
					wOrderIDList);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 已收电报
	 * 
	 * @param request
	 * @param wParam
	 * @return
	 */
	@PostMapping("/Received")
	public Object Received(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Map<String, Object> wResult = new HashMap<String, Object>();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			List<OMSOrder> wOrderList = CloneTool.CloneArray(wParam.get("data"), OMSOrder.class);

			ServiceResult<Integer> wServiceResult = wOMSService.OMS_ReceivedOrder(wLoginUser, wOrderList);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServiceResult.Result);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 订单基础数据检查
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/Check")
	public Object Check(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			// 获取参数
			int wOrderID = StringUtils.parseInt(request.getParameter("OrderID"));

			ServiceResult<List<OMSCheckMsg>> wServiceResult = wOMSService.OMS_Check(wLoginUser, wOrderID);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 齐套数据检查
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/CheckMaterial")
	public Object CheckMaterial(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			// 获取参数
			List<Integer> wOrderIDList = StringUtils.parseIntList(request.getParameter("OrderIDList"), ",");
			// int wOrderID = StringUtils.parseInt(request.getParameter("OrderID"));

			ServiceResult<List<OMSCheckMaterial>> wServiceResult = wOMSService.OMS_CheckMaterial(wLoginUser,
					wOrderIDList);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 齐套数据检查历史
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/CheckMaterialHistory")
	public Object CheckMaterialHistory(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			// 获取参数
			int wOrderID = StringUtils.parseInt(request.getParameter("OrderID"));
			int wCommandID = StringUtils.parseInt(request.getParameter("CommandID"));
			int wWorkShopID = StringUtils.parseInt(request.getParameter("WorkShopID"));
			int wLineID = StringUtils.parseInt(request.getParameter("LineID"));
			int wProductID = StringUtils.parseInt(request.getParameter("ProductID"));
			int wMaterialID = StringUtils.parseInt(request.getParameter("MaterialID"));
			int wCustomerID = StringUtils.parseInt(request.getParameter("CustomerID"));
			String wOrderNo = StringUtils.parseString(request.getParameter("OrderNo"));
			String wMaterialNo = StringUtils.parseString(request.getParameter("MaterialNo"));
			String wPartNo = StringUtils.parseString(request.getParameter("PartNo"));
			String wBOMNo = StringUtils.parseString(request.getParameter("BOMNo"));
			Calendar wStartTime = StringUtils.parseCalendar(request.getParameter("StartTime"));
			Calendar wEndTime = StringUtils.parseCalendar(request.getParameter("EndTime"));
			ServiceResult<List<OMSCheckMaterial>> wServiceResult = wOMSService.OMS_SelectCheckMaterial(wLoginUser,
					wCommandID, wOrderID, wOrderNo, wWorkShopID, wLineID, wProductID, wMaterialID, wMaterialNo,
					wCustomerID, wPartNo, wBOMNo, wStartTime, wEndTime);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 在线库存查询
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/LineStoreMaterial")
	public Object LineStoreMaterial(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			// 获取参数
			List<Integer> wOrderIDList = StringUtils.parseIntList(request.getParameter("OrderIDList"), ",");

			if (wOrderIDList == null || wOrderIDList.size() <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			ServiceResult<List<OMSCheckMaterial>> wServiceResult = wOMSService.OMS_LineStoreMaterial(wLoginUser,
					wOrderIDList);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 刷新订单投入数
	 * 
	 * @param request
	 * @return
	 */
	@PostMapping("/SyncReceive")
	public Object SyncReceive(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Object wResult = new Object();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);
			List<OMSOrder> wList = CloneTool.CloneArray(wParam.get("data"), OMSOrder.class);

			if (wList == null || wList.size() <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}

			List<Integer> wOrderIDList = wList.stream().filter(p -> p.ID > 0).map(p -> p.ID).distinct()
					.collect(Collectors.toList());
			if (wOrderIDList == null || wOrderIDList.size() <= 0) {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
				return wResult;
			}
			ServiceResult<Integer> wServiceResult = wOMSService.OMS_SyncOrderReceiveByOrderIDList(wLoginUser,
					wOrderIDList);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "");
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 删除齐套数据检查历史
	 * 
	 * @param request
	 * @return
	 */
	@PostMapping("/DeleteCheckMaterialHistory")
	public Object DeleteCheckMaterialHistory(HttpServletRequest request, @RequestBody Map<String, Object> wParam) {
		Object wResult = new Object();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);
			List<OMSCheckMaterial> wList = CloneTool.CloneArray(wParam.get("data"), OMSCheckMaterial.class);

			ServiceResult<Integer> wServiceResult = wOMSService.OMS_DeleteCheckMaterial(wLoginUser, wList);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "");
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}

	/**
	 * 导出检验报告
	 * 
	 * @param request
	 * @return
	 */
	@GetMapping("/Export")
	public Object Export(HttpServletRequest request) {
		Object wResult = new Object();
		try {
			if (CheckCookieEmpty(request)) {
				wResult = GetResult(RetCode.SERVER_CODE_UNLOGIN, "");
				return wResult;
			}

			BMSEmployee wLoginUser = GetSession(request);

			// 获取参数
			int wOrderID = StringUtils.parseInt(request.getParameter("OrderID"));

			ServiceResult<String> wServiceResult = wOMSService.ExportCheck(wLoginUser, wOrderID);

			if (StringUtils.isEmpty(wServiceResult.FaultCode)) {
				wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
			} else {
				wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.FaultCode);
			}
		} catch (Exception ex) {
			logger.error(ex.toString());
			wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.toString(), null, null);
		}
		return wResult;
	}
}
