using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnoLetivoTurmasVigentesQueryHandler : IRequestHandler<ObterAnoLetivoTurmasVigentesQuery, IEnumerable<int>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterAnoLetivoTurmasVigentesQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<IEnumerable<int>> Handle(ObterAnoLetivoTurmasVigentesQuery request, CancellationToken cancellationToken)
         => await repositorioTurma.BuscarAnosLetivosComTurmasVigentes(request.UeCodigo);
    }
}
