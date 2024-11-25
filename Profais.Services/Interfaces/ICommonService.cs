namespace Profais.Services.Interfaces;

public interface ICommonService
{
    Task<int> GetTotalPagesAsync(int pageSize);
}
