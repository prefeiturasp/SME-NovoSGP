using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreAtualQueryValidator : AbstractValidator<ObterBimestreAtualQuery>
    {
        public ObterBimestreAtualQueryValidator()
        {
            RuleFor(c => c.Turma)
                .NotNull()
                .WithMessage("Para obter o bimestre atual é necessário informar a turma.");
            RuleFor(c => c.DataReferencia)
                .Must(a => a > DateTime.MinValue)
                .WithMessage("Para obter o bimestre atual é necessário informar a data de referência.");
        }
    }
}