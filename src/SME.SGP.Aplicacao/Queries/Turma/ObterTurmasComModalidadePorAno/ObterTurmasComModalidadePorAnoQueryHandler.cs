using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComModalidadePorAnoQueryHandler : IRequestHandler<ObterTurmasComModalidadePorAnoQuery, IEnumerable<TurmaModalidadeDto>>
    {
        private readonly IRepositorioTurma repositorio;

        public ObterTurmasComModalidadePorAnoQueryHandler(IRepositorioTurma repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<TurmaModalidadeDto>> Handle(ObterTurmasComModalidadePorAnoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterTurmasComModalidadePorAno(request.Ano);
    }
}
