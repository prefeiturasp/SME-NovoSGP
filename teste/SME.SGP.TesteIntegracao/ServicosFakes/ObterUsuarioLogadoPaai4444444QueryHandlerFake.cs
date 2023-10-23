using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFake
{
    public class ObterUsuarioLogadoPaai4444444QueryHandlerFake : IRequestHandler<ObterUsuarioLogadoQuery, Usuario>
    {
        public async Task<Usuario> Handle(ObterUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            var usuario = new Usuario()
            {
                Id = 1,
                CodigoRf = "4444444",
                Login = "4444444",
                Nome = "Usuario Teste",
                PerfilAtual = Guid.Parse(PerfilUsuario.PAAI.Name()),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            };

            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_PAAI } });

            return await Task.FromResult(usuario);
        }
    }
}