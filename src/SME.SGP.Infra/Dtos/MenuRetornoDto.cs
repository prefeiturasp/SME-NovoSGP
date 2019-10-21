using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MenuRetornoDto
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public IList<MenuPermissaoDto> Permissoes { get; set; }
    }
}