using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorCodigosQueryHandler : IRequestHandler<ObterTurmasPorCodigosQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasPorCodigosQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorCodigosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterPorCodigosAsync(request.Codigos);
        }
    }
}
