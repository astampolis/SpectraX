namespace CompanyProject.Services.Rules
{
    public interface IRuleEvaluator
    {
        Task<bool> IsEnabledAsync(string code);
        Task<T?> GetParametersAsync<T>(string code) where T : class;
    }
}
