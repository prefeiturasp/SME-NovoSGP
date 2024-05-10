using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulasPrevistasPorTurmaEBimestreQuery : IRequest<int>
    {
        public string CodigoTurma { get; set; }
        public long TipoCalendarioId { get; set; }
        public int Bimestre { get; set; }

        public ObterQuantidadeAulasPrevistasPorTurmaEBimestreQuery(string codigoTurma, long tipoCalendarioId, int bimestre)
        {
            CodigoTurma = codigoTurma;
            TipoCalendarioId = tipoCalendarioId;
            Bimestre = bimestre;
        }
    }

    public class ObterQuantidadeAulasPrevistasPorTurmaEBimestreQueryValidator : AbstractValidator<ObterQuantidadeAulasPrevistasPorTurmaEBimestreQuery>
    {
        public ObterQuantidadeAulasPrevistasPorTurmaEBimestreQueryValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleFor(x => x.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado.");

            RuleFor(x => x.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado.");
        }
    }
}