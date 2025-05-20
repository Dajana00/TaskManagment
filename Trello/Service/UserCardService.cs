using AutoMapper;
using FluentResults;
using Trello.DTOs;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Service
{
    public class UserCardService : IUserCardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserCardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> AddUserToCardAsync(int cardId, int userId)
        {
            try
            {
                if (cardId == 0 || userId == 0)
                    return Result.Fail("Card ID and User ID must be valid.");

                await _unitOfWork.UserCards.AddUserToCardAsync(cardId, userId);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while adding user to card: {ex.Message}");
            }
        }

        public async Task<Result<ICollection<UserDto>>> GetCardMembersAsync(int cardId)
        {
            try
            {
                if (cardId == 0)
                    return Result.Fail<ICollection<UserDto>>("Card ID must be valid.");

                var users = await _unitOfWork.UserCards.GetCardMembersAsync(cardId);
                var userDtos = _mapper.Map<List<UserDto>>(users);

                return Result.Ok<ICollection<UserDto>>(userDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail<ICollection<UserDto>>($"Error while fetching card members: {ex.Message}");
            }
        }

        public async Task<Result<ICollection<UserDto>>> GetNonMembers(int cardId, int projectId)
        {
            try
            {
                if (cardId == 0 || projectId == 0)
                    return Result.Fail<ICollection<UserDto>>("Card ID and Project ID must be valid.");

                var nonMembers = await _unitOfWork.UserCards.GetNonMembers(cardId, projectId);
                var nonMemberDtos = _mapper.Map<ICollection<UserDto>>(nonMembers);

                return Result.Ok(nonMemberDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail<ICollection<UserDto>>($"Error while fetching non-members: {ex.Message}");
            }
        }
    }
}
