package com.mes.server.service.po.dms.deviceLegder.device;

import java.io.Serializable;
import java.util.DateTime;

/// <summary>
/// 设备台账信息
/// </summary>
public class DMSDeviceLedger implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;

	/// <summary>
	/// 设备ID
	/// </summary>
	public int ID = 0;

	/// <summary>
	/// 设备号
	/// </summary>
	public String Code = "";

	/// <summary>
	/// 名称
	/// </summary>
	public String Name = "";

	/// <summary>
	/// 设备型号ID
	/// </summary>
	public int ModelID = 0;

	public String ModelName = "";

	public String ModelNo = "";

	/// <summary>
	/// 设备加工类型
	/// </summary>
	public int WorkType = 0;

	/// <summary>
	/// 设备加工类型
	/// </summary>
	public String WorkTypeName = "";

	/// <summary>
	/// 型号属性ID
	/// </summary>
	public int ModelPropertyID = 0;
	/// <summary>
	/// 供应商ID
	/// </summary>
	public String SupplierName = "";
	/// <summary>
	/// 供应商编号
	/// </summary>
	public String SupplierModelNo = "";

	public double ScrapValue = 0;

	public double NetValue = 0;

	public int AssetID = 0;

	public String AssetCode = "";

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
	/// 位置ID 可选
	/// </summary>
	public int PositionID = 0;

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

	/// <summary>
	/// 设备使用状态 0：就绪（默认值 加入台账时为就绪状态） 1：使用 2：闲置 3：维修 4：保养 5：报废 6：封存
	/// </summary>
	public int Status;
 

	public DMSDeviceLedger() {

	}

	public int ID {
		return ID;
	}

	public int getStatus() {
		return Status;
	}

	public void setStatus(int status) {
		Status = status;
	}

	public String getDeviceNo() {
		return Code;
	}

	public void setDeviceNo(String deviceNo) {
		Code = deviceNo;
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

	public int getAssetID() {
		return AssetID;
	}

	public void setAssetID(int assetID) {
		AssetID = assetID;
	}

	public String getAssetCode() {
		return AssetCode;
	}

	public void setAssetCode(String assetCode) {
		AssetCode = assetCode;
	}

	public double getNetValue() {
		return NetValue;
	}

	public void setNetValue(double netValue) {
		NetValue = netValue;
	}

	public double getDeviceLife() {
		return Life;
	}

	public void setDeviceLife(double deviceLife) {
		Life = deviceLife;
	}

	public double getScrapValue() {
		return ScrapValue;
	}

	public void setScrapValue(double scrapValue) {
		ScrapValue = scrapValue;
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

	public int getPositionID() {
		return PositionID;
	}

	public void setPositionID(int positionID) {
		PositionID = positionID;
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

	public void setID(int iD) {
		ID = iD;
	}
}
