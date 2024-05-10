using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarExistenciaRelatorioPorCodigoUseCase : AbstractUseCase, IVerificarExistenciaRelatorioPorCodigoUseCase
    {
        public VerificarExistenciaRelatorioPorCodigoUseCase(MediatR.IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(Guid codigoRelatorio)
        {
            bool relatorioExiste = false;
            const int tempoParaExclusaoDoRelatorio = 24;
            var dataCriacaoRelatorio = await mediator.Send(new ObterDataCriacaoRelatorioPorCodigoQuery(codigoRelatorio));

            if (dataCriacaoRelatorio.NaoEhNulo())
            {
                var tempoEmHorasDaCriacao = CalcularTempoEmHoras(dataCriacaoRelatorio.CriadoEm);

                if (tempoEmHorasDaCriacao < tempoParaExclusaoDoRelatorio)
                    relatorioExiste = true;
            }

            return relatorioExiste;
        }

        private static double CalcularTempoEmHoras(DateTime criadoEm)
        {
            TimeSpan tempo = DateTime.Now - criadoEm;
            return tempo.TotalHours;
        }
    }
}
