using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Metrica.Worker.Queries
{
    public class ObterHorasCadastradasComponenteCurricularTurmaQuery : IRequest<int>
    {
        public ObterHorasCadastradasComponenteCurricularTurmaQuery(Turma turma, string componenteCurricularCodigo, bool ehRegencia, DateTime dataAula)
        {
            Turma = turma;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            EhRegencia = ehRegencia;
            DataAula = dataAula;
        }

        public Turma Turma { get; }
        public string ComponenteCurricularCodigo { get; }
        public bool EhRegencia { get; }
        public DateTime DataAula { get; }
    }

    public class ObterHorasCadastradasComponenteCurricularTurmaQueryValidator : AbstractValidator<ObterHorasCadastradasComponenteCurricularTurmaQuery>
    {
        public ObterHorasCadastradasComponenteCurricularTurmaQueryValidator()
        {
            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("Necessário informar a Turma.");
            RuleFor(a => a.ComponenteCurricularCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código do Componente Curricular.");
            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("Necessário informar a Data da Aula.");
        }
    }
}
