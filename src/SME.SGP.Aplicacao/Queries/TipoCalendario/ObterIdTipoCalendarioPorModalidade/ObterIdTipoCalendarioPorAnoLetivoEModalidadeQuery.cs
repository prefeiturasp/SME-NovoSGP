using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery : IRequest<long>
    {
        public ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade modalidade, int anoLetivo, int? semestre)
        {
            Modalidade = modalidade;
            AnoLetivo = anoLetivo;
            Semestre = semestre;
        }

        public Modalidade Modalidade { get; set; }
        public int AnoLetivo { get; set; }
        public int? Semestre { get; set; }
    }

    public class ObterTipoCalendarioPorModalidadeQueryValidator : AbstractValidator<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery>
    {
        public ObterTipoCalendarioPorModalidadeQueryValidator()
        {
            RuleFor(c => c.Modalidade)
                .IsInEnum()
                .WithMessage("A modalidade deve ser informada.");

            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Semestre)
                .NotEmpty()
                .WithMessage("O semestre deve ser informado.")
                .When(c => c.Modalidade == Modalidade.EJA);
        }
    }
}
