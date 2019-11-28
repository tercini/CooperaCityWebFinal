using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenuncieSpam.MapeamentoJson
{
    public class ApiGeocoding
    {
        //public plus_code plus_code { get; set; }
        public List<results> results { get; set; } = new List<results>();
        //public string formatted_address { get; set; }
    }

    
    //public class results
    //{
        //public string formatted_address { get; set; }
        //public List<address_components> address_components { get; set; }
    //}

    /*
    public class address_components
    {
        public string long_name { get; set; }
    }

    public class plus_code
    {
        public string compound_code { get; set; }
        public string global_code { get; set; }
    }
    */
}
