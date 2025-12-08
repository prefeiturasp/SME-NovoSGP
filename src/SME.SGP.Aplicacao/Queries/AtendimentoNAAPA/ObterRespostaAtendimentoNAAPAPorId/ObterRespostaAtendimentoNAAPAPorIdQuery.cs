using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostaAtendimentoNAAPAPorIdQuery : IRequest<RespostaEncaminhamentoNAAPA>
    {
        public ObterRespostaAtendimentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterRespostaAtendimentoNAAPAPorIdQueryValidator : AbstractValidator<ObterRespostaAtendimentoNAAPAPorIdQuery>
    {
        public ObterRespostaAtendimentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do atendimento naapa deve ser informado para a pesquisa");

        }
    }
}
