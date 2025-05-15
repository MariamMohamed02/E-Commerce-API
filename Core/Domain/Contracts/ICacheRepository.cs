using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICacheRepository
    {
        // set [key,value, timeToLive(expiration date)]
        Task SetAsync(string key, object value, TimeSpan duration);

        // get
        Task<string?> GetAsync(string key);
    }
}
