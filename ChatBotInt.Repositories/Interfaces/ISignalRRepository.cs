using System;
using System.Collections.Generic;
using ChatBotInt.Repositories.Models;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInt.Repositories.Interfaces
{
    public interface ISignalRRepository
    {
        Task<IEnumerable<SignalRDto>> FindAll();
        Task<SignalRDto> FindById(object id);
        Task<SignalRDto> FindConnectionByUserId(Guid userId);
        Task<int> InsertAsync(SignalRDto entity);
        Task InsertListAsync(List<SignalRDto> entity);
        Task<int> UpdateAsync(SignalRDto entity);
        Task<int> DeleteAsync(object id);
    }

}
