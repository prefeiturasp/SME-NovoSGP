using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dto
{
    public class TipoCalendarioEscolarCompletoDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Nome do Calendário é obrigatório")]
        [MinLength(1, ErrorMessage = "O Nome do Calendário deve conter no mínimo 1 caracteres.")]
        [MaxLength(50, ErrorMessage = "O Nome do Calendário deve conter no máximo 50 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo Ano Letivo é obrigatório")]
        public int anoLetivo { get; set; }
        [Required(ErrorMessage = "O campo Período é obrigatório")]
        public Periodo Periodo { get; set; }
        [Required(ErrorMessage = "O campo Modalidade é obrigatório")]
        public Modalidade Modalidade { get; set; }
        [Required(ErrorMessage = "O campo Situação é obrigatório")]
        public bool Situacao { get; set; }
        public DateTime AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string UsuarioRf { get; set; }
        public string UsuarioRfInserido { get; set; }
        public string UsuarioRfAlterado { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
    }
}
