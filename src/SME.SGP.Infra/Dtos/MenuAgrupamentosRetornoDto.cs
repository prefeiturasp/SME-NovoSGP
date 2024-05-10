using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MenuAgrupamentosRetornoDto
    {
        public string Agrupamento { get; set; }
        public int AgrupamentoCodigo { get; set; }
        public IList<MenuRetornoDto> Menus { get; set; }
    }
}