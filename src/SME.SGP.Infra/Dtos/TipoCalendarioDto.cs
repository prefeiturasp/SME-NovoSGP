using SME.SGP.Dominio;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class TipoCalendarioDto
    {
        [Required(ErrorMessage = "O campo Ano Letivo é obrigatório")]
        public int AnoLetivo { get; set; }

        public string DescricaoPeriodo { get; set; }
        public long Id { get; set; }
        public int? Semestre { get; set; }
        public bool Migrado { get; set; }

        [EnumeradoRequirido(ErrorMessage = "A Modalidade é obrigatória.")]
        public ModalidadeTipoCalendario Modalidade { get; set; }

        [Required(ErrorMessage = "O campo Nome do Calendário é obrigatório")]
        [MinLength(1, ErrorMessage = "O Nome do Calendário deve conter no mínimo 1 caracteres.")]
        [MaxLength(50, ErrorMessage = "O Nome do Calendário deve conter no máximo 50 caracteres.")]
        public string Nome { get; set; }

        [EnumeradoRequirido(ErrorMessage = "O Período é obrigatório.")]
        public Periodo Periodo { get; set; }

        [Required(ErrorMessage = "O campo Situação é obrigatório")]
        public bool Situacao { get; set; }
        public int? Aplicacao { get; set; }
    }
}