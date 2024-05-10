using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery : IRequest<string[]>
    {
        public ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(int anoLetivo, Modalidade modalidade)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
    }

    public class ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQueryValidator : AbstractValidator<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery>
    {
        public ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo precisa ser informado");
            RuleFor(a => a.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade precisa ser informada");
        }
    }
}
