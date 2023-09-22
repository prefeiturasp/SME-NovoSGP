using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReiniciarSenhaCommandHandler : IRequestHandler<ReiniciarSenhaCommand, UsuarioReinicioSenhaDto>
    {
        private readonly IMediator mediator;

        public ReiniciarSenhaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<UsuarioReinicioSenhaDto> Handle(ReiniciarSenhaCommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioCoreSSOQuery(request.CodigoRf));

            var retorno = new UsuarioReinicioSenhaDto();

            if (usuario.NaoEhNulo() && String.IsNullOrEmpty(usuario.Email))
            {
                await mediator.Send(new ReiniciarSenhaEolCommand(request.CodigoRf));
                retorno.DeveAtualizarEmail = true;
                retorno.Mensagem = $"Usuário {request.CodigoRf} - {usuario.Nome} não possui email cadastrado!";
            }
            else
            {
                await mediator.Send(new ReiniciarSenhaEolCommand(request.CodigoRf));

                await GravarHistoricoReinicioSenha(request.CodigoRf, request.DreCodigo, request.UeCodigo);

                retorno.Mensagem = $"Senha do usuário {request.CodigoRf}{(usuario.NaoEhNulo() ? string.Concat(" - ", usuario.Nome) : string.Empty)} reiniciada com sucesso. O usuário deverá informar a senha {FormatarSenha(request.CodigoRf)} no seu próximo acesso.";
                retorno.DeveAtualizarEmail = false;
            }

            return retorno;
        }

        private async Task GravarHistoricoReinicioSenha(string usuarioRf, string dreCodigo, string ueCodigo)
        {
            await mediator.Send(new GravarHistoricoReinicioSenhaCommand(usuarioRf, dreCodigo, ueCodigo));
        }

        private string FormatarSenha(string codigoRf) 
        {
            string sufixoSenha = codigoRf.Substring(codigoRf.Length - 4, 4);
            return $"Sgp{sufixoSenha}";
        }
    }
}
