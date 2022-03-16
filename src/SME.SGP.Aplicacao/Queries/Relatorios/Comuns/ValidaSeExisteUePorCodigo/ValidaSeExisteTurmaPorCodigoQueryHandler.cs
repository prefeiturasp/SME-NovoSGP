using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ValidaSeExisteTurmaPorCodigoQueryHandler : IRequestHandler<ValidaSeExisteTurmaPorCodigoQuery, bool>
    {
        private readonly IMediator mediator;

        public ValidaSeExisteTurmaPorCodigoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ValidaSeExisteTurmaPorCodigoQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.CodigoTurma))
            {
                int codigoTurma;
                if (int.TryParse(request.CodigoTurma, out codigoTurma) && codigoTurma <= 0)
                    request.CodigoTurma = String.Empty;
                else if (await mediator.Send(new ObterTurmaPorCodigoQuery(request.CodigoTurma)) == null)
                    throw new NegocioException("Não foi possível encontrar a turma");
            }
            return true;
        }
    }
}
