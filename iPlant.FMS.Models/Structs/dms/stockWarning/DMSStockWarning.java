package com.mes.server.service.po.dms.stockWarning;

import java.io.Serializable;
import java.util.List;
import java.util.DateTime;
import java.util.List;

/// <summary>
/// 设备库存预警  不管
/// </summary>
public class DMSStockWarning implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	public long ID;
	/// <summary>
	/// 设备型号ID
	/// </summary>
	public int ModelID;
	/// <summary>
	/// 设备型号编码
	/// </summary>
	public String ModelNo;
	/// <summary>
	/// 设备加工类型
	/// </summary>
	public int WorkType;
	/// <summary>
	/// 供应商ID
	/// </summary>
	public int SupplierID;
	/// <summary>
	/// 供应商名称
	/// </summary>
	public String SupplierName;
	/// <summary>
	/// 供应商编号
	/// </summary>
	public String SupplierModelNo;
	/// <summary>
	/// 需求数量
	/// </summary>
	public int NeedNum;
	/// <summary>
	/// 库存数量
	/// </summary>
	public int StockNum;
	/// <summary>
	/// 需替换设备列表
	/// </summary>
	public List<Long> ReplaceOptions;
	/// <summary>
	/// 预警周期时长
	/// </summary>
	public Double WarningInterval;
	/// <summary>
	/// 预警时刻
	/// </summary>
	public DateTime WarningTime;
	/// <summary>
	/// 最短替换时刻
	/// </summary>
	public DateTime ReplaceMinTime;
	/// <summary>
	/// 设备或备件：1为设备，2为备件
	/// </summary>
	public int DSType;
	/// <summary>
	/// 状态 激活的是正在预警
	/// </summary>
	public int Active;

	public DMSStockWarning() {
		ModelID = 0;
		NeedNum = 0;
		StockNum = 0;
		ReplaceOptions = new List<Long>();
		WarningInterval = (double) 0;
		ReplaceMinTime = DateTime.Now;
		DSType = 0;
	}

	public long ID {
		return ID;
	}

	public void setID(long iD) {
		ID = iD;
	}

	public int getModelID() {
		return ModelID;
	}

	public void setModelID(int modelID) {
		ModelID = modelID;
	}

	public String getModelNo() {
		return ModelNo;
	}

	public void setModelNo(String modelNo) {
		ModelNo = modelNo;
	}

	public int getWorkType() {
		return WorkType;
	}

	public void setWorkType(int workType) {
		WorkType = workType;
	}

	public int getSupplierID() {
		return SupplierID;
	}

	public void setSupplierID(int supplierID) {
		SupplierID = supplierID;
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

	public int getNeedNum() {
		return NeedNum;
	}

	public void setNeedNum(int needNum) {
		NeedNum = needNum;
	}

	public int getStockNum() {
		return StockNum;
	}

	public void setStockNum(int stockNum) {
		StockNum = stockNum;
	}

	public List<Long> getReplaceOptions() {
		return ReplaceOptions;
	}

	public void setReplaceOptions(List<Long> replaceOptions) {
		ReplaceOptions = replaceOptions;
	}

	public Double getWarningInterval() {
		return WarningInterval;
	}

	public void setWarningInterval(Double warningInterval) {
		WarningInterval = warningInterval;
	}

	public DateTime getWarningTime() {
		return WarningTime;
	}

	public void setWarningTime(DateTime warningTime) {
		WarningTime = warningTime;
	}

	public DateTime getReplaceMinTime() {
		return ReplaceMinTime;
	}

	public void setReplaceMinTime(DateTime replaceMinTime) {
		ReplaceMinTime = replaceMinTime;
	}

	public int getDSType() {
		return DSType;
	}

	public void setDSType(int dSType) {
		DSType = dSType;
	}

	public int getActive() {
		return Active;
	}

	public void setActive(int active) {
		Active = active;
	}
 
}
