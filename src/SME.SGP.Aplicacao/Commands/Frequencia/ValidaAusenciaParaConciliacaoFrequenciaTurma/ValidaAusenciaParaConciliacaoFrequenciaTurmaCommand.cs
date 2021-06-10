using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ValidaAusenciaParaConciliacaoFrequenciaTurmaCommand : IRequest<bool>
    {
        public ValidaAusenciaParaConciliacaoFrequenciaTurmaCommand(string turmaCodigo, int bimestre, DateTime dataInicio, DateTime dataFim)
        {
            TurmaCodigo = turmaCodigo;
            Bimestre = bimestre;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public string TurmaCodigo { get; }
        public int Bimestre { get; }
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
    }

    public class ValidaAusenciaParaConciliacaoFrequenciaTurmaCommandValidator : AbstractValidator<ValidaAusenciaParaConciliacaoFrequenciaTurmaCommand>
    {
        public ValidaAusenciaParaConciliacaoFrequenciaTurmaCommandValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para validação de ausências e conciliação de frequência");

            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre turma deve ser informado para validação de ausências e conciliação de frequência");

            RuleFor(a => a.DataInicio)
                .NotEmpty()
                .WithMessage("A data de início deve ser informada para validação de ausências e conciliação de frequência");

            RuleFor(a => a.DataFim)
                .NotEmpty()
                .WithMessage("A data de fim deve ser informada para validação de ausências e conciliação de frequência");
        }
    }
}
