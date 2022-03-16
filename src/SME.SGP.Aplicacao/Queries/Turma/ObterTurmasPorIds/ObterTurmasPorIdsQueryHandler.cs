using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorIdsQueryHandler : IRequestHandler<ObterTurmasPorIdsQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmasPorIdsQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new System.ArgumentNullException(nameof(repositorioTurmaConsulta));
        }
        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorIdsQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurmaConsulta.ObterTurmasPorIds(request.TurmasIds);
        }
    }
}
