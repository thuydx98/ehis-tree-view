using EHISApi.Common.Commands;
using EHISApi.Constants;
using EHISApi.Data.Entities;
using EHISApi.Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EHISApi.Business.Menu.Commands.SaveMenu
{
    public class SaveMenuCommand : ISaveMenuCommand
    {
        private readonly IRepository<EhisMenu> _menuRepository;

        public SaveMenuCommand(IRepository<EhisMenu> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<CommandResult> ExecuteAsync(EhisMenu model)
        {
            try
            {
                CommandResult isNotValidData = CheckValidData(model);

                if (isNotValidData != null)
                {
                    return isNotValidData;
                }

                EhisMenu menu = await _menuRepository.GetByIdAsync(model.Id);

                if (model.Id != 0 && menu == null)
                {
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Description = MessageError.NotFound
                    });
                }

                menu = menu ?? new EhisMenu();
                menu.DisplayName = model.DisplayName;
                menu.ParentId = model.ParentId;

                if (menu.Id == 0)
                {
                    await _menuRepository.InsertAsync(menu);
                }
                else
                {
                    await _menuRepository.UpdateAsync(menu);
                }

                return CommandResult.SuccessWithData(menu);
            }
            catch (Exception)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = MessageError.InternalServerError
                });
            }
        }

        private CommandResult CheckValidData(EhisMenu model)
        {
            if (model == default
                || model.DisplayName == default
                || model.ParentId <= 0
                || (model.Id > 0 && model.Id == model.ParentId))
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Description = MessageError.SomeDataEmptyOrInvalid
                });
            }

            return null;
        }
    }
}
