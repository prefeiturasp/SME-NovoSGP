using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosComComponentePorPlanoAulaIdQuery : IRequest<IEnumerable<ObjetivoAprendizagemComponenteDto>>
    {
        public ObterObjetivosComComponentePorPlanoAulaIdQuery(long planoAulaId)
        {
            PlanoAulaId = planoAulaId;
        }

        public long PlanoAulaId { get; set; }
    }

    public class ObterObjetivosComComponentePorPlanoAulaIdQueryValidator : AbstractValidator<ObterObjetivosComComponentePorPlanoAulaIdQuery>
    {
        public ObterObjetivosComComponentePorPlanoAulaIdQueryValidator()
        {
            RuleFor(c => c.PlanoAulaId)
            .NotEmpty()
            .WithMessage("O id do plano aula deve ser informado para consulta.");

        }
    }
}
