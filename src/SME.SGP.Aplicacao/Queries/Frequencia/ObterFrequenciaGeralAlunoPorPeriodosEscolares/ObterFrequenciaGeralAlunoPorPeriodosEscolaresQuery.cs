using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoPorPeriodosEscolaresQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterFrequenciaGeralAlunoPorPeriodosEscolaresQuery(string codigoAluno, string codigoTurma, long[] idsPeriodosEscolares)
        {
            CodigoAluno = codigoAluno;
            CodigoTurma = codigoTurma;
            IdsPeriodosEscolares = idsPeriodosEscolares;
        }

        public string CodigoAluno { get; set; }
        public string CodigoTurma { get; set; }
        public long[] IdsPeriodosEscolares { get; set; }
    }

    public class ObterFrequenciaGeralAlunoPorPeriodosEscolaresQueryValidator : AbstractValidator<ObterFrequenciaGeralAlunoPorPeriodosEscolaresQuery>
    {
        public ObterFrequenciaGeralAlunoPorPeriodosEscolaresQueryValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para frequência geral por períodos.");

            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para frequência geral por períodos.");

            RuleFor(x => x.IdsPeriodosEscolares)
                .NotNull()
                .WithMessage("Os ids dos períodos escolares não foram informados para frequência geral por períodos.");
        }
    }
}