using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoEUsuarioIdQueryHandler : IRequestHandler<ObterTurmasPorAnoEUsuarioIdQuery, IEnumerable<TurmaNaoHistoricaDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        public ObterTurmasPorAnoEUsuarioIdQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<TurmaNaoHistoricaDto>> Handle(ObterTurmasPorAnoEUsuarioIdQuery request, CancellationToken cancellationToken)
         => await repositorioAbrangencia.ObterTurmasPorUsuarioEAnoLetivo(request.UsuarioId, request.AnoLetivo);
    }
}
