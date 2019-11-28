using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenuncieSpam.MapeamentoJson
{
    public class results
    {
        public string formatted_address { get; set; }
        public List<address_components> address_components { get; set; } = new List<address_components>();
    }
}
