using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacoesPlanoAEEPorIdQuery : IRequest<IEnumerable<PlanoAEEObservacaoDto>>
    {
        public ObterObservacoesPlanoAEEPorIdQuery(long id, string usuarioRF)
        {
            Id = id;
            UsuarioRF = usuarioRF;
        }

        public long Id { get; }
        public string UsuarioRF { get; }
    }

    public class ObterObservacoesPlanoAEEPorIdQueryValidator : AbstractValidator<ObterObservacoesPlanoAEEPorIdQuery>
    {
        public ObterObservacoesPlanoAEEPorIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id do plano deve ser informado para consulta de suas informações.");
        }
    }
}
