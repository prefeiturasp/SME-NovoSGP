using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaDiarioQuery : IRequest<Turma>
    {
        public ObterTurmaDaPendenciaDiarioQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ObterTurmaDaPendenciaDiarioQueryValidator : AbstractValidator<ObterTurmaDaPendenciaDiarioQuery>
    {
        public ObterTurmaDaPendenciaDiarioQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendencia deve ser informado para consulta de sua turma.");
        }
    }
}
