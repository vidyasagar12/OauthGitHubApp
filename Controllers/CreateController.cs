using GitHubOauth.Models;
using GitHubOauth.Models.CreateModel;
using GitHubOauth.Services;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GitHubOauth.Controllers
{
    public class CreateController : Controller
    {
        HttpClient GitClient;
        // GET: Create
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateProjectView()
        {
            return View();
        }

        public object Organization()
        {
           
            List<organization> org = new List<organization>();
            string Url = "https://api.github.com/user/orgs";
            string Method = "Get";
            //Session["PAT"]
            string PAT = (string)Session["PAT"];
            //string PAT = "5c79def89b7b74440072449636ffbe060fc08316";
            //string RequestBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\TestCaseJson.json");
            GitClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.github.com/")
            };
            GitClient.DefaultRequestHeaders.Add("User-Agent", "Ashwin");//"MyConsoleApp");
            GitClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.inertia-preview+json"));
            if (PAT != null)
                GitClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PAT);
            StringContent JsonContent = null;
            var request = new HttpRequestMessage(new HttpMethod(Method), new Uri(Url));
            var response = GitClient.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                //var responseString = response.Content.ReadAsStringAsync().Result;
                var obj = response.Content.ReadAsStringAsync().Result;
                org = JsonConvert.DeserializeObject<List<organization>>(obj);
                string output1 = JsonConvert.SerializeObject(org);
                return output1;
            }
            else
            {
                string OrderData = "";
                string output = JsonConvert.SerializeObject(org);
                return output;
            }
        }
        public object RepositoryCreate(string org,string projectName,string template)
        {
            ForkRepoClass forked_Project = new ForkRepoClass();
            if (template != null)
            {
                string Url = "";
                //string project = "CanarysBankingApplication/forks";
                if (template == "0")
                {
                    Url = "https://api.github.com/repos/Varmaji/CanarysBankingApplication/forks";
                }
                else if (template == "1")
                {
                    Url = "https://api.github.com/repos/Varmaji/CanarysBankingApplication/forks";
                }
                //RepoResponse repoInstance = new RepoResponse();
                APIServicecs ApiObject;
                //string Url = "https://api.github.com/repos/ashwin9627/NewRepo987";     //RepoCreated";
                ApiObject = new APIServicecs(Session["PAT"].ToString());
                var userDetails = JsonConvert.DeserializeObject<UserDetails>(ApiObject.ApiService("https://api.github.com/user"));
                string ownername = userDetails.login;//"ashwin9627";//;
                                                     //string Url = "https://api.github.com/repos/"+ ownername + "/"+repo;     //RepoCreated";
                ApiObject = new APIServicecs(Session["PAT"].ToString());
                string RequestBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\ForkRepo_Template.json");
                RequestBody = RequestBody.Replace("$name$", projectName);
                RequestBody = RequestBody.Replace("$org$", org);
                string responsestring = ApiObject.ApiServiceRepo(Url, "POST", RequestBody);
                forked_Project = JsonConvert.DeserializeObject<ForkRepoClass>(responsestring);
                //    CreateOrgRepoTemplate(org,forked_Project.id.ToString());
                CreateProject(forked_Project.name, projectName);
                //CreateColumn(forked_Project.id.ToString());
                //string output = JsonConvert.SerializeObject(repoInstance);
                return null;//output;
            }
            else
            {
                return null;
            }
        }
        public object CreateRepoTemplate(string repo,string projectName)
        {
            RepoResponse repoInstance = new RepoResponse();
            APIServicecs ApiObject;
            string Url = "https://api.github.com/repos/ashwin9627/NewRepo987";     //RepoCreated";
            ApiObject = new APIServicecs(Session["PAT"].ToString());
            var userDetails = JsonConvert.DeserializeObject<UserDetails>(ApiObject.ApiService("https://api.github.com/user"));
            string ownername = userDetails.login;//"ashwin9627";//;
            //string Url = "https://api.github.com/repos/"+ ownername + "/"+repo;     //RepoCreated";
            ApiObject = new APIServicecs(Session["PAT"].ToString());
            string RequestBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\RepoTemplate.json");
            RequestBody = RequestBody.Replace("$name$", projectName);
            RequestBody = RequestBody.Replace("$owner$", ownername);
            string responsestring=ApiObject.ApiService1(Url, "POST", RequestBody,"");
            repoInstance = JsonConvert.DeserializeObject<RepoResponse>(responsestring);
            string output = JsonConvert.SerializeObject(repoInstance);
            return output;            
        }
        public object CreateOrgRepoTemplate(string org, string projectName)
        {
            APIServicecs ApiObject;
            string Url = "https://api.github.com/orgs/" + org + "/repos";//+projectName+"";     //RepoCreated";
            ApiObject = new APIServicecs(Session["PAT"].ToString());
            string RequestBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\OrgReport.json");
            RequestBody = RequestBody.Replace("$name$", projectName);
            var responseString=ApiObject.ApiService2(Url, "POST", RequestBody, "");
            ProjectResponse pro = JsonConvert.DeserializeObject<ProjectResponse>(responseString);
            //CreateColumn(pro.id.ToString());
            return null;
        }
        public object CreateOrgProject(string org, string projectName)
        {
            //CreateOrgRepoTemplate(org, projectName);
           // CreateRepoTemplate("", projectName);
           // CreateOrgRepo(org, projectName);
            string Url = "https://api.github.com/orgs/"+org+"/projects";
            //string Url = "https://api.github.com/orgs/GitTrail/projects";
            string Method = "POST";
            APIServicecs ApiObject;
            //string PAT1 = (string)Session["PAT"];
            //string PAT = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", PAT1)));
            //string PAT = "5c79def89b7b74440072449636ffbe060fc08316";
            ApiObject = new APIServicecs(Session["PAT"].ToString());
            string RequestBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\ProjectTemplate.json");
            RequestBody = RequestBody.Replace("$name$", projectName);
            //GitClient = new HttpClient()
            //{
            //    BaseAddress = new Uri("https://api.github.com/")
            //};
            //GitClient.DefaultRequestHeaders.Add("User-Agent", "ConsoleApp");//"MyConsoleApp");
            //GitClient.DefaultRequestHeaders.Accept.Add(
            //    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            ////GitClient.DefaultRequestHeaders.Accept.Add(
            ////    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.inertia-preview+json"));
            //if (PAT != null)
            //    GitClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", PAT1);

            //StringContent JsonContent = null;
            //var request = new HttpRequestMessage(new HttpMethod(Method), new Uri(Url));
            //if (RequestBody != null)
            //{
            //    JsonContent = new StringContent(RequestBody, Encoding.UTF8, "application/json");
            //    request.Content = JsonContent;
            //    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.inertia-preview+json"));
            //}
            //var response = GitClient.SendAsync(request).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var responseString = response.Content.ReadAsStringAsync().Result;
            //    return responseString;
            //}
//            string RequestBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\Repo.json");
            ApiObject.ApiService(Url, "POST", RequestBody);

            return null;
        }


