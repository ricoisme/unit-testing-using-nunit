using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MyAPI.Dtos;
using MyAPI.Extension;
using MyAPI.Filters;
using MyAPI.Repositorys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    public class LifeController : Controller
    {
        private readonly ILogger<LifeController> _logger;
        private readonly IDistributedCache _cache;
        private readonly ICustomersRepository _customersRepository;
        private List<StudentResponse> _students;
        public LifeController(
            ILogger<LifeController> logger,
            IDistributedCache cache,
            ICustomersRepository customersRepository)
        {
            _logger = logger;
            _cache = cache;
            _customersRepository = customersRepository;
            LoadStudents().GetAwaiter().GetResult();
        }

        [CacheResultFilter(15 * 1000)]
        [HttpGet("GetStudent")]
        public StudentResponse GetStudent([FromQuery]string name)
        {
            return _students?.Where(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
        }

        [CacheResultFilter(5 * 1000)]
        [HttpGet("GetAllStudent")]
        public List<StudentResponse> GetAllStudent()
        {
            return _students ?? null;
        }

        [HttpGet("GetSymbol")]
        public async Task<string> GetSymbol([FromQuery]string symbolCode)
        {
            var cacheKey = "Life".GenerateCacheKey("GetSymbol", "Get");
            var symbol = await _cache.GetStringAsync(cacheKey).ConfigureAwait(false);

            if (symbol == null)
            {
                symbol = _customersRepository.Find(symbolCode).Name;

                await _cache.SetStringAsync(cacheKey, symbol, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(10 * 1000) })
                    .ConfigureAwait(false);
            }

            return symbol;
        }

        private async Task LoadStudents()
        {
            var cacheKey = "Life".GenerateCacheKey("LoadStudents", "Get");
            var result = await _cache.GetStringAsync(cacheKey);
            if (result == null)
            {
                _students = new List<StudentResponse>()
                {
                    new StudentResponse{ Name = "Rico", Age = 38, CreateDate = DateTime.UtcNow },
                    new StudentResponse{ Name = "Sherry", Age = 36, CreateDate = DateTime.UtcNow },
                    new StudentResponse{ Name = "FiFi", Age = 6, CreateDate = DateTime.UtcNow }
                };
                var json = JsonConvert.SerializeObject(_students);
                await _cache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(60 * 1000) })
                    .ConfigureAwait(false);
            }
            else
            {
                _students = JsonConvert.DeserializeObject<List<StudentResponse>>(result);
            }
        }
    }


}