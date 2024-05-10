using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum TipoEtapaEncaminhamento
    {
        [Display(Name = "Informações Escolares e Descrição do Encaminhamento")]
        InformacoesEscolares_DescricaoEncaminhamento = 1,
        
        [Display(Name = "Parecer Coordenação")]
        ParecerCoordenacao = 2,
        
        [Display(Name = "Parecer AEE")]
        ParecerAEE = 3,
    }
}
