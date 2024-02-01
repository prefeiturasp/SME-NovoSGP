using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake
{
    public class ObterUsuarioLogadoProfessor2222222QueryHandlerFake: IRequestHandler<ObterUsuarioLogadoQuery, Usuario>
    {
        public Task<Usuario> Handle(ObterUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Usuario()
            {
                Id = 1,
                CodigoRf = "2222222",
                Login = "2222222",
                Nome = "Usuario Teste",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });
        }
    }
}