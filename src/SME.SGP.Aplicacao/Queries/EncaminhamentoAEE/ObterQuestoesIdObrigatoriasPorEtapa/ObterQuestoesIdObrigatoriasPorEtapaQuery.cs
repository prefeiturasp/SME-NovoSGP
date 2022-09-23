using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterQuestoesIdObrigatoriasPorEtapaQuery : IRequest<IEnumerable<QuestaoIdSecaoAeeDto>>
    {
        public ObterQuestoesIdObrigatoriasPorEtapaQuery(int etapa)
        {
            Etapa = etapa;
        }

        public int Etapa { get; set; }
        

    }

    public class ObterQuestoesObrigatoriasPorSecoesIdsQueryValidator : AbstractValidator<ObterQuestoesIdObrigatoriasPorEtapaQuery>
    {
        public ObterQuestoesObrigatoriasPorSecoesIdsQueryValidator()
        {
            RuleFor(c => c.Etapa)
            .NotEmpty()
            .WithMessage("Etapa deve ser informada para obtenção das questões obrigatórias.");
        }
    }

}
