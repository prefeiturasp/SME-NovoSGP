using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCicloPorAnoModalidadeQuery :IRequest<CicloDto>
    {
        public ObterCicloPorAnoModalidadeQuery(string ano, Modalidade modalidade)
        {
            Ano = ano;
            Modalidade = modalidade;
        }

        public string Ano { get; set; }
        public Modalidade Modalidade { get; set; }
    }

    public class ObterCicloPorAnoModalidadeQueryValidator : AbstractValidator<ObterCicloPorAnoModalidadeQuery>
    {
        public ObterCicloPorAnoModalidadeQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty().WithMessage("Informe o Ano");
            RuleFor(a => a.Modalidade)
                .NotEmpty().WithMessage("Informe a Modalidade");
        }
    }
}