using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacoesPorTurmaBimestresDisciplinasQuery : IRequest<IEnumerable<AlunoDisciplinaTotalCompensacaoAusenciaDto>>
    {
        public ObterTotalCompensacoesPorTurmaBimestresDisciplinasQuery(int[] bimestres, string[] disciplinasIds, string turmaCodigo)
        {
            Bimestres = bimestres;
            DisciplinasIds = disciplinasIds;
            TurmaCodigo = turmaCodigo;
        }

        public int[] Bimestres { get; }
        public string[] DisciplinasIds { get; }
        public string TurmaCodigo { get; }
    }

    public class ObterTotalCompensacoesPorTurmaBimestresDisciplinasQueryValidator : AbstractValidator<ObterTotalCompensacoesPorTurmaBimestresDisciplinasQuery>
    {
        public ObterTotalCompensacoesPorTurmaBimestresDisciplinasQueryValidator()
        {
            RuleFor(a => a.Bimestres)
                .NotEmpty()
                .WithMessage("O(s) bimestre(s) deve ser informado para consulta de compensações no periodo");

            RuleFor(a => a.DisciplinasIds)
                .NotEmpty()
                .WithMessage("A(s) disciplinas devem ser informados para consulta de compensações no periodo");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O codigo da turma deve ser informado para consulta de compensações no periodo");
        }
    }
}
