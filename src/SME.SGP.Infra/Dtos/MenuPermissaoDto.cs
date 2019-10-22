using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MenuPermissaoDto
    {
        public MenuPermissaoDto()
        {
            SubMenus = new List<MenuPermissaoDto>();
        }

        public bool Alterar { get; set; }
        public string AlterarUrl { get; set; }
        public int Codigo { get; set; }
        public bool Consultar { get; set; }
        public string ConsultarUrl { get; set; }
        public string Descricao { get; set; }
        public bool Excluir { get; set; }
        public bool Incluir { get; set; }
        public string IncluirUrl { get; set; }
        public IList<MenuPermissaoDto> SubMenus { get; set; }
    }
}