using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorAlunoTurmaQuery : IRequest<IEnumerable<TotalAulasPorAlunoTurmaDto>>
    {
        public ObterTotalAulasPorAlunoTurmaQuery(string disciplinaId, string codigoTurma)
        {
            DisciplinaId = disciplinaId;
            CodigoTurma = codigoTurma;
        }
        public string DisciplinaId { get; set; }
        public string CodigoTurma { get; set; }
    }

    public class ObterTotalAulasPorAlunoTurmaQueryValidator : AbstractValidator<ObterTotalAulasPorAlunoTurmaQuery>
    {
        public ObterTotalAulasPorAlunoTurmaQueryValidator()
        {
            RuleFor(x => x.DisciplinaId).NotEmpty().WithMessage("É necessário informar o ID da disciplina para calcular o seu total de aulas.");
            RuleFor(x => x.CodigoTurma).NotEmpty().WithMessage("É necessário informar o código da turma para calcular o seu total de aulas.");
        }
    }
}
