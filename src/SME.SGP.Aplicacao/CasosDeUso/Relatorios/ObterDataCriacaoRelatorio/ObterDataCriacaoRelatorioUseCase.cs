using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Relatorios.ObterDataCriacaoRelatorio
{
    public class ObterDataCriacaoRelatorioUseCase : AbstractUseCase, IObterDataCriacaoRelatorioUseCase
    {
        public ObterDataCriacaoRelatorioUseCase(MediatR.IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(Guid codigoRelatorio)
        {
            bool relatorioExiste = false;
            var dataCriacaoRelatorio = await mediator.Send(new ObterDataCriacaoRelatorioPorCodigoQuery(codigoRelatorio));
            var tempoEmHorasDaCriacao = CalcularTempoEmHoras(dataCriacaoRelatorio.CriadoEm);

            if (tempoEmHorasDaCriacao < 24)
                relatorioExiste = true;

            return relatorioExiste;
        }

        private double CalcularTempoEmHoras(DateTime criadoEm)
        {
            TimeSpan tempo = DateTime.Now - criadoEm;
            return tempo.TotalHours;
        }
    }
}
