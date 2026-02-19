using CompanyProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace CompanyProject.Services.Rules
{
    public class RuleEvaluator : IRuleEvaluator
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public RuleEvaluator(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<bool> IsEnabledAsync(string code)
        {
            var rule = await GetRuleAsync(code);
            return rule?.IsEnabled ?? false;
        }

        public async Task<T?> GetParametersAsync<T>(string code) where T : class
        {
            var rule = await GetRuleAsync(code);
            if (rule == null || string.IsNullOrWhiteSpace(rule.ValueJson))
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(rule.ValueJson);
        }

        private Task<CompanyProject.Data.Models.SystemRule?> GetRuleAsync(string code)
        {
            var cacheKey = $"system-rule:{code}";
            return _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _context.SystemRules.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code);
            })!;
        }
    }
}
