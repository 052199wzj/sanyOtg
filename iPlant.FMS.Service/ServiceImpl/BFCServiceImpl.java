package com.mes.server.serviceimpl;

import java.util.List;
import java.util.DateTime;
import java.util.List;
import java.util.Dictionary;
import java.util.Optional;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import com.mes.server.service.BFCService;
import com.mes.server.service.mesenum.BFCMessageStatus;
import com.mes.server.service.mesenum.BFCMessageType;
import com.mes.server.service.mesenum.BFCQRTypes;
import com.mes.server.service.mesenum.BPMEventModule;
import com.mes.server.service.mesenum.DBEnumType;
import com.mes.server.service.mesenum.MESException;
import com.mes.server.service.po.OutResult;
import com.mes.server.service.po.ServiceResult;
import com.mes.server.service.po.bfc.BFCAuditAction;
import com.mes.server.service.po.bfc.BFCAuditConfig;
import com.mes.server.service.po.bfc.BFCAuditVersion;
import com.mes.server.service.po.bfc.BFCHomePageGroup;
import com.mes.server.service.po.bfc.BFCHomePageModule;
import com.mes.server.service.po.bfc.BFCMessage;
import com.mes.server.service.po.bfc.BFCPlaceQRStruct;
import com.mes.server.service.po.bfc.FPCCustomStruct;
import com.mes.server.service.po.bms.BMSEmployee;
import com.mes.server.service.po.cfg.CFGItem;
import com.mes.server.service.po.cgs.CGSTable;
import com.mes.server.service.po.dms.deviceLegder.device.DMSDeviceLedger;
import com.mes.server.service.po.dms.deviceLegder.spare.DMSSpareLedger;
import com.mes.server.service.po.fmc.FMCBusinessUnit;
import com.mes.server.service.po.fmc.FMCFactory;
import com.mes.server.service.po.fmc.FMCLine;
import com.mes.server.service.po.fmc.FMCStation;
import com.mes.server.service.po.fmc.FMCWorkShop;
import com.mes.server.service.po.fmc.FMCWorkspace;
import com.mes.server.service.po.fpc.FPCPart;
import com.mes.server.service.po.fpc.FPCPartPoint;
import com.mes.server.service.utils.StringUtils;
import com.mes.server.serviceimpl.dao.BaseDAO;
import com.mes.server.serviceimpl.dao.bfc.BFCAuditActionDAO;
import com.mes.server.serviceimpl.dao.bfc.BFCAuditConfigDAO;
import com.mes.server.serviceimpl.dao.bfc.BFCHomePageGroupDAO;
import com.mes.server.serviceimpl.dao.bfc.BFCHomePageModuleDAO;
import com.mes.server.serviceimpl.dao.bfc.BFCMessageDAO;
import com.mes.server.serviceimpl.dao.cgs.CGSDataBaseTableDAO;
import com.mes.server.serviceimpl.dao.cgs.CGSTableConfigDAO;
import com.mes.server.serviceimpl.utils.bfc.BFCConstants;
import com.mes.server.serviceimpl.utils.dms.DMSConstants;
import com.mes.server.serviceimpl.utils.fmc.FMCConstants;

@Service
public class BFCServiceImpl implements BFCService {
	private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(typeof(BFCServiceImpl));
	private static BFCService _instance = null;

	public static BFCService getInstance() {
		if (_instance == null)
			_instance = new BFCServiceImpl();

		return _instance;
	}

