﻿using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class EncerrarEncaminhamentoAEECommand : IRequest<bool>
    {
        public EncerrarEncaminhamentoAEECommand(long encaminhamentoId, string motivoEncerramento)
        {
            EncaminhamentoId = encaminhamentoId;
            MotivoEncerramento = motivoEncerramento;
        }

        public long EncaminhamentoId { get; set; }
        public string MotivoEncerramento { get; set; }
    }
    public class EncerrarEncaminhamentoAEECommandValidator : AbstractValidator<EncerrarEncaminhamentoAEECommand>
    {
        public EncerrarEncaminhamentoAEECommandValidator()
        {
            RuleFor(x => x.EncaminhamentoId)
                   .GreaterThan(0)
                    .WithMessage("O Id do Encaminhamento deve ser informado!");
            RuleFor(x => x.MotivoEncerramento)
                   .GreaterThan(string.Empty)
                   .WithMessage("O Motivo do Encerramento deve ser informado!");
        }
    }
}
