using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public abstract class ConsolidacaoBaseUseCase : AbstractUseCase
    {
        /// <summary>
        /// Tipos de escola válidos para consolidação no painel educacional
        /// </summary>
        private static readonly List<TipoEscola> tiposEscolasValidos =
        [
            TipoEscola.EMEF,
            TipoEscola.EMEFM,
            TipoEscola.CIEJA
        ];

        protected ConsolidacaoBaseUseCase(IMediator mediator) : base(mediator)
        {
        }

        protected static List<Ue> FiltrarUesValidasParaConsolidacao(long dreId, List<Ue> listagemUes)
        {
            return listagemUes
                .FindAll(u => u.DreId == dreId && tiposEscolasValidos.Contains(u.TipoEscola));
        }
    }
}
