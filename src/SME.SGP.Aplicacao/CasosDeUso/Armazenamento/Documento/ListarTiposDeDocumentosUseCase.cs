using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarTiposDeDocumentosUseCase : AbstractUseCase, IListarTiposDeDocumentosUseCase
    {
        public ListarTiposDeDocumentosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<TipoDocumentoDto>> Executar()
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuario.EhGestorEscolar())
                return await mediator.Send(new ObterTipoDocumentoClassificacaoQuery());

            var perfil = usuario.Perfis.Where(x => x.CodigoPerfil == usuario.PerfilAtual).Select(p => p.NomePerfil).ToArray();
            
            var tiposDocumentos =  await mediator.Send(new ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery(perfil));

            return tiposDocumentos;
        }
    }
}
