using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentosTurmaComponentesQuery : IRequest<IEnumerable<Dominio.FechamentoTurmaDisciplina>>
    {
        public ObterFechamentosTurmaComponentesQuery(long turmaId, long[] componentesCurricularesId, int bimestre)
        {
            TurmaId = turmaId;
            ComponentesCurricularesId = componentesCurricularesId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public long[] ComponentesCurricularesId { get; set; }
        public int Bimestre { get; set; }
    }

    public class ObterFechamentosTurmaComponentesQueryValidator : AbstractValidator<ObterFechamentosTurmaComponentesQuery>
    {
        public ObterFechamentosTurmaComponentesQueryValidator()
        {
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para consulta dos fechamentos.");

            RuleFor(c => c.ComponentesCurricularesId)
               .NotEmpty()
               .WithMessage("Os ids dos componentes curriculares deve ser informado para consulta dos fechamentos.");

            RuleFor(c => c.Bimestre)
               .NotNull()
               .WithMessage("O bimestre do período escolar deve ser informado para consulta dos fechamento.");
        }
    }
}
