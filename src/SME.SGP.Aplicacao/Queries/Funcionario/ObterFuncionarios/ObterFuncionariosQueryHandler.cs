using MediatR;
using SME.SGP.Aplicacao.Queries.Funcionario;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Github.ObterVersaoRelease
{
    public class ObterFuncionariosQueryHandler : IRequestHandler<ObterFuncionariosQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IMediator mediator;

        public ObterFuncionariosQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosQuery request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance, cancellationToken);

            FiltroFuncionarioDto filtro = new()
            {
                CodigoDRE = request.CodigoDre,
                CodigoUE = request.CodigoUe,
                CodigoRF = request.CodigoRf,
                NomeServidor = request.NomeServidor
            };

            return await mediator.Send(new ObterFuncionariosPorDreEolQuery(usuario.PerfilAtual, filtro.CodigoDRE, filtro.CodigoUE,
                filtro.CodigoRF, filtro.NomeServidor), cancellationToken);
        }
    }
}
