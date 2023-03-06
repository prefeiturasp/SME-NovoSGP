using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosEolPorTurmaQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterAlunosEolPorTurmaQuery(string turmaId, bool consideraInativos = false)
        {
            TurmaId = turmaId;
            ConsideraInativos = consideraInativos;
        }

        public bool ConsideraInativos { get; set; }

        public string TurmaId { get; set; }
    }

    public class ObterAlunosEolPorTurmaQueryValidator : AbstractValidator<ObterAlunosEolPorTurmaQuery>
    {
        public ObterAlunosEolPorTurmaQueryValidator()
        {
            RuleFor(x => x.TurmaId).NotEmpty().WithMessage("Informe a Turma para Obter Alunos Por Turma");
        }
    }
}