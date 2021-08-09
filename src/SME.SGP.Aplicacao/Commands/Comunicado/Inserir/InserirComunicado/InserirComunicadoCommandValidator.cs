using FluentValidation;
using System;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoCommandValidator : AbstractValidator<InserirComunicadoCommand>
    {
        public InserirComunicadoCommandValidator()
        {
            RuleFor(c => c.DataEnvio)
            .NotEmpty()
            .WithMessage("A data de envio deve ser informada.");

            RuleFor(c => c.DataEnvio)
            .GreaterThanOrEqualTo(DateTime.Now.Date)
            .WithMessage("A data de envio deve ser igual ou maior que a data atual.");

            RuleFor(c => c.Descricao)
            .NotEmpty()
            .WithMessage("É necessário informar a descrição.");

            RuleFor(c => c.Descricao)
            .MinimumLength(5)
            .WithMessage("A descrição deve conter no mínimo 5 caracteres.");

            RuleFor(c => c.Titulo)
           .NotEmpty()
           .WithMessage("É necessário informar o título.");

            RuleFor(c => c.Titulo)
            .MinimumLength(10)
            .WithMessage("O título deve conter no mínimo 10 caracteres.");

            RuleFor(c => c.Titulo)
            .MaximumLength(50)
            .WithMessage("O título deve conter no máximo 50 caracteres.");

            RuleFor(c => c.AnoLetivo)
            .GreaterThan(0)
            .WithMessage("É necessario informar o ano letivo");

            RuleFor(c => c.CodigoDre)
           .NotEmpty()
           .WithMessage("É necessário informar o codigo da DRE");

            RuleFor(c => c.CodigoUe)
           .NotEmpty()
           .WithMessage("É necessário informar o codigo da UE");

            RuleFor(c => c.Modalidades)
           .NotEmpty()
           .WithMessage("A modalidade do comunicado deve ser informada.");

        }
    }
}