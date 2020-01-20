using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamento : IConsultasFechamento
    {
        private readonly IServicoFechamento servicoFechamento;

        public ConsultasFechamento(IServicoFechamento servicoFechamento)
        {
            this.servicoFechamento = servicoFechamento ?? throw new System.ArgumentNullException(nameof(servicoFechamento));
        }

        public FechamentoDto ObterPorTipoCalendarioDreEUe(FiltroFechamentoDto fechamentoDto)
        {
            return servicoFechamento.ObterPorTipoCalendarioDreEUe(fechamentoDto.TipoCalendarioId, fechamentoDto.DreId, fechamentoDto.UeId);
        }
    }
}