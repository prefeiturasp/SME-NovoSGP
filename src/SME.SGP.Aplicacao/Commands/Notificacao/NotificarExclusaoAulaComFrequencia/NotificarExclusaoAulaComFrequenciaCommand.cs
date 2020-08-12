using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class NotificarExclusaoAulaComFrequenciaCommand : IRequest<bool>
    {
        public NotificarExclusaoAulaComFrequenciaCommand(Turma turma, IEnumerable<DateTime> datasAulas)
        {
            Turma = turma;
            DatasAulas = datasAulas;
        }

        public Turma Turma { get; set; }
        public IEnumerable<DateTime> DatasAulas { get; set; }
    }

    public class NotificarExclusaoAulaComFrequenciaCommandValidator : AbstractValidator<NotificarExclusaoAulaComFrequenciaCommand>
    {
        public NotificarExclusaoAulaComFrequenciaCommandValidator()
        {
            RuleFor(c => c.Turma)
               .NotEmpty()
               .WithMessage("A turma deve ser informada.");

            RuleFor(c => c.Turma.Ue)
                .NotEmpty()
                .WithMessage("A UE deve ser informada.");

            RuleFor(c => c.Turma.Ue.Dre)
                .NotEmpty()
                .WithMessage("A Dre deve ser informada.");

            RuleFor(c => c.DatasAulas)
                .NotEmpty()
                .WithMessage("As datas das aulas devem ser informadas.");
        }
    }
}
