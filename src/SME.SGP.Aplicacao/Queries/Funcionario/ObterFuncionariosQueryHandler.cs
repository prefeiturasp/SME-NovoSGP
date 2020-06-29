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

        public ObterFuncionariosQueryHandler(IServicoEol servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosQuery request, CancellationToken cancellationToken)
        {
            FiltroFuncionarioDto filtro = new FiltroFuncionarioDto()
            {
                CodigoDRE = request.CodigoDre,
                CodigoUE = request.CodigoUe,
                CodigoRF = request.CodigoRf,
                NomeServidor = request.NomeServidor
            };

            return await servicoEOL.ObterFuncionariosPorDre(filtro);
        }

    }
}
