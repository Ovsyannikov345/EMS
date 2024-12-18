﻿using ChatService.BLL.Models;

namespace ChatService.BLL.Services.IServices
{
    public interface IChatService
    {
        Task<ChatModel> GetChatAsync(Guid id, string userAuth0Id, CancellationToken cancellationToken = default);

        Task<IEnumerable<ChatModel>> GetEstateChatListAsync(string currentUserAuth0Id, CancellationToken cancellationToken = default);

        Task<IEnumerable<ChatModel>> GetUserChatListAsync(string userAuth0Id, CancellationToken cancellationToken = default);

        Task<ChatModel> CreateChatAsync(string userAuth0Id, Guid estateId, CancellationToken cancellationToken = default);
    }
}
