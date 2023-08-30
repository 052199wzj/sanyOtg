using iPlant.Common.Tools;
using iPlant.Data.EF.Repository;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Service
{
    public interface ILogService
    {
        Task<string> LogInfo(string systemName, string interfaceName, string processName, int stepNo, string info, string details, string versionNo, string partID = "");
        void LogInfo(string systemName, string interfaceName, string processName, int stepNo, string info, string details, string partID = "");
    }
    public class LogService : RepositoryFactory, ILogService
    {
        public string DirPathFileLog = "";
        private string _Version = "20220920";
        public LogService()
        {
            SQLType = Data.EF.DBEnumType.MySQL;
            DirPathFileLog = GlobalConstant.DirectoryLogInfo;
        }


        public async Task<string> LogInfo(string systemName, string interfaceName, string processName, int stepNo, string info, string details, string versionNo, string partID = "")
        {
            string msg = "";
            try
            {
                if (!Directory.Exists(DirPathFileLog))
                    Directory.CreateDirectory(DirPathFileLog);

                string fileName = $"{systemName}_{interfaceName}_{DateTime.Now.ToString("yyyyMMdd")}log" + ".log";
                string wPath = DirPathFileLog + fileName;
                if (File.Exists(wPath))
                {
                    using (StreamWriter fs = new StreamWriter(wPath, true))
                    {
                        fs.WriteLine(details);
                    }
                }
                else
                {
                    File.WriteAllText(wPath, details + "\r\n");
                }
                mcs_loginfo mcs_loginfo = new mcs_loginfo
                {
                    FileName = fileName,
                    CreateTime = DateTime.Now,
                    CreateTimeStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    FilePath = wPath,
                    VersionNo = versionNo,
                    FileType = interfaceName,
                    SystemType = systemName,
                    BOMID = "",
                    BOPID = 0,
                    CustomerName = "",
                    LineName = "",
                    PartNo = partID,
                    ProductNo = "",
                    Info = info,
                    ProcessName = processName,
                    StepNo = stepNo
                };
                await this.BaseRepository().Insert(mcs_loginfo);
            }
            catch (Exception ex)
            {
                msg = ex.ToString() + ex.StackTrace;
            }
            return msg;
        }

        public async void LogInfo(string systemName, string interfaceName, string processName, int stepNo, string info, string details, string partID = "")
        {
            await LogInfo(systemName, interfaceName, processName, stepNo, info, details, _Version, partID);
        }
    }
}
