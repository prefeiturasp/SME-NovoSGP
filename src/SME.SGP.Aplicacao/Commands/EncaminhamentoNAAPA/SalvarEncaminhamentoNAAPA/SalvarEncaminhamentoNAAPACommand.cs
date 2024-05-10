using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public SalvarEncaminhamentoNAAPACommand(EncaminhamentoNAAPA encaminhamentoNAAPA)
        {
            EncaminhamentoNAAPA = encaminhamentoNAAPA;
        }

        public EncaminhamentoNAAPA EncaminhamentoNAAPA { get; set; }
    }

    public class SalvarEncaminhamentoNAAPACommandValidator : AbstractValidator<SalvarEncaminhamentoNAAPACommand>
    {
        public SalvarEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(a => a.EncaminhamentoNAAPA)
               .NotEmpty()
               .WithMessage("O encaminhamento NAAPA deve ser informado para alteração.");
        }
    }
}
