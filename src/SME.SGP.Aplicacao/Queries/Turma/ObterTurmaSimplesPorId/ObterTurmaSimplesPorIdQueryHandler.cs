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
    public class ObterTurmaSimplesPorIdQueryHandler : IRequestHandler<ObterTurmaSimplesPorIdQuery, ObterTurmaSimplesPorIdRetornoDto>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaSimplesPorIdQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<ObterTurmaSimplesPorIdRetornoDto> Handle(ObterTurmaSimplesPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmaSimplesPorId(request.Id);
        }
    }
}
