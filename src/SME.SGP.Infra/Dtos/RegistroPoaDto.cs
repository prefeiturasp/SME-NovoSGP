using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class RegistroPoaDto
    {
        [Required(ErrorMessage = "Deve ser informado o código RF")]
        public string CodigoRf { get; set; }

        [Required(ErrorMessage = "Deve ser informado a descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Deve ser informado o Id da Dre")]
        public string DreId { get; set; }

        public string Nome { get; set; }

        public bool Excluido { get; set; }
        public long Id { get; set; }

        [Range(1, 12, ErrorMessage = "O mes deve ser informado entre Janeiro (1) e Dezembro (12)")]
        public int Mes { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Deve ser informado o ano letivo")]
        public int AnoLetivo { get; set; }

        [Required(ErrorMessage = "Deve ser informado o título")]
        [MaxLength(50, ErrorMessage = "O título deve ter no máximo 50 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Deve ser informado o Id da Ue")]
        public string UeId { get; set; }
    }
}
