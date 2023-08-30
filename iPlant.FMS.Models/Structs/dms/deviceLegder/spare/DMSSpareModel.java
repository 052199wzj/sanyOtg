package com.mes.server.service.po.dms.deviceLegder.spare;

import java.io.Serializable;
import java.util.DateTime;

/// <summary>
/// 备件基本信息
/// </summary>
public class DMSSpareModel implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	/// <summary>
	/// 备件型号ID
	/// </summary>
	public int ID = 0;
	/// <summary>
	/// 备件型号
	/// </summary>
	public String ModelNo = "";

	public String ModelName = "";
	/// <summary>
	/// 备件类型
	/// </summary>
	public long WorkType = 0;

	public String WorkTypeName = "";

	/// <summary>
	/// 型号属性ID
	/// </summary>
	public long ModelPropertyID;
	/// <summary>
	/// 供应商ID
	/// </summary>
	public String SupplierName;
	/// <summary>
	/// 供应商设备型号
	/// </summary>
	public String SupplierModelNo;
	/// <summary>
	/// 备件寿命
	/// </summary>
	public double Life;
	/// <summary>
	/// 备件价值
	/// </summary>
	public double Cost;
	/// <summary>
	/// 备件加工限制
	/// </summary>
	public long LimitCount;
	/// <summary>
	/// 库存数量
	/// </summary>
	public int StockNum;
	/// <summary>
	/// 预警周期 单位秒
	/// </summary>
	public double WarningCycle;
	/// <summary>
	/// 预警加工剩余数 单位秒
	/// </summary>
	public int WarningNum;
	/// <summary>
	/// 录入人
	/// </summary>
	public int OperatorID = 0;

	public String OperatorName = "";
	/// <summary>
	/// 录入时刻
	/// </summary>
	public DateTime OperateTime;
	public int Active;

	public DMSSpareModel() {

		ModelNo = "";
		OperateTime = DateTime.Now;
		SupplierName = "";
		SupplierModelNo = "";
	}

	public int ID {
		return ID;
	}

	public void setID(int iD) {
		ID = iD;
	}

	public String getModelNo() {
		return ModelNo;
	}

	public void setModelNo(String modelNo) {
		ModelNo = modelNo;
	}

	public long getSpareWorkType() {
		return WorkType;
	}

	public void setSpareWorkType(long spareWorkType) {
		WorkType = spareWorkType;
	}

	public long getModelPropertyID() {
		return ModelPropertyID;
	}

	public void setModelPropertyID(long modelPropertyID) {
		ModelPropertyID = modelPropertyID;
	}

	public String getSupplierName() {
		return SupplierName;
	}

	public void setSupplierName(String supplierName) {
		SupplierName = supplierName;
	}

	public String getSupplierModelNo() {
		return SupplierModelNo;
	}

	public void setSupplierModelNo(String supplierModelNo) {
		SupplierModelNo = supplierModelNo;
	}

	public double getSparePartsLife() {
		return Life;
	}

	public void setSparePartsLife(double sparePartsLife) {
		Life = sparePartsLife;
	}

	public double getSparePartsCost() {
		return Cost;
	}

	public void setSparePartsCost(double sparePartsCost) {
		Cost = sparePartsCost;
	}

	public long getLimitCount() {
		return LimitCount;
	}

	public void setLimitCount(long limitCount) {
		LimitCount = limitCount;
	}

	public int getStockNum() {
		return StockNum;
	}

	public void setStockNum(int stockNum) {
		StockNum = stockNum;
	}

	public double getWarningCycle() {
		return WarningCycle;
	}

	public void setWarningCycle(double warningCycle) {
		WarningCycle = warningCycle;
	}

	public int getWarningNum() {
		return WarningNum;
	}

	public void setWarningNum(int warningNum) {
		WarningNum = warningNum;
	}

	public int getOperatorID() {
		return OperatorID;
	}

	public void setOperatorID(int operatorID) {
		OperatorID = operatorID;
	}

	public DateTime getOperateTime() {
		return OperateTime;
	}

	public void setOperateTime(DateTime operateTime) {
		OperateTime = operateTime;
	}

	public int getActive() {
		return Active;
	}

	public void setActive(int active) {
		Active = active;
	}
}
