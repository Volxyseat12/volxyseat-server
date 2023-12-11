using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volxyseat.Domain.Models.BaseModel;
using Volxyseat.Domain.Models.SubscriptionModel;

namespace Volxyseat.Domain.Models.Transaction
{
    public class TransactionModel : Base
    {
        public Guid Subscription { get; set; }
        public Guid Client { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public int TermInDays { get; set; }
    }
}
