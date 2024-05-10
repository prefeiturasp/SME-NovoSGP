using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorCodigosQueryHandler : IRequestHandler<ObterTurmasPorCodigosQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmasPorCodigosQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }
        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorCodigosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurmaConsulta.ObterPorCodigosAsync(request.Codigos);
        }
    }
}
