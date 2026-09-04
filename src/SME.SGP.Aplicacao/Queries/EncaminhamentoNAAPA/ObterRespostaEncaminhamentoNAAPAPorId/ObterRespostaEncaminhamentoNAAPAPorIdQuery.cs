using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostaEncaminhamentoNAAPAPorIdQuery : IRequest<RespostaEncaminhamentoNAAPA>
    {
        public ObterRespostaEncaminhamentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterRespostaEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterRespostaEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterRespostaEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do encaminhamento naapa deve ser informado para a pesquisa");

        }
    }
}
