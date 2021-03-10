using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum SituacaoPlanoAEE
    {
        [Display(Name = "Em andamento")]
        EmAndamento = 1,
        [Display(Name = "Em andamento/Reestruturação")]
        Reestruturado = 2,
        [Display(Name = "Encerrado")]
        Encerrado = 3,
        [Display(Name = "Aguardando devolutiva coordenação")]
        DevolutivaCP = 4,
        [Display(Name = "Aguardando atribuição de PAAI")]
        AtribuicaoPAAI = 5,
        [Display(Name = "Aguardando devolutiva PAAI")]
        DevolutivaPAAI = 6,
        [Display(Name = "Encerrado Automaticamente")]
        EncerradoAutomaticamento = 7,
        [Display(Name = "Expirado")]
        Expirado = 8,
    }
}
