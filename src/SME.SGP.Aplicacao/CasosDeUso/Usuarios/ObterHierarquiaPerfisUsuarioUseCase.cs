using MediatR;
using SME.SGP.Aplicacao.Interfaces;
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
            var perfilUsuario = await mediator.Send(new ObterPerfilAtualQuery());
            var perfis = await mediator.Send(new ObterHierarquiaPerfisPorPerfilQuery(perfilUsuario));

            return perfis
                .OrderBy(a => a.Ordem)
                .Select(a => new KeyValuePair<Guid, string>(a.CodigoPerfil, a.NomePerfil));
        }
    }
}
