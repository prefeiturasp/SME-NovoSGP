using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaPlanoAulaQuery : IRequest<bool>
    {
        public ExistePendenciaPlanoAulaQuery(DateTime data, string turmaId, string disciplinaId, int anoLetivo, short bimestre)
        {
            Data = data;
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
        }

        public DateTime Data { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public int AnoLetivo { get; set; }
        public short Bimestre { get; set; }
    }

    public class ExistePendenciaPlanoAulaQueryValidator : AbstractValidator<ExistePendenciaPlanoAulaQuery>
    {
        public ExistePendenciaPlanoAulaQueryValidator()
        {
            RuleFor(x => x.Data)
             .NotEmpty()
             .WithMessage("A data limite deve ser informada.");

            RuleFor(x => x.TurmaId)
             .NotEmpty()
             .WithMessage("O id da turma deve ser informado.");

            RuleFor(x => x.DisciplinaId)
             .NotEmpty()
             .WithMessage("O id da disciplina deve ser informado.");

            RuleFor(x => x.AnoLetivo)
             .NotEmpty()
             .WithMessage("O ano letivo deve ser informado.");

            RuleFor(x => x.Bimestre)
             .NotEmpty()
             .WithMessage("O bimestre deve ser informado.");
        }
    }
}
