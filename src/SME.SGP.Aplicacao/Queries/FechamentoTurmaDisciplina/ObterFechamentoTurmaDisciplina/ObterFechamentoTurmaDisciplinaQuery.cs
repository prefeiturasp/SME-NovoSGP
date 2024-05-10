using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaQuery : IRequest<FechamentoTurmaDisciplina>
    {
        public ObterFechamentoTurmaDisciplinaQuery(string turmaCodigo, long disciplinaId)
        {
            TurmaCodigo = turmaCodigo;
            DisciplinaId = disciplinaId;
        }

        public string TurmaCodigo { get; set; }
        public long DisciplinaId { get; set; }
    }

    public class ObterFechamentoTurmaDisciplinaQueryValidator : AbstractValidator<ObterFechamentoTurmaDisciplinaQuery>
    {
        public ObterFechamentoTurmaDisciplinaQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para consulta dos fechamentos.");

            RuleFor(c => c.DisciplinaId)
               .NotEmpty()
               .WithMessage("Os ids dos componentes curriculares deve ser informado para consulta dos fechamentos.");
        }
    }
}
