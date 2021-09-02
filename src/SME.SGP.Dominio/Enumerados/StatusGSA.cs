using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum StatusGSA
    {
        [Display(Name = "Não Especificado")]
        NaoEspecificado = 0,
        [Display(Name = "Novo")]
        Novo = 1,
        [Display(Name = "Criado")]
        Criado = 2,
        [Display(Name = "Entregue", Description = "Atividade entregue no Google Sala de Aula")]
        Entregue = 3  ,  
        [Display(Name = "Devolvido")]
        Devolvido = 4,
        [Display(Name = "Reclamada pelo Aluno")]
        ReclamadaPeloAluno = 4,
    }
}
