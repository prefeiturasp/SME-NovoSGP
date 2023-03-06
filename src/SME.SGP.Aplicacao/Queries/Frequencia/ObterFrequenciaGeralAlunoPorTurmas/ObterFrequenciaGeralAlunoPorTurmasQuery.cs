using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoPorTurmasQuery : IRequest<string>
    {
        public ObterFrequenciaGeralAlunoPorTurmasQuery(string codigoAluno, string[] codigosTurmas, string[] codigosDisciplinasTurma, long tipoCalendarioId = 0, IEnumerable<(DateTime? dataMatricula, DateTime? dataSituacaoAluno, bool inativo)> matriculasAlunoNaTurma = null)
        {
            CodigoAluno = codigoAluno;
            CodigosTurmas = codigosTurmas;
            TipoCalendarioId = tipoCalendarioId;
            MatriculasAlunoNaTurma = matriculasAlunoNaTurma;
            CodigosDisciplinasTurma = codigosDisciplinasTurma;
        }

        public string CodigoAluno { get; }
        public string[] CodigosTurmas { get; }
        public long TipoCalendarioId { get; }
        public string[] CodigosDisciplinasTurma { get; set; }
        public IEnumerable<(DateTime? dataMatricula, DateTime? dataSituacao, bool inativo)> MatriculasAlunoNaTurma { get; set; }        
    }

    public class ObterFrequenciaGeralAlunoPorTurmasQueryValidator : AbstractValidator<ObterFrequenciaGeralAlunoPorTurmasQuery>
    {
        public ObterFrequenciaGeralAlunoPorTurmasQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta de sua frequêncial anual");

            RuleFor(a => a.CodigosTurmas)
                .NotEmpty()
                .WithMessage("Os códigos de turmas devem ser informados para consulta da frequêncial anual do aluno");
        }
    }
}
