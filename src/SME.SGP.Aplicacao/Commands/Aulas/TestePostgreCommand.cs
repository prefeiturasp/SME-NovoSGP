using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao.Commands.Aulas
{
    public class TestePostgreCommand : IRequest<bool>
    {
        public TestePostgreCommand(Guid idCorrelacao)
        {
            IdCorrelacao = idCorrelacao;
        }

        public Guid IdCorrelacao { get; set; }
    }


    public class TesteValidator : AbstractValidator<TestePostgreCommand>
    {
        public TesteValidator()
        {

            RuleFor(c => c.IdCorrelacao)
            .NotEmpty()
            .WithMessage("O id de correlação deve ser informado.");
        }
    }
}
