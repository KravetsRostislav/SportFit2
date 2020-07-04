using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SoftPhone.Core.Service.Status
{
    public interface IStatusCodeService
    {
        Task<bool> GetStatusCode();
    }
}
