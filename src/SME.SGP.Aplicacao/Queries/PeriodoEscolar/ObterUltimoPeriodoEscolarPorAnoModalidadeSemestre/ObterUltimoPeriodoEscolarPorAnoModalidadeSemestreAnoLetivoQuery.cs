using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery : IRequest<PeriodoEscolar>
    {
        public ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre)
        {
            Modalidade = modalidade;
            Semestre = semestre;
            AnoLetivo = anoLetivo;
        }

        public ModalidadeTipoCalendario Modalidade { get; }
        public int Semestre { get; }
        public int AnoLetivo { get; }
    }

    public class ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQueryValidator : AbstractValidator<ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery>
    {
        public ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQueryValidator()
        {
            RuleFor(c => c.Modalidade)
                .NotEmpty()
                .NotNull()
                .WithMessage("A modalidade deve ser informada para a busca do último período escolar.");

            RuleFor(c => c.Semestre)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A semestre deve ser informado para a busca do último período escolar.");

            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado para a busca do último período escolar.");
        }
    }
}
