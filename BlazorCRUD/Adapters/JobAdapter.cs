using BlazorCRUD.Models;
using BlazorCRUD.Services;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCRUD.Adapters
{
    public class JobAdapter : DataAdaptor
    {
        private readonly IJobService _jobService;

        public JobAdapter(IJobService jobService)
        {
            _jobService = jobService;
        }

        public override async Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string key = null)
        {
            var jobs = await _jobService.GetJobList();
            int count = jobs.Count;
            return dataManagerRequest.RequiresCounts ? new DataResult() { Result = jobs, Count = count } : jobs;
        }

        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            var job = data as Job;
            if (job != null)
            {
                await _jobService.CreateJob(job);
            }
            return data;
        }

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {
            if (int.TryParse(key, out var jobId))
            {
                await _jobService.DeleteJob(jobId);
            }
            return data;
        }

        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            var job = data as Job;
            if (job != null)
            {
                await _jobService.UpdateJob(job);
            }
            return data;
        }

    }
}
