using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class AulaDto
    {
        [Required(ErrorMessage = "O tipo de aula deve ser informado")]
        public TipoAula TipoAula { get; set; }
        [Required(ErrorMessage = "A disciplina deve ser informada")]
        public int DisciplinaId { get; set; }
        [Required(ErrorMessage = "A quantidade de aulas deve ser informada")]
        public int Quantidade { get; set; }
        [Required(ErrorMessage = "A data e hora devem ser informadas")]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "A recorrência deve ser informada")]
        public RecorrenciaAula RecorrenciaAula { get; set; }
    }
}
