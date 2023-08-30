using System;
using System.Collections.Generic;
using System.Text;
using iPlant.Common.Tools;
using iPlant.FMC.Service;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class OMSOrderDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSOrderDAO));

        #region 单实例
        private OMSOrderDAO() { }
        private static OMSOrderDAO _Instance;

        public static OMSOrderDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new OMSOrderDAO();
                return OMSOrderDAO._Instance;
            }
        }
        #endregion

        public int OMS_SaveOMSOrder(OMSOrder wOMSOrder, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wparms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wOMSOrder.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.oms_order(NCFileUri,DXFFileUri,CuttingNumber,NestingNumber,CutType,DemandFinishDate,CutTimes,NestDate,PlateMaterialNo,StructuralPartID,Texture,Thickness,ManeuverTime,Plate,CutLength,Material,TechThickness,Gas,CuttingMouth,CreateID,CreateTime,EditID,EditTime,CutSpeed,Flag,FinishFQTY,FinishTime,OrderType,ManeuverTimes,Utilization,LesOrderID) VALUES(@wNCFileUri,@wDXFFileUri,@wCuttingNumber,@wNestingNumber,@wCutType,@wDemandFinishDate,@wCutTimes,@wNestDate,@wPlateMaterialNo,@wStructuralPartID,@wTexture,@wThickness,@wManeuverTime,@wplate,@wCutLength,@wMaterial,@wTechThickness,@wGas,@wCuttingMouth,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wCutSpeed,@wFlag,@wFinishFQTY,@wFinishTime,@wOrderType,@wManeuverTimes,@wUtilization,@wLesOrderID);", wInstance);
                else if (wOMSOrder.ID > 0)
                    wSQLText = string.Format("UpDATE {0}.oms_order SET NCFileUri=@wNCFileUri,DXFFileUri=@wDXFFileUri,CuttingNumber=@wCuttingNumber,NestingNumber=@wNestingNumber,CutType=@wCutType,DemandFinishDate=@wDemandFinishDate,CutTimes=@wCutTimes,NestDate=@wNestDate,PlateMaterialNo=@wPlateMaterialNo,StructuralPartID=@wStructuralPartID,Texture=@wTexture,Thickness=@wThickness,ManeuverTime=@wManeuverTime,Plate=@wPlate,CutLength=@wCutLength,Material=@wMaterial,TechThickness=@wTechThickness,Gas=@wGas,CuttingMouth=@wCuttingMouth,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,CutSpeed=@wCutSpeed,Flag=@wFlag,FinishFQTY=@wFinishFQTY,FinishTime=@wFinishTime,OrderType=@wOrderType,ManeuverTimes=@wManeuverTimes ,Utilization=@wUtilization ,LesOrderID=@wLesOrderID WHERE ID=@wID", wInstance);

                wparms.Clear();
                wparms.Add("wID", wOMSOrder.ID);
                wparms.Add("wNCFileUri", wOMSOrder.NCFileUri);
                wparms.Add("wDXFFileUri", wOMSOrder.DXFFileUri);
                wparms.Add("wCuttingNumber", wOMSOrder.CuttingNumber);
                wparms.Add("wNestingNumber", wOMSOrder.NestingNumber);
                wparms.Add("wCutType", wOMSOrder.CutType);
                wparms.Add("wDemandFinishDate", wOMSOrder.DemandFinishDate);
                wparms.Add("wCutTimes", wOMSOrder.CutTimes);
                wparms.Add("wNestDate", wOMSOrder.NestDate);
                wparms.Add("wPlateMaterialNo", wOMSOrder.PlateMaterialNo);
                wparms.Add("wStructuralPartID", wOMSOrder.StructuralPartID);
                wparms.Add("wTexture", wOMSOrder.Texture);
                wparms.Add("wThickness", wOMSOrder.Thickness);
                wparms.Add("wManeuverTime", wOMSOrder.ManeuverTime);
                wparms.Add("wPlate", wOMSOrder.Plate);
                wparms.Add("wCutLength", wOMSOrder.CutLength);
                wparms.Add("wMaterial", wOMSOrder.Material);
                wparms.Add("wTechThickness", wOMSOrder.TechThickness);
                wparms.Add("wGas", wOMSOrder.Gas);
                wparms.Add("wCuttingMouth", wOMSOrder.CuttingMouth);
                wparms.Add("wCreateID", wOMSOrder.CreateID);
                wparms.Add("wCreateTime", wOMSOrder.CreateTime);
                wparms.Add("wEditID", wOMSOrder.EditID);
                wparms.Add("wEditTime", wOMSOrder.EditTime);
                wparms.Add("wCutSpeed", wOMSOrder.CutSpeed);
                wparms.Add("wFlag", wOMSOrder.Flag);
                wparms.Add("wFinishFQTY", wOMSOrder.FinishFQTY);
                wparms.Add("wFinishTime", wOMSOrder.FinishTime);
                wparms.Add("wOrderType", wOMSOrder.OrderType);
                wparms.Add("wLesOrderID", wOMSOrder.LesOrderID);
                wparms.Add("wManeuverTimes", wOMSOrder.ManeuverTimes);
                wparms.Add("wUtilization", wOMSOrder.Utilization);
                wSQLText = this.DMLChange(wSQLText);

                if (wOMSOrder.ID <= 0)
                    wOMSOrder.ID = (int)mDBPool.insert(wSQLText, wparms);
                else
                    mDBPool.update(wSQLText, wparms);
            }
            catch (Exception ex)
            {
                logger.Error("OMS_SaveOMSOrder", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        //10.11更新数据删除逻辑，由删除变更为更新
        public int OMS_DeleteOMSOrderList(List<OMSOrder> wOMSOrderList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wOMSOrderList != null && wOMSOrderList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wOMSOrderList.Count; i++)
                    {
                        if (i == wOMSOrderList.Count - 1)
                            wStringBuilder.Append(wOMSOrderList[i].ID);
                        else
                            wStringBuilder.Append(wOMSOrderList[i].ID + ",");
                    }
                    //String wSQLText = string.Format("DELETE From {1}.oms_order WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    String wSQLText = string.Format("UPDATE  {1}.oms_order SET Displayed=2 WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wparms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wparms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OMS_DeleteOMSOrderList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        //10.11更新数据删除逻辑，由删除变更为更新
        public int OMS_DeleteOMSOrderItemList(List<OMSOrderItem> wOrderItemList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wOrderItemList != null && wOrderItemList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wOrderItemList.Count; i++)
                    {
                        if (i == wOrderItemList.Count - 1)
                            wStringBuilder.Append(wOrderItemList[i].ID);
                        else
                            wStringBuilder.Append(wOrderItemList[i].ID + ",");
                    }
                    //String wSQLText = string.Format("DELETE From {1}.oms_orderitem WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    String wSQLText = string.Format("UPDATE  {1}.oms_orderitem SET Displayed=2 WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wparms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wparms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OMS_DeleteOMSOrderList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<OMSOrder> OMS_QueryOMSOrderList(int wID, string wCuttingNumber, string wNestingNumber, int wCutType, string wPlateMaterialNo, int wStructuralpartID, string wGas, string wCuttingMouth, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, int wOrderType,int wDisplayed, out int wErrorCode)
        {
            List<OMSOrder> wResultList = new List<OMSOrder>();
            wErrorCode = 0;
            try
            {

                if (wOrderType == 1 || wOrderType == 2)
                {
                    wResultList.AddRange(QueryOMSOrderList(wID, wCuttingNumber, wNestingNumber, wCutType, wPlateMaterialNo, wStructuralpartID, wGas, wCuttingMouth, wStartTime, wEndTime, wPagination, wOrderType, wDisplayed, out wErrorCode));
                }
                else
                {
                    wResultList.AddRange(QueryOMSOrderList(wID, wCuttingNumber, wNestingNumber, wCutType, wPlateMaterialNo, wStructuralpartID, wGas, wCuttingMouth, wStartTime, wEndTime, wPagination, 1, wDisplayed, out wErrorCode));
                    wResultList.AddRange(QueryOMSOrderList(wID, wCuttingNumber, wNestingNumber, wCutType, wPlateMaterialNo, wStructuralpartID, wGas, wCuttingMouth, wStartTime, wEndTime, wPagination, 2, wDisplayed,out wErrorCode));
                }
              
            }
            catch (Exception ex)
            {
                logger.Error("OMS_QueryOMSOrderList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }

        private List<OMSOrder> QueryOMSOrderList(int wID, string wCuttingNumber, string wNestingNumber, int wCutType, string wPlateMaterialNo, int wStructuralpartID, string wGas, string wCuttingMouth, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, int wOrderType,int wDisplayed, out int wErrorCode)
        {
            List<OMSOrder> wResultList = new List<OMSOrder>();
            wErrorCode = 0;
            if (wDisplayed == 0)
                wDisplayed = 1;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                string wSQLText = "";

                switch (wOrderType)
                {
                    //10.9号 wzj
                    //新增订单报表查询字段 t1.Code 物料编码
                    //10.11 wzj
                    //新增查询限制条件  Displayed=1
                    case 1:
                        wSQLText = string.Format("SELECT t.*,t1.Name StructuralPartName,t1.Code ,t1.MaterialNo StructuralPartMaterialNo,t1.MaterialTypeNo StructuralPartMaterialTypeNo,t1.Length,t1.Width,t1.Height,t1.Weight,t4.Name Creator,t5.Name Editor,t6.Name MaterialPointName FROM {0}.oms_order t "
                       + " left join {0}.fpc_structuralpart t1 on t.StructuralPartID=t1.ID "
                       + " left join {0}.mbs_user t4 on t.CreateID=t4.ID "
                       + " left join {0}.mbs_user t5 on t.EditID=t5.ID "
                       + " left join {0}.mss_materialpoint t6 on t.PlateMaterialNo=t6.ID "
                       + "WHERE 1=1"
                   + " and(@wID <=0 or t.ID= @wID)"
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
                        break;

                    case 2:
                        wSQLText = string.Format("SELECT t.*,t1.Length,t1.Width,t1.MTWeight Weight,t4.Name Creator,t5.Name Editor,t6.Name MaterialPointName FROM {0}.oms_order t "
                      + " left join {0}.oms_les_order t1 on t.LesOrderID=t1.ID "
                      + " left join {0}.mbs_user t4 on t.CreateID=t4.ID "
                      + " left join {0}.mbs_user t5 on t.EditID=t5.ID "
                      + " left join {0}.mss_materialpoint t6 on t.PlateMaterialNo=t6.ID "
                      + "WHERE 1=1"
                  + " and(@wID <=0 or t.ID= @wID)"
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
                        break;

                    default:
                        break;
                }

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wCuttingNumber", wCuttingNumber);
                wParms.Add("wNestingNumber", wNestingNumber);
                wParms.Add("wCutType", wCutType);
                wParms.Add("wPlateMaterialNo", wPlateMaterialNo);
                wParms.Add("wStructuralPartID", wStructuralpartID);
                wParms.Add("wGas", wGas);
                wParms.Add("wCuttingMouth", wCuttingMouth);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);
                wParms.Add("wOrderType", wOrderType);
                wParms.Add("wDisplayed", wDisplayed);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    OMSOrder wOMSOrder = new OMSOrder();

                    wOMSOrder.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wOMSOrder.NCFileUri = StringUtils.parseString(wSqlDataReader["NCFileUri"]);
                    wOMSOrder.DXFFileUri = StringUtils.parseString(wSqlDataReader["DXFFileUri"]);
                    wOMSOrder.CuttingNumber = StringUtils.parseString(wSqlDataReader["CuttingNumber"]);
                    wOMSOrder.NestingNumber = StringUtils.parseString(wSqlDataReader["NestingNumber"]);
                    wOMSOrder.CutType = StringUtils.parseInt(wSqlDataReader["CutType"]);
                    wOMSOrder.DemandFinishDate = StringUtils.parseDate(wSqlDataReader["DemandFinishDate"]);
                    wOMSOrder.CutTimes = StringUtils.parseInt(wSqlDataReader["CutTimes"]);
                    wOMSOrder.NestDate = StringUtils.parseDate(wSqlDataReader["NestDate"]);
                    wOMSOrder.OrderType = StringUtils.parseInt(wSqlDataReader["OrderType"]);
                    wOMSOrder.PlateMaterialNo = StringUtils.parseString(wSqlDataReader["PlateMaterialNo"]);
                    wOMSOrder.StructuralPartID = StringUtils.parseInt(wSqlDataReader["StructuralPartID"]);
                    if (wOMSOrder.OrderType == 1)
                    {
                        wOMSOrder.StructuralPartName = StringUtils.parseString(wSqlDataReader["StructuralPartName"]);
                        wOMSOrder.StructuralPartMaterialNo = StringUtils.parseString(wSqlDataReader["StructuralPartMaterialNo"]);
                        wOMSOrder.StructuralPartMaterialTypeNo = StringUtils.parseString(wSqlDataReader["StructuralPartMaterialTypeNo"]);
                        wOMSOrder.Height = StringUtils.parseInt(wSqlDataReader["Height"]);
                    }
                    wOMSOrder.Length = StringUtils.parseInt(wSqlDataReader["Length"]);
                    wOMSOrder.Width = StringUtils.parseInt(wSqlDataReader["Width"]);
                    wOMSOrder.Weight = StringUtils.parseDouble(wSqlDataReader["Weight"]);
                    wOMSOrder.Texture = StringUtils.parseString(wSqlDataReader["Texture"]);
                    wOMSOrder.Thickness = StringUtils.parseDouble(wSqlDataReader["Thickness"]);
                    wOMSOrder.ManeuverTime = StringUtils.parseDate(wSqlDataReader["ManeuverTime"]);
                    wOMSOrder.Plate = StringUtils.parseString(wSqlDataReader["Plate"]);
                    wOMSOrder.CutLength = StringUtils.parseDouble(wSqlDataReader["CutLength"]);
                    wOMSOrder.Material = StringUtils.parseString(wSqlDataReader["Material"]);
                    wOMSOrder.TechThickness = StringUtils.parseString(wSqlDataReader["TechThickness"]);
                    wOMSOrder.Gas = StringUtils.parseString(wSqlDataReader["Gas"]);
                    wOMSOrder.CuttingMouth = StringUtils.parseString(wSqlDataReader["CuttingMouth"]);
                    wOMSOrder.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wOMSOrder.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wOMSOrder.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wOMSOrder.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wOMSOrder.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wOMSOrder.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wOMSOrder.CutSpeed = StringUtils.parseDouble(wSqlDataReader["CutSpeed"]);
                    wOMSOrder.Flag = StringUtils.parseInt(wSqlDataReader["Flag"]);
                    wOMSOrder.FinishFQTY = StringUtils.parseInt(wSqlDataReader["FinishFQTY"]);
                    wOMSOrder.FinishTime = StringUtils.parseDate(wSqlDataReader["FinishTime"]);
                    wOMSOrder.MaterialPointName = StringUtils.parseString(wSqlDataReader["MaterialPointName"]);
                    wOMSOrder.ManeuverTimes = StringUtils.parseInt(wSqlDataReader["ManeuverTimes"]);
                    wOMSOrder.Utilization = StringUtils.parseInt(wSqlDataReader["Utilization"]);
                    wOMSOrder.LesOrderID = StringUtils.parseInt(wSqlDataReader["LesOrderID"]);
                    wOMSOrder.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wOMSOrder.Displayed = StringUtils.parseInt(wSqlDataReader["Displayed"]);
                    wResultList.Add(wOMSOrder);
                }
            }
            catch (Exception ex)
            {
                logger.Error("QueryOMSOrderList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

           
            return wResultList;
        }

        //生成订单 如果订单类型为1（中控订单）则切割编号按照年月日加四位流水号生成，
        internal string GetOrdreNoSerial()
        {
            string wResultList = "";

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("select CuttingNumber from {0}.oms_order where OrderType=1 order by ID desc limit 1;", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                string wCuttingNumber = "";
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    wCuttingNumber = StringUtils.parseString(wSqlDataReader["CuttingNumber"]);
                }

                //202208250001
                int wIndex = 1;
                if (string.IsNullOrEmpty(wCuttingNumber) || wCuttingNumber.Length != 12)
                {
                    wIndex = 1;
                }
                else
                {
                    int wOldDay = StringUtils.parseInt(wCuttingNumber.Substring(6, 2));
                    int wCurentDay = DateTime.Now.Day;
                    if (wOldDay != wCurentDay)
                        wIndex = 1;
                    else
                        wIndex = StringUtils.parseInt(wCuttingNumber.Substring(8)) + 1;
                }
                wResultList = string.Format("{0}{1}{2}{3}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"), wIndex.ToString("0000"));
            }
            catch (Exception ex)
            {
                logger.Error("OMS_QuerySaveTotalSize", ex);
            }
            return wResultList;
        }

        internal bool IsExsit(string cuttingNumber, out int wErrorCode)
        {
            bool wResultList = false;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("select CuttingNumber from {0}.oms_order where CuttingNumber=@wCuttingNumber;", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                wParms.Add("wCuttingNumber", cuttingNumber);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                string wCuttingNumber = "";
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    wCuttingNumber = StringUtils.parseString(wSqlDataReader["CuttingNumber"]);
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error("IsExsit", ex);
            }
            return wResultList;
        }
    }
}

