using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum SituacaoAEE
    {
        [Display(Name = "Em digitação")]
        Rascunho = 1,
        [Display(Name = "Aguardando validação da coordenação")]
        Encaminhado = 2,
        [Display(Name = "Aguardando análise do AEE")]
        Analise = 3,
        [Display(Name = "Finalizado")]
        Finalizado = 4,
        [Display(Name = "Encerrado")]
        Encerrado = 5,
        [Display(Name = "Aguardando atribuição de responsável")]
        AtribuicaoResponsavel = 6,
        [Display(Name = "Deferido")]
        Deferido = 7,
        [Display(Name = "Indeferido")]
        Indeferido = 8,
        [Display(Name = "Devolvido pela coordenação")]
        Devolvido = 9,
        [Display(Name = "Encerrado automaticamente")]
        EncerradoAutomaticamente = 10,
        [Display(Name = "Aguardando atribuição de PAAI")]
        AtribuicaoPAAI = 11,
    }

    public static class SituacaoAEEExtension
    {
        static public bool PermiteExclusaoPendenciasEncaminhamentoAEE(this SituacaoAEE situacaoAEE)
        {
            return (situacaoAEE != SituacaoAEE.Encaminhado &&
                situacaoAEE != SituacaoAEE.Analise &&
                situacaoAEE != SituacaoAEE.Rascunho);
        }
    }
}
