using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoQuestionario
    {
        EncaminhamentoAEE = 1,
        PlanoAEE = 2,
        RegistroItinerancia = 3,
        RegistroItineranciaAluno = 4,
        EncaminhamentoNAAPA = 5,
        RelatorioPAP = 6,
        RelatorioDinamicoEncaminhamentoNAAPA = 7,
        RegistroAcaoBuscaAtiva = 8,
        MapeamentoEstudante = 9,
        [Display(Name="Individual")]
        EncaminhamentoNAAPAIndividual = 11,
        [Display(Name= "Institucional")]
        EncaminhamentoNAAPAInstitucional = 12
    }
}