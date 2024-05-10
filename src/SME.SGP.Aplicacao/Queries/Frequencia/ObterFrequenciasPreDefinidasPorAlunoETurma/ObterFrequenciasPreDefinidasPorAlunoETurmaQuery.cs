using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPreDefinidasPorAlunoETurmaQuery : IRequest<IEnumerable<FrequenciaPreDefinidaDto>>
    {
        public ObterFrequenciasPreDefinidasPorAlunoETurmaQuery(long turmaId, long componenteCurricularId, string alunoCodigo)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string AlunoCodigo { get; set; }
    }

    public class ObterFrequenciasPreDefinidasPorAlunoETurmaQueryValidator : AbstractValidator<ObterFrequenciasPreDefinidasPorAlunoETurmaQuery>
    {
        public ObterFrequenciasPreDefinidasPorAlunoETurmaQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da Turma deve ser informado.");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado.");
        }
    }

}
