using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaCalendarioUeCommand : IRequest<bool>
    {
        public SalvarPendenciaCalendarioUeCommand(long tipoCalendarioId, long ueId, string descricao)
        {
            TipoCalendarioId = tipoCalendarioId;
            UeId = ueId;
            Descricao = descricao;
        }

        public long TipoCalendarioId { get; set; }
        public long UeId { get; set; }
        public string Descricao { get; set; }
    }

    public class SalvarPendenciaCalendarioUeCommandValidator : AbstractValidator<SalvarPendenciaCalendarioUeCommand>
    {
        public SalvarPendenciaCalendarioUeCommandValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
            .NotEmpty()
            .WithMessage("O id do tipo de calendário deve ser informado para geração de pendência.");

            RuleFor(c => c.UeId)
            .NotEmpty()
            .WithMessage("O id da UE deve ser informado para geração de pendência.");

            RuleFor(c => c.Descricao)
            .NotEmpty()
            .WithMessage("A descrição deve ser informada para geração de pendência.");
        }

    }
}
