using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class GraficoFrequenciaAlunoDto
    {
        public GraficoFrequenciaAlunoDto()
        {
            DadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>();
        }

        public long QuantidadeFrequenciaRegistrada { get; set; }
        public double PorcentagemAulas { get; set; }
        public string TotalFrequenciaFormatado 
        {
            get => $"{QuantidadeFrequenciaRegistrada} frequência registrada ({PorcentagemAulas}% das aulas)";
        }

        public IEnumerable<DadosRetornoFrequenciaAlunoDashboardDto> DadosFrequenciaDashboard { get; set; }
    }
}
