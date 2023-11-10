using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCepQuery : IRequest<CepDto>
    {
        public ObterCepQuery(string cep)
        {
            Cep = cep;
        }
        public string Cep { get; set; }
    }

    public class ObterCepQueryValidator : AbstractValidator<ObterCepQuery>
    {
        public ObterCepQueryValidator()
        {
            RuleFor(a => a.Cep)
                .NotNull()
                .WithMessage("É necessário informar o cep para a busca das informações postais");
        }
    }
}
