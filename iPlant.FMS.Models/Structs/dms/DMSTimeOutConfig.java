package com.mes.server.service.po.dms;

import java.io.Serializable;

/// <summary>
/// 设备超时配置 暂时不用
/// </summary>
public class DMSTimeOutConfig implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	/// <summary>
	/// 设备保养通知提前时间 单位秒
	/// </summary>
	private int DeviceMaintain;
	/// <summary>
	/// 备件保养通知提前时间 单位秒
	/// </summary>
	private int SpareMaintain;

	/// <summary>
	/// 设备维修通知提前时间 单位秒
	/// </summary>
	private int DeviceRepair;
	/// <summary>
	/// 备件维修通知提前时间 单位秒
	/// </summary>
	private int SpareRepair;
	/// <summary>
	/// 设备预警通知超时 单位秒
	/// </summary>
	private int DeviceLedger;
	/// <summary>
	/// 备件预警通知超时 单位秒
	/// </summary>
	private int SpareLedger;

	public DMSTimeOutConfig() {
		DeviceMaintain = 24 * 60 * 60;
		SpareMaintain = 24 * 60 * 60;
		DeviceRepair = 24 * 60 * 60;
		SpareRepair = 24 * 60 * 60;
		DeviceLedger = 24 * 60 * 60;
		SpareLedger = 24 * 60 * 60;
	}

	public int getDeviceMaintain() {
		return DeviceMaintain;
	}

	public void setDeviceMaintain(int deviceMaintain) {
		DeviceMaintain = deviceMaintain;
	}

	public int getSpareMaintain() {
		return SpareMaintain;
	}

	public void setSpareMaintain(int spareMaintain) {
		SpareMaintain = spareMaintain;
	}
 

	public int getDeviceRepair() {
		return DeviceRepair;
	}

	public void setDeviceRepair(int deviceRepair) {
		DeviceRepair = deviceRepair;
	}

	public int getSpareRepair() {
		return SpareRepair;
	}

	public void setSpareRepair(int spareRepair) {
		SpareRepair = spareRepair;
	}

	public int getDeviceLedger() {
		return DeviceLedger;
	}

	public void setDeviceLedger(int deviceLedger) {
		DeviceLedger = deviceLedger;
	}

	public int getSpareLedger() {
		return SpareLedger;
	}

	public void setSpareLedger(int spareLedger) {
		SpareLedger = spareLedger;
	}
}
