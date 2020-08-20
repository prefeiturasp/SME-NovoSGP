using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AtualizarDiarioBordoComDevolutivaCommand : IRequest<bool>
    {
        public List<long> DiariosBordoIds { get; set; }

        public long DevolutivaId { get; set; }

        public AtualizarDiarioBordoComDevolutivaCommand(List<long> diariosBordoIds, long devolutivaId)
        {
            this.DiariosBordoIds = diariosBordoIds;
            this.DevolutivaId = devolutivaId;
        }

        public class AtualizarDiarioBordoComDevolutivaCommandValidator : AbstractValidator<AtualizarDiarioBordoComDevolutivaCommand>
        {
            public AtualizarDiarioBordoComDevolutivaCommandValidator()
            {
                RuleFor(a => a.DiariosBordoIds)
                       .NotEmpty()
                       .WithMessage("Os diários de bordo devem ser informados!");

                RuleFor(a => a.DevolutivaId)
                       .NotEmpty()
                       .WithMessage("A devolutiva deve ser informada!");
            }
        }
    }
}
