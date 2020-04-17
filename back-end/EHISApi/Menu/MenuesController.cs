using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EHISApi.Business.Menu.Commands.DeleteMenu;
using EHISApi.Business.Menu.Commands.SaveMenu;
using EHISApi.Business.Menu.Queries.GetMenues;
using EHISApi.Common.Commands;
using EHISApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHISApi.Menu
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuesController : ControllerBase
    {
        private readonly IGetMenuesQuery _getMenuesQuery;
        private readonly ISaveMenuCommand _saveMenuCommand;
        private readonly IDeleteMenuCommand _deleteMenuCommand;

        public MenuesController(
            IGetMenuesQuery getMenuesQuery,
            ISaveMenuCommand saveMenuCommand,
            IDeleteMenuCommand deleteMenuCommand)
        {
            _getMenuesQuery = getMenuesQuery;
            _saveMenuCommand = saveMenuCommand;
            _deleteMenuCommand = deleteMenuCommand;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetMenuesAsync()
        {
            var result = await _getMenuesQuery.ExecuteAsync();

            return new ObjectResult(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SaveMenuesAsync(EhisMenu model)
        {
            var result = await _saveMenuCommand.ExecuteAsync(model);

            return StatusCode(result.GetStatusCode(), result.GetData());
        }

        [AllowAnonymous]
        [HttpDelete("{menuId:int=0}")]
        public async Task<ActionResult> DeleteMenuesAsync(int menuId, bool hasDeleteSubMenues = false)
        {
            var result = await _deleteMenuCommand.ExecuteAsync(menuId, hasDeleteSubMenues);

            return StatusCode(result.GetStatusCode(), result.GetData());
        }
    }
}