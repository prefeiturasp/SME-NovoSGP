using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class RegistroPoaFiltroDto
    {
        public int AnoLetivo { get; set; }

        [Required(ErrorMessage = "O código rf é obrigatório")]
        public string CodigoRf { get; set; }

        [Required(ErrorMessage = "É obrigatório informar o id da Dre")]
        public string DreId { get; set; }

        [Range(0, 12, ErrorMessage = "Os meses devem ser infomados entre Janeiro e Dezembro")]
        public int Mes { get; set; }

        public string Titulo { get; set; }

        [Required(ErrorMessage = "E necessário informar o id da unidade escolar")]
        public string UeId { get; set; }
    }
}