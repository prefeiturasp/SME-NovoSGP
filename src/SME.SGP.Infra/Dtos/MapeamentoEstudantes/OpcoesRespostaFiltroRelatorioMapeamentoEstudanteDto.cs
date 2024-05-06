using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.MapeamentoEstudantes
{
    public class OpcoesRespostaFiltroRelatorioMapeamentoEstudanteDto
    {
        public OpcoesRespostaFiltroRelatorioMapeamentoEstudanteDto()
        {
            OpcoesRespostaDistorcaoIdadeAnoASerie = new List<OpcaoRespostaSimplesDto>();
            OpcoesRespostaMigrante = new List<OpcaoRespostaSimplesDto>();
            OpcoesRespostaPossuiPlanoAEE = new List<OpcaoRespostaSimplesDto>();
            OpcoesRespostaAcompanhadoNAAPA = new List<OpcaoRespostaSimplesDto>();
            OpcoesRespostaParticipaPAP = new List<string>() { "Sim", "Não" };
            OpcoesRespostaProgramaSPIntegral = new List<OpcaoRespostaSimplesDto>();
            OpcoesRespostaHipoteseEscritaEstudante = new List<string>() { "Pré-Silábico", "Silábico sem valor", "Silábico com valor", "Silábico alfabético", "Alfabético" };
            OpcoesRespostaAvaliacoesExternasProvaSP = new List<string>() { "Abaixo do básico", "Básico", "Adequado", "Avançado" };
            OpcoesRespostaFrequencia = new List<OpcaoRespostaSimplesDto>();
        }
        public List<OpcaoRespostaSimplesDto> OpcoesRespostaDistorcaoIdadeAnoASerie { get; set; }
        public List<OpcaoRespostaSimplesDto> OpcoesRespostaMigrante { get; set; }
        public List<OpcaoRespostaSimplesDto> OpcoesRespostaPossuiPlanoAEE { get; set; }
        public List<OpcaoRespostaSimplesDto> OpcoesRespostaAcompanhadoNAAPA { get; set; }
        public List<string> OpcoesRespostaParticipaPAP { get; set; }
        public List<OpcaoRespostaSimplesDto> OpcoesRespostaProgramaSPIntegral { get; set; }
        public List<string> OpcoesRespostaHipoteseEscritaEstudante { get; set; }
        public List<string> OpcoesRespostaAvaliacoesExternasProvaSP { get; set; }
        public List<OpcaoRespostaSimplesDto> OpcoesRespostaFrequencia { get; set; }
    }
}
