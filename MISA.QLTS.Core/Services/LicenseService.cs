using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Services
{
    public class LicenseService : BaseService<License>, ILicenseService
    {
        #region Constructor
        ILicenseRepository _licenseRepository;
        public LicenseService(ILicenseRepository licenseRepository) : base(licenseRepository)
        {
            _licenseRepository = licenseRepository;
        }
        #endregion
    }
}
