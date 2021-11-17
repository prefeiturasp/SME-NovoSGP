using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQueryHandler : IRequestHandler<ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQuery, IEnumerable<AvaliacoesPorTurmaComponenteDto>>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorio;

        public ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQueryHandler(IRepositorioAtividadeAvaliativa repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<AvaliacoesPorTurmaComponenteDto>> Handle(ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterAvaliacoesTurmaComponentePorUeNoPeriodo(request.UeId, request.DataInicio, request.DataFim);
    }
}
