using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public string[] AlunosCodigos { get; set; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }

        public ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery(string[] alunosCodigos, string turmaCodigo, string componenteCurricularCodigo = "")
        {
            AlunosCodigos = alunosCodigos;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }
    }

    public class ObterFrequenciaGeralPorAlunosTurmaEComponenteQueryValidator : AbstractValidator<ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery>
    {
        public ObterFrequenciaGeralPorAlunosTurmaEComponenteQueryValidator()
        {
            RuleFor(a => a.AlunosCodigos)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta da frequência");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta da frequência de seus alunos");
        }
    }
}
