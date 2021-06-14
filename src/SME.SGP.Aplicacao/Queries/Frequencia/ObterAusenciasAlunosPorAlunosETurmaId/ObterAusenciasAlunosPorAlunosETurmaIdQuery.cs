﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciasAlunosPorAlunosETurmaIdQuery : IRequest<IEnumerable<AusenciaPorDisciplinaAlunoDto>>
    {
        public ObterAusenciasAlunosPorAlunosETurmaIdQuery(DateTime dataAula, IEnumerable<string> alunos, string turmaId)
        {
            DataAula = dataAula;
            Alunos = alunos;
            TurmaId = turmaId;
        }

        public DateTime DataAula { get; set; }
        public IEnumerable<string> Alunos { get; set; }
        public string TurmaId { get; set; }
    }

    public class ObterAusenciasAlunosPorAlunosETurmaIdQueryValidator : AbstractValidator<ObterAusenciasAlunosPorAlunosETurmaIdQuery>
    {
        public ObterAusenciasAlunosPorAlunosETurmaIdQueryValidator()
        {
            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula precisa ser informada");
            RuleFor(a => a.Alunos)
                .NotEmpty()
                .WithMessage("Os alunos precisam ser informados");
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("A turma precisa ser informada");
        }
    }
}
