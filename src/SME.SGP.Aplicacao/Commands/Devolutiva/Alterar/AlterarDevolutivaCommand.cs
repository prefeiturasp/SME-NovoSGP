using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarDevolutivaCommand: IRequest<AuditoriaDto>
    {
        public Devolutiva Devolutiva { get; set; }

        public AlterarDevolutivaCommand(Devolutiva devolutiva)
        {
            Devolutiva = devolutiva;
        }
    }

    public class AlterarDevolutivaCommandValidator: AbstractValidator<AlterarDevolutivaCommand>
    {
        public AlterarDevolutivaCommandValidator()
        {
            RuleFor(a => a.Devolutiva)
                   .NotEmpty()
                   .WithMessage("A Devolutiva deve ser informada!");
        }
    }
}
