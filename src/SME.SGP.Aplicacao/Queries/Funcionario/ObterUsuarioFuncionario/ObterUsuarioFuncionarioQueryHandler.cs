using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioFuncionarioQueryHandler : IRequestHandler<ObterUsuarioFuncionarioQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IServicoEol servicoEOL;
        private readonly IMediator mediator;

        public ObterUsuarioFuncionarioQueryHandler(IServicoEol servicoEOL, IMediator mediator)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterUsuarioFuncionarioQuery request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            FiltroFuncionarioDto filtro = new FiltroFuncionarioDto()
            {
                CodigoDRE = request.CodigoDre,
                CodigoUE = request.CodigoUe,
                CodigoRF = request.CodigoRf,
                NomeServidor = request.NomeServidor
            };

            return await servicoEOL.ObterUsuarioFuncionario(usuario.PerfilAtual, filtro);
        }
    }
}
