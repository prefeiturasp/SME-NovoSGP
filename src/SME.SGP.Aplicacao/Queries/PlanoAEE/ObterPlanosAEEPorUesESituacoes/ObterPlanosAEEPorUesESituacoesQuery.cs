using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEPorUesESituacoesQuery : IRequest<IEnumerable<PlanoAEE>>
    {
        public ObterPlanosAEEPorUesESituacoesQuery(string[] uesCodigos, SituacaoPlanoAEE[] situacoes, string responsavelPaaiRf = null)
        {
            UesCodigos = uesCodigos;
            Situacoes = situacoes;
            ResponsavelPaaiRf = responsavelPaaiRf;
        }

        public string[] UesCodigos { get; }
        public SituacaoPlanoAEE[] Situacoes { get; }
        public string ResponsavelPaaiRf { get; }
    }
}
