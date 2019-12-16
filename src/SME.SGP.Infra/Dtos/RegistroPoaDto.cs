using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class RegistroPoaDto
    {
        [Required(ErrorMessage = "Deve ser informado o codigo RF")]
        public string CodigoRf { get; set; }

        [Required(ErrorMessage = "Deve ser informado a descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Deve ser informado o Id da Dre")]
        public string DreId { get; set; }

        public string Nome { get; set; }

        public bool Excluido { get; set; }
        public long Id { get; set; }

        [Range(1, 12, ErrorMessage = "Os meses devem estar contidos entre Janeiro (1) e Dezembro (12)")]
        public int Mes { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Deve ser informado o ano letivo")]
        public int AnoLetivo { get; set; }

        [Required(ErrorMessage = "Deve ser informado o titulo")]
        [MaxLength(50, ErrorMessage = "O titulo deve ter no maximo 50 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Deve ser informado o Id da Ue")]
        public string UeId { get; set; }
    }
}