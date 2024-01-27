using BlazorCRUD.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCRUD.Services
{
    public class JobService : IJobService
    {
        private readonly IDbService _dbService;

        public JobService(IDbService dbService)
        {
            _dbService = dbService;
        }

        public async Task<bool> CreateJob(Job job)
        {
            try
            {
                var result = await _dbService.Insert<int>("INSERT INTO public.job (id, description) VALUES (@Id, @Description)", job);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<Job>> GetJobList()
        {
            var jobList = await _dbService.GetAll<Job>("SELECT * FROM public.job", new { });
            return jobList;
        }

        public async Task<Job> GetJobById(int id)
        {
            var job = await _dbService.Get<Job>("SELECT * FROM public.job WHERE id=@Id", new { Id = id });
            return job;
        }

        public async Task<bool> UpdateJob(Job job)
        {
            try
            {
                var updateJob = await _dbService.Update<int>("UPDATE public.job SET description=@Description WHERE id=@Id", job);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteJob(int id)
        {
            try
            {
                var deleteJob = await _dbService.Delete<int>("DELETE FROM public.job WHERE id=@Id", new { Id = id });
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
