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

            var tipoPerfil = usuario.ObterTipoPerfilAtual();

            if (tipoPerfil == Dominio.TipoPerfil.UE)
            {
                var tiposDocumentos =  await mediator.Send(new ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery(
                    usuario.Perfis.Where(x => x.CodigoPerfil == usuario.PerfilAtual).Select(p => p.NomePerfil).ToArray()
                    ));

                if (tiposDocumentos != null && tiposDocumentos.Any())
                    return tiposDocumentos;

            }

            return await mediator.Send(new ObterTipoDocumentoClassificacaoQuery());
        }
    }
}
