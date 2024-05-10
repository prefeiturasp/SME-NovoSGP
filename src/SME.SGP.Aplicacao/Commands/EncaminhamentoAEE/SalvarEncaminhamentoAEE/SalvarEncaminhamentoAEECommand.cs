using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarEncaminhamentoAEECommand : IRequest<bool>
    {
        public SalvarEncaminhamentoAEECommand(EncaminhamentoAEE encaminhamentoAEE)
        {
            EncaminhamentoAEE = encaminhamentoAEE;
        }

        public EncaminhamentoAEE EncaminhamentoAEE { get; set; }
    }

    public class SalvarEncaminhamentoAEECommandValidator : AbstractValidator<SalvarEncaminhamentoAEECommand>
    {
        public SalvarEncaminhamentoAEECommandValidator()
        {
            RuleFor(a => a.EncaminhamentoAEE)
               .NotEmpty()
               .WithMessage("O encaminhamento AEE deve ser informado para alteração.");
        }
    }
}
