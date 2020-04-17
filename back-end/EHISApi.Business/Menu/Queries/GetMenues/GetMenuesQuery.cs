using EHISApi.Business.Menu.ViewModels;
using EHISApi.Common.Extentions;
using EHISApi.Data.Entities;
using EHISApi.Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHISApi.Business.Menu.Queries.GetMenues
{
    public class GetMenuesQuery : IGetMenuesQuery
    {
        private readonly IRepository<EhisMenu> _menuRepository;

        public GetMenuesQuery(IRepository<EhisMenu> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<MenuViewModel>> ExecuteAsync()
        {
            var data = await _menuRepository.TableNoTracking
                .Select(n => new MenuViewModel()
                {
                    Id = n.Id,
                    DisplayName = n.DisplayName,
                    ParentId = n.ParentId,
                    Childrens = new List<MenuViewModel>()
                })
                .ToListAsync();



            foreach (var item in data)
            {
                var temp = data.Where(n => n.ParentId == item.Id);
                item.Childrens.AddRange(temp);
            }

            var result = data.Where(n => n.ParentId == null)
                .ToList();

            return result;
        }
    }
}
