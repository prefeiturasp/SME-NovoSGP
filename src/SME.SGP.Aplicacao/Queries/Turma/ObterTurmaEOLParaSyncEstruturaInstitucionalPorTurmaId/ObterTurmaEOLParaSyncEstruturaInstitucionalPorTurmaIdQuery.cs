using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery : IRequest<TurmaParaSyncInstitucionalDto>
    {
        public ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery(string turmaId)
        {
            TurmaId = turmaId;
        }

        public string TurmaId { get; set; }
    }
    public class ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryValidator : AbstractValidator<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery>
    {
        public ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryValidator()
        {
            RuleFor(c => c.TurmaId)
            .NotEmpty()
            .WithMessage("O id da turma deve ser informado.");
        }
    }
}
