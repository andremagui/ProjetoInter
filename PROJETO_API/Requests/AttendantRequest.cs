using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROJETO_API.Requests
{
    public class AttendantRequest
    {
        public string AttendantName { get; set; }
        public string AttendantCpf { get; set; }
        public string AttendantEmail { get; set; }
        public string AttendantPass { get; set; }
        public int UbsID { get; set; }
    }
}
