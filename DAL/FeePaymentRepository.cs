using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FeePaymentRepository : BaseRepository<FeePayment>
    {
        public FeePaymentRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
