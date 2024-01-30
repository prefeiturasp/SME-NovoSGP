using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroAcaoPorIdQuery : IRequest<RegistroAcaoBuscaAtiva>
    {
        public ObterRegistroAcaoPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterRegistroAcaoPorIdQueryValidator : AbstractValidator<ObterRegistroAcaoPorIdQuery>
    {
        public ObterRegistroAcaoPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do registro de ação deve ser informado para a pesquisa.");
        }
    }
}
