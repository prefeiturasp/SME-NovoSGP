using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TurmaPossuiAvaliacaoNoPeriodoQueryHandler : IRequestHandler<TurmaPossuiAvaliacaoNoPeriodoQuery, bool>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public TurmaPossuiAvaliacaoNoPeriodoQueryHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }

        public Task<bool> Handle(TurmaPossuiAvaliacaoNoPeriodoQuery request, CancellationToken cancellationToken)
           => repositorioAtividadeAvaliativa.TurmaPossuiAvaliacaoNoPeriodo(request.TurmaId, request.PeriodoEscolarId, request.ComponenteCurricularCodigo);
    }
}
