using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class RegistroPoaDto
    {
        [Range(1, double.MaxValue, ErrorMessage = "Deve ser informado o ano letivo")]
        public int AnoLetivo { get; set; }

        [Range(1, 4, ErrorMessage = "O bimestre deve ser informado entre 1 e 4")]
        public int Bimestre { get; set; }

        [Required(ErrorMessage = "Deve ser informado o código RF")]
        public string CodigoRf { get; set; }

        [Required(ErrorMessage = "Deve ser informado a descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Deve ser informado o Id da Dre")]
        public string DreId { get; set; }

        public bool Excluido { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }

        [Required(ErrorMessage = "Deve ser informado o título")]
        [MaxLength(50, ErrorMessage = "O título deve ter no máximo 50 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Deve ser informado o Id da Ue")]
        public string UeId { get; set; }
    }
}