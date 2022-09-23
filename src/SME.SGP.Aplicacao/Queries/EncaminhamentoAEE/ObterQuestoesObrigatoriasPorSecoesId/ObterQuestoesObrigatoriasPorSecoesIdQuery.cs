using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterQuestoesObrigatoriasPorSecoesIdQuery : IRequest<IEnumerable<QuestaoSecaoAeeDto>>
    {
        public ObterQuestoesObrigatoriasPorSecoesIdQuery(long[] secoesId)
        {
            SecoesId = secoesId;
        }

        public long[] SecoesId { get; set; }
        

    }

    public class ObterQuestoesObrigatoriasPorSecoesIdsQueryValidator : AbstractValidator<ObterQuestoesObrigatoriasPorSecoesIdQuery>
    {
        public ObterQuestoesObrigatoriasPorSecoesIdsQueryValidator()
        {
            RuleFor(c => c.SecoesId)
            .NotEmpty()
            .WithMessage("O Id (ou Ids) da seção deve ser informado para obtenção das questões obrigatórias.");
        }
    }

}
