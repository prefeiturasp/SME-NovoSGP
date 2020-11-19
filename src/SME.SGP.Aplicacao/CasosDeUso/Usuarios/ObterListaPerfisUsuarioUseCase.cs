using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaPerfisUsuarioUseCase : IObterListaPerfisUsuarioUseCase
    {
        private readonly IMediator mediator;

        public ObterListaPerfisUsuarioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<IEnumerable<KeyValuePair<Guid, string>>> Executar()
        {
            var perfis = await mediator.Send(new ObterListaPerfisUsuariosQuery());

            return perfis
                .OrderBy(a => a.Ordem)
                .Select(a => new KeyValuePair<Guid, string>(a.CodigoPerfil, a.NomePerfil));
        }
    }
}
