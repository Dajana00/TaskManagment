﻿using Microsoft.EntityFrameworkCore;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;

namespace Trello.Repository
{
    public class SprintRepository : ISprintRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SprintRepository> _logger;

        public SprintRepository(AppDbContext context, ILogger<SprintRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Sprint?> GetActiveByProjectId(int projectId)
        {
            try
            {
                var project = await _context.Sprints
                    .FirstOrDefaultAsync(p => p.ProjectId == projectId);

                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching sprint with project id {projectId}");
                throw new Exception($"An error occurred while retrieving project with this project id: {projectId}. Please try again.");
            }
        }
        public async Task CreateAsync(Sprint sprint)
        {
            try
            {
                _context.Sprints.Add(sprint);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Sprint added successifully");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding sprint.");
                throw new Exception("A database error occurred while adding the sprint. Please try again.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Unexpected error occurred while adding sprint.");
                throw new Exception($"An unexpected error occurred while adding the sprint. Please try again. {ex}");
            }
        }
        public async Task<ICollection<Sprint>> GetByProjectId(int projectId)
        {
            try
            {
                var cards = await _context.Sprints
                     .Where(p => p.ProjectId == projectId)
                     .ToListAsync();
                _logger.LogInformation($"Successiffuly fetching sprints with projct id {projectId}");

                return cards;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching sprints with projectId {projectId}");
                throw new Exception($"An error occurred while retrieving sprints with projectId : {projectId}. Please try again.");
            }
        }

    }
}
