using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasDreUePorCodigosQueryHandler : IRequestHandler<ObterTurmasDreUePorCodigosQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmasDreUePorCodigosQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }
        public async Task<IEnumerable<Turma>> Handle(ObterTurmasDreUePorCodigosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurmaConsulta.ObterTurmasDreUeCompletaPorCodigos(request.Codigos);
        }
    }
}
