using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Autenticacao.DeslogarSuporteUsuario
{
    internal class DeslogarSuporteUsuarioCommandHandler : IRequestHandler<DeslogarSuporteUsuarioCommand, UsuarioAutenticacaoRetornoDto>
    {
        private readonly IServicoAutenticacao servicoAutenticacao;
        private readonly IComandosUsuario comandoUsuario;

        public DeslogarSuporteUsuarioCommandHandler(IServicoAutenticacao servicoAutenticacao,
                                                    IComandosUsuario comandoUsuario)
        {
            this.servicoAutenticacao = servicoAutenticacao ?? throw new ArgumentNullException(nameof(servicoAutenticacao));
            this.comandoUsuario = comandoUsuario ?? throw new ArgumentNullException(nameof(comandoUsuario));
        }

        public async Task<UsuarioAutenticacaoRetornoDto> Handle(DeslogarSuporteUsuarioCommand request, CancellationToken cancellationToken)
        {
            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEolSemSenha(request.Administrador.Login);

            return await comandoUsuario.ObterAutenticacao(retornoAutenticacaoEol, request.Administrador.Login);
        }
    }
}
