﻿using Microsoft.EntityFrameworkCore;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;

namespace Trello.Repository
{
    public class UserStoryRepository : IUserStoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserStoryRepository> _logger;

        public UserStoryRepository(AppDbContext context, ILogger<UserStoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task CreateAsync(UserStory userStory)
        {
            try
            {
                _context.UserStories.Add(userStory);
                await _context.SaveChangesAsync();
                _logger.LogInformation("UserStory added successifully");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding userStory.");
                throw new Exception("A database error occurred while adding the userStory. Please try again.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Unexpected error occurred while adding userStory.");
                throw new Exception($"An unexpected error occurred while adding the userStory. Please try again. {ex}");
            }
        }
        public async Task<ICollection<UserStory>> GetByBacklogId(int backlogId)
        {
            try
            {
                var userStories = await _context.UserStories
                    .Where(us => us.BacklogId == backlogId)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} user stories for BacklogId: {BacklogId}", userStories.Count, backlogId);

                return userStories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user stories for BacklogId: {BacklogId}", backlogId);
                throw new Exception($"An error occurred while fetching user stories for backlogId {backlogId}. Please try again.");
            }
        }
        public async Task<ICollection<UserStory>> GetAll()
        {
            try
            {
                var stories = await _context.UserStories.ToListAsync();
                _logger.LogInformation("Successfully fetched all stories.");
                return stories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all stories.");
                throw new Exception("An error occurred while retrieving all stories. Please try again.");
            }
        }
        public async Task<UserStory?> GetByIdAsync(int id)
        {
            try
            {
                var userStory = await _context.UserStories
                    .FirstOrDefaultAsync(p => p.Id == id);
                _logger.LogInformation($"Successiffuly fetching userStory with id {id}");

                return userStory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching user story with id {id}");
                throw new Exception($"An error occurred while retrieving user story with this id: {id}. Please try again.");
            }
        }
        public async Task Delete(UserStory userStory)
        {
            try
            {
                _context.UserStories.Remove(userStory);
                await _context.SaveChangesAsync();  
                _logger.LogInformation("UserStory with ID {Id} deleted successfully.", userStory.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user story with ID {Id}", userStory.Id);
                throw new Exception("An error occurred while deleting the user story. Please try again.");
            }
        }


    }
}
