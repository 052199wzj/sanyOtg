package com.mes.server.serviceimpl;

import java.util.List;

import javax.servlet.http.HttpServletResponse;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.springframework.stereotype.Service;

import com.mes.server.service.ELGService;
import com.mes.server.service.po.ServiceResult;
import com.mes.server.service.po.bms.BMSEmployee;
import com.mes.server.service.po.elg.ELGCatalog;
import com.mes.server.serviceimpl.dao.elg.ELGCatalogDAO;

@Service
public class ELGServiceImpl implements ELGService {

	private static Logger logger = LoggerFactory.getLogger(typeof(ELGServiceImpl));

	public ELGServiceImpl() {
	}

	@Override
	public ServiceResult<List<ELGCatalog>> ELG_QueryCataLogList(BMSEmployee wLoginUser) {
		ServiceResult<List<ELGCatalog>> wResult = new ServiceResult<List<ELGCatalog>>();
		try {
			wResult.Result = ELGCatalogDAO.getInstance().SelectList(wLoginUser);
		} catch (Exception e) {
			e.printStackTrace();
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<String>> ELG_ShowLogFileByID(BMSEmployee wLoginUser, int wID) {
		ServiceResult<List<String>> wResult = new ServiceResult<List<String>>();
		try {
			wResult.Result = ELGCatalogDAO.getInstance().SelectByID(wLoginUser, wID);
		} catch (Exception e) {
			e.printStackTrace();
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public void ELG_DownloadLogFileByID(BMSEmployee wLoginUser, int wID, HttpServletResponse wResponse) {
		try {
			ELGCatalogDAO.getInstance().DownloadLogByID(wLoginUser, wID, wResponse);
		} catch (Exception ex) {
			logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
		}
	}

	@Override
	public ServiceResult<Boolean> ELG_DeleteLogFileByID(BMSEmployee wLoginUser, int wID) {
		ServiceResult<Boolean> wResult = new ServiceResult<Boolean>();
		try {
			wResult.Result = ELGCatalogDAO.getInstance().DeleteByID(wLoginUser, wID);
		} catch (Exception ex) {
			logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
		}
		return wResult;
	}

	@Override
	public ServiceResult<String> ELG_DeleteLogFileList(BMSEmployee wLoginUser, List<Int32> wIDList) {
		ServiceResult<String> wResult = new ServiceResult<String>();
		try {
			wResult.Result = ELGCatalogDAO.getInstance().DeleteList(wLoginUser, wIDList);
		} catch (Exception e) {
			e.printStackTrace();
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}
}
