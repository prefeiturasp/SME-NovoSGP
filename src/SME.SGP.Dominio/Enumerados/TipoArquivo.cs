using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoArquivo
    {
        [Display(Name ="documentos")]
        Geral = 1,

        [Display(Name ="temp")] 
        Editor = 2,

        [Display(Name ="encaminhamento/aee")]
        EncaminhamentoAEE = 3,

        [Display(Name ="foto/aluno")]
        FotoAluno = 4,

        [Display(Name ="aluno/frequencia")]
        FrequenciaAnotacaoEstudante = 5,   

        [Display(Name ="planejamento/anual/territorio_saber")]
        TerritorioSaber = 6,

        [Display(Name = "conselho_classe")]
        ConselhoClasse = 7,

        [Display(Name ="aluno/ocorrencia")]
        Ocorrencia = 8,

        [Display(Name ="semestral_pap")]
        RelatorioSemestralPAP = 9,

        [Display(Name ="planejamento/aula/descricao")]
        PlanoAula = 10,

        [Display(Name ="planejamento/aula/desenvolvimento")]
        PlanoAulaDesenvolvimento = 11, 

        [Display(Name ="planejamento/aula/recuperacao")]
        PlanoAulaRecuperacao = 12,

        [Display(Name ="planejamento/aula/licao_casa")]
        PlanoAulaLicaoCasa = 13,

        [Display(Name ="fechamento/aluno/anotacao")]
        FechamentoAnotacao = 14,

        [Display(Name ="acompanhamento/aluno")]
        AcompanhamentoAluno = 15,

        [Display(Name ="diario/bordo")]
        DiarioBordo = 16,

        [Display(Name ="devolutiva")]
        Devolutiva = 17,

        [Display(Name ="compensacao/ausencia")]
        CompensacaoAusencia = 18,

        [Display(Name ="registro/individual")]
        RegistroIndividual = 19,

        [Display(Name ="plano/ciclo")]
        PlanoCiclo = 20,

        [Display(Name ="planejamento/anual")] 
        PlanejamentoAnual = 21,

        [Display(Name ="carta/intencoes")]
        CartaIntencoes = 22,

        [Display(Name ="registro/poa")]
        RegistroPOA = 23,
        
        [Display(Name = "itinerancia")]
        Itinerancia = 24,

        [Display(Name = "atendimento/naapa")]
        AtendimentoNAAPA = 25,

        [Display(Name = "relatorio/pap")]
        RelatorioPAP = 26,

        [Display(Name = "registro-coletivo")]
        RegistroColetivo = 27,

        [Display(Name = "atendimento/naapa/itinerancia")]
        ItineranciaAtendimentoNAAPA = 28,

        [Display(Name = "informativo")]
        Informativo = 29,

        [Display(Name = "importacao")]
        Importacao = 30,

        [Display(Name = "temp")]
        temp = 99
    }
}