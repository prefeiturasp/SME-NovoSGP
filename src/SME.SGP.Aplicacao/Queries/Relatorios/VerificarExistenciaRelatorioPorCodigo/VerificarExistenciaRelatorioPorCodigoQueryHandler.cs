using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarExistenciaRelatorioPorCodigoQueryHandler : IRequestHandler<VerificarExistenciaRelatorioPorCodigoQuery, bool>
    {
        private readonly IRepositorioCorrelacaoRelatorio _repositorioCorrelacaoRelatorio;

        public VerificarExistenciaRelatorioPorCodigoQueryHandler(IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio)
        {
            _repositorioCorrelacaoRelatorio = repositorioCorrelacaoRelatorio ?? throw new ArgumentNullException(nameof(repositorioCorrelacaoRelatorio));
        }

        public async Task<bool> Handle(VerificarExistenciaRelatorioPorCodigoQuery request, CancellationToken cancellationToken)
        {
            const int tempoParaExclusaoDoRelatorio = 24;

            bool relatorioExiste = false;
            var dataCriacaoRelatorio = await _repositorioCorrelacaoRelatorio.ObterDataCriacaoRelatorio(request.CodigoRelatorio);

            if (dataCriacaoRelatorio.NaoEhNulo())
            {
                var tempoEmHorasDaCriacao = CalcularTempoEmHoras(dataCriacaoRelatorio.CriadoEm);

                if (tempoEmHorasDaCriacao < tempoParaExclusaoDoRelatorio)
                    relatorioExiste = true;
            }

            return await Task.FromResult(relatorioExiste);
        }

        private static double CalcularTempoEmHoras(DateTime criadoEm)
        {
            TimeSpan tempo = DateTime.Now - criadoEm;
            return tempo.TotalHours;
        }
    }
}
