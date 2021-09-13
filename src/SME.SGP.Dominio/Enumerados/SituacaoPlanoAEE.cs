using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum SituacaoPlanoAEE
    {
        [Display(Name = "Validado")]
        Validado = 1,        
        [Display(Name = "Encerrado")]
        Encerrado = 3,
        [Display(Name = "Aguardando parecer da coordenação")]
        ParecerCP = 4,
        [Display(Name = "Aguardando atribuição de PAAI")]
        AtribuicaoPAAI = 5,
        [Display(Name = "Aguardando parecer do CEFAI")]
        ParecerPAAI = 6,
        [Display(Name = "Encerrado Automaticamente")]
        EncerradoAutomaticamento = 7,
        [Display(Name = "Expirado")]
        Expirado = 8,
        [Display(Name = "Devolvido")]
        Devolvido = 9,
    }
}
