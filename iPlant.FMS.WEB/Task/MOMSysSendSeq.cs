using iPlant.FMS.WEB.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace iPlant.FMS.WEB
{
    public class MOMSysSendSeq: BackgroundService
    {
        private log4net.ILog log;
        private string MOMSysSendSeqApiUrl;
        private string MOMSysSendSeqInterval;
        private string MOMSysSendSeqEnabled;
        public MOMSysSendSeq()
        {
            log = log4net.LogManager.GetLogger(typeof(MOMSysSendSeq));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                MOMSysSendSeqApiUrl = builder.Build().GetSection("Interface").GetSection("MOMSysSendSeq").GetSection("ApiUrl").Value;
                MOMSysSendSeqInterval = builder.Build().GetSection("Interface").GetSection("MOMSysSendSeq").GetSection("Interval").Value;
                MOMSysSendSeqEnabled = builder.Build().GetSection("Interface").GetSection("MOMSysSendSeq").GetSection("Enabled").Value;
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(Convert.ToInt32(MOMSysSendSeqInterval) * 1000, stoppingToken); //启动后10秒执行一次
                    if (MOMSysSendSeqEnabled == "1")
                    {
                        try
                        {
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(MOMSysSendSeqApiUrl);
                            req.Method = "POST";
                            req.ContentType = "application/json";
                            req.Timeout = 15 * 1000;//请求超时时间
                            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                            Stream stream = resp.GetResponseStream();
                            //获取响应内容
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                String result = reader.ReadToEnd();
                                Result result1 = JsonConvert.DeserializeObject<Result>(result);
                                if (result1.resultCode != 1000)
                                {
                                    log.Info("发送请求工序配送信息给MOM系统接口调用失败！" + result);
                                }
                                else
                                {
                                    log.Info("发送请求工序配送信息给MOM系统接口调用成功！");
                                }
                            }

                        }
                        catch (Exception ex) { log.Error("发送请求工序配送信息给MOM系统接口调用异常！" + ex.Message + ex.StackTrace); }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!stoppingToken.IsCancellationRequested)
                {
                    log.Error("发送请求工序配送信息给MOM系统接口调用异常！" + ex.Message + ex.StackTrace);
                }
                else
                {
                    log.Error("发送请求工序配送信息给MOM系统接口调用停止！");
                }
            }

        }
    }
}
