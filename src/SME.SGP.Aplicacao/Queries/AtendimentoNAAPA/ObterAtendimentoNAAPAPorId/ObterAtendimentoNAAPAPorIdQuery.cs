using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoNAAPAPorIdQuery : IRequest<EncaminhamentoNAAPA>
    {
        public ObterAtendimentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }
        

        public long Id { get; }
    }

    public class ObterAtendimentoNAAPAPorIdQueryValidator : AbstractValidator<ObterAtendimentoNAAPAPorIdQuery>
    {
        public ObterAtendimentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do atendimento NAAPA deve ser informado para a pesquisa.");
        }
    }
}
