using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery(int[] modalidadesIds)
        {
            ModalidadesIds = modalidadesIds;
        }

        public int[] ModalidadesIds { get; }
    }

    public class ObterQuestoesEncaminhamentoNAAPAPorModalidadesQueryValidator : AbstractValidator<ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery>
    {
        public ObterQuestoesEncaminhamentoNAAPAPorModalidadesQueryValidator()
        {
           RuleFor(c => c.ModalidadesIds)
            .NotNull()
            .WithMessage("Os Ids das modalidades devem ser informados para consulta dos questionários do encaminhamento NAAPA.");
        }
    }
}
