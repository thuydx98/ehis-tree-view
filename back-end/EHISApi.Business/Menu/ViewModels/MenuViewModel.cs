using System.Collections.Generic;

namespace EHISApi.Business.Menu.ViewModels
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int? ParentId { get; set; }
        public List<MenuViewModel> Childrens { get; set; }
    }
}
