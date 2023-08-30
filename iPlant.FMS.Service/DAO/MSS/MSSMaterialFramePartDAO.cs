using iPlant.Common.Tools;
using iPlant.FMC.Service;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Service
{
    public class MSSMaterialFramePartDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSMaterialFramePartDAO));

        #region 单实例
        private MSSMaterialFramePartDAO() { }
        private static MSSMaterialFramePartDAO _Instance;

        public static MSSMaterialFramePartDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MSSMaterialFramePartDAO();
                return MSSMaterialFramePartDAO._Instance;
            }
        }
        #endregion

        public List<MSSMaterialFramePart> MSS_QueryMSSMaterialFramePartList(
            int wMaterialFrameID, Pagination wPagination, out int wErrorCode)
        {
            List<MSSMaterialFramePart> wResultList = new List<MSSMaterialFramePart>();
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                String wSQLText = String.Format(
                    "SELECT t.*, t1.Name AS Creator, t2.Name AS Editor, t3.StationCode, t3.Status, t3.ErroMsg, t3.SendTime, t4.FrameCode ,t4.Name AS MaterialFrameName" +
                    " FROM {0}.mss_materialframe_parts t " +
                    " LEFT JOIN {0}.mbs_user t1 ON t.CreateID = t1.ID" +
                    " LEFT JOIN {0}.mbs_user t2 ON t.EditID = t2.ID " +
                    " LEFT JOIN {0}.inf_les_updownmaterial t3 ON t.LesUpDownMaterialID = t3.ID " +
                    " LEFT JOIN {0}.mss_materialframe t4 ON t.MaterialFrameID = t4.ID " +
                    " WHERE 1 = 1 " +
                    " AND (@wMaterialFrameID <=0 OR t.MaterialFrameID= @wMaterialFrameID) ", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wMaterialFrameID", wMaterialFrameID);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MSSMaterialFramePart wItem = new MSSMaterialFramePart();

                    wItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    //wItem.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    //wItem.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    //wItem.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    //wItem.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wItem.MaterialFrameName = StringUtils.parseString(wSqlDataReader["MaterialFrameName"]);
                    wItem.MaterialFrameID = StringUtils.parseInt(wSqlDataReader["MaterialFrameID"]);
                    wItem.LesUpDownMaterialID = StringUtils.parseInt(wSqlDataReader["LesUpDownMaterialID"]);
                    wItem.NestId = StringUtils.parseString(wSqlDataReader["NestId"]);
                    wItem.OrderId = StringUtils.parseString(wSqlDataReader["OrderId"]);
                    wItem.PartNo = StringUtils.parseString(wSqlDataReader["PartNo"]);
                    wItem.Quality = StringUtils.parseInt(wSqlDataReader["Quality"]);
                    wItem.StartTime= StringUtils.parseDate(wSqlDataReader["StartTime"]);
                    wItem.EndTime = StringUtils.parseDate(wSqlDataReader["EndTime"]);
                    wItem.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wItem.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wItem.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wItem.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wItem.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wItem.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);

                    wItem.StationCode = StringUtils.parseString(wSqlDataReader["StationCode"]);
                    wItem.Status = StringUtils.parseInt(wSqlDataReader["Status"]);
                    wItem.ErroMsg = StringUtils.parseString(wSqlDataReader["ErroMsg"]);
                    wItem.SendTime = StringUtils.parseDate(wSqlDataReader["SendTime"]);

                    wItem.FrameCode = StringUtils.parseString(wSqlDataReader["FrameCode"]);

                    wResultList.Add(wItem);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(MSS_QueryMSSMaterialFramePartList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MSS_QueryMSSMaterialFramePartList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }

        /// <summary>
        /// 添加报工零件，涉及到2张表mss_materialframe_parts, inf_les_updownmaterial
        /// </summary>
        /// <param name="wItem"></param>
        /// <param name="wErrorCode"></param>
        /// <returns></returns>
        public int MSS_InsertMSSMaterialFramePart(MSSMaterialFramePart wItem, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                // 先插入inf_les_updownmaterial得到主键ID
                String wSQLText = String.Format(
                    "INSERT INTO {0}.inf_les_updownmaterial " +
                    "(FrameCode,NestId,`Order`,ProductNo,StationCode,UseType,CreateTime) VALUES " +
                    "(@wFrameCode,@wNestId,@wOrder,@wProductNo,@wStationCode,@wUseType,now());", wInstance);

                wParms.Clear();
                wParms.Add("wFrameCode", wItem.FrameCode);
                wParms.Add("wNestId", wItem.NestId);
                wParms.Add("wOrder", wItem.OrderId);
                wParms.Add("wProductNo", wItem.PartNo);
                wParms.Add("wStationCode", wItem.StationCode);
                wParms.Add("wUseType", 0);

                wSQLText = this.DMLChange(wSQLText);
                int wLesUpDownMaterialID = (int)mDBPool.insert(wSQLText, wParms);

                // 再插入mss_materialframe_parts
                wSQLText = String.Format("INSERT INTO {0}.mss_materialframe_parts " +
                    "(MaterialFrameID,LesUpDownMaterialID,NestId,OrderId,PartNo,Quality,CreateID,CreateTime,EditID,EditTime) VALUES " +
                    "(@wMaterialFrameID,@wLesUpDownMaterialID,@wNestId,@wOrderId,@wPartNo,@wQuality,@wCreateID,@wCreateTime,@wEditID,@wEditTime)",
                    wInstance);

                wParms.Clear();
                wParms.Add("wMaterialFrameID", wItem.MaterialFrameID);
                wParms.Add("wLesUpDownMaterialID", wLesUpDownMaterialID);
                wParms.Add("wNestId", wItem.NestId);
                wParms.Add("wOrderId", wItem.OrderId);
                wParms.Add("wPartNo", wItem.PartNo);
                wParms.Add("wQuality", wItem.Quality);
                wParms.Add("wCreateID", wItem.CreateID);
                wParms.Add("wCreateTime", wItem.CreateTime);
                wParms.Add("wEditID", wItem.EditID);
                wParms.Add("wEditTime", wItem.EditTime);

                wSQLText = this.DMLChange(wSQLText);
                _ = (int)mDBPool.insert(wSQLText, wParms);

            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(MSS_InsertMSSMaterialFramePart)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MSS_InsertMSSMaterialFramePart", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResult;
        }
    }
}
