using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorCodigoQueryHandler : IRequestHandler<ObterTurmaPorCodigoQuery, Turma>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaPorCodigoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<Turma> Handle(ObterTurmaPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterPorCodigo(request.TurmaCodigo);
        }
    }
}
