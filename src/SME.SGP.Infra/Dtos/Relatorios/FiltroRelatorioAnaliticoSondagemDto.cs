using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioAnaliticoSondagemDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public TipoSondagem TipoSondagem { get; set; }
        public int Periodo { get; set; } 
        public string LoginUsuarioLogado { get; set; }
        public Guid PerfilUsuarioLogado { get; set; }
    }
}
