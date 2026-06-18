using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQuery : IRequest<IEnumerable<FrequenciaAlunoTurmaDto>>
    {
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }

        public ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQuery(string turmaCodigo, string alunoCodigo)
        {
            TurmaCodigo = turmaCodigo;
            AlunoCodigo = alunoCodigo;
        }
    }

    public class ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQueryValidator : AbstractValidator<ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQuery>
    {
        public ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("É necessário informar o código da turma para obter os registros de frequência do aluno dessa turma");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("É necessário informar o código do estudante para obter os registros de frequência do estudante dessa turma");

        }
    }
}
