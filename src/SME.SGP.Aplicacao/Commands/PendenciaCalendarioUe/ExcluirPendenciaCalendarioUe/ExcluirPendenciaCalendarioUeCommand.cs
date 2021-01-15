using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaCalendarioUeCommand : IRequest<bool>
    {
        public ExcluirPendenciaCalendarioUeCommand(long tipoCalendarioId, long ueId, TipoPendencia tipoPendencia)
        {
            TipoCalendarioId = tipoCalendarioId;
            UeId = ueId;
            TipoPendencia = tipoPendencia;
        }

        public long TipoCalendarioId { get; set; }
        public long UeId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class ExcluirPendenciaCalendarioUeCommandValidator : AbstractValidator<ExcluirPendenciaCalendarioUeCommand>
    {
        public ExcluirPendenciaCalendarioUeCommandValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
               .Must(a => a > 0)
               .WithMessage("O id do tipo de calendario deve ser informado para exclusão da pendência calendario.");

            RuleFor(c => c.UeId)
               .Must(a => a > 0)
               .WithMessage("O id da UE deve ser informado para exclusão da pendência calendario.");
        }
    }
}
