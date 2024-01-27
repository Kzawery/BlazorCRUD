using BlazorCRUD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCRUD.Services
{
    public interface IJobService
    {
        Task<bool> CreateJob(Job job);
        Task<List<Job>> GetJobList();
        Task<Job> GetJobById(int id);
        Task<bool> UpdateJob(Job job);
        Task<bool> DeleteJob(int id);
    }
}
