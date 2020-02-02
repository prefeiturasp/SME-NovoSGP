using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamento : IConsultasFechamento
    {
        private readonly IServicoFechamento servicoFechamento;

        public ConsultasFechamento(IServicoFechamento servicoFechamento)
        {
            this.servicoFechamento = servicoFechamento ?? throw new System.ArgumentNullException(nameof(servicoFechamento));
        }

        public async Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(FiltroFechamentoDto fechamentoDto)
        {
            return await servicoFechamento.ObterPorTipoCalendarioDreEUe(fechamentoDto.TipoCalendarioId, fechamentoDto.DreId, fechamentoDto.UeId);
        }
    }
}