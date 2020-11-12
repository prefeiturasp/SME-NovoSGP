using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioNotificacao
    {
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
        public IEnumerable<long> DREs { get; set; }
        public IEnumerable<long> UEs { get; set; }
    }
}
