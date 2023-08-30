using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;

namespace iPlant.Common.Tools
{
    public class QRCodeTool
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QRCodeTool));

        /// <summary>
        /// 生成并保存二维码图片的方法
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="url">保存路径</param>
        /// <param name="filename">文件名</param>
        public static string CreateQRImg(string str)
        {
            string wResult = "";
            try
            {
                // 生成二维码的内容
                string strCode = str;
                QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(strCode, QRCodeGenerator.ECCLevel.Q);
                QRCode qrcode = new QRCode(qrCodeData);

                // qrcode.GetGraphic 方法可参考最下发“补充说明”
                Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
                MemoryStream ms = new MemoryStream();

                string uuid = Guid.NewGuid().ToString().Replace("-", "");
                string wBaseDir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                int wIndex = wBaseDir.LastIndexOf('\\');
                wBaseDir = wBaseDir.Substring(0, wIndex);
                wIndex = wBaseDir.LastIndexOf('\\');
                wBaseDir = wBaseDir.Substring(0, wIndex + 1);
                string wPath = wBaseDir + "MyQRCodes\\" + uuid + ".jpg";
                string dirpath = wBaseDir + "MyQRCodes\\";
                if (!Directory.Exists(dirpath))
                    Directory.CreateDirectory(dirpath);

                qrCodeImage.Save(wPath);

                wResult = uuid + ".jpg";
            }

            //    string enCodeString = str;
            //    //生成设置编码实例
            //    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //    //设置二维码的规模，默认4
            //    qrCodeEncoder.QRCodeScale = 4;
            //    //设置二维码的版本，默认7
            //    qrCodeEncoder.QRCodeVersion = 7;
            //    qrCodeEncoder.QRCodeBackgroundColor = Color.FromArgb(255, 255, 255);
            //    //设置错误校验级别，默认中等
            //    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //    //生成二维码图片
            //    Bitmap bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            //    //保存二维码图片路径
            //    string uuid = Guid.NewGuid().ToString().Replace("-", "");
            //    string wBaseDir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //    int wIndex = wBaseDir.LastIndexOf('\\');
            //    wBaseDir = wBaseDir.Substring(0, wIndex);
            //    wIndex = wBaseDir.LastIndexOf('\\');
            //    wBaseDir = wBaseDir.Substring(0, wIndex + 1);
            //    string wPath = wBaseDir + "MyQRCodes\\" + uuid + ".jpg";
            //    string dirpath = wBaseDir + "MyQRCodes\\";
            //    if (!Directory.Exists(dirpath))
            //        Directory.CreateDirectory(dirpath);
            //    try
            //    {
            //        bt.Save(wPath);
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Error(ex);
            //    }
            //    finally
            //    {
            //        bt.Dispose();   //显式释放资源  
            //    }
            //    wResult = uuid + ".jpg";
            //}
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return wResult;
        }
    }
}
