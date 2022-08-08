using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;

namespace MISA.QLTS.Api.Controllers
{
    public class CostSourceController : BaseController<CostSource>
    {
        public CostSourceController(ICostSourceRepository costSourceRepository, ICostSourceService costSourceService) : base(costSourceRepository, costSourceService)
        {
        }
    }
}
