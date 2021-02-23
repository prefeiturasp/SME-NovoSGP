using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PersistirPlanoAEECommand : IRequest<bool>
    {
        public PersistirPlanoAEECommand(PlanoAEE planoAEE)
        {
            PlanoAEE = planoAEE;
        }

        public PlanoAEE PlanoAEE { get; }
    }

    public class PersistirPlanoAEECommandValidator : AbstractValidator<PersistirPlanoAEECommand>
    {
        public PersistirPlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEE)
                .NotEmpty()
                .WithMessage("A entidade Plano AEE deve ser informada para persistencia");
        }
    }
}
