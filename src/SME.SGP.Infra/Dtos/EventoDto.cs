using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class EventoDto : IValidatableObject
    {
        public EventoDto()
        {
            DataConfirmada = false;
        }

        public bool AlterarARecorrenciaCompleta { get; set; }
        public bool DataConfirmada { get; set; }
        public DateTime? DataFim { get; set; }

        [DataRequerida(ErrorMessage = "A data do evento deve ser informada.")]
        public DateTime DataInicio { get; set; }

        [MaxLength(500, ErrorMessage = "A descrição deve conter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        public string DreId { get; set; }
        public long? EventoPaiId { get; set; }
        public long? FeriadoId { get; set; }
        public EventoLetivo Letivo { get; set; }

        public bool Migrado { get; set; }

        [Required(ErrorMessage = "O nome do evento deve ser informado.")]
        [MaxLength(50, ErrorMessage = "O nome deve conter no máximo 50 caracteres.")]
        public string Nome { get; set; }

        public RecorrenciaEventoDto RecorrenciaEventos { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Tipo do calendário deve ser informado.")]
        public long TipoCalendarioId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "O tipo de evento deve ser informado.")]
        public long TipoEventoId { get; set; }

        public IEnumerable<CopiarEventoDto> TiposCalendarioParaCopiar { get; set; }
        public string UeId { get; set; }

        public int? Bimestre { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataFim.HasValue && DataInicio > DataFim)
            {
                yield return new ValidationResult("A data de início do evento deve ser menor que a data final.");
            }

            if (RecorrenciaEventos != null && RecorrenciaEventos.DataFim.HasValue && RecorrenciaEventos.DataInicio > RecorrenciaEventos.DataFim)
                yield return new ValidationResult("A data de início da recorrência deve ser menor que a data final.");
        }
    }
}