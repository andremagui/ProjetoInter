using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROJETO_API.Results
{
    public class UserDependentResult
    {
        public int UserID { get; set; }
        public string UserCpf { get;set }
        public string UserName { get; set; }
        public List<DependentVaccineResult> Dependentes { get; set; }
      
    }
}
