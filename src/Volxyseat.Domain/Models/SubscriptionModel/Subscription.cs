using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volxyseat.Domain.Models.BaseModel;

namespace Volxyseat.Domain.Models.SubscriptionModel
{
    public class Subscription : Base
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int TermInDays { get; set; }
        public bool IsActive { get; set; }
    }
}
