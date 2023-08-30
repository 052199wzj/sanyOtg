package com.mes.server.service.po.dms.deviceLegder;

import java.io.Serializable;
import java.util.DateTime;

/// <summary>
/// 设备工作类型
/// </summary>
public class DMSWorkType implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	

	public DMSWorkType() {
		OperateTime = DateTime.Now;
		Name = "";
	}

	public int ID {
		return ID;
	}

	public void setID(int iD) {
		ID = iD;
	}

	public String getName() {
		return Name;
	}

	public void setName(String name) {
		Name = name;
	}

	public int getDSType() {
		return DSType;
	}

	public void setDSType(int dSType) {
		DSType = dSType;
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
