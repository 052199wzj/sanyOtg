package com.mes.server.serviceimpl;

import java.util.List;
import java.util.Dictionary;
import java.util.List;
import java.util.Dictionary;
import java.util.stream.Collectors;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import com.mes.server.service.BMSService;
import com.mes.server.service.mesenum.BMSRange;
import com.mes.server.service.mesenum.MESException;
import com.mes.server.service.po.OutResult;
import com.mes.server.service.po.ServiceResult;
import com.mes.server.service.po.bms.BMSChargeGroup;
import com.mes.server.service.po.bms.BMSDepartment;
import com.mes.server.service.po.bms.BMSEmployee;
import com.mes.server.service.po.bms.BMSMenuControl;
import com.mes.server.service.po.bms.BMSPosition;
import com.mes.server.service.po.bms.BMSRole;
import com.mes.server.service.po.bms.BMSRoleItem;
import com.mes.server.service.po.bms.BMSTeamCharge;
import com.mes.server.service.po.bms.BMSTeamItem;
import com.mes.server.service.po.bms.BMSTeamManage;
import com.mes.server.service.utils.CloneTool;
import com.mes.server.service.utils.DesUtil;
import com.mes.server.service.utils.StringUtils;
import com.mes.server.serviceimpl.dao.BaseDAO;
import com.mes.server.serviceimpl.dao.bms.ADCardDAO;
import com.mes.server.serviceimpl.dao.bms.BMSChargeGroupDAO;
import com.mes.server.serviceimpl.dao.bms.BMSDepartmentDAO;
import com.mes.server.serviceimpl.dao.bms.BMSEmployeeDAO;
import com.mes.server.serviceimpl.dao.bms.BMSPositionDAO;
import com.mes.server.serviceimpl.dao.bms.BMSRoleDAO;
import com.mes.server.serviceimpl.dao.bms.BMSTeamChargeDAO;
import com.mes.server.serviceimpl.dao.bms.BMSTeamItemDAO;
import com.mes.server.serviceimpl.dao.bms.BMSTeamManageDAO;
import com.mes.server.serviceimpl.utils.bfc.BFCConstants;

@Service
public class BMSServiceImpl implements BMSService {

	public BMSServiceImpl() {
		// TODO Auto-generated constructor stub
	}

	private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(typeof(BMSServiceImpl));

	private static BMSService Instance = null;

	public static BMSService getInstance() {
		if (Instance == null)
			Instance = new BMSServiceImpl();

		return Instance;
	}

