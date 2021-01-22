using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHierarquiaPerfisUsuarioUseCase : IObterHierarquiaPerfisUsuarioUseCase
    {
        private readonly IMediator mediator;

        public ObterHierarquiaPerfisUsuarioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<IEnumerable<KeyValuePair<Guid, string>>> Executar()
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var perfilUsuario = usuarioLogado.PerfilAtual;
            if (usuarioLogado.EhPerfilProfessor())
            {
                var perfil = await mediator.Send(new ObterPerfilPorGuidQuery(perfilUsuario));
                if (perfil == null)
                    throw new NegocioException("Perfil do usuário não localizado na base de dados do SGP");

                return new[] { new KeyValuePair<Guid, string>( perfil.CodigoPerfil, perfil.NomePerfil) };
            }

            var perfis = await mediator.Send(new ObterHierarquiaPerfisPorPerfilQuery(perfilUsuario));

            return perfis
                .OrderBy(a => a.Ordem)
                .Select(a => new KeyValuePair<Guid, string>(a.CodigoPerfil, a.NomePerfil));
        }
    }
}
