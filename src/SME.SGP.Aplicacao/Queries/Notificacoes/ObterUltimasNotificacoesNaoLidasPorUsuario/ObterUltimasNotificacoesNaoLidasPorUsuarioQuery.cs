using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimasNotificacoesNaoLidasPorUsuarioQuery : IRequest<IEnumerable<NotificacaoBasicaDto>>
    {
        public ObterUltimasNotificacoesNaoLidasPorUsuarioQuery(int anoLetivo, string codigoRf)
        {
            AnoLetivo = anoLetivo;
            CodigoRf = codigoRf;
        }

        public int AnoLetivo { get; set; }
        public string CodigoRf { get; set; }
    }
}
