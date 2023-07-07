using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPorAlunosTurmaQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterFrequenciasPorAlunosTurmaQuery(IEnumerable<string> alunosCodigo, long periodoEscolarId, string turmaId, string[] disciplinaIdsConsideradas, string professor = null)
        {
            AlunosCodigo = alunosCodigo;
            PeriodoEscolarId = periodoEscolarId;
            TurmaId = turmaId;
            DisciplinaIdsConsideradas = disciplinaIdsConsideradas;
            Professor = professor;
        }

        public IEnumerable<string> AlunosCodigo { get; set; }
        public long PeriodoEscolarId { get; set; }
        public string TurmaId { get; set; }
        public string[] DisciplinaIdsConsideradas { get; }
        public string Professor { get; set; }
    }

    public class ObterFrequenciasPorAlunosTurmaQueryValidator : AbstractValidator<ObterFrequenciasPorAlunosTurmaQuery>
    {
        public ObterFrequenciasPorAlunosTurmaQueryValidator()
        {
            RuleFor(a => a.AlunosCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código dos alunos para consulta de suas frequências");            

            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("Necessário informar a turma para consulta de frequências dos alunos");

            RuleFor(a => a.DisciplinaIdsConsideradas)
                .NotEmpty()
                .WithMessage("Necessário informar a disciplina para consulta de frequências dos alunos");
        }
    }
}
