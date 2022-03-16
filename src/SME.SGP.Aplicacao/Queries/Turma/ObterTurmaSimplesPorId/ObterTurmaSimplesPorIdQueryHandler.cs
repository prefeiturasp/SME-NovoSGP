using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaSimplesPorIdQueryHandler : IRequestHandler<ObterTurmaSimplesPorIdQuery, ObterTurmaSimplesPorIdRetornoDto>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmaSimplesPorIdQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }
        public async Task<ObterTurmaSimplesPorIdRetornoDto> Handle(ObterTurmaSimplesPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurmaConsulta.ObterTurmaSimplesPorId(request.Id);
        }
    }
}
