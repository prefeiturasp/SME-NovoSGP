using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReiniciarSenhaCommandHandler : IRequestHandler<ReiniciarSenhaCommand, UsuarioReinicioSenhaDto>
    {
        public readonly IComandosUsuario comandosUsuario;
        private readonly IServicoEol servicoEOL;

        public ReiniciarSenhaCommandHandler(IServicoFila servicoFila)
        {
            this.comandosUsuario = comandosUsuario ?? throw new System.ArgumentNullException(nameof(comandosUsuario));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<UsuarioReinicioSenhaDto> Handle(ReiniciarSenhaCommand request, CancellationToken cancellationToken)
        {
            var usuario = await servicoEOL.ObterMeusDados(request.CodigoRf);

            var retorno = new UsuarioReinicioSenhaDto();

            if (String.IsNullOrEmpty(usuario.Email))
                retorno.DeveAtualizarEmail = true;
            else
            {
                await servicoEOL.ReiniciarSenha(request.CodigoRf);
                retorno.Mensagem = $"Senha do usuário {request.CodigoRf} - {usuario.Nome} reiniciada com sucesso.";
                retorno.DeveAtualizarEmail = false;
            }

            return retorno;
        }
    }
}
