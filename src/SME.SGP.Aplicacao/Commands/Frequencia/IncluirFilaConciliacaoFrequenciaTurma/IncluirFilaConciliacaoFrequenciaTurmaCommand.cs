using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConciliacaoFrequenciaTurmaCommand : IRequest<bool>
    {
        public IncluirFilaConciliacaoFrequenciaTurmaCommand(string turmaCodigo, int bimestre, string componenteCurricularId, DateTime dataReferencia)
        {
            TurmaCodigo = turmaCodigo;
            Bimestre = bimestre;            
            ComponenteCurricularId = componenteCurricularId;
            DataReferencia = dataReferencia;
        }

        public string TurmaCodigo { get; }
        public int Bimestre { get; }        
        public DateTime DataReferencia { get; set; }
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

            RuleFor(a => a.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referência deve ser informada para inclusão da fila de conciliação de frequência");
            
        }
    }
}
