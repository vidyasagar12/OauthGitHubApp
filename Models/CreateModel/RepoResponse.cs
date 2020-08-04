using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubOauth.Models
{
    public class RepoResponse
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
    }
}