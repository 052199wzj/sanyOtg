using System;
using System.Collections.Generic;
using System.Text;
using iPlant.Common.Tools;
using iPlant.FMC.Service;
using iPlant.FMS.Models;
using System.Linq;

namespace iPlant.FMC.Service
{
    public class OMSOrderItemDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSOrderItemDAO));

        #region 单实例
        private OMSOrderItemDAO() { }
        private static OMSOrderItemDAO _Instance;

        public static OMSOrderItemDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new OMSOrderItemDAO();
                return OMSOrderItemDAO._Instance;
            }
        }
        #endregion

        public int OMS_SaveOMSOrderItem(OMSOrderItem wOMSOrderItem, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wparms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wOMSOrderItem.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.oms_orderitem(NCFileUri,DXFFileUri,CuttingNumber,NestingNumber,CutType,DemandFinishDate,CutTimes,NestDate,PlateMaterialNo,StructuralPartID,Texture,Thickness,ManeuverTime,Plate,CutLength,Material,TechThickness,Gas,CuttingMouth,CreateID,CreateTime,EditID,EditTime,OrderID,OrderNo,Status,CutSpeed,OrderNum,StartTime,OrderType,DXFAnalysisStatus,DXFAnalysisFailReason,Active,DXFLocalUrl,NCLocalUrl) VALUES(@wNCFileUri,@wDXFFileUri,@wCuttingNumber,@wNestingNumber,@wCutType,@wDemandFinishDate,@wCutTimes,@wNestDate,@wPlateMaterialNo,@wStructuralPartID,@wTexture,@wThickness,@wManeuverTime,@wplate,@wCutLength,@wMaterial,@wTechThickness,@wGas,@wCuttingMouth,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wOrderID,@wOrderNo,@wStatus,@wCutSpeed,@wOrderNum,@wStartTime,@wOrderType,@wDXFAnalysisStatus,@wDXFAnalysisFailReason,@wActive,@wDXFLocalUrl,@wNCLocalUrl);", wInstance);
                else if (wOMSOrderItem.ID > 0)
                    wSQLText = string.Format("UpDATE {0}.oms_orderitem SET NCFileUri=@wNCFileUri,DXFFileUri=@wDXFFileUri,CuttingNumber=@wCuttingNumber,NestingNumber=@wNestingNumber,CutType=@wCutType,DemandFinishDate=@wDemandFinishDate,CutTimes=@wCutTimes,NestDate=@wNestDate,PlateMaterialNo=@wPlateMaterialNo,StructuralPartID=@wStructuralPartID,Texture=@wTexture,Thickness=@wThickness,ManeuverTime=@wManeuverTime,Plate=@wPlate,CutLength=@wCutLength,Material=@wMaterial,TechThickness=@wTechThickness,Gas=@wGas,CuttingMouth=@wCuttingMouth,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,OrderID=@wOrderID,OrderNo=@wOrderNo,Status=@wStatus,CutSpeed=@wCutSpeed,OrderNum=@wOrderNum,StartTime=@wStartTime,OrderType=@wOrderType,DXFAnalysisStatus=@wDXFAnalysisStatus,DXFAnalysisFailReason=@wDXFAnalysisFailReason,Active=@wActive,DXFLocalUrl=@wDXFLocalUrl,NCLocalUrl=@wNCLocalUrl WHERE ID=@wID", wInstance);

                wparms.Clear();
                wparms.Add("wID", wOMSOrderItem.ID);
                wparms.Add("wNCFileUri", wOMSOrderItem.NCFileUri);
                wparms.Add("wDXFFileUri", wOMSOrderItem.DXFFileUri);
                wparms.Add("wCuttingNumber", wOMSOrderItem.CuttingNumber);
                wparms.Add("wNestingNumber", wOMSOrderItem.NestingNumber);
                wparms.Add("wCutType", wOMSOrderItem.CutType);
                wparms.Add("wDemandFinishDate", wOMSOrderItem.DemandFinishDate);
                wparms.Add("wCutTimes", wOMSOrderItem.CutTimes);
                wparms.Add("wNestDate", wOMSOrderItem.NestDate);
                wparms.Add("wPlateMaterialNo", wOMSOrderItem.PlateMaterialNo);
                wparms.Add("wStructuralPartID", wOMSOrderItem.StructuralPartID);
                wparms.Add("wTexture", wOMSOrderItem.Texture);
                wparms.Add("wThickness", wOMSOrderItem.Thickness);
                wparms.Add("wManeuverTime", wOMSOrderItem.ManeuverTime);
                wparms.Add("wPlate", wOMSOrderItem.Plate);
                wparms.Add("wCutLength", wOMSOrderItem.CutLength);
                wparms.Add("wMaterial", wOMSOrderItem.Material);
                wparms.Add("wTechThickness", wOMSOrderItem.TechThickness);
                wparms.Add("wGas", wOMSOrderItem.Gas);
                wparms.Add("wCuttingMouth", wOMSOrderItem.CuttingMouth);
                wparms.Add("wCreateID", wOMSOrderItem.CreateID);
                wparms.Add("wCreateTime", wOMSOrderItem.CreateTime);
                wparms.Add("wEditID", wOMSOrderItem.EditID);
                wparms.Add("wEditTime", wOMSOrderItem.EditTime);
                wparms.Add("wOrderID", wOMSOrderItem.OrderID);
                wparms.Add("wOrderNo", wOMSOrderItem.OrderNo);
                wparms.Add("wStatus", wOMSOrderItem.Status);
                wparms.Add("wCutSpeed", wOMSOrderItem.CutSpeed);
                wparms.Add("wOrderNum", wOMSOrderItem.OrderNum);
                wparms.Add("wStartTime", wOMSOrderItem.StartTime);
                wparms.Add("wOrderType", wOMSOrderItem.OrderType);
                wparms.Add("OrderType", wOMSOrderItem.OrderType);
                wparms.Add("wDXFAnalysisStatus", wOMSOrderItem.DXFAnalysisStatus);
                wparms.Add("wDXFAnalysisFailReason", wOMSOrderItem.DXFAnalysisFailReason);
                wparms.Add("wActive", wOMSOrderItem.Active);
                wparms.Add("wDXFLocalUrl", wOMSOrderItem.DXFLocalUrl);
                wparms.Add("wNCLocalUrl", wOMSOrderItem.NCLocalUrl);
                wSQLText = this.DMLChange(wSQLText);

                if (wOMSOrderItem.ID <= 0)
                    wOMSOrderItem.ID = (int)mDBPool.insert(wSQLText, wparms);
                else
                    mDBPool.update(wSQLText, wparms);
            }
            catch (Exception ex)
            {
                logger.Error("OMS_SaveOMSOrderItem", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int OMS_DeleteOMSOrderItemList(List<OMSOrderItem> wOMSOrderItemList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wOMSOrderItemList != null && wOMSOrderItemList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wOMSOrderItemList.Count; i++)
                    {
                        if (i == wOMSOrderItemList.Count - 1)
                            wStringBuilder.Append(wOMSOrderItemList[i].ID);
                        else
                            wStringBuilder.Append(wOMSOrderItemList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.oms_orderitem WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wparms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wparms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OMS_DeleteOMSOrderItemList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        /// <summary>
        /// 工单数据统计
        /// </summary>
        /// <param name="wCutType"> 1火焰  2平面 3坡口</param>
        /// <param name="wStartTime"></param>
        /// <param name="wEndTime"></param>
        /// <param name="wStatus">   1已创建、2生产中、3已完成 4已激活</param>
        /// <param name="wPagination"></param>
        /// <param name="wOrderType">  1中控订单  2LES订单</param>
        /// <returns>
        /// wStatus=-1
        /// 获得每日计划工单数量（计划加工钢板数）、计划加工零件总数（当日工单包含零件数据)
        /// 
        /// wStatus=3
        /// 获得每日实际完成工单数量（计划加工钢板数）、每日已向LES报工数量
        /// </returns>
        public OMSOrderItemStatistics OMS_OrderStatistics(int wCutType, DateTime wStartTime, DateTime wEndTime, int wStatus, Pagination wPagination,
            int wOrderType, OutResult<Int32> wErrorCode)
        {
            OMSOrderItemStatistics wOMSOrderItemStatistics = new OMSOrderItemStatistics();
            wErrorCode.set(0);
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                //统计每日生产的钢板编号，每个钢板的零件数
                string wSQLText = string.Format(
                    "SELECT t3.* ,"
                + " (SELECT count(PartNo) FROM {0}.oms_dxf_analysis_parts WHERE {0}.oms_dxf_analysis_parts.DxfAnalysisID = t3.DxfAnalysisID) AS 'NumPartPlan',"
                + " (SELECT count(*) FROM {0}.inf_les_updownmaterial WHERE {0}.inf_les_updownmaterial.Status = 1 AND {0}.inf_les_updownmaterial.NestId = t3.CuttingNumber) AS 'NumPartToLES' "
                + " FROM "
                + " (SELECT t1.OrderID, t1.ID AS OrderItemID, t1.CuttingNumber, t1.CreateTime, t2.ID AS 'DxfAnalysisID' "
                + " FROM {0}.oms_orderitem AS t1 INNER JOIN  {0}.oms_dxf_analysis AS t2 ON t1.ID = t2.OrderItemID "
                + " where  1 = 1 "
                + " and ( @wStartTime <= '2010-1-1' or t1.CreateTime >= @wStartTime )"
                + " and ( @wEndTime <= '2010-1-1' or t1.CreateTime <= @wEndTime )"
                + " and (@wCutType<=0 or t1.CutType = @wCutType ) "
                + " and (@wStatus<=0 or t1.Status = @wStatus ) "
                + " and (@wOrderType<=0 or t1.OrderType = @wOrderType)) as t3", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wCutType", wCutType);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);
                wParms.Add("wStatus", wStatus);
                wParms.Add("wOrderType", wOrderType);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    DateTime date = StringUtils.parseDate(wSqlDataReader["CreateTime"]).Date;
                    if (!wOMSOrderItemStatistics.DicDate_NumPartInPlate.ContainsKey(date))
                        wOMSOrderItemStatistics.DicDate_NumPartInPlate.Add(date, StringUtils.parseInt(wSqlDataReader["NumPartPlan"]));
                    else
                        wOMSOrderItemStatistics.DicDate_NumPartInPlate[date] += StringUtils.parseInt(wSqlDataReader["NumPartPlan"]);

                    if (!wOMSOrderItemStatistics.DicDate_NumPartToLES.ContainsKey(date))
                        wOMSOrderItemStatistics.DicDate_NumPartToLES.Add(date, StringUtils.parseInt(wSqlDataReader["NumPartToLES"]));
                    else
                        wOMSOrderItemStatistics.DicDate_NumPartToLES[date] += StringUtils.parseInt(wSqlDataReader["NumPartToLES"]);

                    if (!wOMSOrderItemStatistics.DicDate_NumPlate.ContainsKey(date))
                        wOMSOrderItemStatistics.DicDate_NumPlate.Add(date, 1);
                    else
                        wOMSOrderItemStatistics.DicDate_NumPlate[date] += 1;

                    if (!wOMSOrderItemStatistics.DicCutID_DatePlan.ContainsKey(StringUtils.parseString(wSqlDataReader["CuttingNumber"])))
                        wOMSOrderItemStatistics.DicCutID_DatePlan.Add(
                            StringUtils.parseString(wSqlDataReader["CuttingNumber"]),
                            StringUtils.parseDate(wSqlDataReader["CreateTime"]));
                    if (!wOMSOrderItemStatistics.DicCutID_NumPartInPlate.ContainsKey(StringUtils.parseString(wSqlDataReader["CuttingNumber"])))
                        wOMSOrderItemStatistics.DicCutID_NumPartInPlate.Add(
                            StringUtils.parseString(wSqlDataReader["CuttingNumber"]),
                            StringUtils.parseInt(wSqlDataReader["NumPartPlan"]));
                    if (!wOMSOrderItemStatistics.DicCutID_NumPartToLES.ContainsKey(StringUtils.parseString(wSqlDataReader["CuttingNumber"])))
                        wOMSOrderItemStatistics.DicCutID_NumPartToLES.Add(
                            StringUtils.parseString(wSqlDataReader["CuttingNumber"]),
                            StringUtils.parseInt(wSqlDataReader["NumPartToLES"]));
                }
                wOMSOrderItemStatistics.NumPlateTotal = wOMSOrderItemStatistics.DicCutID_NumPartInPlate.Keys.Count;
                wOMSOrderItemStatistics.NumPartTotal = (int)wOMSOrderItemStatistics.DicCutID_NumPartInPlate.ToList().Sum(a => a.Value);
                wOMSOrderItemStatistics.NumPartTotalToLES = (int)wOMSOrderItemStatistics.DicCutID_NumPartToLES.ToList().Sum(a => a.Value);
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(OMS_QueryOMSOrderItemList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantCore");

                logger.Error("OMS_QueryOMSOrderItemList", ex);

                wErrorCode.set(MESException.DBSQL.Value);
            }
            return wOMSOrderItemStatistics;
        }


        //10.11 wzj
        //新增查询限制条件  Displayed=1
        public List<OMSOrderItem> OMS_QueryOMSOrderItemList(int wID, int wOrderID, string wOrderNo, string wCuttingNumber, string wNestingNumber, int wCutType, string wPlateMaterialNo, int wStructuralpartID, string wGas, string wCuttingMouth, DateTime wStartTime, DateTime wEndTime, int wStatus, Pagination wPagination, int wOrderType, int wActive, int wDXFAnalysisStatus, int wLesOrderID, int wDisplayed, out int wErrorCode)
        {
            List<OMSOrderItem> wResultList = new List<OMSOrderItem>();
            wErrorCode = 0;
            if (wDisplayed == 0)
                wDisplayed = 1;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name StructuralPartName,t1.Length,t1.Width,t1.Height,t1.Weight,t4.Name Creator,t5.Name Editor FROM {0}.oms_orderitem t "
                    + " left join {0}.fpc_structuralpart t1 on t.StructuralPartID=t1.ID "
                    + " left join {0}.mbs_user t4 on t.CreateID=t4.ID "
                    + " left join {0}.mbs_user t5 on t.EditID=t5.ID "
                    + "WHERE 1=1 "
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wOrderID <=0 or t.OrderID= @wOrderID)"
                  + " and(@wDXFAnalysisStatus <=0 or t.DXFAnalysisStatus= @wDXFAnalysisStatus)"
                    + " and(@wLesOrderID <=0 or t.LesOrderID= @wLesOrderID)"
                + " and(@wStatus <=0 or t.Status= @wStatus)"
                  + " and(@wActive <=0 or t.Active= @wActive)"
                + " and(@wOrderNo is null or @wOrderNo = '' or t.OrderNo= @wOrderNo)"
                + " and(@wCuttingNumber is null or @wCuttingNumber = '' or t.CuttingNumber= @wCuttingNumber)"
                + " and(@wNestingNumber is null or @wNestingNumber = '' or t.NestingNumber= @wNestingNumber)"
                + " and(@wCutType <=0 or t.CutType= @wCutType)"
                + " and(@wPlateMaterialNo is null or @wPlateMaterialNo = '' or t.PlateMaterialNo= @wPlateMaterialNo)"
                + " and(@wStructuralPartID <=0 or t.StructuralPartID= @wStructuralPartID)"
                + " and(@wGas is null or @wGas = '' or t.Gas= @wGas)"
                + " and(@wCuttingMouth is null or @wCuttingMouth = '' or t.CuttingMouth= @wCuttingMouth)"
                + " and(@wStartTime <= '2010-1-1' or t.EditTime>= @wStartTime)"
                + " and(@wEndTime <= '2010-1-1' or t.CreateTime<= @wEndTime)"
                + " and(@wDisplayed <0 or t.Displayed= @wDisplayed)"
                + " and(@wOrderType <=0 or t.OrderType= @wOrderType)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wOrderID", wOrderID);
                wParms.Add("wOrderNo", wOrderNo);
                wParms.Add("wCuttingNumber", wCuttingNumber);
                wParms.Add("wNestingNumber", wNestingNumber);
                wParms.Add("wCutType", wCutType);
                wParms.Add("wPlateMaterialNo", wPlateMaterialNo);
                wParms.Add("wStructuralPartID", wStructuralpartID);
                wParms.Add("wGas", wGas);
                wParms.Add("wCuttingMouth", wCuttingMouth);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);
                wParms.Add("wStatus", wStatus);
                wParms.Add("wOrderType", wOrderType);
                wParms.Add("wActive", wActive);
                wParms.Add("wDXFAnalysisStatus", wDXFAnalysisStatus);
                wParms.Add("wLesOrderID", wLesOrderID);
                wParms.Add("wDisplayed", wDisplayed);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    OMSOrderItem wOMSOrderItem = new OMSOrderItem();

                    wOMSOrderItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wOMSOrderItem.OrderID = StringUtils.parseInt(wSqlDataReader["OrderID"]);
                    wOMSOrderItem.LesOrderID = StringUtils.parseInt(wSqlDataReader["LesOrderID"]);
                    wOMSOrderItem.OrderNo = StringUtils.parseString(wSqlDataReader["OrderNo"]);
                    wOMSOrderItem.Status = StringUtils.parseInt(wSqlDataReader["Status"]);
                    wOMSOrderItem.NCFileUri = StringUtils.parseString(wSqlDataReader["NCFileUri"]);
                    wOMSOrderItem.DXFFileUri = StringUtils.parseString(wSqlDataReader["DXFFileUri"]);
                    wOMSOrderItem.CuttingNumber = StringUtils.parseString(wSqlDataReader["CuttingNumber"]);
                    wOMSOrderItem.NestingNumber = StringUtils.parseString(wSqlDataReader["NestingNumber"]);
                    wOMSOrderItem.CutType = StringUtils.parseInt(wSqlDataReader["CutType"]);
                    wOMSOrderItem.DemandFinishDate = StringUtils.parseDate(wSqlDataReader["DemandFinishDate"]);
                    wOMSOrderItem.CutTimes = StringUtils.parseInt(wSqlDataReader["CutTimes"]);
                    wOMSOrderItem.NestDate = StringUtils.parseDate(wSqlDataReader["NestDate"]);
                    wOMSOrderItem.PlateMaterialNo = StringUtils.parseString(wSqlDataReader["PlateMaterialNo"]);
                    wOMSOrderItem.StructuralPartID = StringUtils.parseInt(wSqlDataReader["StructuralPartID"]);
                    wOMSOrderItem.StructuralPartName = StringUtils.parseString(wSqlDataReader["StructuralPartName"]);
                    wOMSOrderItem.Length = StringUtils.parseInt(wSqlDataReader["Length"]);
                    wOMSOrderItem.Width = StringUtils.parseInt(wSqlDataReader["Width"]);
                    wOMSOrderItem.Height = StringUtils.parseInt(wSqlDataReader["Height"]);
                    wOMSOrderItem.Weight = StringUtils.parseDouble(wSqlDataReader["Weight"]);
                    wOMSOrderItem.Texture = StringUtils.parseString(wSqlDataReader["Texture"]);
                    wOMSOrderItem.Thickness = StringUtils.parseDouble(wSqlDataReader["Thickness"]);
                    wOMSOrderItem.ManeuverTime = StringUtils.parseDate(wSqlDataReader["ManeuverTime"]);
                    wOMSOrderItem.Plate = StringUtils.parseString(wSqlDataReader["Plate"]);
                    wOMSOrderItem.CutLength = StringUtils.parseDouble(wSqlDataReader["CutLength"]);
                    wOMSOrderItem.Material = StringUtils.parseString(wSqlDataReader["Material"]);
                    wOMSOrderItem.TechThickness = StringUtils.parseString(wSqlDataReader["TechThickness"]);
                    wOMSOrderItem.Gas = StringUtils.parseString(wSqlDataReader["Gas"]);
                    wOMSOrderItem.CuttingMouth = StringUtils.parseString(wSqlDataReader["CuttingMouth"]);
                    wOMSOrderItem.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wOMSOrderItem.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wOMSOrderItem.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wOMSOrderItem.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wOMSOrderItem.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wOMSOrderItem.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wOMSOrderItem.CutSpeed = StringUtils.parseDouble(wSqlDataReader["CutSpeed"]);
                    wOMSOrderItem.OrderNum = StringUtils.parseInt(wSqlDataReader["OrderNum"]);
                    wOMSOrderItem.StartTime = StringUtils.parseDate(wSqlDataReader["StartTime"]);
                    wOMSOrderItem.OrderType = StringUtils.parseInt(wSqlDataReader["OrderType"]);
                    wOMSOrderItem.DXFAnalysisStatus = StringUtils.parseInt(wSqlDataReader["DXFAnalysisStatus"]);
                    wOMSOrderItem.DXFAnalysisFailReason = StringUtils.parseString(wSqlDataReader["DXFAnalysisFailReason"]);
                    wOMSOrderItem.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wOMSOrderItem.DXFLocalUrl = StringUtils.parseString(wSqlDataReader["DXFLocalUrl"]);
                    wOMSOrderItem.NCLocalUrl = StringUtils.parseString(wSqlDataReader["NCLocalUrl"]);
                    wOMSOrderItem.Displayed = StringUtils.parseInt(wSqlDataReader["Displayed"]);
                    wResultList.Add(wOMSOrderItem);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(OMS_QueryOMSOrderItemList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantCore");

                logger.Error("OMS_QueryOMSOrderItemList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }

        public List<INFSortsysSendcasing> INF_QueryINFSortsysSendcasingList(String wMissionNo, out int wErrorCode)
        {
            List<INFSortsysSendcasing> wResultList = new List<INFSortsysSendcasing>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                String wSQL = String.Format(
                    "SELECT t.* FROM {0}.inf_sortsys_sendcasing t WHERE 1=1 " +
                    " AND (@wMissionNo is null OR @wMissionNo = '' OR t.MissionNo = @wMissionNo)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                wParms.Add("wMissionNo", wMissionNo);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, wParms, null);
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    INFSortsysSendcasing wItem = new INFSortsysSendcasing();

                    wItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wItem.ProductionLline = StringUtils.parseString(wSqlDataReader["ProductionLline"]);
                    wItem.SortStationNo = StringUtils.parseString(wSqlDataReader["SortStationNo"]);
                    wItem.CutStationNo = StringUtils.parseString(wSqlDataReader["CutStationNo"]);
                    wItem.CasingLocalUrl = StringUtils.parseString(wSqlDataReader["CasingLocalUrl"]);
                    wItem.MissionNo = StringUtils.parseString(wSqlDataReader["MissionNo"]);
                    wItem.Status = StringUtils.parseSByte(wSqlDataReader["Status"]);
                    wItem.ErroMsg = StringUtils.parseString(wSqlDataReader["ErroMsg"]);
                    wItem.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wItem.SendTime = StringUtils.parseDate(wSqlDataReader["SendTime"]);

                    wResultList.Add(wItem);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(INF_QueryINFLesUpDownMaterialList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("INF_QueryINFLesUpDownMaterialList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }


        public List<OMSOrderItem> OMS_QueryOrderItemListByDeviceNo(string wDeviceNo, out int wErrorCode)
        {
            List<OMSOrderItem> wResultList = new List<OMSOrderItem>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name StructuralPartName,t1.Length,t1.Width,t1.Height,t1.Weight,t4.Name Creator,t5.Name Editor FROM {0}.oms_orderitem t "
                    + " left join {0}.fpc_structuralpart t1 on t.StructuralPartID=t1.ID "
                    + " left join {0}.mbs_user t4 on t.CreateID=t4.ID "
                    + " left join {0}.mbs_user t5 on t.EditID=t5.ID "
                    + "WHERE 1=1 and"
                + " t.CutType in (SELECT ModelID FROM {0}.dms_device_ledger where Code=@wDeviceNo) and t.Status=1", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wDeviceNo", wDeviceNo);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    OMSOrderItem wOMSOrderItem = new OMSOrderItem();

                    wOMSOrderItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wOMSOrderItem.OrderID = StringUtils.parseInt(wSqlDataReader["OrderID"]);
                    wOMSOrderItem.OrderNo = StringUtils.parseString(wSqlDataReader["OrderNo"]);
                    wOMSOrderItem.Status = StringUtils.parseInt(wSqlDataReader["Status"]);
                    wOMSOrderItem.NCFileUri = StringUtils.parseString(wSqlDataReader["NCFileUri"]);
                    wOMSOrderItem.DXFFileUri = StringUtils.parseString(wSqlDataReader["DXFFileUri"]);
                    wOMSOrderItem.CuttingNumber = StringUtils.parseString(wSqlDataReader["CuttingNumber"]);
                    wOMSOrderItem.NestingNumber = StringUtils.parseString(wSqlDataReader["NestingNumber"]);
                    wOMSOrderItem.CutType = StringUtils.parseInt(wSqlDataReader["CutType"]);
                    wOMSOrderItem.DemandFinishDate = StringUtils.parseDate(wSqlDataReader["DemandFinishDate"]);
                    wOMSOrderItem.CutTimes = StringUtils.parseInt(wSqlDataReader["CutTimes"]);
                    wOMSOrderItem.NestDate = StringUtils.parseDate(wSqlDataReader["NestDate"]);
                    wOMSOrderItem.PlateMaterialNo = StringUtils.parseString(wSqlDataReader["PlateMaterialNo"]);
                    wOMSOrderItem.StructuralPartID = StringUtils.parseInt(wSqlDataReader["StructuralPartID"]);
                    wOMSOrderItem.StructuralPartName = StringUtils.parseString(wSqlDataReader["StructuralPartName"]);
                    wOMSOrderItem.Length = StringUtils.parseInt(wSqlDataReader["Length"]);
                    wOMSOrderItem.Width = StringUtils.parseInt(wSqlDataReader["Width"]);
                    wOMSOrderItem.Height = StringUtils.parseInt(wSqlDataReader["Height"]);
                    wOMSOrderItem.Weight = StringUtils.parseDouble(wSqlDataReader["Weight"]);
                    wOMSOrderItem.Texture = StringUtils.parseString(wSqlDataReader["Texture"]);
                    wOMSOrderItem.Thickness = StringUtils.parseDouble(wSqlDataReader["Thickness"]);
                    wOMSOrderItem.ManeuverTime = StringUtils.parseDate(wSqlDataReader["ManeuverTime"]);
                    wOMSOrderItem.Plate = StringUtils.parseString(wSqlDataReader["Plate"]);
                    wOMSOrderItem.CutLength = StringUtils.parseDouble(wSqlDataReader["CutLength"]);
                    wOMSOrderItem.Material = StringUtils.parseString(wSqlDataReader["Material"]);
                    wOMSOrderItem.TechThickness = StringUtils.parseString(wSqlDataReader["TechThickness"]);
                    wOMSOrderItem.Gas = StringUtils.parseString(wSqlDataReader["Gas"]);
                    wOMSOrderItem.CuttingMouth = StringUtils.parseString(wSqlDataReader["CuttingMouth"]);
                    wOMSOrderItem.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wOMSOrderItem.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wOMSOrderItem.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wOMSOrderItem.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wOMSOrderItem.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wOMSOrderItem.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wOMSOrderItem.CutSpeed = StringUtils.parseDouble(wSqlDataReader["CutSpeed"]);
                    wOMSOrderItem.OrderNum = StringUtils.parseInt(wSqlDataReader["OrderNum"]);
                    wOMSOrderItem.StartTime = StringUtils.parseDate(wSqlDataReader["StartTime"]);
                    wOMSOrderItem.OrderType = StringUtils.parseInt(wSqlDataReader["OrderType"]);
                    wOMSOrderItem.DXFAnalysisStatus = StringUtils.parseInt(wSqlDataReader["DXFAnalysisStatus"]);
                    wOMSOrderItem.DXFAnalysisFailReason = StringUtils.parseString(wSqlDataReader["DXFAnalysisFailReason"]);
                    wOMSOrderItem.Active = StringUtils.parseInt(wSqlDataReader["Active"]);

                    wOMSOrderItem.DXFLocalUrl = StringUtils.parseString(wSqlDataReader["DXFLocalUrl"]);
                    wOMSOrderItem.NCLocalUrl = StringUtils.parseString(wSqlDataReader["NCLocalUrl"]);

                    wResultList.Add(wOMSOrderItem);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OMS_QueryOrderItemListByDeviceNo", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }

        internal int OMS_QuerySaveTotalSize(out int wErrorCode)
        {
            int wResultList = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT count(*) Number FROM {0}.oms_orderitem;", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                    wResultList = StringUtils.parseInt(wSqlDataReader["Number"]);
            }
            catch (Exception ex)
            {
                logger.Error("OMS_QuerySaveTotalSize", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
        //生成工单 如果订单类型为1（中控订单）则工单号按照年月日加四位流水号生成，如果是LES订单，工单号则为切割编号
        internal string GetOrdreNoSerial()
        {
            string wResultList = "";

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("select OrderNo from {0}.oms_orderitem where id in( SELECT max(t.ID) FROM {0}.oms_orderitem t,iplantsany.oms_order t1 where t.OrderID=t1.ID and t1.OrderType=1);", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                string wOrderNo = "";
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    wOrderNo = StringUtils.parseString(wSqlDataReader["OrderNo"]);
                }

                //202208250001
                int wIndex = 1;
                if (string.IsNullOrEmpty(wOrderNo) || wOrderNo.Length != 12)
                {
                    wIndex = 1;
                }
                else
                {
                    int wOldDay = StringUtils.parseInt(wOrderNo.Substring(6, 2));
                    int wCurentDay = DateTime.Now.Day;
                    if (wOldDay != wCurentDay)
                        wIndex = 1;
                    else
                        wIndex = StringUtils.parseInt(wOrderNo.Substring(8)) + 1;
                }
                wResultList = string.Format("{0}{1}{2}{3}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"), wIndex.ToString("0000"));
            }
            catch (Exception ex)
            {
                logger.Error("OMS_QuerySaveTotalSize", ex);
            }
            return wResultList;
        }


        public int OMS_SaveSortsysSendcasing(INFSortsysSendcasing wINFSortsysSendcasingInfo, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wparms = new Dictionary<String, Object>();

                String wSQLText = "";

                wSQLText = string.Format("INSERT INTO {0}.inf_sortsys_sendcasing(ProductionLline,SortStationNo,CutStationNo,CasingLocalUrl,MissionNo,Status,ErroMsg,CreateTime,SendTime) VALUES(@wProductionLline,@wSortStationNo,@wCutStationNo,@wCasingLocalUrl,@wMissionNo,@wStatus,@wErroMsg,@wCreateTime,@wSendTime);", wInstance);

                wparms.Clear();
                wparms.Add("wID", wINFSortsysSendcasingInfo.ID);
                wparms.Add("wProductionLline", wINFSortsysSendcasingInfo.ProductionLline);
                wparms.Add("wSortStationNo", wINFSortsysSendcasingInfo.SortStationNo);
                wparms.Add("wCutStationNo", wINFSortsysSendcasingInfo.CutStationNo);
                wparms.Add("wCasingLocalUrl", wINFSortsysSendcasingInfo.CasingLocalUrl);
                wparms.Add("wMissionNo", wINFSortsysSendcasingInfo.MissionNo);
                wparms.Add("wStatus", wINFSortsysSendcasingInfo.Status);
                wparms.Add("wErroMsg", wINFSortsysSendcasingInfo.ErroMsg);
                wparms.Add("wCreateTime", wINFSortsysSendcasingInfo.CreateTime);
                wparms.Add("wSendTime", wINFSortsysSendcasingInfo.SendTime);

                wSQLText = this.DMLChange(wSQLText);

                wINFSortsysSendcasingInfo.ID = (int)mDBPool.insert(wSQLText, wparms);

            }
            catch (Exception ex)
            {
                logger.Error("OMS_SaveSortsysSendcasing", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        internal int GetMaxOrdreNumBy()
        {
            int wResultNum = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("select max(OrderNum) as OrderNum from {0}.oms_orderitem;", wInstance);


                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                int wOrderNum = 0;
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    wOrderNum = StringUtils.parseInt(wSqlDataReader["OrderNum"]);
                }

                wResultNum = wOrderNum;
            }
            catch (Exception ex)
            {
                logger.Error("OMS_QuerySaveTotalSize", ex);
            }
            return wResultNum;
        }
    }
}

