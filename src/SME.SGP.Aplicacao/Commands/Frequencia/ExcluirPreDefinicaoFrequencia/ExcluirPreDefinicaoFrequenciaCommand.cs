using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPreDefinicaoFrequenciaCommand : IRequest<bool>
    {
        public ExcluirPreDefinicaoFrequenciaCommand(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class ExcluirPreDefinicaoFrequenciaCommandValidator : AbstractValidator<ExcluirPreDefinicaoFrequenciaCommand>
    {
        public ExcluirPreDefinicaoFrequenciaCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O Id da Turma deve ser informado");
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O Id do componente curricular deve ser informado");
        }
    }
}
