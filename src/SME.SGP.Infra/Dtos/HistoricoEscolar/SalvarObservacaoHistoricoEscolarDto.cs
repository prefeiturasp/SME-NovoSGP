using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Dtos
{
    public class SalvarObservacaoHistoricoEscolarDto
    {
        public SalvarObservacaoHistoricoEscolarDto() { }
        public SalvarObservacaoHistoricoEscolarDto(string observacao)
        {
            Observacao = observacao;
        }

        [Required(ErrorMessage = "Observação deve ser informado"), MaxLength(500)]
        public string Observacao { get; set; }
    }
}
