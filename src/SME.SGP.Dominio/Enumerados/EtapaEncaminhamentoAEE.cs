using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum EtapaEncaminhamentoAEE
    {
        [Display(Name = "Primeira Etapa")]
        PrimeiraEtapa = 1,
        [Display(Name = "Segunda Etapa")]
        SegundaEtapa = 2,
        [Display(Name = "Terceira Etapa")]
        TerceiraEtapa = 3
    }
}
