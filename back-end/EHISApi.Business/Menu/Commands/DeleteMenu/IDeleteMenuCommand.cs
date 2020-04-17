using EHISApi.Common.Commands;
using System.Threading.Tasks;

namespace EHISApi.Business.Menu.Commands.DeleteMenu
{
    public interface IDeleteMenuCommand
    {
        Task<CommandResult> ExecuteAsync(int menuId, bool hasDeleteSubMenues = false);
    }
}