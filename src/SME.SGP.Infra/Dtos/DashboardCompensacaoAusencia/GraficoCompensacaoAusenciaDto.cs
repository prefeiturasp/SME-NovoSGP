using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class GraficoCompensacaoAusenciaDto
    {
        public GraficoCompensacaoAusenciaDto()
        {
            DadosCompensacaoAusenciaDashboard = new List<DadosRetornoAusenciasCompensadasDashboardDto>();
        }
        public string TagTotalCompensacaoAusencia { get; set; }
        public IEnumerable<DadosRetornoAusenciasCompensadasDashboardDto> DadosCompensacaoAusenciaDashboard { get; set; }
    }
}
