using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ValidaAusenciaParaConciliacaoFrequenciaTurmaCommand : IRequest<bool>
    {
        public ValidaAusenciaParaConciliacaoFrequenciaTurmaCommand(string turmaCodigo, DateTime dataInicio, DateTime dataFim, string componenteCurricularId)
        {
            TurmaCodigo = turmaCodigo;
            DataInicio = dataInicio;
            DataFim = dataFim;
            ComponenteCurricularId = componenteCurricularId;
        }

        public string TurmaCodigo { get; }
        
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
        public string ComponenteCurricularId { get; set; }
    }

    public class ValidaAusenciaParaConciliacaoFrequenciaTurmaCommandValidator : AbstractValidator<ValidaAusenciaParaConciliacaoFrequenciaTurmaCommand>
    {
        public ValidaAusenciaParaConciliacaoFrequenciaTurmaCommandValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para validação de ausências e conciliação de frequência");            

            RuleFor(a => a.DataInicio)
                .NotEmpty()
                .WithMessage("A data de início deve ser informada para validação de ausências e conciliação de frequência");

            RuleFor(a => a.DataFim)
                .NotEmpty()
                .WithMessage("A data de fim deve ser informada para validação de ausências e conciliação de frequência");
        }
    }
}
