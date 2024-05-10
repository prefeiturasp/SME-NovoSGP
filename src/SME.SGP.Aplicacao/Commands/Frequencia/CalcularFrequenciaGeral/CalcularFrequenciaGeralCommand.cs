using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaGeralCommand : IRequest<bool>
    {
        public CalcularFrequenciaGeralCommand(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; set; }
    }

    public class CalcularFrequenciaGeralCommandValidator : AbstractValidator<CalcularFrequenciaGeralCommand>
    {
        public CalcularFrequenciaGeralCommandValidator()
        {
            RuleFor(a => a.Ano)
               .NotEmpty()
               .WithMessage("O ano de execução deve ser informada para calculo da frequencia");
        }
    }
}
