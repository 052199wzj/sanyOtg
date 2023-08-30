using System;
using System.Collections.Generic;
using System.Text;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class OMSSparePartsDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSSparePartsDAO));

        #region 单实例
        private OMSSparePartsDAO() { }
        private static OMSSparePartsDAO _Instance;

        public static OMSSparePartsDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new OMSSparePartsDAO();
                return OMSSparePartsDAO._Instance;
            }
        }
        #endregion

        public int OMS_SaveOMSSpareParts(OMSSpareParts wOMSSpareParts, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wOMSSpareParts.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.oms_spareparts(Code,Name,Remark,Active,CreateID,CreateTime,EditID,EditTime,OrderID,PlanNumber,Length,Width,Weigth,PieceNo,FQTY,QRCode,Type,PartType,LesOrderID) VALUES(@wCode,@wName,@wRemark,@wActive,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wOrderID,@wPlanNumber,@wLength,@wWidth,@wWeigth,@wPieceNo,@wFQTY,@wQRCode,@wType,@wPartType,@wLesOrderID);", wInstance);
                else if (wOMSSpareParts.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.oms_spareparts SET Code=@wCode,Name=@wName,Remark=@wRemark,Active=@wActive,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,OrderID=@wOrderID,PlanNumber=@wPlanNumber,Length=@wLength,Width=@wWidth,Weigth=@wWeigth,PieceNo=@wPieceNo,FQTY=@wFQTY,QRCode=@wQRCode,Type=@wType,PartType=@wPartType,LesOrderID=@wLesOrderID WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wOMSSpareParts.ID);
                wParms.Add("wCode", wOMSSpareParts.Code);
                wParms.Add("wName", wOMSSpareParts.Name);
                wParms.Add("wRemark", wOMSSpareParts.Remark);
                wParms.Add("wActive", wOMSSpareParts.Active);
                wParms.Add("wCreateID", wOMSSpareParts.CreateID);
                wParms.Add("wCreateTime", wOMSSpareParts.CreateTime);
                wParms.Add("wEditID", wOMSSpareParts.EditID);
                wParms.Add("wEditTime", wOMSSpareParts.EditTime);
                wParms.Add("wOrderID", wOMSSpareParts.OrderID);
                wParms.Add("wPlanNumber", wOMSSpareParts.PlanNumber);
                wParms.Add("wLength", wOMSSpareParts.Length);
                wParms.Add("wWidth", wOMSSpareParts.Width);
                wParms.Add("wWeigth", wOMSSpareParts.Weigth);
                wParms.Add("wPieceNo", wOMSSpareParts.PieceNo);
                wParms.Add("wFQTY", wOMSSpareParts.FQTY);
                wParms.Add("wQRCode", wOMSSpareParts.QRCode);
                wParms.Add("wType", wOMSSpareParts.Type);
                wParms.Add("wPartType", wOMSSpareParts.PartType);
                wParms.Add("wLesOrderID", wOMSSpareParts.LesOrderID);

                wSQLText = this.DMLChange(wSQLText);

                if (wOMSSpareParts.ID <= 0)
                    wOMSSpareParts.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(OMS_SaveOMSSpareParts)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("OMS_SaveOMSSpareParts", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int OMS_DeleteOMSSparePartsList(List<OMSSpareParts> wOMSSparePartsList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wOMSSparePartsList != null && wOMSSparePartsList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wOMSSparePartsList.Count; i++)
                    {
                        if (i == wOMSSparePartsList.Count - 1)
                            wStringBuilder.Append(wOMSSparePartsList[i].ID);
                        else
                            wStringBuilder.Append(wOMSSparePartsList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.oms_spareparts WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(OMS_DeleteOMSSparePartsList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("OMS_DeleteOMSSparePartsList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<OMSSpareParts> OMS_QueryOMSSparePartsList(int wID, string wCode, int wActive, int wType, int wOrderID, int wLesOrderID, int wPartType ,string wPlanNumber, string wPieceNo, Pagination wPagination, out int wErrorCode)
        {
            List<OMSSpareParts> wResultList = new List<OMSSpareParts>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM oms_spareparts t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                + " and(@wActive <=0 or t.Active= @wActive)"
                + " and(@wType <=0 or t.Type= @wType)"
                + " and(@wOrderID <=0 or t.OrderID= @wOrderID)"
                 + " and(@wLesOrderID <=0 or t.LesOrderID= @wLesOrderID)"
                 + " and(@wPartType <=0 or t.PartType= @wPartType)"
                + " and(@wPlanNumber is null or @wPlanNumber = '' or t.PlanNumber= @wPlanNumber)"
                + " and(@wPieceNo is null or @wPieceNo = '' or t.PieceNo= @wPieceNo)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wCode", wCode);
                wParms.Add("wActive", wActive);
                wParms.Add("wOrderID", wOrderID);
                wParms.Add("wPlanNumber", wPlanNumber);
                wParms.Add("wPieceNo", wPieceNo);
                wParms.Add("wType", wType);
                wParms.Add("wPartType", wType);
                wParms.Add("wLesOrderID", wLesOrderID);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    OMSSpareParts wOMSSpareParts = new OMSSpareParts();
                    wOMSSpareParts.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wOMSSpareParts.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wOMSSpareParts.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wOMSSpareParts.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wOMSSpareParts.QRCode = StringUtils.parseString(wSqlDataReader["QRCode"]);
                    wOMSSpareParts.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wOMSSpareParts.Type = StringUtils.parseInt(wSqlDataReader["Type"]);
                    wOMSSpareParts.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wOMSSpareParts.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wOMSSpareParts.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wOMSSpareParts.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wOMSSpareParts.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wOMSSpareParts.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wOMSSpareParts.OrderID = StringUtils.parseInt(wSqlDataReader["OrderID"]);
                    wOMSSpareParts.PlanNumber = StringUtils.parseString(wSqlDataReader["PlanNumber"]);
                    wOMSSpareParts.Length = StringUtils.parseDouble(wSqlDataReader["Length"]);
                    wOMSSpareParts.Width = StringUtils.parseDouble(wSqlDataReader["Width"]);
                    wOMSSpareParts.Weigth = StringUtils.parseDouble(wSqlDataReader["Weigth"]);
                    wOMSSpareParts.PieceNo = StringUtils.parseString(wSqlDataReader["PieceNo"]);
                    wOMSSpareParts.FQTY = StringUtils.parseInt(wSqlDataReader["FQTY"]);
                    wOMSSpareParts.PartType = StringUtils.parseInt(wSqlDataReader["PartType"]);
                    wOMSSpareParts.LesOrderID = StringUtils.parseInt(wSqlDataReader["LesOrderID"]);
                    wResultList.Add(wOMSSpareParts);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(OMS_QueryOMSSparePartsList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("OMS_QueryOMSSparePartsList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