	@Override
	public ServiceResult<BMSEmployee> BMS_LoginEmployee(String wLoginName, String wPassword, long wMAC) {
		ServiceResult<BMSEmployee> wResult = new ServiceResult<BMSEmployee>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSEmployeeDAO.getInstance().BMS_LoginEmployee(wLoginName, wPassword, wMAC, wErrorCode);

			// 更新在线时间
			BMSEmployeeDAO.getInstance().BMS_UpdateOnline(wResult.Result, wErrorCode);

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<BMSEmployee> BMS_LoginEmployeeByUserName(String wUserName, String wPassword) {
		ServiceResult<BMSEmployee> wResult = new ServiceResult<BMSEmployee>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSEmployeeDAO.getInstance().BMS_LoginEmployeeByUserName(wUserName, wPassword, wErrorCode);

			if (wResult.Result == null || wResult.Result.ID <= 0) {
				//调用ERP登录接口
			}
			if (wResult.Result != null && wResult.Result.ID > 0) {
				// 更新在线时间
				BMSEmployeeDAO.getInstance().BMS_UpdateOnline(wResult.Result, wErrorCode);
			}

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<BMSEmployee> BMS_LoginEmployeeByToken(String wLoginName, String wToken) {
		ServiceResult<BMSEmployee> wResult = new ServiceResult<BMSEmployee>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSEmployeeDAO.getInstance().BMS_LoginEmployeeByToken(wLoginName, wToken, wErrorCode);
			// wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
			BMSEmployeeDAO.getInstance().BMS_UpdateOnline(wResult.Result, wErrorCode);
		} catch (Exception e) {

			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<BMSEmployee> BMS_LoginEmployeeByOne(String wLoginID) {
		ServiceResult<BMSEmployee> wResult = new ServiceResult<BMSEmployee>(new BMSEmployee());
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			for (BMSEmployee wBMSEmployeeTemp : BFCConstants.GetBMSEmployeeList().values()) {
				if (!wBMSEmployeeTemp.LoginID.equals(wLoginID))
					continue;
				wResult.Result = CloneTool.Clone(wBMSEmployeeTemp, typeof(BMSEmployee));
				wResult.Result.LoginName = DesUtil.encrypt(wResult.Result.LoginName, BaseDAO.appSecret);
				break;
			}

			BMSEmployeeDAO.getInstance().BMS_UpdateOnline(wResult.Result, wErrorCode);
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSEmployee>> BMS_QueryEmployeeList(BMSEmployee wLoginUser, int wDepartmentID,
			int wPosition, int wDepartmentType, int wPositionType, int wActive) {
		ServiceResult<List<BMSEmployee>> wResult = new ServiceResult<List<BMSEmployee>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSEmployeeDAO.getInstance().BMS_QueryEmployeeList(wLoginUser, wDepartmentID, wPosition,
					wDepartmentType, wPositionType, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			wResult.FaultCode += e.ToString();
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSEmployee>> BMS_QueryEmployeeList(BMSEmployee wLoginUser, List<Int32> wIDList) {
		ServiceResult<List<BMSEmployee>> wResult = new ServiceResult<List<BMSEmployee>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSEmployeeDAO.getInstance().BMS_QueryEmployeeList(wLoginUser, wIDList, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			wResult.FaultCode += e.ToString();
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Dictionary<Int32, BMSEmployee>> BMS_QueryEmployeeDic() {
		ServiceResult<Dictionary<Int32, BMSEmployee>> wResult = new ServiceResult<Dictionary<Int32, BMSEmployee>>(
				new Dictionary<Int32, BMSEmployee>());
		try {

			wResult.Result = BFCConstants.GetBMSEmployeeList();

		} catch (Exception e) {
			wResult.FaultCode += e.ToString();
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<BMSEmployee> BMS_QueryEmployeeByID(BMSEmployee wLoginUser, int wID) {
		ServiceResult<BMSEmployee> wResult = new ServiceResult<BMSEmployee>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSEmployeeDAO.getInstance().BMS_QueryEmployeeByID(wLoginUser, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			wResult.FaultCode += e.ToString();
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_AddEmployee(BMSEmployee wLoginUser, BMSEmployee wEmployee) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);

			BMSEmployeeDAO.getInstance().BMS_AddEmployee(wLoginUser, wEmployee, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			wResult.FaultCode += e.ToString();
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_AutoSetEmployeeSuperior(BMSEmployee wLoginUser, List<BMSEmployee> wEmployeeList,
			List<Int32> wDutyOrders) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();

		try {

			if (wDutyOrders == null || wEmployeeList == null || wEmployeeList.Count <= 0 || wDutyOrders.Count <= 0)
				return wResult;

			wErrorCode.set(0);
			BFCConstants.RestPositionTime(-1);
			BFCConstants.RestDepartmentTime(-1);
			BFCConstants.RestEmployeeTime(-1);
			int wIndex = -1;
			int wParentDuty = 0;
			int wDepartmentID = 0;
			List<BMSPosition> wBMSPositionAllList = BMSPositionDAO.getInstance().BMS_QueryPositionList(wLoginUser, 1,
					wErrorCode);

			Dictionary<Int32, BMSPosition> wBMSPositionDic = wBMSPositionAllList.stream()
					.collect(Collectors.toMap(p -> p.ID, p -> p));

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

			for (BMSEmployee wEmployee : wEmployeeList) {

				wEmployee.DutyID = wBMSPositionDic.ContainsKey(wEmployee.Position)
						? wBMSPositionDic.get(wEmployee.Position).DutyID
						: wEmployee.DutyID;

				wIndex = wDutyOrders.indexOf(wEmployee.DutyID);

				wParentDuty = wDutyOrders[0];

				if (wIndex == 0) {
					wDepartmentID = BFCConstants.GetBMSDepartment(wEmployee.DepartmentID).getParentID();
				} else {
					wDepartmentID = wEmployee.DepartmentID;
				}
				wErrorCode.set(0);
				this.BMS_GetSuperior(wLoginUser, wEmployee, wDepartmentID, wParentDuty, wDutyOrders,
						wBMSPositionAllList, wErrorCode);
				wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
				if (wEmployee.SuperiorID > 0 && wEmployee.ID > 0) {
					BMSEmployeeDAO.getInstance().BMS_SaveEmployeeSuperiorID(wLoginUser, wEmployee, wErrorCode);
					wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
				}
			}

		} catch (Exception e) {
			wResult.FaultCode += e.ToString();
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}

		return wResult;
	}

	private void BMS_GetSuperiorNext(BMSEmployee wLoginUser, BMSEmployee wEmployee, int wDepartmentID, int wParentDuty,
			List<Int32> wDutyOrders, List<BMSPosition> wBMSPositionAllList, OutResult<Int32> wErrorCode) {

		if (wErrorCode.Result != 0)
			return;

		int wIndex = wDutyOrders.indexOf(wParentDuty);
		if (wIndex < 0) {
			return;
		}

		if (wIndex == 0) {
			if (wDepartmentID != wEmployee.DepartmentID) {
				if (wDutyOrders.Count <= wIndex + 1)
					return;
				wParentDuty = wDutyOrders.get(wIndex + 1);
			} else {
				if (wDutyOrders.Count <= wIndex + 1 || wEmployee.DutyID == wDutyOrders.get(wIndex + 1)) {
					wParentDuty = wDutyOrders[0];
					wDepartmentID = BFCConstants.GetBMSDepartment(wDepartmentID).getParentID();

				} else {
					wParentDuty = wDutyOrders.get(wIndex + 1);
				}
			}

		} else {
			if (wDepartmentID != wEmployee.DepartmentID) {
				if (wDutyOrders.Count <= wIndex + 1)
					return;
				wParentDuty = wDutyOrders.get(wIndex + 1);
			} else {

				if (wDutyOrders.Count <= wIndex + 1 || wEmployee.DutyID == wDutyOrders.get(wIndex + 1)) {
					wParentDuty = wDutyOrders[0];
					wDepartmentID = BFCConstants.GetBMSDepartment(wDepartmentID).getParentID();
				} else {
					wParentDuty = wDutyOrders.get(wIndex + 1);
				}
			}
		}

		this.BMS_GetSuperior(wLoginUser, wEmployee, wDepartmentID, wParentDuty, wDutyOrders, wBMSPositionAllList,
				wErrorCode);
	}

	private void BMS_GetSuperior(BMSEmployee wLoginUser, BMSEmployee wEmployee, int wDepartmentID, int wParentDuty,
			List<Int32> wDutyOrders, List<BMSPosition> wBMSPositionAllList, OutResult<Int32> wErrorCode) {

		if (wErrorCode.Result != 0)
			return;

		if (wLoginUser == null || wEmployee == null || wDepartmentID <= 0 || wParentDuty <= 0 || wDutyOrders == null
				|| wDutyOrders.Count <= 0 || wBMSPositionAllList == null || wBMSPositionAllList.Count <= 0)
			return;

		final int wParentDutyID = wParentDuty;
		final int wDeptID = wDepartmentID;
		List<Int32> wBMSPositionTemp = wBMSPositionAllList.stream()
				.filter(p -> p.DutyID == wParentDutyID && p.DepartmentID == wDeptID).map(p -> p.ID)
				.collect(Collectors.toList());

		if (wBMSPositionTemp == null || wBMSPositionTemp.Count <= 0) {
			BMS_GetSuperiorNext(wLoginUser, wEmployee, wDepartmentID, wParentDuty, wDutyOrders, wBMSPositionAllList,
					wErrorCode);
			return;
		}
		List<BMSEmployee> wBMSEmployeeList = BFCConstants.GetBMSEmployeeList().values().stream()
				.filter(p -> p.DepartmentID == wDeptID && p.Active == 1 && wBMSPositionTemp.contains(p.Position))
				.collect(Collectors.toList());

		if (wBMSEmployeeList == null || wBMSEmployeeList.Count <= 0) {
			BMS_GetSuperiorNext(wLoginUser, wEmployee, wDepartmentID, wParentDuty, wDutyOrders, wBMSPositionAllList,
					wErrorCode);
			return;
		}

		wEmployee.SuperiorID = wBMSEmployeeList[0].ID;

	}

	@Override
	public ServiceResult<Int32> BMS_GetEmployeeSuperior(BMSEmployee wLoginUser, BMSEmployee wEmployee,
			List<Int32> wDutyOrders, List<BMSPosition> wBMSPositionAllList,
			Dictionary<Int32, BMSPosition> wBMSPositionDic) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

		try {

			if (wDutyOrders == null || wDutyOrders.Count <= 0)
				return wResult;

			int wDepartmentID = wEmployee.DepartmentID;

			if (wBMSPositionAllList == null || wBMSPositionAllList.Count <= 0) {
				wBMSPositionAllList = BMSPositionDAO.getInstance().BMS_QueryPositionList(wLoginUser, 1, wErrorCode);
				wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
			}
			if (wBMSPositionDic == null || wBMSPositionDic.Count <= 0) {
				wBMSPositionDic = wBMSPositionAllList.stream().collect(Collectors.toMap(p -> p.ID, p -> p));
			}

			wEmployee.DutyID = wBMSPositionDic.ContainsKey(wEmployee.Position)
					? wBMSPositionDic.get(wEmployee.Position).DutyID
					: wEmployee.DutyID;

			int wIndex = wDutyOrders.indexOf(wEmployee.DutyID);

			int wParentDuty = wDutyOrders[0];

			if (wIndex == 0) {
				wDepartmentID = BFCConstants.GetBMSDepartment(wEmployee.DepartmentID).getParentID();
			}
			wErrorCode.set(0);
			this.BMS_GetSuperior(wLoginUser, wEmployee, wDepartmentID, wParentDuty, wDutyOrders, wBMSPositionAllList,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			wResult.FaultCode += e.ToString();
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_SaveEmployee(BMSEmployee wLoginUser, BMSEmployee wEmployee) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSEmployeeDAO.getInstance().BMS_SaveEmployee(wLoginUser, wEmployee, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_SavePassword(BMSEmployee wLoginUser, BMSEmployee wEmployee) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSEmployeeDAO.getInstance().BMS_SavePassword(wLoginUser, wEmployee, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_DisableEmployee(BMSEmployee wLoginUser, BMSEmployee wEmployee) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSEmployeeDAO.getInstance().BMS_DisableEmployee(wLoginUser, wEmployee, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_DeleteEmployee(BMSEmployee wLoginUser, BMSEmployee wEmployee) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSEmployeeDAO.getInstance().BMS_DeleteEmployee(wLoginUser, wEmployee, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_ActiveEmployee(BMSEmployee wLoginUser, BMSEmployee wEmployee) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSEmployeeDAO.getInstance().BMS_ActiveEmployee(wLoginUser, wEmployee, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_ResetPassword(BMSEmployee wLoginUser, BMSEmployee wEmployee) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSEmployeeDAO.getInstance().BMS_ResetPassword(wLoginUser, wEmployee, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}

		return wResult;
	}

	@Override
	public ServiceResult<List<BMSMenuControl>> BMS_QueryMenuControlList(BMSEmployee wLoginUser) {
		ServiceResult<List<BMSMenuControl>> wResult = new ServiceResult<List<BMSMenuControl>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSRoleDAO.getInstance().BMS_QueryMenuControlList(wLoginUser, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<BMSRole> BMS_AddRole(BMSEmployee wLoginUser, BMSRole wRole) {
		ServiceResult<BMSRole> wResult = new ServiceResult<BMSRole>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSRoleDAO.getInstance().BMS_AddRole(wLoginUser, wRole, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSRole>> BMS_GetRoleList(BMSEmployee wLoginUser, int wActive) {
		ServiceResult<List<BMSRole>> wResult = new ServiceResult<List<BMSRole>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSRoleDAO.getInstance().BMS_GetRoleList(wLoginUser, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_RemoveRole(BMSEmployee wLoginUser, int wID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSRoleDAO.getInstance().BMS_RemoveRole(wLoginUser, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_SaveRole(BMSEmployee wLoginUser, BMSRole wRole) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSRoleDAO.getInstance().BMS_SaveRole(wLoginUser, wRole, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_SaveRoleRangeList(BMSEmployee wLoginUser, int wRoleID,
			List<BMSRoleItem> wFunctionList) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSRoleDAO.getInstance().BMS_SaveRoleRangeList(wLoginUser, wRoleID, wFunctionList,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_SaveRoleFunctionList(BMSEmployee wLoginUser, int wRoleID,
			List<BMSRoleItem> wFunctionList) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_SaveRoleFunctionList(wLoginUser, wRoleID, wFunctionList,
				wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_SaveRoleUserList(BMSEmployee wLoginUser, int wRoleID,
			List<BMSRoleItem> wUserList) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_SaveRoleUserList(wLoginUser, wRoleID, wUserList, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<List<BMSRoleItem>> BMS_QueryRangeListByRoleID(BMSEmployee wLoginUser, int wRoleID) {
		ServiceResult<List<BMSRoleItem>> wResult = new ServiceResult<List<BMSRoleItem>>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_QueryRangeListByRoleID(wLoginUser, wRoleID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<List<BMSRoleItem>> BMS_QueryRangeListByOperatorID(BMSEmployee wLoginUser, int wOperatorID,
			int wFunctionID) {
		ServiceResult<List<BMSRoleItem>> wResult = new ServiceResult<List<BMSRoleItem>>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_QueryRangeList(wLoginUser, wOperatorID, wFunctionID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<List<BMSRoleItem>> BMS_QueryFunctionListByRoleID(BMSEmployee wLoginUser, int wRoleID) {
		ServiceResult<List<BMSRoleItem>> wResult = new ServiceResult<List<BMSRoleItem>>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_QueryFunctionListByRoleID(wLoginUser, wRoleID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<List<BMSRoleItem>> BMS_GetFunctionListByLoginID(BMSEmployee wLoginUser, int wUserID) {
		ServiceResult<List<BMSRoleItem>> wResult = new ServiceResult<List<BMSRoleItem>>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_QueryFunctionListByLoginID(wLoginUser, wUserID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<List<BMSRoleItem>> BMS_QueryUserListByFunctionID(BMSEmployee wLoginUser, int wFunctionID) {
		ServiceResult<List<BMSRoleItem>> wResult = new ServiceResult<List<BMSRoleItem>>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_QueryUserListByFunctionID(wLoginUser, wFunctionID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<List<BMSRoleItem>> BMS_QueryUserListByRoleID(BMSEmployee wLoginUser, int wRoleID) {
		ServiceResult<List<BMSRoleItem>> wResult = new ServiceResult<List<BMSRoleItem>>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_QueryUserListByRoleID(wLoginUser, wRoleID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_DisableRole(BMSEmployee wLoginUser, int wID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_DisableRole(wLoginUser, wID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_ActiveRole(BMSEmployee wLoginUser, int wID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_ActiveRole(wLoginUser, wID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Boolean> BMS_CheckPowerByLoginID(int wCompanyID, int wLoginID, int wAuthorityID, int wRangeID,
			int wRangeType) {
		ServiceResult<Boolean> wResult = new ServiceResult<Boolean>(false);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSRoleDAO.getInstance().BMS_CheckPowerByLoginID(wCompanyID, wLoginID, wAuthorityID,
					BMSRange.getEnumType(wRangeType), wRangeID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}

		return wResult;
	}

	@Override
	public ServiceResult<BMSRoleItem> BMS_UpdateFunctionNodeTree(BMSEmployee wLoginUser, BMSRoleItem wBMSRoleItem) {
		ServiceResult<BMSRoleItem> wResult = new ServiceResult<BMSRoleItem>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			BMSRoleDAO.getInstance().BMS_UpdateFunctionNodeTree(wLoginUser, wBMSRoleItem, wErrorCode);
			wResult.Result = wBMSRoleItem;
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_ActiveFunctionNodeTree(BMSEmployee wLoginUser, List<Int32> wFunctionIDList,
			int wActive) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			BMSRoleDAO.getInstance().BMS_ActiveFunctionNodeTree(wLoginUser, wFunctionIDList, wActive, wErrorCode);

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_DeleteFunctionNodeTree(BMSEmployee wLoginUser, List<Int32> wFunctionIDList) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
			BMSRoleDAO.getInstance().BMS_DeleteFunctionNodeTree(wLoginUser, wFunctionIDList, wErrorCode);

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSRoleItem>> BMS_GetFunctionNodeTree(BMSEmployee wLoginUser, int wActive) {
		ServiceResult<List<BMSRoleItem>> wResult = new ServiceResult<List<BMSRoleItem>>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSRoleDAO.getInstance().BMS_GetFunctionNodeTree(wLoginUser, 0, wActive, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_AddPosition(BMSEmployee wLoginUser, BMSPosition wPosition) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSPositionDAO.getInstance().BMS_AddPosition(wLoginUser, wPosition, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_SavePosition(BMSEmployee wLoginUser, BMSPosition wPosition) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSPositionDAO.getInstance().BMS_SavePosition(wLoginUser, wPosition, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_ActivePositionByID(BMSEmployee wLoginUser, int wID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSPositionDAO.getInstance().BMS_ActivePositionByID(wLoginUser, wID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_DisablePositionByID(BMSEmployee wLoginUser, int wID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		wResult.Result = BMSPositionDAO.getInstance().BMS_DisablePositionByID(wLoginUser, wID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_DeletePositionByID(BMSEmployee wLoginUser, int wID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();

		OutResult<Int32> wErrorCode = new OutResult<Int32>();
		wErrorCode.set(0);
		BMSPositionDAO.getInstance().BMS_DeletePositionByID(wLoginUser, wID, wErrorCode);
		wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		return wResult;
	}

	@Override
	public ServiceResult<List<BMSPosition>> BMS_QueryPositionList(BMSEmployee wLoginUser, int wActive) {
		ServiceResult<List<BMSPosition>> wResult = new ServiceResult<List<BMSPosition>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSPositionDAO.getInstance().BMS_QueryPositionList(wLoginUser, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<BMSPosition> BMS_QueryPositionByID(BMSEmployee wLoginUser, int wID, String wCode) {
		ServiceResult<BMSPosition> wResult = new ServiceResult<BMSPosition>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSPositionDAO.getInstance().BMS_QueryPositionByID(wLoginUser, wID, wCode, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_AddDepartment(BMSEmployee wLoginUser, BMSDepartment wDepartment) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSDepartmentDAO.getInstance().BMS_AddDepartment(wLoginUser, wDepartment, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_SaveDepartment(BMSEmployee wLoginUser, BMSDepartment wDepartment) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSDepartmentDAO.getInstance().BMS_SaveDepartment(wLoginUser, wDepartment, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_ActiveDepartmentByID(BMSEmployee wLoginUser, int wID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSDepartmentDAO.getInstance().BMS_ActiveDepartmentByID(wLoginUser, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_DisableDepartmentByID(BMSEmployee wLoginUser, int wID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSDepartmentDAO.getInstance().BMS_DisableDepartmentByID(wLoginUser, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_DeleteDepartmentByID(BMSEmployee wLoginUser, int wID) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSDepartmentDAO.getInstance().BMS_DeleteDepartmentByID(wLoginUser, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSDepartment>> BMS_QueryDepartmentList(BMSEmployee wLoginUser, int wActive) {
		ServiceResult<List<BMSDepartment>> wResult = new ServiceResult<List<BMSDepartment>>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSDepartmentDAO.getInstance().BMS_QueryDepartmentList(wLoginUser, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<BMSDepartment> BMS_QueryDepartmentByID(BMSEmployee wLoginUser, int wID, String wCode) {
		ServiceResult<BMSDepartment> wResult = new ServiceResult<BMSDepartment>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSDepartmentDAO.getInstance().BMS_QueryDepartmentByID(wLoginUser, wID, wCode, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}

		return wResult;
	}

	@Override
	public ServiceResult<String> ADC_GetLoginIDByCardID(String wCardID) {
		ServiceResult<String> wResult = new ServiceResult<String>("");
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = ADCardDAO.getInstance().ADC_GetLoginIDByCardID(wCardID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSTeamManage>> BMS_GetTeamManageList(BMSEmployee wLoginUser, int wWorkShopID,
			int wDepartmentID, int wModuleID, int wLeaderID, int wActive, int wHasItem) {
		ServiceResult<List<BMSTeamManage>> wResult = new ServiceResult<List<BMSTeamManage>>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSTeamManageDAO.getInstance().BMS_GetTeamManageList(wLoginUser, wWorkShopID,
					wDepartmentID, wModuleID, wLeaderID, wActive, wErrorCode);

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

			if (wHasItem == 1 && wResult.Result != null && wResult.Result.Count > 0) {

				List<BMSTeamItem> wTeamItemList = BMSTeamItemDAO.getInstance().BMS_GetTeamItemList(wLoginUser, -1,
						wWorkShopID, wDepartmentID, wModuleID, -1, -1, -1, -1, wActive, wErrorCode);
				wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

				if (wTeamItemList != null && wTeamItemList.Count > 0) {

					Dictionary<Int32, List<BMSTeamItem>> wTeamItemMap = wTeamItemList.stream()
							.collect(Collectors.groupingBy(p -> p.TeamID));

					for (BMSTeamManage wBMSTeamManage : wResult.Result) {
						if (wTeamItemMap.ContainsKey(wBMSTeamManage.ID))
							wBMSTeamManage.TeamItemList = wTeamItemMap.get(wBMSTeamManage.ID);
					}

				}

			}

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSTeamManage>> BMS_GetTeamManageList(BMSEmployee wLoginUser, int wLeaderID) {
		ServiceResult<List<BMSTeamManage>> wResult = new ServiceResult<List<BMSTeamManage>>();
		try {
			wResult.Result = new List<BMSTeamManage>();
			OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

			List<BMSTeamManage> wBMSTeamManageList = BMSTeamManageDAO.getInstance().BMS_GetTeamManageList(wLoginUser,
					-1, -1, -1, wLeaderID, 1, wErrorCode);

			for (BMSTeamManage wBMSTeamManage : wBMSTeamManageList) {
				if (wBMSTeamManage.LeaderID == null || wBMSTeamManage.LeaderID.Count <= 0)
					continue;

				if (!wBMSTeamManage.LeaderID.contains(wLeaderID))
					continue;

				wBMSTeamManage.ChargeGroupList = BMSChargeGroupDAO.getInstance().BMS_GetChargeGroupList(wLoginUser,
						wBMSTeamManage.ID, 1, wErrorCode);
				wBMSTeamManage.TeamChargeList = BMSTeamChargeDAO.getInstance().BMS_GetTeamChargeList(wLoginUser,
						wBMSTeamManage.ID, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 1, wErrorCode);
				wResult.Result.Add(wBMSTeamManage);
			}

			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSTeamManage>> BMS_GetTeamManageList(BMSEmployee wLoginUser, List<Int32> wIDList) {
		ServiceResult<List<BMSTeamManage>> wResult = new ServiceResult<List<BMSTeamManage>>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSTeamManageDAO.getInstance().BMS_GetTeamManageList(wLoginUser, wIDList, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<BMSTeamManage> BMS_GetTeamManage(BMSEmployee wLoginUser, int wID) {
		ServiceResult<BMSTeamManage> wResult = new ServiceResult<BMSTeamManage>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSTeamManageDAO.getInstance().BMS_GetTeamManage(wLoginUser, wID, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_UpdateTeamManage(BMSEmployee wLoginUser, BMSTeamManage wBMSTeamManage) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSTeamManageDAO.getInstance().BMS_UpdateTeamManage(wLoginUser, wBMSTeamManage, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_ActiveTeamManage(BMSEmployee wLoginUser, int wActive,
			BMSTeamManage wBMSTeamManage) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSTeamManageDAO.getInstance().BMS_ActiveTeamManage(wLoginUser, wActive, wBMSTeamManage, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_DeleteTeamManage(BMSEmployee wLoginUser, BMSTeamManage wBMSTeamManage) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSTeamManageDAO.getInstance().BMS_DeleteTeamManage(wLoginUser, wBMSTeamManage, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSTeamCharge>> BMS_GetTeamChargeList(BMSEmployee wLoginUser, int wTeamID, int wGroupID,
			int wWorkShopID, int wDepartmentID, int wModuleID, int wLineID, int wPartID, int wPartPointID,
			int wStationID, int wMateUserID, int wShiftIndex, int wActive) {
		ServiceResult<List<BMSTeamCharge>> wResult = new ServiceResult<List<BMSTeamCharge>>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			List<BMSTeamCharge> wBMSTeamChargeList = BMSTeamChargeDAO.getInstance().BMS_GetTeamChargeList(wLoginUser,
					wTeamID, wGroupID, wWorkShopID, wDepartmentID, wModuleID, wLineID, wPartID, wPartPointID,
					wStationID, wMateUserID, wShiftIndex, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

			if (wErrorCode.Result != 0)
				return wResult;

			if (wGroupID <= 0 || wTeamID <= 0) {
				wResult.Result = wBMSTeamChargeList;
				return wResult;
			}

			BMSChargeGroup wBMSChargeGroup = BMSChargeGroupDAO.getInstance().BMS_GetChargeGroup(wLoginUser, wGroupID,
					wErrorCode);

			if (wErrorCode.Result != 0) {
				wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
				return wResult;
			}

			if (wBMSChargeGroup == null || wBMSChargeGroup.ID <= 0 || wBMSChargeGroup.ID != wGroupID) {
				wResult.FaultCode += StringUtils.Format("GroupID:{0}  不存在", wGroupID);
				return wResult;
			}

			List<BMSTeamItem> wBMSTeamItemList = BMSTeamItemDAO.getInstance().BMS_GetTeamItemList(wLoginUser, wTeamID,
					wWorkShopID, wDepartmentID, wModuleID, wLineID, wPartID, wPartPointID, wStationID, wActive,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

			if (wErrorCode.Result != 0) {
				wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
				return wResult;
			}

			if (wBMSTeamChargeList.Count == wBMSTeamItemList.Count) {
				wResult.Result = wBMSTeamChargeList;
			} else if (wBMSTeamItemList.Count > 0) {

				Dictionary<Int32, BMSTeamCharge> wBMSTeamChargeMap = wBMSTeamChargeList.stream()
						.collect(Collectors.toMap(p -> p.ItemID, p -> p, (o1, o2) -> o2.Active == 1 ? o2 : o1));

				BMSTeamCharge wBMSTeamCharge = null;
				for (BMSTeamItem bmsTeamItem : wBMSTeamItemList) {
					if (wBMSTeamChargeMap.ContainsKey(bmsTeamItem.ID))
						continue;

					wBMSTeamCharge = new BMSTeamCharge(bmsTeamItem);
					wBMSTeamCharge.GroupID = wBMSChargeGroup.ID;
					wBMSTeamCharge.GroupName = wBMSChargeGroup.Name;
					wBMSTeamCharge.ShiftIndex = wBMSChargeGroup.ShiftIndex;
					wBMSTeamCharge.ShiftName = wBMSChargeGroup.ShiftName;
					wBMSTeamCharge.CreatorID = wLoginUser.ID;
					wBMSTeamCharge.Creator = wLoginUser.getName();
					wBMSTeamCharge.EditorID = wLoginUser.ID;
					wBMSTeamCharge.Editor = wLoginUser.getName();
					wBMSTeamChargeMap.Add(wBMSTeamCharge.ItemID, wBMSTeamCharge);

					BMSTeamChargeDAO.getInstance().BMS_UpdateTeamCharge(wLoginUser, wBMSTeamCharge, wErrorCode);

					if (wErrorCode.Result != 0) {
						wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
						return wResult;
					}
				}
				wResult.Result = new List<BMSTeamCharge>(wBMSTeamChargeMap.values());
			}
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_UpdateTeamCharge(BMSEmployee wLoginUser, BMSTeamCharge wBMSTeamCharge) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSTeamChargeDAO.getInstance().BMS_UpdateTeamCharge(wLoginUser, wBMSTeamCharge, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSTeamItem>> BMS_GetTeamItemList(BMSEmployee wLoginUser, int wTeamID, int wWorkShopID,
			int wDepartmentID, int wModuleID, int wLineID, int wPartID, int wPartPointID, int wStationID, int wActive) {
		ServiceResult<List<BMSTeamItem>> wResult = new ServiceResult<List<BMSTeamItem>>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSTeamItemDAO.getInstance().BMS_GetTeamItemList(wLoginUser, wTeamID, wWorkShopID,
					wDepartmentID, wModuleID, wLineID, wPartID, wPartPointID, wStationID, wActive, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_UpdateTeamItem(BMSEmployee wLoginUser, BMSTeamItem wBMSTeamItem) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSTeamItemDAO.getInstance().BMS_UpdateTeamItem(wLoginUser, wBMSTeamItem, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_ActiveTeamItem(BMSEmployee wLoginUser, int wActive, BMSTeamItem wBMSTeamItem) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSTeamItemDAO.getInstance().BMS_ActiveTeamItem(wLoginUser, wActive, wBMSTeamItem, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_DeleteTeamItem(BMSEmployee wLoginUser, BMSTeamItem wBMSTeamItem) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSTeamItemDAO.getInstance().BMS_DeleteTeamItem(wLoginUser, wBMSTeamItem, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<BMSChargeGroup>> BMS_GetChargeGroupList(BMSEmployee wLoginUser, int wTeamID,
			int wActive) {
		ServiceResult<List<BMSChargeGroup>> wResult = new ServiceResult<List<BMSChargeGroup>>();
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = BMSChargeGroupDAO.getInstance().BMS_GetChargeGroupList(wLoginUser, wTeamID, wActive,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> BMS_UpdateChargeGroup(BMSEmployee wLoginUser, BMSChargeGroup wChargeGroup) {
		ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
		try {

			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			BMSChargeGroupDAO.getInstance().BMS_UpdateChargeGroup(wLoginUser, wChargeGroup, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();
		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

}
