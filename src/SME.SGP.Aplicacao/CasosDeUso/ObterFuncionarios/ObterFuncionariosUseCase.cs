using MediatR;
using SME.SGP.Aplicacao.Queries.Funcionario;
using SME.SGP.Aplicacao.Queries.Usuario.ObterUsuariosDoConectaPorCodigoUe;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var funcionarios = (await mediator.Send(new ObterFuncionariosQuery()
            {
                CodigoDre = filtroFuncionariosDto.CodigoDRE,
                CodigoUe = filtroFuncionariosDto.CodigoUE,
                CodigoRf = filtroFuncionariosDto.CodigoRF,
                NomeServidor = filtroFuncionariosDto.NomeServidor
            })).ToList();

            var acessosABAE = await mediator.Send(new ObterCadastroAcessoABAEPorDreQuery(
                                                                                filtroFuncionariosDto.CodigoRF,
                                                                                filtroFuncionariosDto.CodigoDRE,
                                                                                filtroFuncionariosDto.CodigoUE,
                                                                                filtroFuncionariosDto.NomeServidor));
            foreach (var acesso in acessosABAE)
                funcionarios.Add(new UsuarioEolRetornoDto()
                {
                    CodigoRf = acesso.Cpf.SomenteNumeros(),
                    NomeServidor = acesso.Nome
                });

            if (string.IsNullOrEmpty(filtroFuncionariosDto.CodigoUE))
                return funcionarios;

            var usuarioConecta = await mediator.Send(new ObterUsuariosDoConectaPorCodigoUeQuery(
                                                                                filtroFuncionariosDto.CodigoUE,
                                                                                filtroFuncionariosDto.CodigoRF,
                                                                                filtroFuncionariosDto.NomeServidor));

            funcionarios.AddRange(usuarioConecta.Select(u => new UsuarioEolRetornoDto()
            {
                CodigoRf = u.Login.SomenteNumeros(),
                NomeServidor = u.Nome
            }));

            return funcionarios;
        }
    }
}