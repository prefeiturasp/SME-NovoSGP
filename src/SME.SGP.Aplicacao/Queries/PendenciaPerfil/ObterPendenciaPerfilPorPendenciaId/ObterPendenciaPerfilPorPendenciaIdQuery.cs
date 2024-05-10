using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaPerfilPorPendenciaIdQuery : IRequest<IEnumerable<PendenciaPerfil>>
    {
        public ObterPendenciaPerfilPorPendenciaIdQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; }
    }

    public class ObterPendenciaPerfilPorPendenciaIdQueryValidator : AbstractValidator<ObterPendenciaPerfilPorPendenciaIdQuery>
    {
        public ObterPendenciaPerfilPorPendenciaIdQueryValidator()
        {
            RuleFor(a => a.PendenciaId)
                .NotEmpty()
                .WithMessage("O id da pendência deve ser informado para consulta da relação de perfis da mesma.");
        }
    }
}
