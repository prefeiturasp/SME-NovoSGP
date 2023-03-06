using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPAEETurmaQueryHandler : IRequestHandler<ObterPAEETurmaQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IMediator mediator;

        public ObterPAEETurmaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterPAEETurmaQuery request, CancellationToken cancellationToken)
        {
            var funcionariosUe = await mediator.Send(new PesquisaFuncionariosPorDreUeQuery("", "", request.CodigoDRE, request.CodigoUE), cancellationToken);

            var atividadeFuncaoPAEE = 6;
            return funcionariosUe.Where(c => c.CodigoFuncaoAtividade == atividadeFuncaoPAEE);
        }
    }
}
