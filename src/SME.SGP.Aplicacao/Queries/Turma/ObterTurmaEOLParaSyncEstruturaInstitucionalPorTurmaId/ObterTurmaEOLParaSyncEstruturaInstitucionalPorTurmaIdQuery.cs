using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery : IRequest<TurmaParaSyncInstitucionalDto>
    {
        public ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery(long turmaId, string ueCodigo)
        {
            TurmaId = turmaId;
            UeCodigo = ueCodigo;
        }

        public long TurmaId { get; set; }
        public string UeCodigo { get; set; }
    }
    public class ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryValidator : AbstractValidator<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery>
    {
        public ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryValidator()
        {
            RuleFor(c => c.TurmaId)
            .NotEmpty()
            .WithMessage("O id da turma deve ser informado.");

            RuleFor(c => c.UeCodigo)
              .NotEmpty()
              .WithMessage("O código da Ue deve ser informado.");
        }
    }
}
