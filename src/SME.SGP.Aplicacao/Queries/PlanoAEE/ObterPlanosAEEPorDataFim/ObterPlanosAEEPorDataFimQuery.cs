using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEPorDataFimQuery : IRequest<IEnumerable<PlanoAEE>>
    {
        public ObterPlanosAEEPorDataFimQuery(DateTime dataFim, bool desconsideraPendencias = true, bool desconsideraNotificados = false, NotificacaoPlanoAEETipo tipo = NotificacaoPlanoAEETipo.PlanoCriado)
        {
            DataFim = dataFim;
            DesconsideraPendencias = desconsideraPendencias;
            DesconsideraNotificados = desconsideraNotificados;
            Tipo = tipo;
        }

        public DateTime DataFim { get; }
        public bool DesconsideraPendencias { get; }
        public bool DesconsideraNotificados { get; }
        public NotificacaoPlanoAEETipo Tipo { get; }
    }

    public class ObterPlanosAEEPorDataFimQueryValidator : AbstractValidator<ObterPlanosAEEPorDataFimQuery>
    {
        public ObterPlanosAEEPorDataFimQueryValidator()
        {
            RuleFor(a => a.DataFim)
                .NotEmpty()
                .WithMessage("A data final do plano AEE deve ser informada para pesquisa");
        }
    }
}
