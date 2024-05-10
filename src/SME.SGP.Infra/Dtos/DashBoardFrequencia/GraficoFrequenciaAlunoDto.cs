using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class GraficoFrequenciaAlunoDto
    {
        public GraficoFrequenciaAlunoDto()
        {
            DadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>();
        }

        public string TagTotalFrequencia { get; set; }

        public string TotalFrequenciaFormatado { get; set; }        

        public IEnumerable<DadosRetornoFrequenciaAlunoDashboardDto> DadosFrequenciaDashboard { get; set; }
    }
}
