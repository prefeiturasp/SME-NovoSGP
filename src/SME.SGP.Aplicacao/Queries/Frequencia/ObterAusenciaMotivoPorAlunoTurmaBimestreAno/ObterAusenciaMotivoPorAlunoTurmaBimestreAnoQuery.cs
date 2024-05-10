using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQuery : IRequest<IEnumerable<AusenciaMotivoDto>>
    {
        public ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQuery(string codigoAluno, string turma, short bimestre, short ano)
        {
            CodigoAluno = codigoAluno;
            Turma = turma;
            Bimestre = bimestre;
            AnoLetivo = ano;
        }

        public string CodigoAluno { get; }
        public string Turma { get; }
        public short Bimestre { get; }
        public short AnoLetivo { get; }
    }

    public class ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQueryValidator : AbstractValidator<ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQuery>
    {
        public ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("Necessário código EOL do aluno.");
            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("Necessário código da turma do aluno.");
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("Necessário bimestre do aluno.");
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("Necessário ano letivo do aluno.");
        }
    }
}
