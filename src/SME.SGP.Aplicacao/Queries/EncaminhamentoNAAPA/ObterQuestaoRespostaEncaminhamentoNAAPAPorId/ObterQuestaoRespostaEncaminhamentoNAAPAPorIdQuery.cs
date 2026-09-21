using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQuery : IRequest<IEnumerable<RespostaQuestaoEncaminhamentoNAAPADto>>
    {
        public ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do encaminhamento naapa deve ser informado para a pesquisa");

        }
    }
}
