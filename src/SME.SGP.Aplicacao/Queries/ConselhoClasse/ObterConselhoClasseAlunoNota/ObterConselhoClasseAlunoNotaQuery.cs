using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunoNotaQuery : IRequest<IEnumerable<ConselhoClasseAlunoNotaDto>>
    {
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }

    public class ObterConselhoClasseAlunoNotaQueryValidator : AbstractValidator<ObterConselhoClasseAlunoNotaQuery>
    {
        public ObterConselhoClasseAlunoNotaQueryValidator()
        {
            RuleFor(x => x.Bimestre).GreaterThan(0).WithMessage("'Informe um bimestre para realizar a consulta'");
            RuleFor(x => x.TurmaId).GreaterThan(0).WithMessage("'Informe o Id da turma para  realizar a consulta'");
        }
    }
}