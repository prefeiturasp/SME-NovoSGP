using MediatR;
using SME.SGP.Aplicacao.Queries.Funcionario;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosUseCase : IObterFuncionariosUseCase
    {
        private readonly IMediator mediator;

        public ObterFuncionariosUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Executar(FiltroFuncionarioDto filtroFuncionariosDto)
        {
            return await mediator.Send(new ObterFuncionariosQuery()
            {
                CodigoDre = filtroFuncionariosDto.CodigoDRE,
                CodigoUe = filtroFuncionariosDto.CodigoUE,
                CodigoRf = filtroFuncionariosDto.CodigoRF,
                NomeServidor = filtroFuncionariosDto.NomeServidor
            });
        }
    }
}