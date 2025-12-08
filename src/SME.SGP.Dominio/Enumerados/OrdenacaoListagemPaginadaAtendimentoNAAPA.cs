using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum OrdenacaoListagemPaginadaAtendimentoNAAPA
    {

        [Display(Name = "Unidade Escolar (UE)")]
        UE = 1,
        
        [Display(Name = "Criança/Estudante")]
        Estudante = 2,

        [Display(Name = "Data da entrada da queixa")]
        DataEntradaQueixa = 3,

        [Display(Name = "Unidade Escolar (UE) decrescente")]
        UEDesc = -1,

        [Display(Name = "Criança/Estudante decrescente")]
        EstudanteDesc = -2,
        
        [Display(Name = "Data da entrada da queixa decrescente")]
        DataEntradaQueixaDesc = -3,
    }
}