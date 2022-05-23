using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioFrequenciaMensalDto
    {
        public bool ExibirHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public IList<string> CodigosTurmas { get; set; }
        public IList<string> MesesReferencias { get; set; }
        public int? ApenasAlunosPercentualAbaixoDe { get; set; }
        public TipoFormatoRelatorio TipoFormatoRelatorio { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
    }
}
