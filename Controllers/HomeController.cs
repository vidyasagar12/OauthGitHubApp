using GitHubOauth.Models.Responses;
using GitHubOauth.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GitHubOauth.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Login()
        {
            Session["visited"] = "1";
            string url = "https://github.com/login/oauth/authorize?client_id={0}&response_type=Assertion&state=User1&scope=user%20public_repo&redirect_uri={1}";
            string redirectUrl = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"];
            string clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"];           
            url = string.Format(url, clientId, redirectUrl);
            return Redirect(url);
        }
        public ActionResult Index()
        {
           /* if (Session["visited"] == null)
                return RedirectToAction("../Account/Verify");*/
            string code = Session["PAT"] == null ? Request.QueryString["code"] : Session["PAT"].ToString();

            string reqBody= "client_id={0}&client_secret={1}&response_type=Assertion&code={2}&redirect_uri={3}";
            string redirectUrl = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"];
            string clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"];
            string ClientSecreat= System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];
            string scope = "admin:org";
            reqBody = string.Format(reqBody, HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(ClientSecreat), HttpUtility.UrlEncode(code),"");
            string access_Token = ApiObject.GetAccessToken(reqBody);
            if(access_Token!= "bad_verification_code")
            Session["PAT"] = access_Token;
            return View();
        }
        
        public static APIServicecs ApiObject = new APIServicecs(); 
       public JsonResult RepositoryList()
        {
            try
            {
                List<RepositoryResponse> RepoList = new List<RepositoryResponse>();
                ApiObject = new APIServicecs(Session["PAT"].ToString());
                string RepositoriesString = ApiObject.ApiService("https://api.github.com/orgs/VarmasGitHub/repos");
                if (!string.IsNullOrEmpty(RepositoriesString))
                    RepoList = JsonConvert.DeserializeObject<List<RepositoryResponse>>(RepositoriesString);
                return Json(RepoList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex) {
                return null;
            }
        }
    }
}
