using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake
{
    public class ObterUsuarioLogadoPaee5555555QueryHandlerFake : IRequestHandler<ObterUsuarioLogadoQuery, Usuario>
    {
        public async Task<Usuario> Handle(ObterUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new Usuario()
            {
                Id = 1,
                CodigoRf = "5555555",
                Login = "5555555",
                Nome = "Usuario Teste",
                PerfilAtual = Guid.Parse(PerfilUsuario.PAEE.Name()),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });
        }
    }
}
