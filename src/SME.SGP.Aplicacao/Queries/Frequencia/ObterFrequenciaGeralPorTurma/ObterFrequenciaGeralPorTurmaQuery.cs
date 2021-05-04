using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralPorTurmaQuery : IRequest<IEnumerable<FrequenciaAlunoDto>>
    {
        public ObterFrequenciaGeralPorTurmaQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; }
    }

    public class ObterFrequenciaGeralPorTurmaQueryValidator : AbstractValidator<ObterFrequenciaGeralPorTurmaQuery>
    {
        public ObterFrequenciaGeralPorTurmaQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta da frequência de seus alunos");
        }
    }
}
