using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IRedisCacheService
    {
        T GetData<T>(string key);
        void SetData<T>(string key, T data);
    }
}