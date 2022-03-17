using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioAcompanhamentoAprendizagemDto
    {
        public FiltroRelatorioAcompanhamentoAprendizagemDto()
        {
            TipoRelatorio = TipoRelatorio.AcompanhamentoAprendizagem;
        }
        public long TurmaId { get; set; }
        public long? AlunoCodigo { get; set; }
        public int Semestre { get; set; }
        public long ComponenteCurricularId { get; set; }
        public TipoRelatorio TipoRelatorio { get; set; }

    }
}
