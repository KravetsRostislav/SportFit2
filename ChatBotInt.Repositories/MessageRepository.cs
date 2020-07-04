using ChatBotInt.Repositories.Interfaces;
using ChatBotInt.Repositories.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ChatBotInt.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        #region Fields

        private readonly string connectionString = null;        

        #endregion

        #region Ctor

        public MessageRepository(string conn)
        {
            connectionString = conn;
        }

        #endregion

        #region Methods

        // Отримати всі повідомлення по id сесії
        public async Task<IEnumerable<ChatMessageDto>> GetAllMessagesBySessionId(Guid sessionid)
        {            
            // створюємо колекцію динамічних параметрів і додаємо параметр sessionid, згідно якого буде відбуватися пошук повідомлень.
            var parametrs = new DynamicParameters();
            parametrs.Add("@sessionid", sessionid);

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {

                    // отримуємо колекцію з повідомленнями, використовуємо хранімку Messages_GetAllMessages_bySession
                    // (вхідний параметр: ідентифікатор сесії sessionid, згідно з яким буде відбуватися пошук повідомлень
                    var result = await db.QueryAsync<ChatMessageDto>("[dbo].[Messages_GetAllMessages_bySession]", parametrs, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Отримати всі повідомлення
        public async Task<IEnumerable<GetAllMessagesDto>> GetAllMessages()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    // отримуємо колекцію з повідомленнями, використовуємо хранімку Messages_GetAllMessages_forSupervisor
                    var result = await db.QueryAsync<GetAllMessagesDto>("[dbo].[Messages_GetAllMessages_forSupervisor]", commandType: CommandType.StoredProcedure);                   
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Отримати всі остані активні сесії по їх id
        public async Task<IEnumerable<MessageForSessionModel>> GetLastMessages(string sessionsId)
        {
            // створюємо колекцію динамічних параметрів і додаємо параметр sessionsId, згідно якого буде відбуватися пошук останніх активних сесій.
            var parametrs = new DynamicParameters();
            parametrs.Add("@sessions", sessionsId);

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    // отримуємо колекцію з повідомленнями, використовуємо хранімку Get_last_messages_by_sessions
                    // (вхідний параметр: ідентифікатор сесії sessionid, згідно з яким буде відбуватися пошук повідомлень
                    var result = await db.QueryAsync<MessageForSessionModel>("[dbo].[Get_last_messages_by_sessions]", parametrs, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Отримати всі повідомлення по оператору
        public async Task<IEnumerable<MonitoringChats>> GetMonitoringChatsByUserId(Guid userId)
        {
            // створюємо колекцію динамічних параметрів і додаємо параметр userId, згідно якого буде відбуватися пошук повідомлень.
            var parametrs = new DynamicParameters();
            parametrs.Add("@sessions", userId);

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    // отримуємо колекцію з повідомленнями, використовуємо хранімку GetMonitoringChatsByUserId
                    // (вхідний параметр: ідентифікатор userId, згідно з яким буде відбуватися пошук повідомлень
                    var result = await db.QueryAsync<MonitoringChats>("[dbo].[GetMonitoringChatsByUserId]", parametrs, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Закрити сесію на стороні чат-бота
        public async Task<bool> OperatorCloseSession(Guid sessionid)
        {
            // створюємо колекцію динамічних параметрів і додаємо параметр sessionid, згідно якого буде закрито сесію на стороні чат-бота.
            var parametrs = new DynamicParameters();
            parametrs.Add("@SessionId", sessionid);
            parametrs.Add("@servclose", 2);

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    // отримуємо bool-результат операції, використовуємо хранімку ChatSession_Close
                    // (вхідний параметри: ідентифікатор сесії sessionid та servclose - 2, згідно з яким буде закрито сесію
                    await db.ExecuteAsync("[dbo].[ChatSession_Close]", parametrs, commandType: CommandType.StoredProcedure);                    
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Відправка повідомлення від оператора до клієнта
        public async Task<string> SetMessageToDB(SendMessageModel sendMessage)
        {
            // створюємо колекцію динамічних параметрів і додаємо параметри, для відправки повідомлення від оператора до клієнта
            var parametrs = new DynamicParameters();
            parametrs.Add("@sessionid", sendMessage.SessionId);
            parametrs.Add("@MessageContent", sendMessage.MessageText);
            parametrs.Add("@DateTime", sendMessage.DateTime);
            parametrs.Add("@UserId", sendMessage.UserId);
            //parametrs.Add("@UserEmail", sendMessage.UserEmail);

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    // отримуємо повідомлення, використовуємо хранімку Send_message_from_operator
                    // (вхідний параметри: sessionid, MessageContent, DateTime, UserId, які потрібні для відправки повідомлення
                    string result = await db.QuerySingleAsync<string>("[dbo].[Send_message_from_operator]", parametrs, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Зберегти тему, по якій клієнт звернувся з питанням
        public async Task<bool> SaveThemeOfTreatment(ThemeOfTreatmentModel model)
        {
            try
            {
                // створюємо колекцію динамічних параметрів і додаємо параметр, згідно якого буде відбуватися пошук Dictionary.
                var parameters = new DynamicParameters();
                parameters.Add("@SessionId", model.SessionId);
                parameters.Add("@Theme", model.Theme);
                parameters.Add("@SubTheme", model.SubTheme);
                parameters.Add("@Comment", model.Comment);
                parameters.Add("@UserId", model.UserId);
                parameters.Add("@ThemeId", model.ThemeId);
                parameters.Add("@SubThemeId", model.SubThemeId);

                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    await db.ExecuteAsync("[dbo].[TODO_procedure]", parameters, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Перевести клієнта назад в ITR
        //public async Task<bool> ReturnBrancheToITR(BrancheITRModel iTRDto)
        public async Task<bool> ReturnBrancheToITR(ReturnBrancheITRModel iTRDto)
        {
            // створюємо колекцію динамічних параметрів і додаємо параметри, для переводу клієнта в ITR
            var parametrs = new DynamicParameters();
            parametrs.Add("@sessionid", iTRDto.SessionId);
            parametrs.Add("@menuitemid", iTRDto.MenuItemId);
            parametrs.Add("@AutoText", iTRDto.AutoText);

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    // отримуємо bool-результат операції, використовуємо хранімку opeator_chat_return_to_menu
                    // (вхідний параметри: sessionid, menuitemid, AutoText, які потрібні для перевода клієнта в ITR
                    await db.ExecuteAsync("[dbo].[opeator_chat_return_to_menu]", parametrs, commandType: CommandType.StoredProcedure);                    
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Отримати інформацію по клієнту
        public async Task<CrmClientDto> GetClientBySessionId(Guid sessionId)
        {
            // створюємо колекцію динамічних параметрів і додаємо параметр sessionId, згідно якого буде відбуватися пошук інформації по клієнту.
            var parametrs = new DynamicParameters();
            parametrs.Add("@sessionid", sessionId);

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    // отримуємо інформацію по клієнту, використовуємо хранімку Get_client_info_for_operator
                    // (вхідний параметри: sessionid, згідно якого буде відбуватися пошук інформації по клієнту
                    var client = await db.QuerySingleAsync<CrmClientDto>("[dbo].[Get_client_info_for_operator]", parametrs, commandType: CommandType.StoredProcedure);                    
                    return client;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion
    }
}
