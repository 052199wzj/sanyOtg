using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    public class MSSMaterialFrameDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSMaterialFrameDAO));

        #region 单实例
        private MSSMaterialFrameDAO() { }
        private static MSSMaterialFrameDAO _Instance;

        public static MSSMaterialFrameDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MSSMaterialFrameDAO();
                return MSSMaterialFrameDAO._Instance;
            }
        }
        #endregion

        public int MSS_SaveMSSMaterialFrame(MSSMaterialFrame wMSSMaterialFrame, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wMSSMaterialFrame.ID == 0)
                    wSQLText = String.Format(
                        "INSERT INTO {0}.mss_materialframe " +
                        "(Code,Name,Remark,Active,CreateID,CreateTime,EditID,EditTime," +
                        "FrameHeight,SteelPlateHeight,SteelPlateNum,FrameCode,IsExistFrame,ArrivalTime) VALUES " +
                        "(@wCode,@wName,@wRemark,@wActive,@wCreateID,@wCreateTime,@wEditID,@wEditTime," +
                        "@wFrameHeight,@wSteelPlateHeight,@wSteelPlateNum,@FrameCode,@IsExistFrame,@ArrivalTime);", wInstance);
                else if (wMSSMaterialFrame.ID > 0)
                    wSQLText = String.Format(
                        "UPDATE {0}.mss_materialframe " +
                        "SET Code=@wCode,Name=@wName,Remark=@wRemark,Active=@wActive," +
                        "CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime," +
                        "FrameHeight=@wFrameHeight,SteelPlateHeight=@wSteelPlateHeight,SteelPlateNum=@wSteelPlateNum " +
                        "FrameCode=@wFrameCode,IsExistFrame=@wIsExistFrame,ArrivalTime=@wArrivalTime " +
                        "WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wMSSMaterialFrame.ID);
                wParms.Add("wCode", wMSSMaterialFrame.Code);
                wParms.Add("wName", wMSSMaterialFrame.Name);
                wParms.Add("wRemark", wMSSMaterialFrame.Remark);
                wParms.Add("wActive", wMSSMaterialFrame.Active);
                wParms.Add("wCreateID", wMSSMaterialFrame.CreateID);
                wParms.Add("wCreateTime", wMSSMaterialFrame.CreateTime);
                wParms.Add("wEditID", wMSSMaterialFrame.EditID);
                wParms.Add("wEditTime", wMSSMaterialFrame.EditTime);
                wParms.Add("wFrameHeight", wMSSMaterialFrame.FrameHeight);
                wParms.Add("wSteelPlateHeight", wMSSMaterialFrame.SteelPlateHeight);
                wParms.Add("wSteelPlateNum", wMSSMaterialFrame.SteelPlateNum);
                wParms.Add("wFrameCode", wMSSMaterialFrame.FrameCode);
                wParms.Add("wIsExistFrame", wMSSMaterialFrame.IsExistFrame);
                wParms.Add("wArrivalTime", wMSSMaterialFrame.ArrivalTime);

                wSQLText = this.DMLChange(wSQLText);

                if (wMSSMaterialFrame.ID <= 0)
                    wMSSMaterialFrame.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(MSS_SaveMSSMaterialFrame)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MSS_SaveMSSMaterialFrame", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResult;
        }

        public int MSS_DeleteMSSMaterialFrameList(List<MSSMaterialFrame> wMSSMaterialFrameList)
        {
            int wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wMSSMaterialFrameList != null && wMSSMaterialFrameList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wMSSMaterialFrameList.Count; i++)
                    {
                        if (i == wMSSMaterialFrameList.Count - 1)
                            wStringBuilder.Append(wMSSMaterialFrameList[i].ID);
                        else
                            wStringBuilder.Append(wMSSMaterialFrameList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.mss_materialframe WHERE ID in ({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(MSS_DeleteMSSMaterialFrameList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MSS_DeleteMSSMaterialFrameList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wErrorCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wID"></param>
        /// <param name="wCode">点位编号</param>
        /// <param name="wName">名称</param>
        /// <param name="wFrameCode">当前料框号</param>
        /// <param name="wIsExistFrame">有无料框</param>
        /// <param name="wActive"></param>
        /// <param name="wPagination"></param>
        /// <param name="wErrorCode"></param>
        /// <returns></returns>
        public List<MSSMaterialFrame> MSS_QueryMSSMaterialFrameList(
            int wID, string wCode, string wName, string wFrameCode, SByte wIsExistFrame, int wActive, Pagination wPagination, out int wErrorCode)
        {
            List<MSSMaterialFrame> wResultList = new List<MSSMaterialFrame>();
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = String.Format(
                    "SELECT t.*,t1.Name Creator, t2.Name Editor FROM {0}.mss_materialframe t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + "WHERE 1=1" + " and(@wID <=0 or t.ID= @wID)"
                    + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                    + " and(@wName is null or @wName = '' or t.Name= @wName)"
                    + " and(@wFrameCode is null or @wFrameCode = '' or t.FrameCode= @wFrameCode)"
                    + " and(@wIsExistFrame <0 or t.IsExistFrame = @wIsExistFrame)"
                    + " and(@wActive <=0 or t.Active= @wActive)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wCode", wCode);
                wParms.Add("wName", wName);
                wParms.Add("wFrameCode", wFrameCode);
                wParms.Add("wIsExistFrame", wIsExistFrame);
                wParms.Add("wActive", wActive);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MSSMaterialFrame wMSSMaterialFrame = new MSSMaterialFrame();
                    wMSSMaterialFrame.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wMSSMaterialFrame.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wMSSMaterialFrame.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wMSSMaterialFrame.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wMSSMaterialFrame.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wMSSMaterialFrame.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wMSSMaterialFrame.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wMSSMaterialFrame.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wMSSMaterialFrame.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wMSSMaterialFrame.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wMSSMaterialFrame.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wMSSMaterialFrame.FrameHeight = StringUtils.parseDouble(wSqlDataReader["FrameHeight"]);
                    wMSSMaterialFrame.SteelPlateHeight = StringUtils.parseDouble(wSqlDataReader["SteelPlateHeight"]);
                    wMSSMaterialFrame.SteelPlateNum = StringUtils.parseDouble(wSqlDataReader["SteelPlateNum"]);
                    wMSSMaterialFrame.FrameCode = StringUtils.parseString(wSqlDataReader["FrameCode"]);
                    wMSSMaterialFrame.IsExistFrame = StringUtils.parseBoolean(wSqlDataReader["IsExistFrame"]);
                    wMSSMaterialFrame.ArrivalTime = StringUtils.parseDate(wSqlDataReader["ArrivalTime"]);

                    wResultList.Add(wMSSMaterialFrame);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(MSS_QueryMSSMaterialFrameList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MSS_QueryMSSMaterialFrameList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }

        /// <summary>
        /// 料点料框信息
        /// </summary>
        /// <param name="wID"></param>
        /// <param name="wCode">点位编号</param>
        /// <param name="wName">名称</param>
        /// <param name="wFrameCode">当前料框号</param>
        /// <param name="wIsExistFrame">有无料框</param>
       /// <param name="wStepNo">步序</param>
        /// <param name="wActive"></param>
        /// <param name="wPagination"></param>
        /// <param name="wErrorCode"></param>
        /// <returns></returns>
        public List<MSSMaterialFrame> MSS_QueryMSSMaterialFrameInfoList(
        int wID, string wCode, string wName, string wFrameCode, SByte wIsExistFrame, int wActive,int wStepNo, Pagination wPagination, out int wErrorCode)
        {
            List<MSSMaterialFrame> wResultList = new List<MSSMaterialFrame>();
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = String.Format(
                    "SELECT t.*,t1.Name Creator, t2.Name Editor,t3.OrderNo,t3.MaterialNo,t3.PartID,t3.PartType FROM {0}.mss_materialframe t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + " left join {0}.mss_parttrace t3 on t.ID=t3.UnloadPositionNo "
                    + "WHERE 1=1" + " and(@wID <=0 or t.ID= @wID)"
                    + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                    + " and(@wName is null or @wName = '' or t.Name= @wName)"
                    + " and(@wFrameCode is null or @wFrameCode = '' or t.FrameCode= @wFrameCode)"
                    + " and(@wIsExistFrame <=0 or t.IsExistFrame = @wIsExistFrame)"
                    + " and(@wStepNo <0 or t3.StepNo=@wStepNo)"
                    + " and(@wActive <=0 or t.Active= @wActive)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wCode", wCode);
                wParms.Add("wName", wName);
                wParms.Add("wFrameCode", wFrameCode);
                wParms.Add("wIsExistFrame", wIsExistFrame);
                wParms.Add("wStepNo", wStepNo);

                wParms.Add("wActive", wActive);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MSSMaterialFrame wMSSMaterialFrame = new MSSMaterialFrame();
                    wMSSMaterialFrame.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wMSSMaterialFrame.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wMSSMaterialFrame.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wMSSMaterialFrame.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wMSSMaterialFrame.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wMSSMaterialFrame.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wMSSMaterialFrame.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wMSSMaterialFrame.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wMSSMaterialFrame.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wMSSMaterialFrame.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wMSSMaterialFrame.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wMSSMaterialFrame.FrameHeight = StringUtils.parseDouble(wSqlDataReader["FrameHeight"]);
                    wMSSMaterialFrame.SteelPlateHeight = StringUtils.parseDouble(wSqlDataReader["SteelPlateHeight"]);
                    wMSSMaterialFrame.SteelPlateNum = StringUtils.parseDouble(wSqlDataReader["SteelPlateNum"]);
                    wMSSMaterialFrame.FrameCode = StringUtils.parseString(wSqlDataReader["FrameCode"]);
                    wMSSMaterialFrame.IsExistFrame = StringUtils.parseBoolean(wSqlDataReader["IsExistFrame"]);
                    wMSSMaterialFrame.ArrivalTime = StringUtils.parseDate(wSqlDataReader["ArrivalTime"]);
                    wMSSMaterialFrame.OrderNo = StringUtils.parseString(wSqlDataReader["OrderNo"]);
                    wMSSMaterialFrame.MaterialNo = StringUtils.parseString(wSqlDataReader["MaterialNo"]);
                    wMSSMaterialFrame.PartID = StringUtils.parseString(wSqlDataReader["PartID"]);
                    wMSSMaterialFrame.PartType = StringUtils.parseString(wSqlDataReader["PartType"]);
                    wMSSMaterialFrame.FrameStatus = StringUtils.parseInt(wSqlDataReader["FrameStatus"]);
                    wMSSMaterialFrame.CallStatus = StringUtils.parseInt(wSqlDataReader["CallStatus"]);

                    wResultList.Add(wMSSMaterialFrame);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(MSS_QueryMSSMaterialFrameList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MSS_QueryMSSMaterialFrameList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }

        public List<MSSMaterialFrame> MSS_QueryMSSMaterialFrameStatusList(out int wErrorCode)
        {
            List<MSSMaterialFrame> wResultList = new List<MSSMaterialFrame>();
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = String.Format(
                    "SELECT t.ID,t.FrameStatus,t.CallStatus FROM {0}.mss_materialframe t "
                    + "WHERE 1=1" , wInstance);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, null);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MSSMaterialFrame wMSSMaterialFrame = new MSSMaterialFrame();
                    wMSSMaterialFrame.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                   
                    wMSSMaterialFrame.FrameStatus = StringUtils.parseInt(wSqlDataReader["FrameStatus"]);
                    wMSSMaterialFrame.CallStatus = StringUtils.parseInt(wSqlDataReader["CallStatus"]);

                    wResultList.Add(wMSSMaterialFrame);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(MSS_QueryMSSMaterialFrameList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MSS_QueryMSSMaterialFrameList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }
    }
}
