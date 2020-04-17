using System.Collections.Generic;

namespace EHISApi.Data.Entities
{
    public partial class EhisMenu
    {
        public EhisMenu()
        {
            SubMenus = new HashSet<EhisMenu>();
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int? ParentId { get; set; }

        public virtual EhisMenu Parent { get; set; }
        public virtual ICollection<EhisMenu> SubMenus { get; set; }
    }
}
