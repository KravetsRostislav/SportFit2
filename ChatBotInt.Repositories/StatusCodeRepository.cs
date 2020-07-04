using ChatBotInt.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace ChatBotInt.Repositories
{
    public class StatusCodeRepository : IStatusCodeRepository
    {

        private readonly string connectionString = null;

        public StatusCodeRepository(string conn)
        {
            connectionString = conn;
        }

        async Task<bool> IStatusCodeRepository.GetStatusCode()
        {
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    if (db.State != ConnectionState.Open)
                        await db.OpenAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
