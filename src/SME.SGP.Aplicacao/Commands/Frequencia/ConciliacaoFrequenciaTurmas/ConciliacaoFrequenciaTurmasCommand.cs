using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasCommand : IRequest<bool>
    {
        public ConciliacaoFrequenciaTurmasCommand(DateTime data, string turmaCodigo, string componenteCurricularId)
        {
            Data = data;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularId = componenteCurricularId;
        }

        public DateTime Data { get; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularId { get; set; }
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
