using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFimPeriodoRecorrenciaQueryHandler : IRequestHandler<ObterFimPeriodoRecorrenciaQuery, DateTime>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioCache repositorioCache;
        
        public ObterFimPeriodoRecorrenciaQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar, IRepositorioCache repositorioCache)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<DateTime> Handle(ObterFimPeriodoRecorrenciaQuery request, CancellationToken cancellationToken)
        {
            var periodos = await repositorioCache.ObterAsync(
                $"TipoCalendario-{request.TipoCalendarioId}", 
                async () => await repositorioPeriodoEscolar.ObterPorTipoCalendarioAsync(request.TipoCalendarioId));

            if (periodos == null || !periodos.Any())
                throw new NegocioException("Não foi possível obter os períodos deste tipo de calendário.");

            DateTime fimRecorrencia = DateTime.MinValue;
            switch (request.Recorrencia)
            {
                case RecorrenciaAula.RepetirBimestreAtual:
                    // Busca ultimo dia do periodo atual
                    fimRecorrencia = periodos.Where(a => a.PeriodoFim >= request.DataInicio)
                        .OrderBy(a => a.PeriodoInicio)
                        .FirstOrDefault().PeriodoFim;
                    break;
                case RecorrenciaAula.RepetirTodosBimestres:
                    // Busca ultimo dia do ultimo periodo
                    fimRecorrencia = periodos.Max(a => a.PeriodoFim);
                    break;
                default:
                    break;
            }

            return fimRecorrencia;
        }
    }
}
