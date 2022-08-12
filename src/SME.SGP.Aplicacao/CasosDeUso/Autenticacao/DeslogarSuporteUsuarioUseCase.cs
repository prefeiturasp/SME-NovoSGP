﻿using MediatR;
using SME.SGP.Aplicacao.Commands.Autenticacao.DeslogarSuporteUsuario;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DeslogarSuporteUsuarioUseCase : AbstractUseCase, IDeslogarSuporteUsuarioUseCase
    {
        public DeslogarSuporteUsuarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<UsuarioAutenticacaoRetornoDto> Executar()
        {
            var administrador = await mediator.Send(new ObterAdministradorDoSuporteQuery());

            if (administrador == null || string.IsNullOrEmpty(administrador.Login))
            {
                throw new NegocioException($"O usuário não está em suporte de um administrador!");
            }

            return await mediator.Send(new DeslogarSuporteUsuarioCommand(administrador));
        }
    }
}
