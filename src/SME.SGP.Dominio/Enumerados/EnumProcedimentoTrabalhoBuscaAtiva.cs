using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum EnumProcedimentoTrabalhoBuscaAtiva
    {
        [Display(Name = "Ligação telefonica")]
        LigacaoTelefonica = 1,
        [Display(Name = "Visita Domiciliar")]
        VisitaDomiciliar = 2
    }
}
