using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class TipoAvaliacaoDto
    {
        [Required(ErrorMessage = "A descrição do tipo de avaliação deve ser informada.")]
        [MaxLength(200, ErrorMessage = "A descrição deve conter no máximo 200 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O nome do tipo de avaliação deve ser informado.")]
        [MaxLength(30, ErrorMessage = "O nome deve conter no máximo 30 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Situação é obrigatório")]
        public bool Situacao { get; set; }
    }
}