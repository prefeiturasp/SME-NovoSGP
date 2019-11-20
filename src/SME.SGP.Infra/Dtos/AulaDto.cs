using SME.SGP.Dominio;
using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AulaDto
    {
        [Required(ErrorMessage = "A data e hora devem ser informadas")]
        public DateTime DataAula { get; set; }

        [Required(ErrorMessage = "A disciplina deve ser informada")]
        public string DisciplinaId { get; set; }

        [Required(ErrorMessage = "A quantidade de aulas deve ser informada")]
        public int Quantidade { get; set; }

        [EnumeradoRequirido(ErrorMessage = "A recorrência deve ser informada")]
        public RecorrenciaAula RecorrenciaAula { get; set; }

        [EnumeradoRequirido(ErrorMessage = "O tipo de aula deve ser informado")]
        public TipoAula TipoAula { get; set; }

        [Required(ErrorMessage = "O tipo de calendário deve ser informado")]
        public long TipoCalendarioId { get; set; }

        [Required(ErrorMessage = "A turma deve ser informada")]
        public string TurmaId { get; set; }

        [Required(ErrorMessage = "A UE deve ser informada")]
        public string UeId { get; set; }
    }
}