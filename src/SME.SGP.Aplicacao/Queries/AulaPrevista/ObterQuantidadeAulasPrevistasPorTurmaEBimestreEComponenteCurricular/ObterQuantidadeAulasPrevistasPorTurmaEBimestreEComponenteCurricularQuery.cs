using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery : IRequest<int>
    {
        public string CodigoTurma { get; set; }
        public long TipoCalendarioId { get; set; }
        public long[] ComponentesCurricularesId { get; set; }
        public int? Bimestre { get; set; }
        public string Professor { get; set; }

        public ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery(string codigoTurma, long tipoCalendarioId, long[] componentesCurricularesId, int? bimestre, string professor = null)
        {
            CodigoTurma = codigoTurma;
            TipoCalendarioId = tipoCalendarioId;
            ComponentesCurricularesId = componentesCurricularesId;
            Bimestre = bimestre;
            Professor = professor;
        }
    }

    public class ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQueryValidator : AbstractValidator<ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery>
    {
        public ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQueryValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleFor(x => x.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado.");

            RuleFor(x => x.ComponentesCurricularesId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado.");

            RuleFor(x => x.Bimestre)
                .NotEmpty()
                .When(x => x.Bimestre.HasValue)
                .WithMessage("O bimestre deve ser informado.");
        }
    }
}