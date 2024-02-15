using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaMensalPorTurmaMesAlunoQuery : IRequest<RegistroFrequenciaAlunoPorTurmaEMesDto>
    {
        public ObterFrequenciaMensalPorTurmaMesAlunoQuery(string turmaCodigo, string alunoCodigo, DateTime dataRef, int mes = 0)
        {
            TurmaCodigo = turmaCodigo;
            AlunoCodigo = alunoCodigo;
            DataRef = dataRef;
            Mes = mes;
        }

        public string TurmaCodigo { get; set; }
        public int Mes { get; set; }
        public string AlunoCodigo { get; set; }
        public DateTime DataRef { get; set; }
    }

    public class ObterFrequenciaMensalPorTurmaMesAlunoQueryValidator : AbstractValidator<ObterFrequenciaMensalPorTurmaMesAlunoQuery>
    {
        public ObterFrequenciaMensalPorTurmaMesAlunoQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("A turma precisa ser informada");

            RuleFor(c => c.Mes)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(12)
                .WithMessage("Um mês válido precisa ser informado");

            RuleFor(c => c.AlunoCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O aluno precisa ser informado");

            RuleFor(c => c.DataRef)
                .NotNull()
                .NotEmpty()
                .WithMessage("A data de referência dentro do mês precisa ser informada");
        }
    }
}
