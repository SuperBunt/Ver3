using AreaAnalyserVer3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.ViewModels
{
    public class Education
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public Education() {
            localSchools = new List<School>();
            feederInfo = new List<Leaver>();
        }

        public Education(int id)
        {
            TownId = id;
            localSchools = new List<School>();
            feederInfo = new List<Leaver>();
        }

        // Fields
        private List<School> localSchools;
        private List<Leaver> feederInfo;
        // Properties
        public int TownId { get; set; }
        public List<School> LocalSchools {
            get
            {
                return db.School.Where(x => x.TownId == TownId).ToList();             
            }
        }
        public List<Leaver> FeederInfo
        {
            get
            {
                var listIds = (from i in localSchools
                                 select i.SchoolId).Cast<int?>();
                return db.Leaving.Where(id => listIds.Contains(id.SchoolId)).ToList();
            }
        }
    }
}