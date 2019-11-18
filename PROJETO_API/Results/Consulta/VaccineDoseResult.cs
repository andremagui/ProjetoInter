using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROJETO_API.Results
{
    public class VaccineDoseResult
    {
        public int VaccineID { get; set; }
        public string VaccineName { get; set; }
        public DateTime VaccineDate { get; set; }
        public List<DoseResult> Doses { get; set; }


    }
}
