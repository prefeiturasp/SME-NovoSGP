using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioMapeamentoEstudantesDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int? Semestre { get; set; }
        public string[] TurmasCodigo { get; set; }
        public string AlunoCodigo { get; set; }
        public long[] PareceresConclusivosIdAnoAnterior { get; set; }
        public long OpcaoRespostaIdDistorcaoIdadeAnoSerie { get; set; }
        public long OpcaoRespostaIdPossuiPlanoAEE { get; set; }
        public long OpcaoRespostaIdAcompanhadoNAAPA { get; set; }
        public string OpcaoRespostaPossuiPAP { get; set; }
        public long OpcaoRespostaIdProgramaSPIntegral { get; set; }
        public string OpcaoRespostaHipoteseEscrita { get; set; }
        public string OpcaoRespostaAvaliacaoExternaProvaSP { get; set; }
        public long OpcaoRespostaIdFrequencia { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
    }
}
