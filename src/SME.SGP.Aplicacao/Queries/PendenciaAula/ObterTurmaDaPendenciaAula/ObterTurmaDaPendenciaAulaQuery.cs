using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaAulaQuery : IRequest<Turma>
    {
        public ObterTurmaDaPendenciaAulaQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ObterTurmaDaPendenciaQueryValidator : AbstractValidator<ObterTurmaDaPendenciaAulaQuery>
    {
        public ObterTurmaDaPendenciaQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendencia deve ser informado para consulta de sua turma.");
        }
    }
}
