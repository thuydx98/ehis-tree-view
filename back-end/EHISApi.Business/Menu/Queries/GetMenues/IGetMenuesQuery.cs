using EHISApi.Business.Menu.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EHISApi.Business.Menu.Queries.GetMenues
{
    public interface IGetMenuesQuery
    {
        Task<List<MenuViewModel>> ExecuteAsync();
    }
}