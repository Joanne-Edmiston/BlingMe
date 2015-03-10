using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingMe.Domain.Entities
{
    public class Charm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public int ParentID { get; set; }

        public virtual Bracelet Parent { get; set; }
        public virtual ICollection<Bracelet> Children { get; set; }
    }
}
