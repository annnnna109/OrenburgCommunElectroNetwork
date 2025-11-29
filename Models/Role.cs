using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrenburgCommunElectroNetwork.Models
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string Permissions { get; set; }
        public string Description { get; set; }
    }
}