using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPorAlunosTurmaQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterFrequenciasPorAlunosTurmaQuery(IEnumerable<string> alunosCodigo, IEnumerable<long?> periodosEscolaresId, string turmaId)
        {
            AlunosCodigo = alunosCodigo;
            PeriodosEscolaresId = periodosEscolaresId;
            TurmaId = turmaId;
        }

        public IEnumerable<string> AlunosCodigo { get; set; }
        public IEnumerable<long?> PeriodosEscolaresId { get; set; }
        public string TurmaId { get; set; }
    }

    public class ObterFrequenciasPorAlunosTurmaQueryValidator : AbstractValidator<ObterFrequenciasPorAlunosTurmaQuery>
    {
        public ObterFrequenciasPorAlunosTurmaQueryValidator()
        {
            RuleFor(a => a.AlunosCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código dos alunos para consulta de suas frequências");

            RuleFor(a => a.PeriodosEscolaresId)
                .NotEmpty()
                .WithMessage("Necessário informar os períodos escolares para consulta de frequências dos alunos");

            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("Necessário informar a turma para consulta de frequências dos alunos");
        }
    }
}
