using System.ComponentModel;

namespace SME.SGP.Dominio
{
    public enum TipoArquivo
    {
        [Description("documentos")]
        Geral = 1,

        [Description("temp")] 
        Temp = 2,

        [Description("encaminhamento/aee")]
        EncaminhamentoAEE = 3, //não localizado

        [Description("foto/aluno")] //não encontrei
        FotoAluno = 4,

        [Description("aluno/frequencia")]
        FrequenciaAnotacaoEstudante = 5,   

        [Description("planejamento/anual/territorio_saber")]
        TerritorioSaber = 6,

        [Description("conselho_classe")]
        ConselhoClasse = 7,

        [Description("aluno/ocorrencia")]
        Ocorrencia = 8,

        [Description("semestral_pap")]
        RelatorioSemestralPAP = 9,

        [Description("planejamento/aula/descricao")]
        PlanoAula = 10,

        [Description("planejamento/aula/desenvolvimento")]
        PlanoAulaDesenvolvimento = 11, //dentro do Plano Aula

        [Description("planejamento/aula/recuperacao")]
        PlanoAulaRecuperacao = 12, //dentro do Plano Aula

        [Description("planejamento/aula/licao_casa")]
        PlanoAulaLicaoCasa = 13, //dentro do Plano Aula

        [Description("fechamento/aluno/anotacao")]
        FechamentoAnotacao = 14,

        [Description("acompanhamento/aluno")]
        AcompanhamentoAluno = 15,

        [Description("diario/bordo")]
        DiarioBordo = 16,

        [Description("devolutiva")]
        Devolutiva = 17,

        [Description("compensacao/ausencia")]
        CompensacaoAusencia = 18,

        [Description("registro/individual")]
        RegistroIndividual = 19,

        [Description("plano/ciclo")]
        PlanoCiclo = 20,

        [Description("planejamento/anual")] 
        PlanejamentoAnual = 21,

        [Description("carta/intencoes")]
        CartaIntencoes = 22,

        [Description("registro/poa")]
        RegistroPOA = 23,        
    }
}