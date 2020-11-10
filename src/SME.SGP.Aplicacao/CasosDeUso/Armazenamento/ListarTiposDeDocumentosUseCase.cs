using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarTiposDeDocumentosUseCase : AbstractUseCase, IListarTipoDeDocumentosUseCase
    {
        public ListarTiposDeDocumentosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<TipoDocumentoDto>> Executar()
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            string[] perfis = usuario.Perfis.Select(p => p.NomePerfil).ToArray();

            return await mediator.Send(new ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery(perfis));
        }
    }
}
