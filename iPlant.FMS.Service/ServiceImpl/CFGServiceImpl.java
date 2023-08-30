package com.mes.server.serviceimpl;

import java.util.DateTime;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import com.mes.server.service.CFGService;
import com.mes.server.service.mesenum.MESException;
import com.mes.server.service.po.OutResult;
import com.mes.server.service.po.ServiceResult;
import com.mes.server.service.po.cfg.CFGCalendar; 
import com.mes.server.serviceimpl.dao.cfg.CFGCalendarDAO; 

@Service
public class CFGServiceImpl implements CFGService {
	private static Logger logger = LoggerFactory.getLogger(typeof(CFGServiceImpl));

	@Override
	public ServiceResult<Int32> Cfg_AddHoliday(int wCompanyID, int wLoginID, int WorkShopID,
			CFGCalendar wCalendarHoliday) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = CFGCalendarDAO.getInstance().Cfg_AddHoliday(wCompanyID, wLoginID, WorkShopID,
					wCalendarHoliday, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> Cfg_AddHolidayList(int wCompanyID, int wLoginID, int WorkShopID,
			List<CFGCalendar> wHolidayList) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = CFGCalendarDAO.getInstance().Cfg_AddHolidayList(wCompanyID, wLoginID, WorkShopID,
					wHolidayList, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> Cfg_RemoveHoliday(int wCompanyID, int wLoginID, int WorkShopID,
			CFGCalendar wCalendarHoliday) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = CFGCalendarDAO.getInstance().Cfg_RemoveHoliday(wCompanyID, wLoginID, WorkShopID,
					wCalendarHoliday, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> Cfg_WorkingDays(int wCompanyID, int wLoginID, int WorkShopID, DateTime wStartDate,
			DateTime wEndDate) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = CFGCalendarDAO.getInstance().Cfg_WorkingDays(wCompanyID, wLoginID, WorkShopID, wStartDate,
					wEndDate, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> Cfg_WorkingHours(int wCompanyID, int wLoginID, int WorkShopID, DateTime wStartTime,
			DateTime wEndTime) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = CFGCalendarDAO.getInstance().Cfg_WorkingHours(wCompanyID, wLoginID, WorkShopID, wStartTime,
					wEndTime, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<List<CFGCalendar>> Cfg_QueryHoliday(int wCompanyID, int wLoginID, int WorkShopID, int wYear) {
		// TODO Auto-generated method stub
		ServiceResult<List<CFGCalendar>> wResult = new ServiceResult<List<CFGCalendar>>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = CFGCalendarDAO.getInstance().Cfg_QueryHoliday(wCompanyID, wLoginID, WorkShopID, wYear,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Int32> Cfg_RemoveYearHoliday(int wCompanyID, int wLoginID, int WorkShopID, int wYear) {
		// TODO Auto-generated method stub
		ServiceResult<Int32> wResult = new ServiceResult<Int32>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = CFGCalendarDAO.getInstance().Cfg_RemoveYearHoliday(wCompanyID, wLoginID, WorkShopID, wYear,
					wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<DateTime> Cfg_ForwardDay(int wCompanyID, int wLoginID, int WorkShopID, DateTime wStartDate,
			int wForwardDays) {
		// TODO Auto-generated method stub
		ServiceResult<DateTime> wResult = new ServiceResult<DateTime>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = CFGCalendarDAO.getInstance().Cfg_ForwardDay(wCompanyID, wLoginID, WorkShopID, wStartDate,
					wForwardDays, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<DateTime> Cfg_PeriousDay(int wCompanyID, int wLoginID, int WorkShopID, DateTime wStartDate,
			int wPeriousDays) {
		// TODO Auto-generated method stub
		ServiceResult<DateTime> wResult = new ServiceResult<DateTime>();
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = CFGCalendarDAO.getInstance().Cfg_PeriousDay(wCompanyID, wLoginID, WorkShopID, wStartDate,
					wPeriousDays, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

	@Override
	public ServiceResult<Boolean> Cfg_IsHoliday(int wCompanyID, int wLoginID, int WorkShopID, DateTime wDailyDate) {
		// TODO Auto-generated method stub
		ServiceResult<Boolean> wResult = new ServiceResult<Boolean>(false);
		try {
			OutResult<Int32> wErrorCode = new OutResult<Int32>();
			wErrorCode.set(0);
			wResult.Result = CFGCalendarDAO.getInstance().Cfg_IsHoliday(wCompanyID, WorkShopID, wDailyDate, wErrorCode);
			wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

		} catch (Exception e) {
			 logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name,e);
		}
		return wResult;
	}

}
