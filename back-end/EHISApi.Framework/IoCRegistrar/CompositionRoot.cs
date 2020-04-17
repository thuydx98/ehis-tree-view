using DryIoc;
using System;
using System.Net.Http;
using EHISApi.Data.Services;
using EHISApi.Business.Menu.Queries.GetMenues;
using EHISApi.Business.Menu.Commands.SaveMenu;
using EHISApi.Business.Menu.Commands.DeleteMenu;

namespace EHISApi.Framework.IoCRegistrar
{
    public class CompositionRoot
    {
        public CompositionRoot(IRegistrator registrator)
        {
            // General
            registrator.Register<Lazy<HttpClient>>(Reuse.InWebRequest);
            registrator.Register<IDbContext, ApplicationDbContext>(Reuse.InWebRequest);
            registrator.Register(typeof(IRepository<>), typeof(EfRepository<>), Reuse.InWebRequest);

            // Menu Business
            registrator.Register<IGetMenuesQuery, GetMenuesQuery>(Reuse.InWebRequest);
            registrator.Register<ISaveMenuCommand, SaveMenuCommand>(Reuse.InWebRequest);
            registrator.Register<IDeleteMenuCommand, DeleteMenuCommand>(Reuse.InWebRequest);

        }
    }
}
