using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class UserResults
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Level { get; set; }
        public string UserId { get; set; }
        public List<CompetencyModel> Competences { get; set; }
    }
}