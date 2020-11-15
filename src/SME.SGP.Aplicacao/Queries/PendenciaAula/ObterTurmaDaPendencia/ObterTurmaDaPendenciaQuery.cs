using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaQuery : IRequest<Turma>
    {
        public ObterTurmaDaPendenciaQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ObterTurmaDaPendenciaQueryValidator : AbstractValidator<ObterTurmaDaPendenciaQuery>
    {
        public ObterTurmaDaPendenciaQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendencia deve ser informado para consulta de sua turma.");
        }
    }
}
