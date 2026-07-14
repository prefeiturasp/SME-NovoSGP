using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoRespostaMapeamentoEstudantePorIdQuery : IRequest<IEnumerable<RespostaQuestaoMapeamentoEstudanteDto>>
    {
        public ObterQuestaoRespostaMapeamentoEstudantePorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterQuestaoRespostaMapeamentoEstudantePorIdQueryValidator : AbstractValidator<ObterQuestaoRespostaMapeamentoEstudantePorIdQuery>
    {
        public ObterQuestaoRespostaMapeamentoEstudantePorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do mapeamento de estudante deve ser informado para a pesquisa");

        }
    }
}
