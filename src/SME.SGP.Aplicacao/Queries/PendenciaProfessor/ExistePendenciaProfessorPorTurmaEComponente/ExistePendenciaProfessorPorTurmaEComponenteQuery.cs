using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaProfessorPorTurmaEComponenteQuery : IRequest<bool>
    {
        public ExistePendenciaProfessorPorTurmaEComponenteQuery(long turmaId, long componenteCurricularId, string professorRf, TipoPendencia tipoPendencia)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            ProfessorRf = professorRf;
            TipoPendencia = tipoPendencia;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string ProfessorRf { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class ExistePendenciaProfessorPorTurmaEComponenteQueryValidator : AbstractValidator<ExistePendenciaProfessorPorTurmaEComponenteQuery>
    {
        public ExistePendenciaProfessorPorTurmaEComponenteQueryValidator()
        {
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para consulta de pendência do professor.");

            RuleFor(c => c.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O id do componente curricular deve ser informado para consulta de pendência do professor.");

            RuleFor(c => c.ProfessorRf)
               .NotEmpty()
               .WithMessage("O RF do professor deve ser informado para consulta de pendência do professor.");

            RuleFor(c => c.TipoPendencia)
               .Must(a => new[] { TipoPendencia.AusenciaDeAvaliacaoProfessor, TipoPendencia.AusenciaDeAvaliacaoCP, TipoPendencia.AusenciaFechamento }.Contains(a))
               .WithMessage("O tipo de pendencia deve ser um que gere pendência do professor para consulta de sua existência.");
        }
    }
}
