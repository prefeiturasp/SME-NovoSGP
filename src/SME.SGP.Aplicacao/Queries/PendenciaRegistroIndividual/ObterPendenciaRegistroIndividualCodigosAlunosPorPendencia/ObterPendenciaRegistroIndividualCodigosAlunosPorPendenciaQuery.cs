using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public long PendenciaId { get; set; }

        public ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }
    }

    public class ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQueryValidator : AbstractValidator<ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQuery>
    {
        public ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQueryValidator()
        {
            RuleFor(x => x.PendenciaId)
                .NotEmpty()
                .WithMessage("O identificador da pendência deve ser informado.");
        }
    }
}