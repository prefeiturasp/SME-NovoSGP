using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaProfessorQuery : IRequest<Turma>
    {
        public ObterTurmaDaPendenciaProfessorQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ObterTurmaDaPendenciaProfessorQueryValidator : AbstractValidator<ObterTurmaDaPendenciaProfessorQuery>
    {
        public ObterTurmaDaPendenciaProfessorQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
               .Must(a => a > 0)
               .WithMessage("O id da pendência deve ser informado para consulta da turma.");
        }
    }
}
