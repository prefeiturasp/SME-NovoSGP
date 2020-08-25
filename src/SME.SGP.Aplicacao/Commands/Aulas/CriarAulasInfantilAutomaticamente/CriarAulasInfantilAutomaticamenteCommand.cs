using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilAutomaticamenteCommand : IRequest<bool>
    {
        public CriarAulasInfantilAutomaticamenteCommand(IEnumerable<DiaLetivoDto> diasLetivos, IEnumerable<Turma> turmas, long tipoCalendarioId)
        {
            DiasLetivos = diasLetivos;
            Turmas = turmas;
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; set; }
        public IEnumerable<DiaLetivoDto> DiasLetivos { get; set; }
        public IEnumerable<Turma> Turmas { get; set; }
    }

    public class CriarAulasInfantilAutomaticamenteCommandValidator : AbstractValidator<CriarAulasInfantilAutomaticamenteCommand>
    {
        public CriarAulasInfantilAutomaticamenteCommandValidator()
        {
            RuleFor(c => c.Turmas)
                .NotEmpty()
                .WithMessage("As Turmas devem ser informadas.");

            RuleFor(c => c.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado.");

            RuleFor(c => c.DiasLetivos)
              .NotEmpty()
              .WithMessage("Os dias letivos e não letivos devem ser informados.");
        }
    }

}
