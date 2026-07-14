using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasSemFrequenciaPorTurmaQuery : IRequest<IEnumerable<TotalAulasPorAlunoTurmaDto>>
    {
        public ObterTotalAulasSemFrequenciaPorTurmaQuery(string disciplinaId, string codigoTurma)
        {
            CodigoTurma = codigoTurma;
            DisciplinaId = disciplinaId;
        }
        public string CodigoTurma { get; set; }
        public string DisciplinaId { get; set; }
    }

    public class ObterTotalAulasSemFrequenciaPorTurmaQueryValidator : AbstractValidator<ObterTotalAulasSemFrequenciaPorTurmaQuery>
    {
        public ObterTotalAulasSemFrequenciaPorTurmaQueryValidator()
        {
            RuleFor(x => x.CodigoTurma).NotEmpty().WithMessage("É necessário informar o código da turma para calcular o total de aulas que não registram frequência.");
            RuleFor(x => x.DisciplinaId).NotEmpty().WithMessage("É necessário informar o id da discplina para calcular o total de aulas que não registram frequência.");
        }
    }
}
