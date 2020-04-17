using EHISApi.Common.Commands;
using EHISApi.Constants;
using EHISApi.Data.Entities;
using EHISApi.Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EHISApi.Business.Menu.Commands.DeleteMenu
{
    public class DeleteMenuCommand : IDeleteMenuCommand
    {
        private readonly IRepository<EhisMenu> _menuRepository;

        public DeleteMenuCommand(IRepository<EhisMenu> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<CommandResult> ExecuteAsync(int menuId, bool hasDeleteSubMenues = false)
        {
            try
            {
                if (menuId == default)
                {
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Description = MessageError.SomeDataEmptyOrInvalid
                    });
                }

                EhisMenu menu = await _menuRepository.Table
                    .Where(n => n.Id == menuId)
                    .Include(n => n.SubMenus)
                    .SingleOrDefaultAsync();

                if (menu == null)
                {
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Description = MessageError.NotFound
                    });
                }



                if (hasDeleteSubMenues == true)
                {
                    List<EhisMenu> deleteData = RecursiveQuery(menu);

                    await _menuRepository.DeleteAsync(deleteData);
                }
                else
                {
                    foreach (var item in menu.SubMenus)
                    {
                        item.ParentId = menu.ParentId;
                    }

                    await _menuRepository.UpdateAsync(menu.SubMenus);
                    await _menuRepository.DeleteAsync(menu);
                }

                return CommandResult.Success;
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

        private List<EhisMenu> RecursiveQuery(EhisMenu parent)
        {
            var temp = new List<EhisMenu>();

            if (parent.SubMenus != null)
            {
                var children = _menuRepository.Table
                    .Where(x => x.ParentId == parent.Id)
                    .Include(x => x.SubMenus);

                foreach (var child in children)
                {
                    temp.AddRange(RecursiveQuery(child));
                }
            }

            temp.Add(parent);

            return temp;
        }
    }
}
