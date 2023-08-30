package com.mes.server.serviceimpl;

import java.util.List;
import java.util.DateTime;
import java.util.Dictionary;
import java.util.List;
import java.util.Dictionary;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import com.mes.server.service.FMCService;
import com.mes.server.service.mesenum.MESException;
import com.mes.server.service.po.OutResult;
import com.mes.server.service.po.ServiceResult;
import com.mes.server.service.po.bms.BMSEmployee;
import com.mes.server.service.po.fmc.FMCBusinessUnit;
import com.mes.server.service.po.fmc.FMCFactory;
import com.mes.server.service.po.fmc.FMCLine;
import com.mes.server.service.po.fmc.FMCLineUnit;
import com.mes.server.service.po.fmc.FMCResource;
import com.mes.server.service.po.fmc.FMCShift;
import com.mes.server.service.po.fmc.FMCStation;
import com.mes.server.service.po.fmc.FMCTimeZone;
import com.mes.server.service.po.fmc.FMCWorkArea;
import com.mes.server.service.po.fmc.FMCWorkDay;
import com.mes.server.service.po.fmc.FMCWorkShop;
import com.mes.server.service.po.fmc.FMCWorkspace;
import com.mes.server.service.po.fmc.FMCWorkspaceRecord;
import com.mes.server.serviceimpl.dao.fmc.FMCFactoryDAO;
import com.mes.server.serviceimpl.dao.fmc.FMCLineDAO;
import com.mes.server.serviceimpl.dao.fmc.FMCLineUnitDAO;
import com.mes.server.serviceimpl.dao.fmc.FMCResourceDAO;
import com.mes.server.serviceimpl.dao.fmc.FMCShiftDAO;
import com.mes.server.serviceimpl.dao.fmc.FMCStationDAO;
import com.mes.server.serviceimpl.dao.fmc.FMCWorkAreaDAO;
import com.mes.server.serviceimpl.dao.fmc.FMCWorkspaceDAO;
import com.mes.server.serviceimpl.utils.fmc.FMCConstants;
import com.mes.server.serviceimpl.utils.mes.MESServer;

@Service
public class FMCServiceImpl implements FMCService {
	private static Logger logger = LoggerFactory.getLogger(typeof(BMSServiceImpl));

	public FMCServiceImpl() {
		// TODO Auto-generated constructor stub
	}

	private static FMCService Instance = null;

	public static FMCService getInstance() {
		if (Instance == null)
			Instance = new FMCServiceImpl();

		return Instance;
	}

