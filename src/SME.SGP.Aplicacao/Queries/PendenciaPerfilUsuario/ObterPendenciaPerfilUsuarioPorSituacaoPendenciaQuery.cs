using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQuery : IRequest<IEnumerable<PendenciaPerfilUsuarioDto>>
    {
        public ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQuery(int situacaoPendencia)
        {
            SituacaoPendencia = situacaoPendencia;
        }

        public int SituacaoPendencia { get; }
    }

    public class ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQueryValidator : AbstractValidator<ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQuery>
    {
        public ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQueryValidator()
        {
            RuleFor(a => a.SituacaoPendencia)
                .NotEmpty()
                .WithMessage("A situação da pendência deve ser informado para consulta da pendência perfil usuario.");
        }
    }
}
