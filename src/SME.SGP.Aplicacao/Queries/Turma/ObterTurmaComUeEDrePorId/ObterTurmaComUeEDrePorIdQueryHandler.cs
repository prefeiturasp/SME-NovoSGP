using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaComUeEDrePorIdQueryHandler : IRequestHandler<ObterTurmaComUeEDrePorIdQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmaComUeEDrePorIdQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<Turma> Handle(ObterTurmaComUeEDrePorIdQuery request, CancellationToken cancellationToken)
            => await repositorioTurmaConsulta.ObterTurmaComUeEDrePorId(request.TurmaId);
    }
}
