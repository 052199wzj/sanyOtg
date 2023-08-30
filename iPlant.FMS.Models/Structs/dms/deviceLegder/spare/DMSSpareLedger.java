package com.mes.server.service.po.dms.deviceLegder.spare;

import java.io.Serializable;
import java.util.DateTime;

/// <summary>
/// 设备台账信息
/// </summary>
public class DMSSpareLedger implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;

	/// <summary>
	/// 设备ID
	/// </summary>
	public int ID = 0;

	/// <summary>
	/// 备件号
	/// </summary>
	public String Code = "";

	/// <summary>
	/// 名称
	/// </summary>
	public String Name = "";

	/// <summary>
	/// 备件型号ID
	/// </summary>
	public int ModelID = 0;

	public String ModelName = "";

	public String ModelNo = "";

	/// <summary>
	/// 设备ID
	/// </summary>
	public int DeviceID = 0;

	public String DeviceNo = "";

	/// <summary>
	/// 设备可装备件类型ID 装上用的那个类型装的？？？
	/// </summary>
	public int EquipID = 0;

	/// <summary>
	/// 备件寿命
	/// </summary>
	public double Life = 0;
	/// <summary>
	/// 备件加工限制
	/// </summary>
	public long LimitCount = 0;
	/// <summary>
	/// 设备所属部门
	/// </summary>
	public int BusinessUnitID = 0;

	public String BusinessUnitName = "";
	/// <summary>
	/// 设备所属生产基地ID
	/// </summary>
	public int BaseID = 0;

	public String BaseName = "";
	/// <summary>
	/// 设备所属生产基地下的工厂ID
	/// </summary>
	public int FactoryID = 0;

	public String FactoryName = "";

	/// <summary>
	/// 车间ID
	/// </summary>
	public int WorkShopID = 0;

	public String WorkShopName = "";
	/// <summary>
	/// 产线ID
	/// </summary>
	public int LineID = 0;

	public String LineName = "";
	/// <summary>
	/// 状态
	/// </summary>
	public int Status = 0;
	/// <summary>
	/// 录入人
	/// </summary>
	public int CreatorID = 0;

	public String CreatorName = "";
	/// <summary>
	/// 录入时刻
	/// </summary>
	public DateTime CreateTime = DateTime.Now;

	/// <summary>
	/// 编辑人
	/// </summary>
	public int EditorID = 0;

	public String EditorName = "";
	/// <summary>
	/// 编辑时刻
	/// </summary>
	public DateTime EditTime = DateTime.Now;

	public DMSSpareLedger() {
	}

	public int ID {
		return ID;
	}

	public void setID(int iD) {
		ID = iD;
	}

	public String getCode() {
		return Code;
	}

	public void setCode(String code) {
		Code = code;
	}

	public String getName() {
		return Name;
	}

	public void setName(String name) {
		Name = name;
	}

	public int getModelID() {
		return ModelID;
	}

	public void setModelID(int modelID) {
		ModelID = modelID;
	}

	public String getModelName() {
		return ModelName;
	}

	public void setModelName(String modelName) {
		ModelName = modelName;
	}

	public String getModelNo() {
		return ModelNo;
	}

	public void setModelNo(String modelNo) {
		ModelNo = modelNo;
	}

	public int getDeviceID() {
		return DeviceID;
	}

	public void setDeviceID(int deviceID) {
		DeviceID = deviceID;
	}

	public String getDeviceNo() {
		return DeviceNo;
	}

	public void setDeviceNo(String deviceNo) {
		DeviceNo = deviceNo;
	}

	public int getEquipID() {
		return EquipID;
	}

	public void setEquipID(int equipID) {
		EquipID = equipID;
	}

	public double getLife() {
		return Life;
	}

	public void setLife(double life) {
		Life = life;
	}

	public long getLimitCount() {
		return LimitCount;
	}

	public void setLimitCount(long limitCount) {
		LimitCount = limitCount;
	}

	public int getBusinessUnitID() {
		return BusinessUnitID;
	}

	public void setBusinessUnitID(int businessUnitID) {
		BusinessUnitID = businessUnitID;
	}

	public String getBusinessUnitName() {
		return BusinessUnitName;
	}

	public void setBusinessUnitName(String businessUnitName) {
		BusinessUnitName = businessUnitName;
	}

	public int getBaseID() {
		return BaseID;
	}

	public void setBaseID(int baseID) {
		BaseID = baseID;
	}

	public String getBaseName() {
		return BaseName;
	}

	public void setBaseName(String baseName) {
		BaseName = baseName;
	}

	public int getFactoryID() {
		return FactoryID;
	}

	public void setFactoryID(int factoryID) {
		FactoryID = factoryID;
	}

	public String getFactoryName() {
		return FactoryName;
	}

	public void setFactoryName(String factoryName) {
		FactoryName = factoryName;
	}

	public int getWorkShopID() {
		return WorkShopID;
	}

	public void setWorkShopID(int workShopID) {
		WorkShopID = workShopID;
	}

	public String getWorkShopName() {
		return WorkShopName;
	}

	public void setWorkShopName(String workShopName) {
		WorkShopName = workShopName;
	}

	public int getLineID() {
		return LineID;
	}

	public void setLineID(int lineID) {
		LineID = lineID;
	}

	public String getLineName() {
		return LineName;
	}

	public void setLineName(String lineName) {
		LineName = lineName;
	}

	public int getStatus() {
		return Status;
	}

	public void setStatus(int status) {
		Status = status;
	}

	public int getCreatorID() {
		return CreatorID;
	}

	public void setCreatorID(int creatorID) {
		CreatorID = creatorID;
	}

	public String getCreatorName() {
		return CreatorName;
	}

	public void setCreatorName(String creatorName) {
		CreatorName = creatorName;
	}

	public DateTime getCreateTime() {
		return CreateTime;
	}

	public void setCreateTime(DateTime createTime) {
		CreateTime = createTime;
	}

	public int getEditorID() {
		return EditorID;
	}

	public void setEditorID(int editorID) {
		EditorID = editorID;
	}

	public String getEditorName() {
		return EditorName;
	}

	public void setEditorName(String editorName) {
		EditorName = editorName;
	}

	public DateTime getEditTime() {
		return EditTime;
	}

	public void setEditTime(DateTime editTime) {
		EditTime = editTime;
	}

}
