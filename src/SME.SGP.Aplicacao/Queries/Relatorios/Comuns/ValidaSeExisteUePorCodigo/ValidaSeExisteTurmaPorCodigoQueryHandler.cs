using System;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Queries
{
    public class ValidaSeExisteTurmaPorCodigoQueryHandler : IRequestHandler<ValidaSeExisteTurmaPorCodigoQuery, bool>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ValidaSeExisteTurmaPorCodigoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<bool> Handle(ValidaSeExisteTurmaPorCodigoQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.CodigoTurma))
            {
                int codigoTurma;
                if (int.TryParse(request.CodigoTurma, out codigoTurma) && codigoTurma <= 0)
                    request.CodigoTurma = String.Empty;
                else if (await repositorioTurma.ObterPorCodigo(request.CodigoTurma) == null)
                    throw new NegocioException("Não foi possível encontrar a turma");
            }
            return true;
        }
    }
}
