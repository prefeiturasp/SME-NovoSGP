using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConciliacaoFrequenciaTurmaCommand : IRequest<bool>
    {
        public IncluirFilaConciliacaoFrequenciaTurmaCommand(string turmaCodigo, int bimestre, DateTime dataInicio, DateTime dataFim, string componenteCurricularId)
        {
            TurmaCodigo = turmaCodigo;
            Bimestre = bimestre;
            DataInicio = dataInicio;
            DataFim = dataFim;
            ComponenteCurricularId = componenteCurricularId;
        }

        public string TurmaCodigo { get; }
        public int Bimestre { get; }
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
        public string ComponenteCurricularId { get; set; }
    }

    public class IncluirFilaConciliacaoFrequenciaTurmaCommandValidator : AbstractValidator<IncluirFilaConciliacaoFrequenciaTurmaCommand>
    {
        public IncluirFilaConciliacaoFrequenciaTurmaCommandValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para inclusão da fila de conciliação de frequência");

            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre da turma deve ser informado para inclusão da fila de conciliação de frequência");

            RuleFor(a => a.DataInicio)
                .NotEmpty()
                .WithMessage("A data de início deve ser informada para inclusão da fila de conciliação de frequência");

            RuleFor(a => a.DataFim)
                .NotEmpty()
                .WithMessage("A data de fim deve ser informada para inclusão da fila de conciliação de frequência");
        }
    }
}
