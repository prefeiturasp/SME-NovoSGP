using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class RegistroPoaFiltroDto
    {
        public int AnoLetivo { get; set; }

        [Range(0, 4, ErrorMessage = "Os bimestres devem ser infomados entre 1 e 4")]
        public int Bimestre { get; set; }

        [Required(ErrorMessage = "O código rf é obrigatório")]
        public string CodigoRf { get; set; }

        [Required(ErrorMessage = "É obrigatório informar o id da Dre")]
        public string DreId { get; set; }

        public string Titulo { get; set; }

        [Required(ErrorMessage = "E necessário informar o id da unidade escolar")]
        public string UeId { get; set; }
    }
}