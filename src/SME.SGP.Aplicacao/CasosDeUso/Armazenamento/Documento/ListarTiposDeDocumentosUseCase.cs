using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ListarTiposDeDocumentosUseCase : AbstractUseCase, IListarTiposDeDocumentosUseCase
    {
        public ListarTiposDeDocumentosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<TipoDocumentoDto>> Executar()
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuario.EhAdmGestao())
                return await mediator.Send(ObterTipoDocumentoClassificacaoQuery.Instance);

            string[] perfis = usuario.Perfis.Where(x => x.CodigoPerfil == usuario.PerfilAtual).Select(p => p.NomePerfil).ToArray();
            string[] perfisNormalizadosComAcronimo = NormalizarPerfisComAcronimo(perfis);

            var tiposDocumentos = await mediator.Send(new ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery(perfisNormalizadosComAcronimo));
            return tiposDocumentos;
        }

        public static string[] NormalizarPerfisComAcronimo(string[] perfils)
        {
            var acronimos = new List<string>
            {
                Dominio.Enumerados.ClassificacaoDocumento.PAEE.ToString(),
                Dominio.Enumerados.ClassificacaoDocumento.PAP.ToString(),
                Dominio.Enumerados.ClassificacaoDocumento.POA.ToString(),
                Dominio.Enumerados.ClassificacaoDocumento.POED.ToString(),
                Dominio.Enumerados.ClassificacaoDocumento.POEI.ToString(),
                Dominio.Enumerados.ClassificacaoDocumento.POSL.ToString(),
                Dominio.Enumerados.ClassificacaoDocumento.PEA.ToString(),
                Dominio.Enumerados.ClassificacaoDocumento.PPP.ToString()
            };

            return perfils.Select(p =>
            {
                var primeiraPalavra = p.Split(' ').FirstOrDefault();
                return acronimos.Contains(primeiraPalavra) ? primeiraPalavra : p;
            }).ToArray();
        }
    }
}
