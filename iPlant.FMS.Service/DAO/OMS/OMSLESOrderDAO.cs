using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace iPlant.FMC.Service
{
    public class OMSLESOrderDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSLESOrderDAO));

        #region 单实例
        private OMSLESOrderDAO() { }
        private static OMSLESOrderDAO _Instance;

        public static OMSLESOrderDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new OMSLESOrderDAO();
                return OMSLESOrderDAO._Instance;
            }
        }
        #endregion

        public int OMS_SaveOMSLESOrder(OMSLESOrder wOMSLESOrder, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wOMSLESOrder.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.oms_les_order(NestID,GroupID,Rate,FactoryID,ExMaterielID,MaterielName,MaterielTypeID,Texture,Thickness,Width,Length,MTWeight,NestDate,BookSheet,OptionID,RequireDoneDate,NestTaskID,CutLength,WorkTime,JsonMap,JsonMapError,State,NCUrl,DXFUrl,DXFGetState,DXFGetFailReason,NCGetState,NCGetFailReason,IssueState,IssueDate,CreateTime,EditTime,CreateID,EditID,DXFUrlName,NCUrlName,NCLocalUrl,DXFLocalUrl) VALUES(@wNestID,@wGroupID,@wRate,@wFactoryID,@wExMaterielID,@wMaterielName,@wMaterielTypeID,@wTexture,@wThickness,@wWidth,@wLength,@wMTWeight,@wNestDate,@wBookSheet,@wOptionID,@wRequireDoneDate,@wNestTaskID,@wCutLength,@wWorkTime,@wJsonMap,@wJsonMapError,@wState,@wNCUrl,@wDXFUrl,@wDXFGetState,@wDXFGetFailReason,@wNCGetState,@wNCGetFailReason,@wIssueState,@wIssueDate,@wCreateTime,@wEditTime,@wCreateID,@wEditID,@wDXFUrlName,@wNCUrlName,@wNCLocalUrl,@wDXFLocalUrl);", wInstance);
                else if (wOMSLESOrder.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.oms_les_order SET NestID=@wNestID,GroupID=@wGroupID,Rate=@wRate,FactoryID=@wFactoryID,ExMaterielID=@wExMaterielID,MaterielName=@wMaterielName,MaterielTypeID=@wMaterielTypeID,Texture=@wTexture,Thickness=@wThickness,Width=@wWidth,Length=@wLength,MTWeight=@wMTWeight,NestDate=@wNestDate,BookSheet=@wBookSheet,OptionID=@wOptionID,RequireDoneDate=@wRequireDoneDate,NestTaskID=@wNestTaskID,CutLength=@wCutLength,WorkTime=@wWorkTime,JsonMap=@wJsonMap,JsonMapError=@wJsonMapError,State=@wState,NCUrl=@wNCUrl,DXFUrl=@wDXFUrl,DXFGetState=@wDXFGetState,DXFGetFailReason=@wDXFGetFailReason ,NCGetState=@wNCGetState,NCGetFailReason=@wNCGetFailReason,IssueState=@wIssueState,IssueDate=@wIssueDate ,CreateTime=@wCreateTime,EditTime=@wEditTime ,CreateID=@wCreateID ,EditID=@wEditID ,DXFUrlName=@wDXFUrlName ,NCUrlName=@wNCUrlName ,NCLocalUrl=@wNCLocalUrl,DXFLocalUrl=@wDXFLocalUrl WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wOMSLESOrder.ID);
                wParms.Add("wNestID", wOMSLESOrder.NestID);
                wParms.Add("wGroupID", wOMSLESOrder.GroupID);
                wParms.Add("wRate", wOMSLESOrder.Rate);
                wParms.Add("wFactoryID", wOMSLESOrder.FactoryID);
                wParms.Add("wExMaterielID", wOMSLESOrder.ExMaterielID);
                wParms.Add("wMaterielName", wOMSLESOrder.MaterielName);
                wParms.Add("wMaterielTypeID", wOMSLESOrder.MaterielTypeID);
                wParms.Add("wTexture", wOMSLESOrder.Texture);
                wParms.Add("wCreateID", wOMSLESOrder.CreateID);
                wParms.Add("wCreateTime", wOMSLESOrder.CreateTime);
                wParms.Add("wEditID", wOMSLESOrder.EditID);
                wParms.Add("wEditTime", wOMSLESOrder.EditTime);
                wParms.Add("wThickness", wOMSLESOrder.Thickness);
                wParms.Add("wWidth", wOMSLESOrder.Width);
                wParms.Add("wLength", wOMSLESOrder.Length);
                wParms.Add("wMTWeight", wOMSLESOrder.MTWeight);
                wParms.Add("wNestDate", wOMSLESOrder.NestDate);
                wParms.Add("wBookSheet", wOMSLESOrder.BookSheet);
                wParms.Add("wOptionID", wOMSLESOrder.OptionID);
                wParms.Add("wRequireDoneDate", wOMSLESOrder.RequireDoneDate);
                wParms.Add("wNestTaskID", wOMSLESOrder.NestTaskID);
                wParms.Add("wCutLength", wOMSLESOrder.CutLength);
                wParms.Add("wWorkTime", wOMSLESOrder.WorkTime);
                wParms.Add("wJsonMap", wOMSLESOrder.JsonMap);
                wParms.Add("wJsonMapError", wOMSLESOrder.JsonMapError);
                wParms.Add("wState", wOMSLESOrder.State);
                wParms.Add("wNCUrl", wOMSLESOrder.NCUrl);
                wParms.Add("wDXFUrl", wOMSLESOrder.DXFUrl);
                wParms.Add("wDXFGetState", wOMSLESOrder.DXFGetState);
                wParms.Add("wDXFGetFailReason", wOMSLESOrder.DXFGetFailReason);
                wParms.Add("wNCGetState", wOMSLESOrder.NCGetState);
                wParms.Add("wNCGetFailReason", wOMSLESOrder.NCGetFailReason);
                wParms.Add("wIssueState", wOMSLESOrder.IssueState);
                wParms.Add("wIssueDate", wOMSLESOrder.IssueDate);
                wParms.Add("wDXFUrlName", wOMSLESOrder.DXFUrlName);
                wParms.Add("wNCUrlName", wOMSLESOrder.NCUrlName);
                wParms.Add("wNCLocalUrl", wOMSLESOrder.NCLocalUrl);
                wParms.Add("wDXFLocalUrl", wOMSLESOrder.DXFLocalUrl);

                wSQLText = this.DMLChange(wSQLText);

                if (wOMSLESOrder.ID <= 0)
                    wOMSLESOrder.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("OMS_SaveOMSLESOrder", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int OMS_DeleteOMSLESOrderList(List<OMSLESOrder> wOMSLESOrderList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wOMSLESOrderList != null && wOMSLESOrderList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wOMSLESOrderList.Count; i++)
                    {
                        if (i == wOMSLESOrderList.Count - 1)
                            wStringBuilder.Append(wOMSLESOrderList[i].ID);
                        else
                            wStringBuilder.Append(wOMSLESOrderList[i].ID + ",");
                    }
                    //String wSQLText = string.Format("DELETE From {1}.oms_les_order WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    String wSQLText = string.Format("UPDATE  {1}.oms_les_order SET Displayed=2 WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OMS_DeleteOMSLESOrderList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<OMSLESOrder> OMS_QueryOMSLESOrderList(int wID, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, string wNestID, int wOptionID, double wThickness, int wDXFGetState, int wNCGetState, int wIssueState, int wDisplayed,out int wErrorCode)
        {
            List<OMSLESOrder> wResultList = new List<OMSLESOrder>();
            wErrorCode = 0;
            if (wDisplayed == 0)
                wDisplayed = 1;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM oms_les_order t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                   + " and(@wDXFGetState <= 0 or t.DXFGetState= @wDXFGetState)"
                + " and(@wOptionID < 0 or t.OptionID= @wOptionID)"
                + " and(@wThickness < 0 or t.Thickness= @wThickness)"
                + " and(@wNCGetState <= 0 or t.NCGetState= @wNCGetState)"
                + " and(@wIssueState <= 0 or t.IssueState= @wIssueState)"
                + " and(@wNestID is null or @wNestID = '' or t.NestID= @wNestID)"
                + " and(@wDisplayed < 0 or t.Displayed= @wDisplayed)"
                + " and(@wStartTime <= '2010-1-1' or t.CreateTime>= @wStartTime)"
                + " and(@wEndTime <= '2010-1-1' or t.CreateTime<= @wEndTime)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wNestID", wNestID);
                wParms.Add("wOptionID", wOptionID);
                wParms.Add("wThickness", wThickness);
                wParms.Add("wIssueState", wIssueState);
                wParms.Add("wNCGetState", wNCGetState);
                wParms.Add("wDXFGetState", wDXFGetState);
                wParms.Add("wDisplayed", wDisplayed);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    OMSLESOrder wOMSLESOrder = new OMSLESOrder();
                    wOMSLESOrder.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wOMSLESOrder.NestID = StringUtils.parseString(wSqlDataReader["NestID"]);
                    wOMSLESOrder.GroupID = StringUtils.parseString(wSqlDataReader["GroupID"]);
                    wOMSLESOrder.Rate = StringUtils.parseDouble(wSqlDataReader["Rate"]);
                    wOMSLESOrder.FactoryID = StringUtils.parseString(wSqlDataReader["FactoryID"]);
                    wOMSLESOrder.ExMaterielID = StringUtils.parseString(wSqlDataReader["ExMaterielID"]);
                    wOMSLESOrder.MaterielName = StringUtils.parseString(wSqlDataReader["MaterielName"]);
                    wOMSLESOrder.MaterielTypeID = StringUtils.parseString(wSqlDataReader["MaterielTypeID"]);
                    wOMSLESOrder.Texture = StringUtils.parseString(wSqlDataReader["Texture"]);
                    wOMSLESOrder.Thickness = StringUtils.parseDouble(wSqlDataReader["Thickness"]);
                    wOMSLESOrder.Width = StringUtils.parseDouble(wSqlDataReader["Width"]);
                    wOMSLESOrder.Length = StringUtils.parseDouble(wSqlDataReader["Length"]);
                    wOMSLESOrder.MTWeight = StringUtils.parseDouble(wSqlDataReader["MTWeight"]);
                    wOMSLESOrder.NestDate = StringUtils.parseDate(wSqlDataReader["NestDate"]);
                    wOMSLESOrder.BookSheet = StringUtils.parseString(wSqlDataReader["BookSheet"]);
                    wOMSLESOrder.OptionID = StringUtils.parseInt(wSqlDataReader["OptionID"]);
                    wOMSLESOrder.RequireDoneDate = StringUtils.parseDate(wSqlDataReader["RequireDoneDate"]);
                    wOMSLESOrder.NestTaskID = StringUtils.parseString(wSqlDataReader["NestTaskID"]);
                    wOMSLESOrder.CutLength = StringUtils.parseDouble(wSqlDataReader["CutLength"]);
                    wOMSLESOrder.WorkTime = StringUtils.parseDouble(wSqlDataReader["WorkTime"]);
                    wOMSLESOrder.JsonMap = StringUtils.parseString(wSqlDataReader["JsonMap"]);
                    wOMSLESOrder.JsonMapError = StringUtils.parseString(wSqlDataReader["JsonMapError"]);
                    wOMSLESOrder.State = StringUtils.parseString(wSqlDataReader["State"]);
                    wOMSLESOrder.NCUrl = StringUtils.parseString(wSqlDataReader["NCUrl"]);
                    wOMSLESOrder.DXFUrl = StringUtils.parseString(wSqlDataReader["DXFUrl"]);
                    wOMSLESOrder.DXFGetState = StringUtils.parseInt(wSqlDataReader["DXFGetState"]);
                    wOMSLESOrder.DXFGetFailReason = StringUtils.parseString(wSqlDataReader["DXFGetFailReason"]);
                    wOMSLESOrder.NCGetState = StringUtils.parseInt(wSqlDataReader["NCGetState"]);
                    wOMSLESOrder.NCGetFailReason = StringUtils.parseString(wSqlDataReader["NCGetFailReason"]);
                    wOMSLESOrder.IssueState = StringUtils.parseInt(wSqlDataReader["IssueState"]);
                    wOMSLESOrder.IssueDate = StringUtils.parseDate(wSqlDataReader["IssueDate"]);
                    wOMSLESOrder.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wOMSLESOrder.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wOMSLESOrder.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wOMSLESOrder.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wOMSLESOrder.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wOMSLESOrder.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wOMSLESOrder.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wOMSLESOrder.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wOMSLESOrder.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wOMSLESOrder.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wOMSLESOrder.DXFUrlName = StringUtils.parseString(wSqlDataReader["DXFUrlName"]);
                    wOMSLESOrder.NCUrlName = StringUtils.parseString(wSqlDataReader["NCUrlName"]);
                    wOMSLESOrder.NCLocalUrl = StringUtils.parseString(wSqlDataReader["NCLocalUrl"]);
                    wOMSLESOrder.DXFLocalUrl = StringUtils.parseString(wSqlDataReader["DXFLocalUrl"]);
                    wOMSLESOrder.Displayed = StringUtils.parseInt(wSqlDataReader["Displayed"]);
                    wResultList.Add(wOMSLESOrder);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OMS_QueryOMSLESOrderList",
                       ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

