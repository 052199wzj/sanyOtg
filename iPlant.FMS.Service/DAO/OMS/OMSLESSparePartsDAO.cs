using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace iPlant.FMC.Service
{
    public class OMSLESSparePartsDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSLESSparePartsDAO));

        #region 单实例
        private OMSLESSparePartsDAO() { }
        private static OMSLESSparePartsDAO _Instance;

        public static OMSLESSparePartsDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new OMSLESSparePartsDAO();
                return OMSLESSparePartsDAO._Instance;
            }
        }
        #endregion

        public int OMS_SaveOMSLESSpareParts(OMSLESSpareParts wOMSLESSpareParts, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wOMSLESSpareParts.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.oms_les_order_parts(ID,LesOrderID,LineNo,PartID,PartName,PartDesc,PartWidth,PartLength,PartSN,Technics,NestPlanID,RequireDoneDate,NetWeight,CutPlateWeight,RequireFactoryID,ORD_XLBG,IND_ORD,PlanAmount,Amount,ARBPL,ZNUM,ABLAD,CreateTime,EditTime,CreateID,EditID) VALUES(@wID,@wLesOrderID,@wLineNo,@wPartID,@wPartName,@wPartDesc,@wPartWidth,@wPartLength,@wPartSN,@wTechnics,@wNestPlanID,@wRequireDoneDate,@wNetWeight,@wCutPlateWeight,@wRequireFactoryID,@wORD_XLBG,@wIND_ORD,@wPlanAmount,@wAmount,@wARBPL,@wZNUM,@wABLAD,@wCreateTime,@wEditTime,@wCreateID,@wEditID);", wInstance);
                else if (wOMSLESSpareParts.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.oms_les_order_parts SET     ID=@wID,LesOrderID=@wLesOrderID,LineNo=@wLineNo,PartID=@wPartID,PartName=@wPartName,PartDesc=@wPartDesc,PartWidth=@wPartWidth,PartLength=@wPartLength,PartSN=@wPartSN,Technics=@wTechnics,NestPlanID=@wNestPlanID,RequireDoneDate=@wRequireDoneDate,NetWeight=@wNetWeight,CutPlateWeight=@wCutPlateWeight,RequireFactoryID=@wRequireFactoryID,ORD_XLBG=@wORD_XLBG,IND_ORD=@wIND_ORD,PlanAmount=@wPlanAmount,Amount=@wAmount,ARBPL=@wARBPL,ZNUM=@wZNUM,ABLAD=@wABLAD,CreateTime=@wCreateTime,EditTime=@wEditTime,CreateID=@wCreateID,EditID=@wEditID ,EditID=@wEditID WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wOMSLESSpareParts.ID);
                wParms.Add("wLesOrderID", wOMSLESSpareParts.LesOrderID);
                wParms.Add("wLineNo", wOMSLESSpareParts.LineNo);
                wParms.Add("wPartID", wOMSLESSpareParts.PartID);
                wParms.Add("wPartName", wOMSLESSpareParts.PartName);
                wParms.Add("wPartDesc", wOMSLESSpareParts.PartDesc);
                wParms.Add("wPartWidth", wOMSLESSpareParts.PartWidth);
                wParms.Add("wPartLength", wOMSLESSpareParts.PartLength);
                wParms.Add("wPartSN", wOMSLESSpareParts.PartSN);
                wParms.Add("wTechnics", wOMSLESSpareParts.Technics);
                wParms.Add("wNestPlanID", wOMSLESSpareParts.NestPlanID);
                wParms.Add("wRequireDoneDate", wOMSLESSpareParts.RequireDoneDate);
                wParms.Add("wNetWeight", wOMSLESSpareParts.NetWeight);
                wParms.Add("wCutPlateWeight", wOMSLESSpareParts.CutPlateWeight);
                wParms.Add("wRequireFactoryID", wOMSLESSpareParts.RequireFactoryID);
                wParms.Add("wORD_XLBG", wOMSLESSpareParts.ORD_XLBG);
                wParms.Add("wIND_ORD", wOMSLESSpareParts.IND_ORD);
                wParms.Add("wPlanAmount", wOMSLESSpareParts.PlanAmount);
                wParms.Add("wAmount", wOMSLESSpareParts.Amount);
                wParms.Add("wARBPL", wOMSLESSpareParts.ARBPL);
                wParms.Add("wZNUM", wOMSLESSpareParts.ZNUM);
                wParms.Add("wABLAD", wOMSLESSpareParts.ABLAD);
                wParms.Add("wCreateTime", wOMSLESSpareParts.CreateTime);
                wParms.Add("wEditTime", wOMSLESSpareParts.EditTime);
                wParms.Add("wCreateID", wOMSLESSpareParts.CreateID);
                wParms.Add("wEditID", wOMSLESSpareParts.EditID);

                wSQLText = this.DMLChange(wSQLText);

                if (wOMSLESSpareParts.ID <= 0)
                    wOMSLESSpareParts.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("OMS_SaveOMSLESSpareParts", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int OMS_DeleteOMSLESSparePartsList(List<OMSLESSpareParts> wOMSLESSparePartsList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wOMSLESSparePartsList != null && wOMSLESSparePartsList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wOMSLESSparePartsList.Count; i++)
                    {
                        if (i == wOMSLESSparePartsList.Count - 1)
                            wStringBuilder.Append(wOMSLESSparePartsList[i].ID);
                        else
                            wStringBuilder.Append(wOMSLESSparePartsList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.oms_les_order_parts WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OMS_DeleteOMSLESSparePartsList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<OMSLESSpareParts> OMS_QueryOMSLESSparePartsList(int wID, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, string wPartID, string wPartName, string wTechnics, string wORD_XLBG, string wABLAD,int wLesOrderID, out int wErrorCode)
        {
            List<OMSLESSpareParts> wResultList = new List<OMSLESSpareParts>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM oms_les_order_parts t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                  + " and(@wLesOrderID <=0 or t.LesOrderID= @wLesOrderID)"
                + " and(@wPartID is null or @wPartID = '' or t.PartID= @wPartID)"
                + " and(@wPartName is null or @wPartName = '' or t.PartName= @wPartName)"
                + " and(@wABLAD is null or @wABLAD = '' or t.ABLAD= @wABLAD)"
                + " and(@wTechnics is null or @wTechnics = '' or t.Technics= @wTechnics)"
                + " and(@wORD_XLBG is null or @wORD_XLBG = '' or t.ORD_XLBG= @wORD_XLBG)"
                + " and(@wStartTime <= '2010-1-1' or t.CreateTime>= @wStartTime)"
                + " and(@wEndTime <= '2010-1-1' or t.CreateTime<= @wEndTime)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wPartID", wPartID);
                wParms.Add("wPartName", wPartName);
                wParms.Add("wTechnics", wTechnics);
                wParms.Add("wORD_XLBG", wORD_XLBG);
                wParms.Add("wABLAD", wABLAD);
                wParms.Add("wLesOrderID", wLesOrderID);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {

                    OMSLESSpareParts wOMSLESSpareParts = new OMSLESSpareParts();
                    wOMSLESSpareParts.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wOMSLESSpareParts.LesOrderID = StringUtils.parseInt(wSqlDataReader["LesOrderID"]);
                    wOMSLESSpareParts.LineNo = StringUtils.parseString(wSqlDataReader["LineNo"]);
                    wOMSLESSpareParts.PartID = StringUtils.parseString(wSqlDataReader["PartID"]);
                    wOMSLESSpareParts.PartName = StringUtils.parseString(wSqlDataReader["PartName"]);
                    wOMSLESSpareParts.PartDesc = StringUtils.parseString(wSqlDataReader["PartDesc"]);
                    wOMSLESSpareParts.PartWidth = StringUtils.parseDouble(wSqlDataReader["PartWidth"]);
                    wOMSLESSpareParts.PartLength = StringUtils.parseDouble(wSqlDataReader["PartLength"]);
                    wOMSLESSpareParts.PartSN = StringUtils.parseString(wSqlDataReader["PartSN"]);
                    wOMSLESSpareParts.Technics = StringUtils.parseString(wSqlDataReader["Technics"]);
                    wOMSLESSpareParts.NestPlanID = StringUtils.parseString(wSqlDataReader["NestPlanID"]);
                    wOMSLESSpareParts.RequireDoneDate = StringUtils.parseDate(wSqlDataReader["RequireDoneDate"]);
                    wOMSLESSpareParts.NetWeight = StringUtils.parseDouble(wSqlDataReader["NetWeight"]);
                    wOMSLESSpareParts.CutPlateWeight = StringUtils.parseDouble(wSqlDataReader["CutPlateWeight"]);
                    wOMSLESSpareParts.RequireFactoryID = StringUtils.parseString(wSqlDataReader["RequireFactoryID"]);
                    wOMSLESSpareParts.ORD_XLBG = StringUtils.parseString(wSqlDataReader["ORD_XLBG"]);
                    wOMSLESSpareParts.IND_ORD = StringUtils.parseString(wSqlDataReader["IND_ORD"]);
                    wOMSLESSpareParts.PlanAmount = StringUtils.parseDouble(wSqlDataReader["PlanAmount"]);
                    wOMSLESSpareParts.Amount = StringUtils.parseDouble(wSqlDataReader["Amount"]);
                    wOMSLESSpareParts.ARBPL = StringUtils.parseString(wSqlDataReader["ARBPL"]);
                    wOMSLESSpareParts.ZNUM = StringUtils.parseString(wSqlDataReader["ZNUM"]);
                    wOMSLESSpareParts.ABLAD = StringUtils.parseString(wSqlDataReader["ABLAD"]);
                    wOMSLESSpareParts.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wOMSLESSpareParts.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wOMSLESSpareParts.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wOMSLESSpareParts.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wOMSLESSpareParts.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wOMSLESSpareParts.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wResultList.Add(wOMSLESSpareParts);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OMS_QueryOMSLESSparePartsList",
                       ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

