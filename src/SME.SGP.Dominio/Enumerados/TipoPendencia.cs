using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoPendencia
    {
        [Display(Name = "Avaliação sem notas / conceitos lançados")]
        AvaliacaoSemNotaParaNenhumAluno = 1,

        [Display(Name = "Aula de reposição pendente de aprovação")]
        AulasReposicaoPendenteAprovacao = 2,

        [Display(Name = "Aula sem plano de aula registrado")]
        AulasSemPlanoAulaNaDataDoFechamento = 3,

        [Display(Name = "Aulas sem frequência registrada")]
        AulasSemFrequenciaNaDataDoFechamento = 4,

        [Display(Name = "")]
        ResultadosFinaisAbaixoDaMedia = 5
    }
}