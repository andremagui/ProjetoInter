using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROJETO_API.Requests
{
    public class DependentRequest
    {
        public string DependentName { get; set; }
        public DateTime DependentBirth { get; set; }
        public string DependentBlood { get; set; }
        public string DependentAllergy { get; set; }
        public string DependentSusNo { get; set; }
        public int UserID { get; set; }
    }
}
