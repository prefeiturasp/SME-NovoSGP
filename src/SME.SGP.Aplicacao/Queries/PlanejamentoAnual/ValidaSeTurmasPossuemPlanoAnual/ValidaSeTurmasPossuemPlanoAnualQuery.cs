using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ValidaSeTurmasPossuemPlanoAnualQuery : IRequest<IEnumerable<TurmaParaCopiaPlanoAnualDto>>
    {
        public ValidaSeTurmasPossuemPlanoAnualQuery(string[] turmasId, bool consideraHistorico)
        {
            this.turmasId = turmasId;
            ConsideraHistorico = consideraHistorico;
        }
        public string[] turmasId { get; set; }
        public bool ConsideraHistorico { get; set; }
    }

    public class ValidaSeTurmasPossuemPlanoAnualQueryValidator : AbstractValidator<ValidaSeTurmasPossuemPlanoAnualQuery>
    {
        public ValidaSeTurmasPossuemPlanoAnualQueryValidator()
        {
            RuleFor(c => c.turmasId)
                .NotEmpty()
                .WithMessage("As turmas devem ser informadas.");
        }
    }
}
