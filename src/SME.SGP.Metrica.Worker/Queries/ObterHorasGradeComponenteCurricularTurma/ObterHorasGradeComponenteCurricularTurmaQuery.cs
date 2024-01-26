using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Metrica.Worker.Queries
{
    public class ObterHorasGradeComponenteCurricularTurmaQuery : IRequest<int>
    {
        public ObterHorasGradeComponenteCurricularTurmaQuery(Turma turma, string componenteCurricularCodigo, bool ehRegencia)
        {
            Turma = turma;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            EhRegencia = ehRegencia;
        }

        public Turma Turma { get; }
        public string ComponenteCurricularCodigo { get; }
        public bool EhRegencia { get; }
    }

    public class ObterHorasGradeComponenteCurricularTurmaQueryValidator : AbstractValidator<ObterHorasGradeComponenteCurricularTurmaQuery>
    {
        public ObterHorasGradeComponenteCurricularTurmaQueryValidator()
        {
            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("Necessário informar a Turma.");
            RuleFor(a => a.ComponenteCurricularCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código do Componente Curricular.");
        }
    }
}
