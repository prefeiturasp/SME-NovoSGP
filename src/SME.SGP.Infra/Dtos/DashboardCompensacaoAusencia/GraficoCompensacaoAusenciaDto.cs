using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class GraficoCompensacaoAusenciaDto
    {
        public GraficoCompensacaoAusenciaDto()
        {
            DadosCompensacaoAusenciaDashboard = new List<DadosRetornoAusenciasCompensadasDashboardDto>();
        }

        public long QuantidadeAusenciasRegistrada { get; set; }
        public double PorcentagemAulas { get; set; }
        public string TotalAusenciasFormatado
        {
            get => $"{QuantidadeAusenciasRegistrada} ausências compensadas ({PorcentagemAulas}% das aulas)";
        }

        public IEnumerable<DadosRetornoAusenciasCompensadasDashboardDto> DadosCompensacaoAusenciaDashboard { get; set; }
    }
}
