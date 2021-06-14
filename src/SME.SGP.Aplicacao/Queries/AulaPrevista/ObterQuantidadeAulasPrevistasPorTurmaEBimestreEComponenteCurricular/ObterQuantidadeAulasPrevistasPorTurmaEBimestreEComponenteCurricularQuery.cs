using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery : IRequest<int>
    {
        public string CodigoTurma { get; set; }
        public long TipoCalendarioId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public int? Bimestre { get; set; }

        public ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery(string codigoTurma, long tipoCalendarioId, long componenteCurricularId, int? bimestre)
        {
            CodigoTurma = codigoTurma;
            TipoCalendarioId = tipoCalendarioId;
            ComponenteCurricularId = componenteCurricularId;
            Bimestre = bimestre;
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

            RuleFor(x => x.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado.");

            RuleFor(x => x.Bimestre)
                .NotEmpty()
                .When(x => x.Bimestre != null)
                .WithMessage("O bimestre deve ser informado.");
        }
    }
}