	@Override
	public ServiceResult<List<BFCHomePageGroup>> BFC_GetHomePageGroupList(int wType, int wGrad) {
		ServiceResult<List<BFCHomePageGroup>> wResult = new ServiceResult<List<BFCHomePageGroup>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.setResult(BFCHomePageGroupDAO.getInstance().SelectAll(wType, wGrad, wErrorCode));
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<BFCHomePageGroup> BFC_GetHomePageGroupByID() {
		ServiceResult<BFCHomePageGroup> wResult = new ServiceResult<BFCHomePageGroup>();
		// wResult.setResult(BFCHomePageGroupDAO.getInstance().SelectAll(wType));
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_UpdateHomePageGroup(BFCHomePageGroup wBFCHomePageGroup) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.setResult(BFCHomePageGroupDAO.getInstance().Update(wBFCHomePageGroup, wErrorCode));
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BFCHomePageModule>> BFC_GetHomePageModuleList(int wType, int wGrad) {
		ServiceResult<List<BFCHomePageModule>> wResult = new ServiceResult<List<BFCHomePageModule>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.setResult(BFCHomePageModuleDAO.getInstance().SelectAll(0, wType, wGrad, wErrorCode));
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<BFCHomePageModule> BFC_GetHomePageModuleByID(int wID) {
		// TODO Auto-generated method stub
		ServiceResult<BFCHomePageModule> wResult = new ServiceResult<BFCHomePageModule>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.setResult(BFCHomePageModuleDAO.getInstance().Select(wID, wErrorCode));
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_UpdateHomePageModule(BFCHomePageModule wBFCHomePageModule) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.setResult(BFCHomePageModuleDAO.getInstance().Update(wBFCHomePageModule, wErrorCode));
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<String> BFC_GetQRCode(BMSEmployee wLoginUser, BFCQRTypes wQRTypes, long wID) {
		ServiceResult<String> wResult = new ServiceResult<String>();
		try {
			switch (wQRTypes) {
			case Default:
				break;
			case Business:

				FMCBusinessUnit wFMCBusinessUnit = FMCConstants.GetFMCBusinessUnit((int) wID);
				if (wFMCBusinessUnit != null && wFMCBusinessUnit.ID > 0) {
					wResult.Result = wFMCBusinessUnit.Code;
					wResult.Put("Name", wFMCBusinessUnit.Name);
				}

				break;

			case Factory:

				FMCFactory wFMCFactory = FMCConstants.GetFMCFactory((int) wID);
				if (wFMCFactory != null && wFMCFactory.ID > 0) {
					wResult.Result = wFMCFactory.Code;
					wResult.Put("Name", wFMCFactory.Name);
				}

				break;
			case WorkShop:

				FMCWorkShop wFMCWorkShop = FMCConstants.GetFMCWorkShop((int) wID);
				if (wFMCWorkShop != null && wFMCWorkShop.ID > 0) {
					wResult.Result = wFMCWorkShop.Code;
					wResult.Put("Name", wFMCWorkShop.Name);
				}

				break;
			case Line:

				FMCLine wFMCLine = FMCConstants.GetFMCLine((int) wID);
				if (wFMCLine != null && wFMCLine.ID > 0) {
					wResult.Result = wFMCLine.Code;
					wResult.Put("Name", wFMCLine.Name);
				}

				break;
			case Station:

				FMCStation wFMCStation = FMCConstants.GetFMCStation((int) wID);
				if (wFMCStation != null && wFMCStation.ID > 0) {
					wResult.Result = wFMCStation.Code;
					wResult.Put("Name", wFMCStation.Name);
				}

				break;
			case Storehouse:
				// iPlant.Interface.Invoke.FMCService.FMCWorkShop wFMCStorehouse =
				// wFMCService.GetWorkShop(wLoginUser.getCompanyID(),
				// wLoginUser.ID, wQRCode);
				break;
			case StoreLocation:
				// iPlant.Interface.Invoke.FMCService.FMCWorkShop wFMCWorkShop =
				// wFMCService.GetWorkShop(wLoginUser.getCompanyID(),
				// wLoginUser.ID, wQRCode);
				break;
			case Device:
				DMSDeviceLedger wDMSDeviceLedger = DMSConstants.GetDMSDeviceLedger((int) wID);
				if (wDMSDeviceLedger != null && wDMSDeviceLedger.ID > 0) {
					wResult.setResult(wDMSDeviceLedger.Code);
					wResult.Put("Name", wDMSDeviceLedger.Name);
				}
				break;
			case Spare:
				DMSSpareLedger wDMSSpareLedger = DMSConstants.GetDMSSpareLedger((int) wID);
				if (wDMSSpareLedger != null && wDMSSpareLedger.ID > 0) {
					wResult.setResult(wDMSSpareLedger.Code);
					wResult.Put("Name", wDMSSpareLedger.Name);
				}
				break;
			case WorkPlace:

				FMCWorkspace wFMCWorkspace = FMCConstants.getFMCWorkspace((int) wID);
				if (wFMCWorkspace != null && wFMCWorkspace.ID > 0) {
					wResult.Result = wFMCWorkspace.Code;
					wResult.Put("Name", wFMCWorkspace.Name);
				}
				break;
			case Part:

				FPCPart wFPCPart = FMCConstants.GetFPCPart((int) wID);
				if (wFPCPart != null && wFPCPart.ID > 0) {
					wResult.Result = wFPCPart.Code;
					wResult.Put("Name", wFPCPart.Name);
				}
				break;
			case PartPoint:

				FPCPartPoint wFPCPartPoint = FMCConstants.GetFPCStep((int) wID);
				if (wFPCPartPoint != null && wFPCPartPoint.ID > 0) {
					wResult.Result = wFPCPartPoint.Code;
					wResult.Put("Name", wFPCPartPoint.Name);
				}
				break;
			default:
				break;
			}
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_GetIDByQRCode(BMSEmployee wLoginUser, String wCode) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {

			BFCQRTypes wQRTypes = BFCQRTypes.getEnumType(this.BFC_GetPlaceQRTypeByCode(wCode).getResult());
			switch (wQRTypes) {
			case Default:
				break;
			case Business:

				Optional<FMCBusinessUnit> wFMCBusinessUnit = FMCConstants.GetFMCBusinessUnitList().values().stream()
						.filter(p -> p.Code.equalsIgnoreCase(wCode)).findFirst();
				if (wFMCBusinessUnit != null && wFMCBusinessUnit.isPresent() && wFMCBusinessUnit.get().ID > 0) {
					wResult.Result = wFMCBusinessUnit.get().ID;
					wResult.Put("Name", wFMCBusinessUnit.get().Name);
				}

				break;

			case Factory:

				Optional<FMCFactory> wFMCFactory = FMCConstants.GetFMCFactoryList().values().stream()
						.filter(p -> p.Code.equalsIgnoreCase(wCode)).findFirst();
				if (wFMCFactory != null && wFMCFactory.isPresent() && wFMCFactory.get().ID > 0) {
					wResult.Result = wFMCFactory.get().ID;
					wResult.Put("Name", wFMCFactory.get().Name);
				}

				break;
			case WorkShop:

				Optional<FMCWorkShop> wFMCWorkShop = FMCConstants.GetFMCWorkShopList().values().stream()
						.filter(p -> p.Code.equalsIgnoreCase(wCode)).findFirst();
				if (wFMCWorkShop != null && wFMCWorkShop.isPresent() && wFMCWorkShop.get().ID > 0) {
					wResult.Result = wFMCWorkShop.get().ID;
					wResult.Put("Name", wFMCWorkShop.get().Name);
				}

				break;
			case Line:

				Optional<FMCLine> wFMCLine = FMCConstants.GetFMCLineList().values().stream()
						.filter(p -> p.Code.equalsIgnoreCase(wCode)).findFirst();
				if (wFMCLine != null && wFMCLine.isPresent() && wFMCLine.get().ID > 0) {
					wResult.Result = wFMCLine.get().ID;
					wResult.Put("Name", wFMCLine.get().Name);
				}

				break;
			case Station:

				Optional<FMCStation> wFMCStation = FMCConstants.GetFMCStationList().values().stream()
						.filter(p -> p.Code.equalsIgnoreCase(wCode)).findFirst();
				if (wFMCStation != null && wFMCStation.isPresent() && wFMCStation.get().ID > 0) {
					wResult.Result = wFMCStation.get().ID;
					wResult.Put("Name", wFMCStation.get().Name);
				}

				break;
			case Storehouse:
				// iPlant.Interface.Invoke.FMCService.FMCWorkShop wFMCStorehouse =
				// wFMCService.GetWorkShop(wLoginUser.getCompanyID(),
				// wLoginUser.ID, wQRCode);
				break;
			case StoreLocation:
				// iPlant.Interface.Invoke.FMCService.FMCWorkShop wFMCWorkShop =
				// wFMCService.GetWorkShop(wLoginUser.getCompanyID(),
				// wLoginUser.ID, wQRCode);
				break;
			case Device:
				Optional<DMSDeviceLedger> wDMSDeviceLedger = DMSConstants.GetDMSDeviceLedgerDic().values().stream()
						.filter(p -> p.Code.equalsIgnoreCase(wCode)).findFirst();
				if (wDMSDeviceLedger != null && wDMSDeviceLedger.isPresent() && wDMSDeviceLedger.get().ID > 0) {
					wResult.setResult((int) wDMSDeviceLedger.get().ID);
					wResult.Put("Name", wDMSDeviceLedger.get().Name);
				}
				break;
			case Spare:
				Optional<DMSSpareLedger> wDMSSpareLedger = DMSConstants.GetDMSSpareLedgerDic().values().stream()
						.filter(p -> p.Code.equalsIgnoreCase(wCode)).findFirst();
				if (wDMSSpareLedger != null && wDMSSpareLedger.isPresent() && wDMSSpareLedger.get().ID > 0) {
					wResult.setResult((int) wDMSSpareLedger.get().ID);
					wResult.Put("Name", wDMSSpareLedger.get().Name);
				}
				break;
			case WorkPlace:

				Optional<FMCWorkspace> wFMCWorkspace = FMCConstants.getFMCWorkspaceDic().values().stream()
						.filter(p -> p.Code.equalsIgnoreCase(wCode)).findFirst();
				if (wFMCWorkspace != null && wFMCWorkspace.isPresent() && wFMCWorkspace.get().ID > 0) {
					wResult.Result = wFMCWorkspace.get().ID;
					wResult.Put("Name", wFMCWorkspace.get().Name);
				}
				break;
			case Part:

				Optional<FPCPart> wFPCPart = FMCConstants.GetFPCPartList().values().stream()
						.filter(p -> p.Code.equalsIgnoreCase(wCode)).findFirst();
				if (wFPCPart != null && wFPCPart.isPresent() && wFPCPart.get().ID > 0) {
					wResult.Result = wFPCPart.get().ID;
					wResult.Put("Name", wFPCPart.get().Name);
				}
				break;
			case PartPoint:

				Optional<FPCPartPoint> wFPCPartPoint = FMCConstants.GetFPCStepList().values().stream()
						.filter(p -> p.Code.equalsIgnoreCase(wCode)).findFirst();
				if (wFPCPartPoint != null && wFPCPartPoint.isPresent() && wFPCPartPoint.get().ID > 0) {
					wResult.Result = wFPCPartPoint.get().ID;
					wResult.Put("Name", wFPCPartPoint.get().Name);
				}
				break;
			default:
				wResult.Put("Name", "");
				break;
			}
			wResult.Put("QRType", wQRTypes.getValue());
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BFCPlaceQRStruct>> BFC_GetPlaceQRTypeList() {
		ServiceResult<List<BFCPlaceQRStruct>> wResult = new ServiceResult<List<BFCPlaceQRStruct>>();

		wResult.setResult(BFCConstants.getPlaceQRTypeList());

		return wResult;
	}

	@Override
	public ServiceResult<List<FPCCustomStruct>> BFC_GetCustomStructList() {
		ServiceResult<List<FPCCustomStruct>> wResult = new ServiceResult<List<FPCCustomStruct>>();

		wResult.setResult(BFCConstants.getFPCCustomStruct());

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_GetPlaceQRTypeByCode(String wQRCode) {

		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			wResult.Result = BFCConstants.getPlaceQRTypeByCode(wQRCode);
		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_GetPlaceQRTypeByCode ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BFCMessage>> BFC_GetMessageList(BMSEmployee wLoginUser, int wResponsorID, int wType,
			int wModuleID, List<Int32> wMessageIDList, int wActive, int wSendStatus, int wShiftID,
			DateTime wStartTime, DateTime wEndTime, int wStepID) {
		ServiceResult<List<BFCMessage>> wResult = new ServiceResult<List<BFCMessage>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.setResult(BFCMessageDAO.getInstance().BFC_GetMessageList(wLoginUser, wResponsorID, wType, wModuleID,
					wMessageIDList, wActive, wSendStatus, wShiftID, wStartTime, wEndTime, wStepID, wErrorCode));

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_GetMessageList ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}

		return wResult;
	}

	@Override
	public ServiceResult<List<BFCMessage>> BFC_GetUndoMessageList(BMSEmployee wLoginUser, int wResponsorID,
			int wModuleID, int wMessageID, int wShiftID, DateTime wStartTime, DateTime wEndTime) {
		ServiceResult<List<BFCMessage>> wResult = new ServiceResult<List<BFCMessage>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

			wResult.setResult(BFCMessageDAO.getInstance().BFC_GetUndoMessageList(wLoginUser, wResponsorID, wModuleID,
					wMessageID, wShiftID, wStartTime, wEndTime, wErrorCode));
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_GetUndoMessageList ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_UpdateMessageList(BMSEmployee wLoginUser, List<BFCMessage> wBFCMessageList) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

			if (wBFCMessageList != null && wBFCMessageList.Count > 0) {
				for (BFCMessage wBFCMessage : wBFCMessageList) {
					BFCMessageDAO.getInstance().BFC_UpdateMessage(wLoginUser, wBFCMessage, wErrorCode);
					wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
				}
			}
		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_UpdateMessageList ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_SendMessageList(BMSEmployee wLoginUser, List<BFCMessage> wBFCMessageList) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			if (wLoginUser == null)
				wLoginUser = BaseDAO.SysAdmin;
			if (wBFCMessageList != null && wBFCMessageList.Count > 0) {
				for (BFCMessage wBFCMessage : wBFCMessageList) {
					BFCMessageDAO.getInstance().BFC_SendMessageList(wLoginUser, wBFCMessage, wErrorCode);
					wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
				}
			}

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_SendMessageList ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Dictionary<Int32, Int32>> BFC_GetUndoMessagCount(BMSEmployee wLoginUser, int wResponsorID,
			int wShiftID) {
		ServiceResult<Dictionary<Int32, Int32>> wResult = new ServiceResult<>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

			wResult.setResult(BFCMessageDAO.getInstance().BFC_GetUndoMessageCount(wLoginUser, wResponsorID, wShiftID,
					wErrorCode));
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_GetUndoMessagCount ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_ReceiveMessage(BMSEmployee wLoginUser, int wResponsorID, List<Long> wMsgIDList,
			List<Int32> wStepID, int wModuleID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			if (wMsgIDList != null && wMsgIDList.Count > 0) {
				BFCMessageDAO.getInstance().BFC_ReceiveMessage(wLoginUser, wResponsorID, wMsgIDList, wStepID, wModuleID,
						wErrorCode);
				wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
			}
		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_ReceiveMessage ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_HandleMessageByIDList(BMSEmployee wLoginUser, List<Long> wMsgIDList, int wStatus,
			int wSendStatus) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			if (wMsgIDList != null && wMsgIDList.Count > 0) {
				BFCMessageDAO.getInstance().BFC_HandleMessageByIDList(wLoginUser, wMsgIDList, wStatus, wSendStatus,
						wErrorCode);
				wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
			}
		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_ReceiveMessage ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_HandleMessage(BMSEmployee wLoginUser, int wResponsorID, List<Long> wMsgIDList,
			List<Int32> wStepID, int wModuleID, int wType, int wStatus) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			if (wMsgIDList != null && wMsgIDList.Count > 0) {
				BFCMessageDAO.getInstance().BFC_HandleMessage(wLoginUser, wResponsorID, wMsgIDList, wStepID, wModuleID,
						wType, wStatus, wErrorCode);
				wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
			}
		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_HandleMessage ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_ForwardMessage(BMSEmployee wLoginUser, int wResponsorID,
			List<Int32> wForwarderList, int wModuleID, long wMessageID, int wStepID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

			if (wModuleID <= 0 || wMessageID <= 0 || wStepID <= 0 || (wResponsorID <= 0 && wResponsorID != -100)
					|| wForwarderList == null || wForwarderList.Count <= 0) {
				return wResult;
			}

			BFCMessageDAO.getInstance().BFC_ForwardMessage(wLoginUser, wResponsorID, wForwarderList, wModuleID,
					(int) wMessageID, wStepID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_HandleMessage ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_HandleTaskMessage(BMSEmployee wLoginUser, int wResponsorID,
			List<Int32> wTaskIDList, int wModuleID, int wStepID, int wStatus, int wAuto) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			if (wTaskIDList != null && wTaskIDList.Count > 0) {
				BFCMessageDAO.getInstance().BFC_HandleTaskMessage(wLoginUser, wResponsorID, wTaskIDList, wModuleID,
						wStepID, wStatus, wAuto, wErrorCode);
				wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
			}
		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_HandleTaskMessage ERROR:", e.ToString());
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BFCAuditConfig>> BFC_GetAuditConfigList(BMSEmployee wLoginUser, String wName,
			int wEventModule, String wVersionNo, int wActive, int wFunctionID) {
		ServiceResult<List<BFCAuditConfig>> wResult = new ServiceResult<List<BFCAuditConfig>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.Result = BFCAuditConfigDAO.getInstance().BFC_GetAuditConfigList(wLoginUser, wName, wEventModule,
					wVersionNo, wActive, wFunctionID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_GetAuditConfigList ERROR：", e);
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BFCAuditVersion>> BFC_GetVersionList(BMSEmployee wLoginUser, int wEventModule) {
		ServiceResult<List<BFCAuditVersion>> wResult = new ServiceResult<List<BFCAuditVersion>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.Result = BFCAuditConfigDAO.getInstance().BFC_GetVersionList(wLoginUser, wEventModule, wErrorCode);

			if (wEventModule <= 0) {

				List<CFGItem> enumList = BPMEventModule.getEnumList(8000);
				boolean wIsContain = false;

				BFCAuditVersion wBFCAuditVersionTemp = null;
				for (CFGItem cfgItem : enumList) {
					wIsContain = false;
					for (BFCAuditVersion wBFCAuditVersion : wResult.Result) {
						if (wBFCAuditVersion.EventModule != cfgItem.ID)
							continue;
						wIsContain = true;
						break;
					}
					if (!wIsContain) {
						wBFCAuditVersionTemp = new BFCAuditVersion();

						wBFCAuditVersionTemp.EventModule = cfgItem.ID;
						wBFCAuditVersionTemp.EventModuleName = cfgItem.getItemText();

						wResult.Result.Add(wBFCAuditVersionTemp);
					}
				}

			}

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_GetVersionList ERROR：", e);
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_OtherSaveAuditConfigList(BMSEmployee wLoginUser, int wEventModule,
			String wVersionNo, List<BFCAuditConfig> wBFCAuditConfigList) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			BFCAuditConfigDAO.getInstance().BFC_OtherSaveAuditConfigList(wLoginUser, wEventModule, wVersionNo,
					wBFCAuditConfigList, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_OtherSaveAuditConfigList ERROR：", e);
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_UpdateAuditConfig(BMSEmployee wLoginUser, BFCAuditConfig wBFCAuditConfig) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.Result = BFCAuditConfigDAO.getInstance().BFC_UpdateAuditConfig(wLoginUser, wBFCAuditConfig,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_UpdateAuditConfig ERROR：", e);
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_ActiveAuditConfig(BMSEmployee wLoginUser, int wEventModule, String wVersionNo,
			int wActive) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			BFCAuditConfigDAO.getInstance().BFC_ActiveAuditConfig(wLoginUser, wEventModule, wVersionNo, wActive,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_ActiveAuditConfig ERROR：", e);
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BFC_DelectAuditConfig(BMSEmployee wLoginUser, BFCAuditConfig wBFCAuditConfig,
			String wVersion) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			BFCAuditConfigDAO.getInstance().BFC_DelectAuditConfig(wLoginUser, wBFCAuditConfig, wVersion, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_DelectAuditConfig ERROR：", e);
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BFCAuditAction>> BFC_GetAuditActionList(BMSEmployee wLoginUser, int wTaskID,
			int wEventModule) {
		ServiceResult<List<BFCAuditAction>> wResult = new ServiceResult<List<BFCAuditAction>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.Result = BFCAuditActionDAO.getInstance().BFC_GetAuditActionList(wLoginUser, wTaskID, wEventModule,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_GetAuditActionList ERROR：", e);
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<BFCAuditConfig> BFC_GetCurrentConfig(BMSEmployee wLoginUser, int wUserID, int wTaskID,
			int wEventModule) {
		ServiceResult<BFCAuditConfig> wResult = new ServiceResult<BFCAuditConfig>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.Result = BFCAuditActionDAO.getInstance().BFC_GetCurrentConfig(wLoginUser, wUserID, wTaskID,
					wEventModule, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_GetCurrentConfig ERROR：", e);
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BFCAuditConfig>> BFC_GetTaskConfigList(BMSEmployee wLoginUser, int wTaskID,
			int wEventModule) {

		ServiceResult<List<BFCAuditConfig>> wResult = new ServiceResult<List<BFCAuditConfig>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

			List<BFCAuditAction> wBFCAuditActionList = BFCAuditActionDAO.getInstance()
					.BFC_GetAuditActionList(wLoginUser, wTaskID, wEventModule, wErrorCode);

			if (wBFCAuditActionList == null || wBFCAuditActionList.Count < 0) {
				wResult.Result = BFCAuditConfigDAO.getInstance().BFC_GetAuditConfigList(wLoginUser, "", wEventModule,
						"", 1, 0, wErrorCode);
			} else {
				BFCAuditConfig wBFCAuditConfig = BFCAuditConfigDAO.getInstance().BFC_GetAuditConfig(wLoginUser,
						wBFCAuditActionList[0].ConfigID, wErrorCode);
				if (wBFCAuditConfig != null && wBFCAuditConfig.ID > 0
						&& StringUtils.isNotEmpty(wBFCAuditConfig.VersionNo)) {
					wResult.Result = BFCAuditConfigDAO.getInstance().BFC_GetAuditConfigList(wLoginUser, "",
							wEventModule, wBFCAuditConfig.VersionNo, -1, 0, wErrorCode);
				}

			}

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_GetAuditConfigList ERROR：", e);
			wResult.FaultCode += e.getMessage();
		}
		return wResult;

	}

	@Override
	public ServiceResult<Int32> BFC_UpdateAuditAction(BMSEmployee wLoginUser, BFCAuditAction wBFCAuditAction,
			String wTitle) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.Result = BFCAuditActionDAO.getInstance().BFC_UpdateAuditAction(wLoginUser, wBFCAuditAction, wTitle,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			logger.error("BFCServiceImpl BFC_GetCurrentConfig ERROR：", e);
			wResult.FaultCode += e.getMessage();
		}
		return wResult;
	}

	// 查询已办消息列表
	@Override
	public ServiceResult<List<BFCMessage>> BFC_GetHasDoNoticeCount(BMSEmployee wLoginUser, int wResponsorID,
			DateTime wStartTime, DateTime wEndTime) {
		ServiceResult<List<BFCMessage>> wResult = new ServiceResult<List<BFCMessage>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			List<BFCMessage> wBFCMessageList = new List<BFCMessage>();

			wBFCMessageList = BFCMessageDAO.getInstance().BFC_GetMessageList(wLoginUser, wResponsorID,
					(int)BFCMessageType.Task(), 0, 0, -1, (int)BFCMessageStatus.Finished(), -1, 0, wStartTime,
					wEndTime, wErrorCode);

			wResult.setResult(wBFCMessageList);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			// TODO: handle exception
			wResult.FaultCode += e.getMessage();
		}

		return wResult;
	}

	// 查询通知列表
	@Override
	public ServiceResult<List<BFCMessage>> BFC_GetNoticeList(BMSEmployee wLoginUser, int wResponsorID, int wStatus,
			int wUseTime, DateTime wStartTime, DateTime wEndTime, OutResult<Int32> wCount) {
		ServiceResult<List<BFCMessage>> wResult = new ServiceResult<List<BFCMessage>>();
		wCount.set(0);

		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			// 消息集合
			List<BFCMessage> wBFCMessageList = new List<BFCMessage>();

			if (wStatus <= 0) {
				if (wUseTime > 0) {
					wBFCMessageList = BFCMessageDAO.getInstance().BFC_GetMessageList(wLoginUser, wResponsorID,
							(int)BFCMessageType.Notify(), -1, -1, -1, (int)BFCMessageStatus.Sent(), -1, -1,
							wStartTime, wEndTime, wErrorCode);

					wBFCMessageList.addAll(BFCMessageDAO.getInstance().BFC_GetMessageList(wLoginUser, wResponsorID,
							(int)BFCMessageType.Notify(), -1, -1, -1, (int)BFCMessageStatus.Default(), -1, -1,
							wStartTime, wEndTime, wErrorCode));
				} else {
					wBFCMessageList = BFCMessageDAO.getInstance().BFC_GetMessageList(wLoginUser, wResponsorID,
							(int)BFCMessageType.Notify(), -1, (int)BFCMessageStatus.Sent(), -1, wErrorCode);

					wBFCMessageList.addAll(BFCMessageDAO.getInstance().BFC_GetMessageList(wLoginUser, wResponsorID,
							(int)BFCMessageType.Notify(), -1, (int)BFCMessageStatus.Default(), -1, wErrorCode));
				}

				wCount.Result = wBFCMessageList.Count;

				List<BFCMessage> wBFCMessageTemp = BFCMessageDAO.getInstance().BFC_GetMessageList(wLoginUser,
						wResponsorID, (int)BFCMessageType.Notify(), -1, -1, -1, (int)BFCMessageStatus.Read(),
						-1, -1, wStartTime, wEndTime, wErrorCode);

				wBFCMessageList.addAll(wBFCMessageTemp);

			} else if (wStatus == (int)BFCMessageStatus.Read()) {
				wBFCMessageList.addAll(BFCMessageDAO.getInstance().BFC_GetMessageList(wLoginUser, wResponsorID,
						(int)BFCMessageType.Notify(), -1, -1, -1, (int)BFCMessageStatus.Read(), -1, -1,
						wStartTime, wEndTime, wErrorCode));
			}

			if (wBFCMessageList.Count > 0) {
				wBFCMessageList.sort((o1, o2) -> o2.CreateTime.CompareTo(o1.CreateTime));
			}

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
			wResult.Result = wBFCMessageList;

		} catch (Exception e) {
			// TODO: handle exception
			wResult.FaultCode += e.getMessage();
			logger.error("BFCServiceImpl BFC_GetWaitNoticeList Error:", e.ToString());
		}

		return wResult;
	}

	@Override
	public ServiceResult<List<CGSTable>> CGS_SelectCGTableByName(BMSEmployee wLoginUser, int wUserID, String wTableName,
			String wModleName) {
		ServiceResult<List<CGSTable>> wResult = new ServiceResult<List<CGSTable>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.Result = CGSTableConfigDAO.getInstance().CGS_GetTableConfigList(wLoginUser, wUserID, wTableName,
					wModleName, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.getMessage();
			logger.error("BFCServiceImpl SelectCGTableByName Error:", e.ToString());
		}
		return wResult;
	}

	// 保存表格配置
	@Override
	public ServiceResult<Int32> CGS_SaveCGTable(BMSEmployee wLoginUser, List<CGSTable> wCGTable) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			CGSTableConfigDAO.getInstance().CGS_SaveTableConfigList(wLoginUser, wCGTable, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.getMessage();
			logger.error("BFCServiceImpl SaveCGTable Error:", e.ToString());
		}
		return wResult;

	}

	@Override
	public ServiceResult<Int32> CGS_DeleteCGTable(BMSEmployee wLoginUser, List<CGSTable> wCGTable) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			CGSTableConfigDAO.getInstance().CGS_DeleteTableConfigList(wLoginUser, wCGTable, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.getMessage();
			logger.error("BFCServiceImpl CGS_DeleteCGTable Error:", e.ToString());
		}

		return wResult;
	}

	@Override
	public ServiceResult<List<Dictionary<String, Object>>> CGS_GetDataTableList(BMSEmployee wLoginUser, int wSqlType) {
		ServiceResult<List<Dictionary<String, Object>>> wResult = new ServiceResult<List<Dictionary<String, Object>>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.Result=CGSDataBaseTableDAO.getInstance(DBEnumType.getEnumType(wSqlType)).CGS_GetDataTableList(wLoginUser,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.getMessage();
			logger.error("BFCServiceImpl CGS_DeleteCGTable Error:", e.ToString());
		}

		return wResult;
	}

	@Override
	public ServiceResult<List<Dictionary<String, Object>>> CGS_GetDataTableInfo(BMSEmployee wLoginUser, int wSqlType,
			String wDBName, String wTableName) {
		ServiceResult<List<Dictionary<String, Object>>> wResult = new ServiceResult<List<Dictionary<String, Object>>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			wResult.Result=CGSDataBaseTableDAO.getInstance(DBEnumType.getEnumType(wSqlType)).CGS_GetDataTableInfo(wLoginUser, wDBName,
					wTableName, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.getMessage();
			logger.error("BFCServiceImpl CGS_DeleteCGTable Error:", e.ToString());
		}

		return wResult;
	}

}
