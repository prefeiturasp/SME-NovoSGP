using FluentValidation;
using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasPorCodigoTurmaQuery : IRequest<IEnumerable<DisciplinaResposta>>
    {
        public ObterDisciplinasPorCodigoTurmaQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }
        public string CodigoTurma { get; set; }
    }

    public class ObterDisciplinasPorCodigoTurmaQueryValidator : AbstractValidator<ObterDisciplinasPorCodigoTurmaQuery>
    {
        public ObterDisciplinasPorCodigoTurmaQueryValidator()
        {
            RuleFor(a => a.CodigoTurma)
               .NotEmpty()
               .WithMessage("É preciso informar o código da turma para consultar as disciplinas");
        }
    }
}
