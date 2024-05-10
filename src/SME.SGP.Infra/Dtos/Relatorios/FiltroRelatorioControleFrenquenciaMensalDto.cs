using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroRelatorioControleFrenquenciaMensalDto
    {
        public bool ExibirHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public string CodigoTurma { get; set; }
        public string[] AlunosCodigo { get; set; }
        public IList<string> MesesReferencias { get; set; }
        public TipoFormatoRelatorio TipoFormatoRelatorio { get; set; }
        public string NomeUsuario { get; set; }
        public string CodigoRf { get; set; }
    }
}
