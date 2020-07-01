using MediatR;
using SME.SGP.Aplicacao.Integracoes;
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
        private readonly IServicoEol servicoEOL;
        private readonly IMediator mediator;

        public ObterFuncionariosQueryHandler(IServicoEol servicoEOL, IMediator mediator)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosQuery request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            FiltroFuncionarioDto filtro = new FiltroFuncionarioDto()
            {
                CodigoDRE = request.CodigoDre,
                CodigoUE = request.CodigoUe,
                CodigoRF = request.CodigoRf,
                NomeServidor = request.NomeServidor
            };

            return await servicoEOL.ObterFuncionariosPorDre(usuario.PerfilAtual, filtro);
        }
    }
}
