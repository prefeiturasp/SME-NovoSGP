using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroColetivoNAAPACompletoPorIdQuery : IRequest<RegistroColetivoCompletoDto>
    {
        public ObterRegistroColetivoNAAPACompletoPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterRegistroColetivoNAAPACompletoPorIdQueryValidator : AbstractValidator<ObterRegistroColetivoNAAPACompletoPorIdQuery>
    {
        public ObterRegistroColetivoNAAPACompletoPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do Registro Coletivo NAAPA deve ser informado para a pesquisa.");
        }
    }
}
