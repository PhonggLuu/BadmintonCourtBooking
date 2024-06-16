﻿using BY.Data.Base;
using BY.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BY.Data.Repository
{
    public class ScheduleRepository : GenericRepository<Schedule>
    {
        public ScheduleRepository()
        {
        }
        public ScheduleRepository(Net1704_221_2_BYContext unitOfWorkContext) => _context = unitOfWorkContext;

        public async Task<List<string?>> GetTimeAsync(int courtId, DateOnly date)
        {
            List<string?> times = new List<string?>();

            var schedules = await _context.Schedules
                .Where(s => s.CourtId == courtId && s.Date == date)
                .ToListAsync();
                
            foreach (var schedule in schedules){
                if(schedule.From != null)
                {
                    times.Add(schedule.From.Value.ToString("HH:mm"));
                }
            }
            return times;
        }
    }
}
