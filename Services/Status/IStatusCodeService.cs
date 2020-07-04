using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Status
{
    public interface IStatusCodeService
    {
        Task<bool> GetStatusCode();
    }
}
