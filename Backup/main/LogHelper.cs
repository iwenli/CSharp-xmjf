using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace main
{
    public class LogHelper
    {
        #region  创建日志
        ///-----------------------------------------------------------------------------
        /// <summary>创建错误日志 在c:\\xiongmaojinfu\\info</summary>
        /// <param name="message">记录信息</param> 
        ///-----------------------------------------------------------------------------
        public static void CreateLogTxt(string message)
        {
           CreateLogTxt(message,"c:\\xiongmaojinfu\\info");
        }

     

        /// <summary>
        /// 创建错误日志
        /// </summary>
        /// <param name="message">记录信息</param>
        /// <param name="path">文件路径</param>
        public static void CreateLogTxt(string message,string path)
        {
            string strPath = path;                                                   //文件的路径
            DateTime dt = DateTime.Now;
            try
            {  
                if (Directory.Exists(strPath) == false)                          //目录是否存在,不存在则没有此目录
                {
                    Directory.CreateDirectory(strPath);                     
                }
                strPath = strPath + "\\" + dt.ToString("yyyy");

                if (Directory.Exists(strPath) == false)
                {
                    Directory.CreateDirectory(strPath);
                }
                strPath = strPath + "\\" + dt.Year.ToString() + "-" + dt.Month.ToString() + ".txt";

                StreamWriter FileWriter = new StreamWriter(strPath, true);           //创建日志文件
                FileWriter.WriteLine("[" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "]  " + message);
                FileWriter.Close();                                                 //关闭StreamWriter对象
            }
            catch (Exception ex)
            {
                string str = ex.Message.ToString();
            }
        }
        #endregion
    }
}