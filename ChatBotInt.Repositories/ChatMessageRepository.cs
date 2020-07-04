using ChatBotInt.Repositories.Interfaces;
using ChatBotInt.Repositories.Models;
using Dapper;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotInt.Repositories
{
    public class ChatMessageRepository: IChatMessageRepository
    {
        #region Fields

        private readonly string connectionString = null;
        Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Ctor

        public ChatMessageRepository(string conn)
        {
            connectionString = conn;
        }

        #endregion

        #region Methods
        //Get all message by id sessions
        public async Task<IEnumerable<ChatMessageDTO>> GetAllMessagesBySessionId(Guid sessionId)
        {
            _logger.Info(String.Format($"{"ChatMessageRepository",-20} >>> {"GetAllMessagesBySessionId",-20} >>> {"START: SessionId",-10}{sessionId, -20}"));
            IEnumerable<ChatMessageDTO> result = new List<ChatMessageDTO>();
           
            string sql = @"Select   msg.[MessageDateTime]
                                    , msg.[SessionId]
                                    , msg.[MessageContent]
                                    , msg.[IsRead]
                                    , msg.[SenderUserId]
                                    , msg.[IsGroup]
                                    , msg.[GroupId]
                            from [Chat].[dbo].[Messages] msg
                            join [dbo].[Session] cs on msg.SessionId = cs.Id
                            where msg.[SessionId] = @sessionId and (len(msg.[MessageContent]) > 0 or msg.[MessageContent] <> '')
                            order by msg.[MessageDateTime] asc";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {

                    //Get message collection by the sessionId
                    //(Input parameter: Session ID)
                    result = await db.QueryAsync<ChatMessageDTO>(sql, new { sessionId });
                    
                    _logger.Debug($"ChatMessage: GetAllMessagesBySessionId; SessionId: {sessionId}, Result: {JsonConvert.SerializeObject(result)}");
                    
                    //var result = await db.QueryAsync<ChatMessageDTO>("[dbo].[ChatMessages_GetAllMessageBySessionId]", parametrs, commandType: CommandType.StoredProcedure);
                    #region procedureText
                    /*USE Chat
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Kirill Boev
-- Create date: 30.04.2020
-- Description: Get message collection
--              (Input parameter: Session ID of the sessionid that will search for messages)
-- =============================================
CREATE PROCEDURE [dbo].[ChatMessages_GetAllMessageBySessionId]
@sessionId uniqueidentifier
with execute as self
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

Select   msg.[MessageDateTime]
		,msg.[SessionId]
		,msg.[MessageContent]
		,msg.[IsRead]
		,msg.[SenderUserId]
		,msg.[IsGroup]
		,msg.[GroupId]
from [dbo].[Messages] msg
join [dbo].[Session] cs on msg.SessionId = cs.Id 
where [SessionId]=@sessionId and (len([MessageContent])>0 or [MessageContent]<>'')
order by msg.[MessageDateTime] asc

END

--Create PROCEDURE [dbo].[SP_GetMessagesDescription_BySessionId]
--	@SessionId UNIQUEIDENTIFIER
--AS
--BEGIN
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
--	SET NOCOUNT ON;

--    -- Insert statements for procedure here
--	SELECT [Description],[DateTime],[Type]
--	FROM [dbo].[Messages]
--	WHERE [SessionId] = @SessionId AND [Description] is not null
--	ORDER BY [DateTime] 
--END


*/
                    #endregion
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return result;
            }

            return result;
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetAllMessagesByGroupId(Guid groupId)
        {
            _logger.Info(String.Format($"{"ChatMessageRepository",-20} >>> {"GetAllMessagesByGroupId",-20} >>> {"START: GroupId",-10}{groupId}"));
            
            IEnumerable<ChatMessageDTO> result=new List<ChatMessageDTO>();
            if (groupId == null || groupId == Guid.Empty)
                return result;

            var parametrs = new DynamicParameters();
            parametrs.Add("@groupId", groupId);

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    //Get message collection
                    result = await db.QueryAsync<ChatMessageDTO>(@"Select     msg.[MessageDateTime]
                                                                                , msg.[SessionId]
                                                                                , msg.[MessageContent]
                                                                                , msg.[IsRead]
                                                                                , msg.[SenderUserId]
                                                                                , msg.[IsGroup]
                                                                                , msg.[GroupId]
                                                                        from [dbo].[Messages] msg
                                                                        where [GroupId] = @groupId and (len(msg.[MessageContent]) > 0 or msg.[MessageContent] <> '')
                                                                        order by msg.[MessageDateTime] asc", parametrs);
                    
                    _logger.Debug(String.Format($"{"ChatMessageRepository",-20} >>> {"GetAllMessagesByGroupId",-20} >>> {"Result",-10}{result} >>> {"Group:",-20}{groupId}"));
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace, 20}.");
                return result;
            }

            return result;
        }
        public async Task<IEnumerable<ChatMessageDTO>> GetAllMessagesByUserId(Guid userId)
        {
            _logger.Info(String.Format($"{"ChatMessageRepository",-20} >>> {"GetAllMessagesByUserId",-20} >>> {"Start",-10} >>> {"UserId",-10}{userId}"));
            IEnumerable<ChatMessageDTO> result= new List<ChatMessageDTO>();

            var parametrs = new DynamicParameters();
            parametrs.Add("@userId", userId);
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    //Get message collection
                    result = await db.QueryAsync<ChatMessageDTO>(@"Select       msg.[MessageDateTime]
                                                                                , msg.[SessionId]
                                                                                , msg.[MessageContent]
                                                                                , msg.[IsRead]
                                                                                , msg.[SenderUserId]
                                                                                , msg.[IsGroup]
                                                                                , msg.[GroupId]
                                                                        from [dbo].[Messages] msg
                                                                        where [SenderUserId] = @userId and (len(msg.[MessageContent]) > 0 or msg.[MessageContent] <> '')
                                                                        order by msg.[MessageDateTime] asc", parametrs);

                    _logger.Debug(String.Format($"{"ChatMessageRepository",-20} >>> {"GetAllMessagesByUserId",-20} >>> {"Result",-10}{result,20} >>> {"UserId",-20}{userId}"));
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return result;
            }

            return result;
        }
        // Відправка повідомлення від оператора до оператора
        public async Task<int> SaveMessageToDB(ChatMessageDTO sendMessage)
        {
            int result=0;
            // створюємо колекцію динамічних параметрів і додаємо параметри, для відправки повідомлення від оператора до клієнта
            var parametrs = new DynamicParameters();
            parametrs.Add("@SessionId", sendMessage.SessionId);
            parametrs.Add("@MessageContent", sendMessage.MessageContent);
            parametrs.Add("@MessageDateTime", sendMessage.MessageDateTime);
            parametrs.Add("@IsRead", sendMessage.IsRead);
            parametrs.Add("@SenderUserId", sendMessage.SenderUserId);
            parametrs.Add("@IsGroup", sendMessage.IsGroup);
            parametrs.Add("@GroupId", sendMessage.GroupId);
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    result = await db.ExecuteAsync(@"
                                            INSERT INTO [dbo].[messages] 
                                                        ([messageid], 
                                                            [sessionid], 
                                                            [messagedatetime], 
                                                            [messagecontent], 
                                                            [isread], 
                                                            [senderuserid], 
                                                            [isgroup], 
                                                            [groupid]) 
                                            VALUES      (Newid(), 
                                                            @SessionId, 
                                                            @MessageDateTime, 
                                                            @MessageContent, 
                                                            @IsRead, 
                                                            @SenderUserId, 
                                                            @IsGroup, 
                                                            @GroupId) ", parametrs);

                    _logger.Debug($"ChatMessage: SaveMessageToDB; Message: {JsonConvert.SerializeObject(sendMessage)}, Result: {result}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return result;
            }

            return result;
        }

        public async Task<int> AddSendMessages(SendMessageModel message)
        {
            int result=0;
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    result=await db.ExecuteAsync(@"
                                    INSERT INTO [dbo].[messages] 
                                                ([messageid], 
                                                 [sessionid], 
                                                 [messagedatetime], 
                                                 [messagecontent], 
                                                 [isread], 
                                                 [senderuserid]) 
                                    VALUES      (Newid(), 
                                                 @sessionId, 
                                                 @messageDateTime, 
                                                 @messageContent, 
                                                 @isRead, 
                                                 @senderUserId)"
                                    , new { sessionId = message.SessionId, messageContent = message.MessageText, messageDateTime = message.DateTime
                                    , isread=true, senderuserid = message.UserId });

                    _logger.Debug($"ChatMessage AddSendMessages; Result: {result}, Message: {JsonConvert.SerializeObject(message)}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return result;
            }

            return result;
        }
        //todo change to update and upd some is delete
        public async Task<bool> DeleteSendMessages(int messageId) 
        {
            int result;
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    if (db.State != ConnectionState.Open)
                        await db.OpenAsync();

                   result = await db.ExecuteAsync($@"DELETE FROM [dbo].[Messages]
			    WHERE [MessageId]=@messageId", new { messageId });

                    _logger.Debug($"ChatMessage DeleteSendMessages; MessageId: {messageId}, Result: {result}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return false;
            }

            return result == 1 ? true : false;

        }

        public async Task<List<Guid>> GetUsersBySessionId(Guid sessionId)
        {
            IEnumerable<Guid> usersBySession = new List<Guid>();
            var parametrs = new DynamicParameters();
            parametrs.Add("@sessionId", sessionId);
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    //Get users collection
                    usersBySession = await db.QueryAsync<Guid>(@"Select msg.[SenderUserId] from [dbo].[Messages] msg where msg.[SessionId] = @sessionId");
                    _logger.Debug($"ChatMessage: GetUsersBySessionId; Session: {sessionId}, Users: {usersBySession.Count()}");                
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return usersBySession.AsList();
            }

            var result = usersBySession.AsList();
            return result;
        }
        public async Task<IEnumerable<Guid>> GetUsersByGroupId(Guid groupId)
        {
            IEnumerable<Guid> usersByGroup=new List<Guid>();

            if (groupId == null||groupId==Guid.Empty)
                return usersByGroup;
            
            var parametrs = new DynamicParameters();
            parametrs.Add("@groupId", groupId);
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    //Get users collection
                    usersByGroup = await db.QueryAsync<Guid>(@"Select msg.[SenderUserId] from [dbo].[Messages] msg where msg.[GroupId] = @groupId");
                    _logger.Debug($"ChatMessage: GetUsersByGroupId; Group: {groupId}, Users: {usersByGroup.Count()}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return usersByGroup;
            }

            var result = usersByGroup.AsList();
            return result;
        }
        #endregion

    }
}
