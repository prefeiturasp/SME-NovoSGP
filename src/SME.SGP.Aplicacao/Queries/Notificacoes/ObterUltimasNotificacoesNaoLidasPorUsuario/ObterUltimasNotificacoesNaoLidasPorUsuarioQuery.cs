using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimasNotificacoesNaoLidasPorUsuarioQuery : IRequest<IEnumerable<NotificacaoBasicaDto>>
    {
        public ObterUltimasNotificacoesNaoLidasPorUsuarioQuery(int anoLetivo, string codigoRf, bool tituloReduzido = false)
        {
            AnoLetivo = anoLetivo;
            CodigoRf = codigoRf;
            TituloReduzido = tituloReduzido;
        }

        public int AnoLetivo { get; set; }
        public string CodigoRf { get; set; }
        public bool TituloReduzido { get; set; }
    }
}
