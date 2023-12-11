using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volxyseat.Domain.Models.BaseModel
{
    public abstract class Base
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
