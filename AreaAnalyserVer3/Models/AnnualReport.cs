using System.ComponentModel.DataAnnotations;

namespace AreaAnalyserVer3.Models
{
    public class AnnualReport
    {
       
        public AnnualReport() { }
        // Properties
        [Key]
        public int ReportID { get; set; }
        public int StationId { get; set; }
        public int Year { get; set; }
        public int NumAttemptedMurderAssault { get; set; }
        public int NumDangerousActs { get; set; }
        public int NumKidnapping { get; set; }
        public int NumRobbery { get; set; }
        public int NumBurglary { get; set; }
        public int NumTheft { get; set; }
        public int NumFraud { get; set; }
        public int NumDrugs { get; set; }
        public int NumWeapons { get; set; }
        public int NumDamage { get; set; }
        public int NumPublicOrder { get; set; }
        public int NumGovernment { get; set; }

        // Foreign key reference
        public virtual GardaStation Station { get; set; }
    }
}