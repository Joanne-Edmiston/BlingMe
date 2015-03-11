using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingMe.Domain.Entities
{
    public enum BraceletType
    {
        Person,
        Location,
        Project,
        Group,
        Interest
    }
    public class Bracelet
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public BraceletType Type { get; set; }
        public string Email { get; set; }
        public byte[] Avatar { get; set; }

        public virtual ICollection<Charm> Charms { get; set; }
    }
}