	@Override
	public ServiceResult<Int32> FMC_AddFactory(int wCompanyID, int wLoginID, FMCFactory wFactory) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_AddFactory(wCompanyID, wLoginID, wFactory, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveFactory(int wCompanyID, int wLoginID, FMCFactory wFactory) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_SaveFactory(wCompanyID, wLoginID, wFactory, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DisableFactory(int wCompanyID, int wLoginID, FMCFactory wFactory) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_DisableFactory(wCompanyID, wLoginID, wFactory, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_ActiveFactory(int wCompanyID, int wLoginID, FMCFactory wFactory) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_ActiveFactory(wCompanyID, wLoginID, wFactory, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AuditFactory(int wCompanyID, int wLoginID, FMCFactory wFactory) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_AuditFactory(wCompanyID, wLoginID, wFactory, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCFactory> FMC_QueryFactoryByID(int wCompanyID, int wLoginID, int wID) {
		// TODO Auto-generated method stub
		ServiceResult<FMCFactory> wResult = new ServiceResult<FMCFactory>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryFactoryByID(wCompanyID, wLoginID, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCFactory> FMC_QueryFactoryByCode(int wCompanyID, int wLoginID, String wCode) {
		// TODO Auto-generated method stub
		ServiceResult<FMCFactory> wResult = new ServiceResult<FMCFactory>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryFactoryByCode(wCompanyID, wLoginID, wCode,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCFactory>> FMC_QueryFactoryList(int wCompanyID, int wLoginID, boolean wLoad) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCFactory>> wResult = new ServiceResult<List<FMCFactory>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryFactoryList(wCompanyID, wLoginID, wLoad, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AddBusinessUnit(int wCompanyID, int wLoginID, FMCBusinessUnit wFactory) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_AddBusinessUnit(wCompanyID, wLoginID, wFactory,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveBusinessUnit(int wCompanyID, int wLoginID, FMCBusinessUnit wFactory) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_SaveBusinessUnit(wCompanyID, wLoginID, wFactory,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DisableBusinessUnit(int wCompanyID, int wLoginID, FMCBusinessUnit wFactory) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_DisableBusinessUnit(wCompanyID, wLoginID, wFactory,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_ActiveBusinessUnit(int wCompanyID, int wLoginID, FMCBusinessUnit wFactory) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_ActiveBusinessUnit(wCompanyID, wLoginID, wFactory,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AuditBusinessUnit(int wCompanyID, int wLoginID, FMCBusinessUnit wFactory) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_AuditBusinessUnit(wCompanyID, wLoginID, wFactory,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCBusinessUnit> FMC_QueryBusinessUnitByID(int wCompanyID, int wLoginID, int wID) {
		// TODO Auto-generated method stub
		ServiceResult<FMCBusinessUnit> wResult = new ServiceResult<FMCBusinessUnit>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryBusinessUnitByID(wCompanyID, wLoginID, wID,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCBusinessUnit> FMC_QueryBusinessUnitByCode(int wCompanyID, int wLoginID, String wCode) {
		// TODO Auto-generated method stub
		ServiceResult<FMCBusinessUnit> wResult = new ServiceResult<FMCBusinessUnit>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryBusinessUnitByCode(wCompanyID, wLoginID, wCode,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCBusinessUnit>> FMC_QueryBusinessUnitList(int wCompanyID, int wLoginID, boolean wLoad) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCBusinessUnit>> wResult = new ServiceResult<List<FMCBusinessUnit>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryBusinessUnitList(wCompanyID, wLoginID, wLoad,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AddWorkShop(int wCompanyID, int wLoginID, FMCWorkShop wWorkShop) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_AddWorkShop(wCompanyID, wLoginID, wWorkShop, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveWorkShop(int wCompanyID, int wLoginID, FMCWorkShop wWorkShop) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_SaveWorkShop(wCompanyID, wLoginID, wWorkShop, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DisableWorkShop(int wCompanyID, int wLoginID, FMCWorkShop wWorkShop) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_DisableWorkShop(wCompanyID, wLoginID, wWorkShop,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_ActiveWorkShop(int wCompanyID, int wLoginID, FMCWorkShop wWorkShop) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_ActiveWorkShop(wCompanyID, wLoginID, wWorkShop,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AuditWorkShop(int wCompanyID, int wLoginID, FMCWorkShop wWorkShop) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_AuditWorkShop(wCompanyID, wLoginID, wWorkShop, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCWorkShop> FMC_QueryWorkShopByID(int wCompanyID, int wLoginID, int wID) {
		// TODO Auto-generated method stub
		ServiceResult<FMCWorkShop> wResult = new ServiceResult<FMCWorkShop>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryWorkShopByID(wCompanyID, wLoginID, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCWorkShop> FMC_QueryWorkShopByCode(int wCompanyID, int wLoginID, String wCode) {
		// TODO Auto-generated method stub
		ServiceResult<FMCWorkShop> wResult = new ServiceResult<FMCWorkShop>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryWorkShopByCode(wCompanyID, wLoginID, wCode,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCWorkShop>> FMC_QueryWorkShopList(int wCompanyID, int wLoginID, int wFactoryID,
			int wBusinessUnitID, boolean wLoad) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCWorkShop>> wResult = new ServiceResult<List<FMCWorkShop>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryWorkShopList(wCompanyID, wLoginID, wFactoryID,
					wBusinessUnitID, wLoad, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AddLine(int wCompanyID, int wLoginID, FMCLine wLine) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCLineDAO.getInstance().FMC_AddLine(wCompanyID, wLoginID, wLine, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveLine(int wCompanyID, int wLoginID, FMCLine wLine) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCLineDAO.getInstance().FMC_SaveLine(wCompanyID, wLoginID, wLine, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DisableLine(int wCompanyID, int wLoginID, FMCLine wLine) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCLineDAO.getInstance().FMC_DisableLine(wCompanyID, wLoginID, wLine, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_ActiveLine(int wCompanyID, int wLoginID, FMCLine wLine) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCLineDAO.getInstance().FMC_ActiveLine(wCompanyID, wLoginID, wLine, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AuditLine(int wCompanyID, int wLoginID, FMCLine wLine) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCLineDAO.getInstance().FMC_AuditLine(wCompanyID, wLoginID, wLine, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCLine> FMC_QueryLineByID(int wCompanyID, int wLoginID, int wID) {
		// TODO Auto-generated method stub
		ServiceResult<FMCLine> wResult = new ServiceResult<FMCLine>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCLineDAO.getInstance().FMC_QueryLineByID(wCompanyID, wLoginID, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCLine> FMC_QueryLineByCode(int wCompanyID, int wLoginID, String wCode) {
		// TODO Auto-generated method stub
		ServiceResult<FMCLine> wResult = new ServiceResult<FMCLine>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCLineDAO.getInstance().FMC_QueryLineByCode(wCompanyID, wLoginID, wCode, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCLine>> FMC_QueryLineList(int wCompanyID, int wLoginID, int wBusinessUnitID,
			int wFactoryID, int wWorkShopID, boolean wLoad) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCLine>> wResult = new ServiceResult<List<FMCLine>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCLineDAO.getInstance().FMC_QueryLineList(wCompanyID, wLoginID, wBusinessUnitID,
					wFactoryID, wWorkShopID, wLoad, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Dictionary<Int32, FMCLine>> FMC_QueryLineDic() {
		// TODO Auto-generated method stub
		ServiceResult<Dictionary<Int32, FMCLine>> wResult = new ServiceResult<Dictionary<Int32, FMCLine>>(
				new Dictionary<Int32, FMCLine>());
		try {

			wResult.Result = FMCConstants.GetFMCLineList();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AddLineUnit(int wCompanyID, int wLoginID, FMCLineUnit wLineUnit) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);

			FMCLineUnitDAO.getInstance().FMC_AddLineUnit(wCompanyID, wLoginID, wLineUnit, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_CopyLineUnit(int wCompanyID, int wLoginID, int wOldLineID, int wOldProductID,
			int wOldCustomerID, int wLineID, int wProductID, int wCustomerID) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCLineUnitDAO.getInstance().FMC_CopyLineUnit(wCompanyID, wLoginID, wOldLineID, wOldProductID,
					wOldCustomerID, wLineID, wProductID, wCustomerID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveLineUnit(int wCompanyID, int wLoginID, FMCLineUnit wLineUnit) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCLineUnitDAO.getInstance().FMC_SaveLineUnit(wCompanyID, wLoginID, wLineUnit, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DeleteLineUnitByID(int wCompanyID, int wLoginID, int wID) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCLineUnitDAO.getInstance().FMC_DeleteLineUnitByID(wCompanyID, wLoginID, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_ActiveLineUnit(int wCompanyID, int wLoginID, FMCLineUnit wLineUnit) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCLineUnitDAO.getInstance().FMC_ActiveLineUnit(wCompanyID, wLoginID, wLineUnit,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DisableLineUnit(int wCompanyID, int wLoginID, FMCLineUnit wLineUnit) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCLineUnitDAO.getInstance().FMC_DisableLineUnit(wCompanyID, wLoginID, wLineUnit,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCLineUnit>> FMC_QueryLineUnitListByLineID(int wCompanyID, int wLoginID, int wProductID,
			int wCustomerID, int wLineID, int wID, boolean wIsList) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCLineUnit>> wResult = new ServiceResult<List<FMCLineUnit>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCLineUnitDAO.getInstance().FMC_QueryLineUnitListByLineID(wCompanyID, wLoginID,
					wProductID, wCustomerID, wLineID, wID, wIsList, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCLineUnit>> FMC_QueryLineUnitListByStationID(int wCompanyID, int wLoginID,
			int wProductID, int wCustomerID, int wLineID, int wStationID) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCLineUnit>> wResult = new ServiceResult<List<FMCLineUnit>>();
		wResult.Result = new List<FMCLineUnit>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			List<FMCLineUnit> wLineUnitList = FMCLineUnitDAO.getInstance().FMC_QueryLineUnitListByLineID(wCompanyID,
					wLoginID, wProductID, wCustomerID, wLineID, 0, false, wErrorCode);

			for (FMCLineUnit wPartUnit : wLineUnitList) {
				for (FMCLineUnit wStepUnit : wPartUnit.UnitList) {
					if (wStepUnit.UnitList != null && wStepUnit.UnitList.Count > 0
							&& wStepUnit.UnitList.stream().anyMatch(p -> p.UnitID == wStationID)) {
						wResult.Result.Add(wPartUnit);
						break;
					}
				}
			}

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCLineUnit>> FMC_QueryLineUnitListByPartID(int wCompanyID, int wLoginID, int wProductID,
			int wCustomerID, int wLineID, int wPartID) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCLineUnit>> wResult = new ServiceResult<List<FMCLineUnit>>();
		wResult.Result = new List<FMCLineUnit>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			List<FMCLineUnit> wLineUnitList = FMCLineUnitDAO.getInstance().FMC_QueryLineUnitListByLineID(wCompanyID,
					wLoginID, wProductID, wCustomerID, wLineID, 0, false, wErrorCode);

			for (FMCLineUnit wPartUnit : wLineUnitList) {

				if (wPartUnit.UnitList == null || wPartUnit.UnitList.Count <= 0 || wPartUnit.UnitID != wPartID
						|| wPartUnit.Active != 1)
					continue;

				for (FMCLineUnit wStepUnit : wPartUnit.UnitList) {

					if (wStepUnit.UnitList == null || wStepUnit.UnitList.Count <= 0 || wStepUnit.Active != 1)
						continue;

					for (FMCLineUnit wStationUnit : wStepUnit.UnitList) {
						if (wStepUnit.Active != 1 || wStepUnit.ID <= 0 || wStepUnit.UnitID <= 0)
							continue;

						wResult.Result.Add(wStationUnit);
					}
				}
			}

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCStation>> FMC_QueryStationListByPartID(int wCompanyID, int wLoginID, int wProductID,
			int wCustomerID, int wLineID, int wPartID) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCStation>> wResult = new ServiceResult<List<FMCStation>>();
		wResult.Result = new List<FMCStation>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			List<FMCLineUnit> wLineUnitList = FMCLineUnitDAO.getInstance().FMC_QueryLineUnitListByLineID(wCompanyID,
					wLoginID, wProductID, wCustomerID, wLineID, 0, false, wErrorCode);

			List<Int32> wStationIDList = new List<Int32>();
			for (FMCLineUnit wPartUnit : wLineUnitList) {

				if (wPartUnit.UnitList == null || wPartUnit.UnitList.Count <= 0 || wPartUnit.UnitID != wPartID
						|| wPartUnit.Active != 1)
					continue;

				for (FMCLineUnit wStepUnit : wPartUnit.UnitList) {

					if (wStepUnit.UnitList == null || wStepUnit.UnitList.Count <= 0 || wStepUnit.Active != 1)
						continue;

					for (FMCLineUnit wStationUnit : wStepUnit.UnitList) {
						if (wStepUnit.Active != 1 || wStepUnit.ID <= 0 || wStepUnit.UnitID <= 0
								|| wStationIDList.contains(wStepUnit.UnitID))
							continue;
						wStationIDList.Add(wStationUnit.UnitID);
						wResult.Result.Add(FMCConstants.GetFMCStation(wStationUnit.UnitID));
					}
				}
			}

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AddResource(BMSEmployee wLoginUser, FMCResource wResource) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCResourceDAO.getInstance().FMC_AddResource(wLoginUser, wResource, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveResource(BMSEmployee wLoginUser, FMCResource wResource) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCResourceDAO.getInstance().FMC_SaveResource(wLoginUser, wResource, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DisableResource(BMSEmployee wLoginUser, FMCResource wResource) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCResourceDAO.getInstance().FMC_DisableResource(wLoginUser, wResource, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_ActiveResource(BMSEmployee wLoginUser, FMCResource wResource) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCResourceDAO.getInstance().FMC_ActiveResource(wLoginUser, wResource, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCResource> FMC_QueryResourceByID(BMSEmployee wLoginUser, int wID) {
		// TODO Auto-generated method stub
		ServiceResult<FMCResource> wResult = new ServiceResult<FMCResource>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCResourceDAO.getInstance().FMC_QueryResourceByID(wLoginUser, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCResource>> FMC_QueryResourceList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
			int wStationID, int wAreaID, int wResourceID, int wType, int wActive) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCResource>> wResult = new ServiceResult<List<FMCResource>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCResourceDAO.getInstance().FMC_QueryResourceList(wLoginUser, wWorkShopID, wLineID,
					wStationID, wAreaID, wResourceID, wType, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AddWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCWorkAreaDAO.getInstance().FMC_AddWorkArea(wLoginUser, wWorkArea, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCWorkAreaDAO.getInstance().FMC_SaveWorkArea(wLoginUser, wWorkArea, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DisableWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCWorkAreaDAO.getInstance().FMC_DisableWorkArea(wLoginUser, wWorkArea, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_ActiveWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCWorkAreaDAO.getInstance().FMC_ActiveWorkArea(wLoginUser, wWorkArea, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCWorkArea> FMC_QueryWorkArea(BMSEmployee wLoginUser, int wID, String wCode) {
		// TODO Auto-generated method stub
		ServiceResult<FMCWorkArea> wResult = new ServiceResult<FMCWorkArea>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCWorkAreaDAO.getInstance().FMC_QueryWorkArea(wLoginUser, wID, wCode, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCWorkArea>> FMC_QueryWorkAreaList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
			int wParentID, int wActive) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCWorkArea>> wResult = new ServiceResult<List<FMCWorkArea>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCWorkAreaDAO.getInstance().FMC_QueryWorkAreaList(wLoginUser, "", wWorkShopID, wLineID,
					wParentID, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AddStation(BMSEmployee wLoginUser, FMCStation wStation) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCStationDAO.getInstance().FMC_AddStation(wLoginUser, wStation, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveStation(BMSEmployee wLoginUser, FMCStation wStation) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCStationDAO.getInstance().FMC_SaveStation(wLoginUser, wStation, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DisableStation(BMSEmployee wLoginUser, FMCStation wStation) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCStationDAO.getInstance().FMC_DisableStation(wLoginUser, wStation, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_ActiveStation(BMSEmployee wLoginUser, FMCStation wStation) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			FMCStationDAO.getInstance().FMC_ActiveStation(wLoginUser, wStation, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCStation> FMC_QueryStation(BMSEmployee wLoginUser, int wID, String wCode) {
		// TODO Auto-generated method stub
		ServiceResult<FMCStation> wResult = new ServiceResult<FMCStation>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCStationDAO.getInstance().FMC_QueryStation(wLoginUser, wID, wCode, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCStation>> FMC_QueryStationList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
			int wWorkAreaID, int wActive) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCStation>> wResult = new ServiceResult<List<FMCStation>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCStationDAO.getInstance().FMC_QueryStationList(wLoginUser, "", wWorkShopID, wLineID,
					wWorkAreaID, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AddWorkDay(int wCompanyID, int wLoginID, FMCWorkDay wShift) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_AddWorkDay(wCompanyID, wLoginID, wShift, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveWorkDay(int wCompanyID, int wLoginID, FMCWorkDay wShift) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

			wResult.Result = FMCShiftDAO.getInstance().FMC_SaveWorkDay(wCompanyID, wLoginID, wShift, wErrorCode);

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DisableWorkDay(int wCompanyID, int wLoginID, FMCWorkDay wShift) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_DisableWorkDay(wCompanyID, wLoginID, wShift, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_ActiveWorkDay(int wCompanyID, int wLoginID, FMCWorkDay wShift) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_ActiveWorkDay(wCompanyID, wLoginID, wShift, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_AuditWorkDay(int wCompanyID, int wLoginID, FMCWorkDay wShift) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_AuditWorkDay(wCompanyID, wLoginID, wShift, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCWorkDay> FMC_QueryWorkDayByID(int wCompanyID, int wLoginID, int wID) {
		// TODO Auto-generated method stub
		ServiceResult<FMCWorkDay> wResult = new ServiceResult<FMCWorkDay>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);

			wResult.Result = FMCShiftDAO.getInstance().FMC_QueryWorkDayByID(wCompanyID, wLoginID, wID, wErrorCode);
			if (wResult.Result != null && wResult.Result.ID > 0) {
				wResult.Result.ShiftList = FMCShiftDAO.getInstance().FMC_QueryShiftList(wCompanyID, wLoginID,
						wResult.Result.ID, -1, wErrorCode);
				if (wResult.Result.ShiftList != null && wResult.Result.ShiftList.Count > 0) {

					wResult.Result.ShiftList.sort((o1, o2) -> o1.LevelID - o2.LevelID);
				}
			}
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCWorkDay> FMC_QueryActiveWorkDay(int wCompanyID, int wLoginID, int wFactoryID,
			int wWorkShopID) {
		// TODO Auto-generated method stub
		ServiceResult<FMCWorkDay> wResult = new ServiceResult<FMCWorkDay>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

			wResult.Result = FMCShiftDAO.getInstance().FMC_QueryActiveWorkDay(wCompanyID, wLoginID, wFactoryID,
					wWorkShopID, wErrorCode);
			if (wResult.Result != null && wResult.Result.ID > 0) {
				wResult.Result.ShiftList = FMCShiftDAO.getInstance().FMC_QueryShiftList(wCompanyID, wLoginID,
						wResult.Result.ID, 1, wErrorCode);

				if (wResult.Result.ShiftList != null && wResult.Result.ShiftList.Count > 0) {
					wResult.Result.ShiftList.RemoveAll(p -> p.Active != 1);

					wResult.Result.ShiftList.sort((o1, o2) -> o1.LevelID - o2.LevelID);
				}
			}

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCWorkDay>> FMC_QueryWorkDayList(int wCompanyID, int wLoginID, int wFactoryID,
			int wWorkShopID, int wActive) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCWorkDay>> wResult = new ServiceResult<List<FMCWorkDay>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_QueryWorkDayList(wCompanyID, wLoginID, wFactoryID,
					wWorkShopID, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCTimeZone>> FMC_QueryShiftTimeZoneList(int wCompanyID, int wLoginID, int wShiftID) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCTimeZone>> wResult = new ServiceResult<List<FMCTimeZone>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_QueryShiftTimeZoneList(wCompanyID, wLoginID, wShiftID,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveShiftTimeZoneList(int wCompanyID, int wLoginID,
			List<FMCTimeZone> wTimeZoneList, int wShiftID) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_SaveShiftTimeZoneList(wCompanyID, wLoginID, wTimeZoneList,
					wShiftID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCShift>> FMC_QueryShiftList(int wCompanyID, int wLoginID, int wWorkDayID, int wActive) {
		// TODO Auto-generated method stub
		ServiceResult<List<FMCShift>> wResult = new ServiceResult<List<FMCShift>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_QueryShiftList(wCompanyID, wLoginID, wWorkDayID, wActive,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveShiftList(int wCompanyID, int wLoginID, List<FMCShift> wShiftList) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_SaveShiftList(wCompanyID, wLoginID, wShiftList, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveShift(int wCompanyID, int wLoginID, FMCShift wShift) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_SaveShift(wCompanyID, wLoginID, wShift, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCShift> FMC_QueryShiftByID(int wCompanyID, int wLoginID, int wWorkDayID) {
		// TODO Auto-generated method stub
		ServiceResult<FMCShift> wResult = new ServiceResult<FMCShift>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_QueryShiftByID(wCompanyID, wLoginID, wWorkDayID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_DeleteShiftByID(int wCompanyID, int wLoginID, int wID) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCShiftDAO.getInstance().FMC_DeleteShiftByID(wCompanyID, wLoginID, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCWorkspace>> FMC_GetFMCWorkspaceList(int wCompanyID, int wLoginID, int wProductID,
			int wPartID, String wPartNo, int wPlaceType, int wActive) {
		ServiceResult<List<FMCWorkspace>> wResult = new ServiceResult<List<FMCWorkspace>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCWorkspaceDAO.getInstance().FMC_GetFMCWorkspaceList(wCompanyID, wLoginID, wProductID,
					wPartID, wPartNo, wPlaceType, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<FMCWorkspace> FMC_GetFMCWorkspace(int wCompanyID, int wLoginID, int wID, String wCode) {
		ServiceResult<FMCWorkspace> wResult = new ServiceResult<FMCWorkspace>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCWorkspaceDAO.getInstance().FMC_GetFMCWorkspace(wCompanyID, wLoginID, wID, wCode,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_SaveFMCWorkspace(int wCompanyID, int wLoginID, FMCWorkspace wFMCWorkspace) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			if (wFMCWorkspace.ID <= 0) {
				FMCWorkspaceDAO.getInstance().FMC_AddFMCWorkspace(wCompanyID, wLoginID, wFMCWorkspace, wErrorCode);
			} else {
				FMCWorkspaceDAO.getInstance().FMC_EditFMCWorkspace(wCompanyID, wLoginID, wFMCWorkspace, wErrorCode);
			}
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_BindFMCWorkspace(int wCompanyID, int wLoginID, FMCWorkspace wFMCWorkspace) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);

			wResult.Result = FMCWorkspaceDAO.getInstance().FMC_BindFMCWorkspace(wCompanyID, wLoginID, wFMCWorkspace,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("FMCServiceImpl FMC_BindFMCWorkspace Error:", e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> FMC_ActiveFMCWorkspace(int wCompanyID, int wLoginID, int wActive,
			FMCWorkspace wFMCWorkspace) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCWorkspaceDAO.getInstance().FMC_ActiveFMCWorkspace(wCompanyID, wLoginID, wActive,
					wFMCWorkspace, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<FMCWorkspaceRecord>> FMC_GetFMCWorkspaceRecordList(int wCompanyID, int wLoginID,
			int wProductID, int wPartID, String wPartNo, int wPlaceID, int wPlaceType, int wLimit, DateTime wStartTime,
			DateTime wEndTime) {
		ServiceResult<List<FMCWorkspaceRecord>> wResult = new ServiceResult<List<FMCWorkspaceRecord>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wErrorCode.set(0);
			wResult.Result = FMCWorkspaceDAO.getInstance().FMC_GetFMCWorkspaceRecordList(wCompanyID, wLoginID,
					wProductID, wPartID, wPartNo, wPlaceID, wPlaceType, wStartTime, wEndTime, wLimit, wErrorCode);

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	public ServiceResult<Int32> FMC_QueryShiftID(int wCompanyID, int wLoginID, int wWorkShopID, DateTime wShiftTime,
			int wShifts, OutResult<Int32> wShiftIndex) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			wResult.Result = MESServer.MES_QueryShiftID(wWorkShopID, wShiftTime, wShiftIndex);
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}
}
