using GitHubOauth.Models;
using GitHubOauth.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VstsDemoBuilder.Extensions;

namespace GitHubOauth.Controllers
{
    public class VarmaController : Controller
    {
        // GET: Varma
        public ActionResult Index()
        {
            string code = Session["PAT"] == null ? Request.QueryString["code"] : Session["PAT"].ToString();

            string reqBody = "client_id={0}&client_secret={1}&response_type=Assertion&code={2}&redirect_uri={3}";
            string redirectUrl = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"];
            string clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"];
            string ClientSecreat = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];
            reqBody = string.Format(reqBody, HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(ClientSecreat), HttpUtility.UrlEncode(code), "");
            string access_Token = ApiObject.GetAccessToken(reqBody);
            if (access_Token != "bad_verification_code")
                Session["PAT"] = access_Token;
            return View();
        }
        public static APIServicecs ApiObject = new APIServicecs();
        public JsonResult OrgDropDown()
        {
            List<OrgDetails> ListOrg = new List<OrgDetails>();
            try
            {
                
                ApiObject = new APIServicecs(Session["PAT"].ToString());
                ListOrg = JsonConvert.DeserializeObject<List<OrgDetails>>(ApiObject.ApiService("https://api.github.com/user/orgs"));
            }
            catch(Exception Ex)
            {

            }
            return Json(ListOrg,JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateProject(string org)
        {
            ProjectDetails projectDetails = new ProjectDetails();
            try
            {
                string templatesPath = Server.MapPath("~") + @"\JSON Template\";
                ApiObject = new APIServicecs(Session["PAT"].ToString());
                string url = "https://api.github.com/repos/orgs/'"+org+"'/projects";
                string RqstBody = "";
                if (System.IO.File.Exists(templatesPath + @"\projectDetails.json"))
                {
                    RqstBody = projectDetails.ReadJsonFile(templatesPath + @"\projectDetails.json");
                }
                projectDetails = JsonConvert.DeserializeObject<ProjectDetails>(ApiObject.ApiService(url, "POST", RqstBody));
            }
            catch (Exception ex)
            {

            }
            return Json(projectDetails, JsonRequestBehavior.AllowGet);
        }

    }
}