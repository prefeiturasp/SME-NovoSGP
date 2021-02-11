using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostasPlanoAEEPorVersaoQuery : IRequest<IEnumerable<RespostaQuestaoDto>>
    {
        public ObterRespostasPlanoAEEPorVersaoQuery(long versaoPlanoId)
        {
            VersaoPlanoId = versaoPlanoId;
        }

        public long VersaoPlanoId { get; }
    }

    public class ObterRespostasPlanoAEEPorVersaoQueryValidator : AbstractValidator<ObterRespostasPlanoAEEPorVersaoQuery>
    {
        public ObterRespostasPlanoAEEPorVersaoQueryValidator()
        {
            RuleFor(a => a.VersaoPlanoId)
                .NotEmpty()
                .WithMessage("O id da versão do plano deve ser informado para buscar suas respostas.");
        }
    }

}
