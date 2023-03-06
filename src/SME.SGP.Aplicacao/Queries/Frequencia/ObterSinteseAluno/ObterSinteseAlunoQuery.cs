using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterSinteseAlunoQuery : IRequest<SinteseDto>
    {
        public ObterSinteseAlunoQuery(double? percentualFrequencia, DisciplinaDto disciplina, int anoLetivo)
        {
            PercentualFrequencia = percentualFrequencia;
            Disciplina = disciplina;
            AnoLetivo = anoLetivo;
        }

        public double? PercentualFrequencia;
        public DisciplinaDto Disciplina;
        public int AnoLetivo;
    }

    public class ObterSinteseAlunoQueryValidator : AbstractValidator<ObterSinteseAlunoQuery>
    {
        public ObterSinteseAlunoQueryValidator()
        {
            RuleFor(a => a.PercentualFrequencia)
                .NotEmpty()
                .WithMessage("É preciso informar o percentual de frequência para consultar a síntese.");
            RuleFor(a => a.Disciplina)
                .NotEmpty()
                .WithMessage("É preciso informar a disciplina para consultar a síntese.");
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("É preciso informar o ano letivo para consultar a síntese.");
        }
    }
}
