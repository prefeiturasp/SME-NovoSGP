using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasCommand : IRequest<bool>
    {
        public ConciliacaoFrequenciaTurmasCommand(DateTime data)
        {
            Data = data;
        }

        public DateTime Data { get; }
    }

    public class ConciliacaoFrequenciaTurmasCommandValidator : AbstractValidator<ConciliacaoFrequenciaTurmasCommand>
    {
        public ConciliacaoFrequenciaTurmasCommandValidator()
        {
            RuleFor(a => a.Data)
                .NotEmpty()
                .WithMessage("A data de execução deve ser informada para calculo da conciliação de frequência das turmas no ano");
        }
    }
}
