using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoRespostaAtendimentoNAAPAPorIdQuery : IRequest<IEnumerable<RespostaQuestaoAtendimentoNAAPADto>>
    {
        public ObterQuestaoRespostaAtendimentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterQuestaoRespostaAtendimentoNAAPAPorIdQueryValidator : AbstractValidator<ObterQuestaoRespostaAtendimentoNAAPAPorIdQuery>
    {
        public ObterQuestaoRespostaAtendimentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do atendimento naapa deve ser informado para a pesquisa");

        }
    }
}
