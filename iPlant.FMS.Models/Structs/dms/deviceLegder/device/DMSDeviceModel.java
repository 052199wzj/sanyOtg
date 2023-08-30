package com.mes.server.service.po.dms.deviceLegder.device;

import java.io.Serializable;
import java.util.DateTime;

/// <summary>
/// 设备基本信息表
/// </summary>
public class DMSDeviceModel implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	/// <summary>
	/// 设备型号ID
	/// </summary>
	

	public DMSDeviceModel() {
		ModelNo = "";
		OperateTime = DateTime.Now;
		ControllerModel = "";
		ControllerName = "";
		SystemName = "";
		SystemVersion = "";
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

	public int getDeviceWorkType() {
		return WorkType;
	}

	public void setDeviceWorkType(int deviceWorkType) {
		WorkType = deviceWorkType;
	}

	public int getModelPropertyID() {
		return ModelPropertyID;
	}

	public void setModelPropertyID(int modelPropertyID) {
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

	public int getSystemPropertyID() {
		return SystemPropertyID;
	}

	public void setSystemPropertyID(int systemPropertyID) {
		SystemPropertyID = systemPropertyID;
	}

	public String getSystemName() {
		return SystemName;
	}

	public void setSystemName(String systemName) {
		SystemName = systemName;
	}

	public String getSystemVersion() {
		return SystemVersion;
	}

	public void setSystemVersion(String systemVersion) {
		SystemVersion = systemVersion;
	}

	public int getControllerPropertyID() {
		return ControllerPropertyID;
	}

	public void setControllerPropertyID(int controllerPropertyID) {
		ControllerPropertyID = controllerPropertyID;
	}

	public String getControllerName() {
		return ControllerName;
	}

	public void setControllerName(String controllerName) {
		ControllerName = controllerName;
	}

	public String getControllerModel() {
		return ControllerModel;
	}

	public void setControllerModel(String controllerModel) {
		ControllerModel = controllerModel;
	}

	public double getDeviceCost() {
		return Cost;
	}

	public void setDeviceCost(double deviceCost) {
		Cost = deviceCost;
	}

	public double getDeviceLife() {
		return Life;
	}

	public void setDeviceLife(double deviceLife) {
		Life = deviceLife;
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
