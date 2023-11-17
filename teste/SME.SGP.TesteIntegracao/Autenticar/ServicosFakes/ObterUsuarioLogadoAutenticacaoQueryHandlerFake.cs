using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Autenticar 
{ 
    public class ObterUsuarioLogadoAutenticacaoQueryHandlerFake : IRequestHandler<ObterUsuarioLogadoQuery, Usuario>
    {
        public async Task<Usuario> Handle(ObterUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            var usuario = new Usuario()
            {
                Id = 1,
                CodigoRf = "PROFINF1",
                Login = "PROFINF1",
                Nome = "Usuario Teste",
                PerfilAtual = Perfis.PERFIL_PROFESSOR_INFANTIL,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            };

            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_PROFESSOR_INFANTIL} });

            return await Task.FromResult(usuario);
        }
    }
}
