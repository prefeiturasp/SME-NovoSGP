using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterSolicitacaoRelatorioQuery : IRequest<IEnumerable<SolicitacaoRelatorio>>
    {
        public ObterSolicitacaoRelatorioQuery(TipoRelatorio tipoRelatorio, TipoFormatoRelatorio extensaoRelatorio, string usuarioQueSolicitou)
        {
            TipoRelatorio = tipoRelatorio;
            ExtensaoRelatorio = extensaoRelatorio;
            UsuarioQueSolicitou = usuarioQueSolicitou;
        }

        public TipoRelatorio TipoRelatorio { get; set; }
        public TipoFormatoRelatorio ExtensaoRelatorio { get; set; }
        public string UsuarioQueSolicitou { get; set; }
    }
}
