using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQueryHandler : IRequestHandler<ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQuery, IEnumerable<TurmaEComponenteDto>>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQueryHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }

        public async Task<IEnumerable<TurmaEComponenteDto>> Handle(ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQuery request, CancellationToken cancellationToken)
            => await repositorioAtividadeAvaliativa.ObterTurmaEComponenteSemAvaliacaoNoPeriodo(request.TipoCalendarioId, request.DataInicio, request.DataFim);
    }
}
