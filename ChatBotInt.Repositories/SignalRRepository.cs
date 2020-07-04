using ChatBotInt.Repositories.Interfaces;
using ChatBotInt.Repositories.Models;
using Dapper;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInt.Repositories
{
    public class SignalRRepository: ISignalRRepository
    {
        #region Fields
        Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly string connectionString = null;

        #endregion

        #region Ctor

        public SignalRRepository(string conn)
        {
            connectionString = conn;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<SignalRDto>> FindAll()
        {
            IEnumerable<SignalRDto> items = null;

            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    if (db.State != ConnectionState.Open)
                        await db.OpenAsync();

                    items = await db.QueryAsync<SignalRDto>("Select * From [dbo].[SignalRConnection];");

                    _logger.Debug($"SignalR: FindAll; Result: {JsonConvert.SerializeObject(items)}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"SignalR: FindAll; Message: {e.Message}.");
            }

            return items;
        }

        public async Task<SignalRDto> FindById(object id)
        {
            if (id == null) 
                return null;
            SignalRDto item = null;

            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    if (db.State != ConnectionState.Open)
                        await db.OpenAsync();

                    item = await db.QueryFirstOrDefaultAsync<SignalRDto>("Select * From [dbo].[SignalRConnection] Where ConnectionID = @id", new { id });
                    _logger.Debug($"SignalR: FindById; Result: {JsonConvert.SerializeObject(item)}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"SignalR: FindById; Id: {id}, Message: {e.Message}.");
            }

            return item;
        }

        public async Task<SignalRDto> FindConnectionByUserId(Guid userId)
        {
            if (userId == null)
                return null;
            SignalRDto item = null;

            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    if (db.State != ConnectionState.Open)
                        await db.OpenAsync();

                    item = await db.QueryFirstOrDefaultAsync<SignalRDto>("Select * From [dbo].[SignalRConnection] Where [UserId] = @userId", new { userId });
                    
                    _logger.Debug($"SignalR: FindConnectionByUserId; Result: {JsonConvert.SerializeObject(item)}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"SignalR: FindConnectionByUserId; User: {userId}, Message: {e.Message}.");
            }

            return item;
        }

        public async Task<int> InsertAsync(SignalRDto entity)
        {
            if (entity == null)
                return 0;
            
            var result = 0;

            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    if (db.State != ConnectionState.Open)
                        await db.OpenAsync();

                    result=await db.ExecuteAsync($@"INSERT INTO [dbo].[SignalRConnection]
                                                   ([SignalRConnectionId]
                                                   ,[ConnectionID]
                                                   ,[Connected]
                                                   ,[TimeStart]
                                                   ,[TimeEnd]
                                                   ,[UserId]
                                                   ,[StatusConnectionID])
                                             VALUES
                                                   (newid()
                                                   ,@{nameof(entity.ConnectionID)}
                                                   ,@{nameof(entity.Connected)}
                                                   ,@{nameof(entity.TimeStart)}
                                                   ,@{nameof(entity.TimeEnd)}
                                                   ,@{nameof(entity.UserId)}
                                                   ,@{nameof(entity.StatusConnectionID)})", entity);

                    _logger.Debug($"SignalR: InsertAsync; Result: {result}, Entity: {JsonConvert.SerializeObject(entity)}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"SignalR: InsertAsync; Entity: {JsonConvert.SerializeObject(entity)}, Message: {e.Message}.");
            }

            return result;
        }

        public async Task InsertListAsync(List<SignalRDto> entity)
        {
            if (entity == null)
                return;
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    if (db.State != ConnectionState.Open)
                        await db.OpenAsync();

                    var result=await db.ExecuteAsync($@"INSERT INTO [dbo].[SignalRConnection]
                                                   ([SignalRConnectionId]
                                                   ,[ConnectionID]
                                                   ,[Connected]
                                                   ,[TimeStart]
                                                   ,[TimeEnd]
                                                   ,[UserId]
                                                   ,[StatusConnectionID])
                                             VALUES
                                                   (newid()
                                                   ,@ConnectionID
                                                   ,@Connected
                                                   ,@TimeStart
                                                   ,@TimeEnd
                                                   ,@UserId
                                                   ,@StatusConnectionID)", entity);

                    _logger.Debug($"SignalR: InsertListAsync; Result: {result} Entity: {JsonConvert.SerializeObject(entity)}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"SignalR: InsertListAsync; Entity: {JsonConvert.SerializeObject(entity)}, Message: {e.Message}.");
            }
        }

        public async Task<int> UpdateAsync(SignalRDto entity)
        {
            if (entity == null)
                return 0;

            var result = 0;

            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    if (db.State != ConnectionState.Open)
                        await db.OpenAsync();

                    result= await db.ExecuteAsync($@"UPDATE [dbo].[SignalRConnection]
                                               SET [ConnectionID] = @{nameof(entity.ConnectionID)}
                                                  ,[Connected] = @{nameof(entity.Connected)}
                                                  ,[TimeStart] = @{nameof(entity.TimeStart)}
                                                  ,[TimeEnd] = @{nameof(entity.TimeEnd)}
                                                  ,[UserId] = @{nameof(entity.UserId)}
                                                  ,[StatusConnectionID] = @{nameof(entity.StatusConnectionID)}
                                             WHERE [SignalRConnectionId] = @{nameof(entity.SignalRConnectionId)}", entity);

                    _logger.Debug($"SignalR: UpdateAsync; Result: {result}, Entity: {JsonConvert.SerializeObject(entity)}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"SignalR: UpdateAsync; Entity: {JsonConvert.SerializeObject(entity)}, Message: {e.Message}.");
            }

            return result;
        }

        public async Task<int> DeleteAsync(object id)
        {
            if (id == null)
                return 0;

            var result = 0;
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    if (db.State != ConnectionState.Open)
                        await db.OpenAsync();

                    result=await db.ExecuteAsync($@"DELETE FROM [dbo].[SignalRConnection]
                                                   WHERE [ConnectionID] = @id", new { id });

                    _logger.Debug($"SignalR: DeleteAsync; Result: {result}, Connection: {id}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"SignalR. DeleteAsync: Connection: {id}, Message: {e.Message}.");
            }

            return result;
        }

        #endregion
    }
}
