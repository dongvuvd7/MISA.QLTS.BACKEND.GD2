using Microsoft.Extensions.Configuration;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Infrastructure.Repositories
{
    public class CostSourceRepository : BaseRepository<CostSource>, ICostSourceRepository
    {
        #region Constructor
        public CostSourceRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion
    }
}
