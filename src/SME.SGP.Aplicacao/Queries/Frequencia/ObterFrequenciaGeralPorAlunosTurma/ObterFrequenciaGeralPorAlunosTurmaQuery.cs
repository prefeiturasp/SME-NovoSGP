using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SME.SGP.Dominio;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralPorAlunosTurmaQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterFrequenciaGeralPorAlunosTurmaQuery(string[] codigosAlunos, string codigoTurma)
        {
            CodigosAlunos = codigosAlunos;
            CodigoTurma = codigoTurma;
        }

        public string[] CodigosAlunos { get; set; }
        public string CodigoTurma { get; set; }
    }
    public class ObterFrequenciaGeralPorAlunosTurmaQueryValidator : AbstractValidator<ObterFrequenciaGeralPorAlunosTurmaQuery>
    {
        public ObterFrequenciaGeralPorAlunosTurmaQueryValidator()
        {
            RuleFor(a => a.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta da frequência de seus alunos");

            RuleFor(a => a.CodigosAlunos)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta da frequência");
        }
    }
}
