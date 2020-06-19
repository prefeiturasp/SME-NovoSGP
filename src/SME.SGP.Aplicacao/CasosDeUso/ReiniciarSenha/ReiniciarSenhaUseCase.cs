using MediatR;
using SME.SGP.Aplicacao.Queries.Funcionario;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReiniciarSenhaUseCase : IReiniciarSenhaUseCase
    {
        private readonly IMediator mediator;

        public ReiniciarSenhaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Executar(FiltroFuncionarioDto filtroFuncionariosDto)
        {
            return await mediator.Send(new ObterFuncionariosQuery(filtroFuncionariosDto.CodigoDRE, filtroFuncionariosDto.CodigoUE));
        }

        public async Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf)
        {
            return await mediator.Send(new ReiniciarSenhaCommand(codigoRf));
        }
    }
}
