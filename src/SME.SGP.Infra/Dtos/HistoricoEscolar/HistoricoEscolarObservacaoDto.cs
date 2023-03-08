using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Dtos
{
    public class HistoricoEscolarObservacaoDto
    {
        public HistoricoEscolarObservacaoDto() { }
        public HistoricoEscolarObservacaoDto(string codigoAluno, string observacao)
        {
            CodigoAluno = codigoAluno;
            Observacao = observacao;
        }

        [Required(ErrorMessage ="Código do aluno deve ser informado")]
        public string CodigoAluno { get; set; }
        [Required(ErrorMessage = "Observação deve ser informado")]
        [MaxLength(500)]
        public string Observacao { get; set; }
    }
}