public JsonResult CreateProject(string repo,string projectName)
        {
          //  projectName = "mind";
            APIServicecs ApiObject;
          //  ProjectDetails projectDetails = new ProjectDetails();
            try
            {
                string templatesPath = Server.MapPath("~") + @"\JSON Template\";
                ApiObject = new APIServicecs(Session["PAT"].ToString());
               var userDetails = JsonConvert.DeserializeObject<UserDetails>(ApiObject.ApiService("https://api.github.com/user"));
                string ownername = userDetails.login;//"ashwin9627";//;
                string url = "https://api.github.com/repos/" + ownername + "/" + repo + "/projects";
                string RqstBody = "";
                //if (System.IO.File.Exists(templatesPath + @"\projectDetails.json"))
                //{
                //    RqstBody = projectDetails.ReadJsonFile(templatesPath + @"\projectDetails.json");
                //}
                string RequestBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\Repo.json");
                RequestBody=RequestBody.Replace("$name$",projectName);
                string responseString = ApiObject.ApiService(url, "POST", RequestBody);
              //  var responseString = response.Content.ReadAsStringAsync().Result;
                ProjectResponse pro = JsonConvert.DeserializeObject<ProjectResponse>(responseString);
                CreateColumn(pro.id.ToString());
                return Json(pro, JsonRequestBehavior.AllowGet);

                //projectDetails = JsonConvert.DeserializeObject<ProjectDetails>(ApiObject.ApiService(url, "POST", RqstBody));
            }
            catch (Exception ex)
            {
                ProjectResponse pro1 = new ProjectResponse();
                return Json(pro1, JsonRequestBehavior.AllowGet);
            }            
        }

        public bool CreateColumn(string projectId)
        {
            APIServicecs ApiObject;
            //  ProjectDetails projectDetails = new ProjectDetails();
            try
            {
                
                ApiObject = new APIServicecs(Session["PAT"].ToString());
                // userDetails = JsonConvert.DeserializeObject<UserDetails>(ApiObject.ApiService("https://api.github.com/user"));
                string url = "https://api.github.com/projects/"+projectId+"/columns";
                string RqstBody = "";
                string RequestBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\ColumnTemplate.json");
                var response = ApiObject.ApiService(url, "POST", RequestBody);
                return true;
            }
            catch
            {
                return false;
            }
            }




        public object CreateOrgRepo(string org,string projectName)
        {
            //List<organization> org = new List<organization>();
            //string Url = "https://api.github.com/repos/ashwin9627/ProjectHealthReport";
            string Url = "https://api.github.com/repos/Varmaji/MavenWeb";
            string Method = "POST";
            //Session["PAT"]
            string PAT = (string)Session["PAT"];
            //string PAT = "5c79def89b7b74440072449636ffbe060fc08316";
            string RequestBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\Repo.json");
            RequestBody = RequestBody.Replace("$name$", projectName);
            GitClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.github.com/")
            };
            GitClient.DefaultRequestHeaders.Add("User-Agent", "Ashwin");//"MyConsoleApp");
            //GitClient.DefaultRequestHeaders.Accept.Add(
            //    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.baptiste-preview+json"));
            if (PAT != null)
                GitClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", PAT);
            StringContent JsonContent = null;
            var request = new HttpRequestMessage(new HttpMethod(Method), new Uri(Url));
            if (RequestBody != null)
            {
                JsonContent = new StringContent(RequestBody, Encoding.UTF8, "application/json");
                request.Content = JsonContent;
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.inertia-preview+json"));
            }

            var response = GitClient.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                //var responseString = response.Content.ReadAsStringAsync().Result;
                //var obj = response.Content.ReadAsStringAsync().Result;
                //org = JsonConvert.DeserializeObject<List<organization>>(obj);
                //string output1 = JsonConvert.SerializeObject(org);
                //return output1;
                return null;
            }
            else
            {
                string OrderData = "";
                string output = JsonConvert.SerializeObject(org);
                return output;
            }
        }
        public JsonResult searchuser(string q)//search)
       {
            UserResponse userDetails = new UserResponse();
            Item userDetails1 = new Item();
            APIServicecs ApiObject;
            //  ProjectDetails projectDetails = new ProjectDetails();
            try
            {

                ApiObject = new APIServicecs(Session["PAT"].ToString());
                // userDetails = JsonConvert.DeserializeObject<UserDetails>(ApiObject.ApiService("https://api.github.com/user"));
                string url = "https://api.github.com/search/users?q=" +q;//search;
                string RqstBody = "";
        //        string RequestBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\ColumnTemplate.json");
                var response = ApiObject.ApiService(url, "GET", null);
                userDetails = JsonConvert.DeserializeObject<UserResponse>(response);
                userDetails1 = JsonConvert.DeserializeObject<Item>(response);
                return Json(userDetails1.login, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(userDetails, JsonRequestBehavior.AllowGet);
            }
        }
        }
}