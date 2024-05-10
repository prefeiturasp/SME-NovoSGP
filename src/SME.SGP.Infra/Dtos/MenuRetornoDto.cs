using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MenuRetornoDto
    {
        public MenuRetornoDto()
        {
            Menus = new List<MenuPermissaoDto>();
        }

        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public bool EhMenu { get; set; }
        public string Icone { get; set; }
        public IList<MenuPermissaoDto> Menus { get; set; }
        public int QuantidadeMenus { get { return Menus.Count; } }
    }
}