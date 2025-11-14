using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioFuncionarioUseCase : IObterUsuarioFuncionarioUseCase
    {
        private readonly IMediator mediator;

        public ObterUsuarioFuncionarioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Executar(FiltroFuncionarioDto filtroFuncionariosDto)
        {
            var usuarios = (await mediator.Send(new ObterUsuarioFuncionarioQuery(filtroFuncionariosDto))).ToList();

            var acessosABAE = await mediator.Send(new ObterCadastroAcessoABAEPorDreQuery(
                                                                                filtroFuncionariosDto.CodigoRF,
                                                                                filtroFuncionariosDto.CodigoDRE,
                                                                                filtroFuncionariosDto.CodigoUE,
                                                                                filtroFuncionariosDto.NomeServidor));
            foreach (var acesso in acessosABAE)
                usuarios.Add(new UsuarioEolRetornoDto()
                {
                    CodigoRf = acesso.Cpf.SomenteNumeros(),
                    NomeServidor = acesso.Nome
                });

            return usuarios;
        }
    }
}