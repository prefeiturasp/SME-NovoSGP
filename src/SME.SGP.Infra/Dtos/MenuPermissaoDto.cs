using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MenuPermissaoDto
    {
        public MenuPermissaoDto()
        {
            SubMenus = new List<MenuPermissaoDto>();
        }

        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public bool PodeAlterar { get; set; }
        public bool PodeConsultar { get; set; }
        public bool PodeExcluir { get; set; }
        public bool PodeIncluir { get; set; }
        public IList<MenuPermissaoDto> SubMenus { get; set; }
        public string Url { get; set; }
        public int Ordem { get; set; }

    }
}