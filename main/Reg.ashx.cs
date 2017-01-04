using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

namespace main
{
    /// <summary>
    /// Reg 的摘要说明
    /// </summary>
    public class Reg : IHttpHandler
    {

        //当天同一个手机号请求验证码超过5次  {"error":true,"msg":["\u5f53\u5929\u83b7\u53d6\u9a8c\u8bc1\u7801\u8d85\u8fc75\u6b21\uff0c\u8bf7\u660e\u5929\u518d\u8bd5!"],"data":""}
        //正确请求{"error":false,"msg":"","data":""} 
        //纬经度值22.543099,114.057868

        private PostHelper post = new PostHelper();


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request.Params["action"];
            
            string postUrl = string.Empty;
            string strArgs = string.Empty;
             string result = string.Empty;

             string strReferer = "https://www.xiongmaojinfu.com/auth/h5Register?mobile=18310807769&qsrc=xm-001-0384";
              
            switch (action)
            {
                case "sendCode":
                    postUrl = "https://www.xiongmaojinfu.com/auth/sendCode";
                    strArgs = string.Format("mobile={0}&type=register", context.Request.Params["mobile"]);
                    break;
                case "register": 
                    postUrl = "https://www.xiongmaojinfu.com/auth/register";
                    strArgs = string.Format("mobile={0}&pwd=4e{1}&code_key={2}&inkey={3}&channel_no={4}&lashow={5}&loshow={6}",
                        context.Request.Params["mobile"],
                        context.Request.Params["pwd"],
                        context.Request.Params["code_key"],
                        context.Request.Params["inkey"],
                        context.Request.Params["channel_no"],
                        context.Request.Params["lashow"],
                        context.Request.Params["loshow"]
                        );
                    LogHelper.CreateLogTxt(string.Format("注册信息【手机号:{0}  密码:{1}  邀请人:{2}  纬度:{3}  经度:{4}】",
                        context.Request.Params["mobile"],
                        context.Request.Params["pwd"],
                        context.Request.Params["inkey"],
                        context.Request.Params["lashow"],
                        context.Request.Params["loshow"]
                        ));
                    break;
                default:
                    break;
            }
            result = post.PostData(postUrl, strArgs, strReferer, "utf-8", "POST", "application/x-www-form-urlencoded; charset=UTF-8");
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}