using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Domain.Common
{
    public class Entity:IEntity
    {
        public int Id { get; set; }
        // public bool isDeleted { get; set; }
    }
}
