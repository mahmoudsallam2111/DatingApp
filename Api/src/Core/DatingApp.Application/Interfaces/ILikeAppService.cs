﻿using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;

namespace DatingApp.Application.Interfaces
{
    public interface ILikeAppService
    {
        Task AddLikeAsync(int sourceUserId, string likedUserName);
        Task<List<LikeDto>> GetUserLikeAsync(string predicate, int userId);
    }
}
