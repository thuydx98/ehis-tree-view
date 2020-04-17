using EHISApi.Common.Commands;
using EHISApi.Data.Entities;
using System.Threading.Tasks;

namespace EHISApi.Business.Menu.Commands.SaveMenu
{
    public interface ISaveMenuCommand
    {
        Task<CommandResult> ExecuteAsync(EhisMenu model);
    }
}