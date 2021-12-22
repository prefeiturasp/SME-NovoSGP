using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class SolicitaRelatorioDevolutivasCommand : IRequest<Guid>
    {
        public SolicitaRelatorioDevolutivasCommand(long devolutivaId)
        {
            DevolutivaId = devolutivaId;
        }
        public long DevolutivaId { get; set; }

        public class SolicitaRelatorioDevolutivasCommandValidator : AbstractValidator<SolicitaRelatorioDevolutivasCommand>
        {
            public SolicitaRelatorioDevolutivasCommandValidator()
            {
                RuleFor(c => c.DevolutivaId)
                   .NotEmpty()
                   .WithMessage("O DevolutivaId deve ser informado para solicitação do relatório.");

            }
        }
    }
}
