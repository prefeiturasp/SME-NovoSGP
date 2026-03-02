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
        protected static readonly List<TipoEscola> TiposEscolasValidos = new()
        {
            TipoEscola.EMEF,
            TipoEscola.EMEFM,
            TipoEscola.CIEJA
        };

        /// <summary>
        /// Modalidade de turmas válidos para consolidação no painel educacional
        /// </summary>
        protected static readonly List<Modalidade> ModalidadesTurmas = new()
        {
            Modalidade.Fundamental,
            Modalidade.Medio,
            Modalidade.EJA
        };

        /// <summary>
        /// Modalidade de turmas válidos para consolidação no painel educacional educacao integral
        /// </summary>
        protected static readonly List<Modalidade> ModalidadesTurmasEducacaoIntegral = new()
        {
            Modalidade.EducacaoInfantil,
            Modalidade.Fundamental,
            Modalidade.Medio,
        };

        /// <summary>
        /// Modalidade de turmas válidos para consolidação no painel educacional informacoes educacionais
        /// </summary>
        protected static readonly List<Modalidade> ModalidadesTurmasInformacoesEducacionais = new()
        {
            Modalidade.Fundamental
        };

        protected static readonly HashSet<string> PareceresPromocao = new HashSet<string>
        {
            "Continuidade dos estudos",
            "Promovido",
            "Promovido pelo conselho"
        };

        protected const string ParecerRetencaoNota = "Retido";
        protected const string ParecerRetencaoFrequencia = "Retido por frequência";

        protected ConsolidacaoBaseUseCase(IMediator mediator) : base(mediator)
        {
        }

        protected static List<Ue> FiltrarUesValidasParaConsolidacao(long dreId, List<Ue> listagemUes)
        {
            return listagemUes
                .FindAll(u => u.DreId == dreId && TiposEscolasValidos.Contains(u.TipoEscola));
        }
    }
}
