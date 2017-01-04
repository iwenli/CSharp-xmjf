using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Text;
using System.IO;

namespace main
{
    public class PostHelper
    {
        /// <summary> 
        /// 网站Cookies 
        /// </summary> 
        private string _cookieHeader = string.Empty;
        public string CookieHeader
        {
            get
            {
                return _cookieHeader;
            }
            set
            {
                _cookieHeader = value;
            }
        }
        /// <summary> 
        /// 网站编码 
        /// </summary> 
        private string _code = string.Empty;
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }


        private string _pageContent = string.Empty;
        public string PageContent
        {
            get { return _pageContent; }
            set { _pageContent = value; }
        }

        private Dictionary<string, string> _para = new Dictionary<string, string>();
        public Dictionary<string, string> Para
        {
            get { return _para; }
            set { _para = value; }
        }


        /**/
        /// <summary> 
        /// 功能描述：模拟登录页面，提交登录数据进行登录，并记录Header中的cookie 
        /// </summary> 
        /// <param name="strURL">登录数据提交的页面地址</param> 
        /// <param name="strArgs">用户登录数据</param> 
        /// <param name="strReferer">引用地址</param> 
        /// <param name="code">网站编码</param> 
        /// <returns>可以返回页面内容或不返回</returns> 
        public string PostData(string strURL, string strArgs, string strReferer, string code, string method)
        {
            return PostData(strURL, strArgs, strReferer, code, method, string.Empty);
        }
        public string PostData(string strURL, string strArgs, string strReferer, string code, string method, string contentType)
        {
            try
            {
                string strResult = "";
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(strURL);
                myHttpWebRequest.AllowAutoRedirect = true;
                myHttpWebRequest.KeepAlive = true;
                myHttpWebRequest.Accept = "application/json, text/javascript, */*; q=0.01";
                myHttpWebRequest.Referer = strReferer;
                myHttpWebRequest.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1";
                myHttpWebRequest.Method = method;
                myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                myHttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;

                if (string.IsNullOrEmpty(contentType))
                {
                    myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                }
                else
                {
                    myHttpWebRequest.ContentType = contentType;
                }

                if (myHttpWebRequest.CookieContainer == null)
                {
                    myHttpWebRequest.CookieContainer = new CookieContainer();
                }

                if (this.CookieHeader.Length > 0)
                {
                    myHttpWebRequest.Headers.Add("cookie:" + this.CookieHeader);
                    myHttpWebRequest.CookieContainer.SetCookies(new Uri(strURL), this.CookieHeader);
                }

                byte[] postData = Encoding.GetEncoding(code).GetBytes(strArgs);
                myHttpWebRequest.ContentLength = postData.Length;

                System.IO.Stream PostStream = myHttpWebRequest.GetRequestStream();
                PostStream.Write(postData, 0, postData.Length);
                PostStream.Close();

                HttpWebResponse response = null;
                System.IO.StreamReader sr = null;
                response = (HttpWebResponse)myHttpWebRequest.GetResponse();



                if (myHttpWebRequest.CookieContainer != null)
                {
                    this.CookieHeader = myHttpWebRequest.CookieContainer.GetCookieHeader(new Uri(strURL)); 
                }

                sr = new System.IO.StreamReader(response.GetResponseStream(), Encoding.GetEncoding(code));
                strResult = sr.ReadToEnd();
                sr.Close();
                response.Close();
                return strResult;
            }
            catch (Exception ex)
            {
                LogHelper.CreateLogTxt(string.Format("ex.Message=>{0}  FormData=>{1}", ex.Message, strArgs), "c:\\xiongmaojinfu\\error");
            }
            return strArgs;
        }

        /**/
        /// <summary> 
        /// 功能描述：在PostLogin成功登录后记录下Headers中的cookie，然后获取此网站上其他页面的内容 
        /// </summary> 
        /// <param name="strURL">获取网站的某页面的地址</param> 
        /// <param name="strReferer">引用的地址</param> 
        /// <returns>返回页面内容</returns> 
        public string GetPage(string strURL, string strReferer, string code)
        {
            return GetPage(strURL, strReferer, code, string.Empty);
        }
        public string GetPage(string strURL, string strReferer, string code, string contentType)
        {
            string strResult = "";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(strURL);
            myHttpWebRequest.AllowAutoRedirect = true;
            myHttpWebRequest.KeepAlive = true;
            //myHttpWebRequest.Referer = strReferer;
            myHttpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1";

            if (string.IsNullOrEmpty(contentType))
            {
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            }
            else
            {
                myHttpWebRequest.ContentType = contentType;
            }
            myHttpWebRequest.Method = "GET";

            if (myHttpWebRequest.CookieContainer == null)
            {
                myHttpWebRequest.CookieContainer = new CookieContainer();
            }

            if (this.CookieHeader.Length > 0)
            {
                myHttpWebRequest.Headers.Add("cookie:" + this.CookieHeader);
                myHttpWebRequest.CookieContainer.SetCookies(new Uri(strURL), this.CookieHeader);
            }


            HttpWebResponse response = null;
            System.IO.StreamReader sr = null;
            response = (HttpWebResponse)myHttpWebRequest.GetResponse();


            Stream streamReceive;
            string gzip = response.ContentEncoding;

            if (string.IsNullOrEmpty(gzip) || gzip.ToLower() != "gzip")
            {
                streamReceive = response.GetResponseStream();
            }
            else
            {
                streamReceive = new System.IO.Compression.GZipStream(response.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
            }

            sr = new System.IO.StreamReader(streamReceive, Encoding.GetEncoding(code));

            if (response.ContentLength > 1)
            {
                strResult = sr.ReadToEnd();
            }
            else
            {
                char[] buffer = new char[256];
                int count = 0;
                StringBuilder sb = new StringBuilder();
                while ((count = sr.Read(buffer, 0, buffer.Length)) > 0)
                {
                    sb.Append(new string(buffer));
                }
                strResult = sb.ToString();
            }
            sr.Close();
            response.Close();
            return strResult;
        }
    }
}