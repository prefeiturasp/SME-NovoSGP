using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class EventoDto : IValidatableObject
    {
        public DateTime? DataFim { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "A data do evento deve ser informada.")]
        public DateTime? DataInicio { get; set; }

        public string Descricao { get; set; }
        public string DreId { get; set; }
        public long? FeriadoId { get; set; }
        public EventoLetivo Letivo { get; set; }

        [Required(ErrorMessage = "O nome do evento deve ser informado.")]
        public string Nome { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Tipo do calendário deve ser informado.")]
        public long TipoCalendarioId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "O tipo de evento deve ser informado.")]
        public long TipoEventoId { get; set; }

        public string UeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataFim.HasValue && DataInicio > DataFim)
            {
                yield return new ValidationResult("A data de início do evento deve ser menor que a data final.");
            }
        }
    }
}