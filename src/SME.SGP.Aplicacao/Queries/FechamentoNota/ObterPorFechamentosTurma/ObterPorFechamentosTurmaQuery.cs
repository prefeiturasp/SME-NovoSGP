using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPorFechamentosTurmaQuery : IRequest<IEnumerable<FechamentoNotaAlunoAprovacaoDto>>
    {
        public long[] Ids { get; set; }
        public ObterPorFechamentosTurmaQuery(long [] ids)
        {
            Ids = ids;
        }
    }

    public class ObterPorFechamentosTurmaQueryValidator : AbstractValidator<ObterPorFechamentosTurmaQuery>
    {
        public ObterPorFechamentosTurmaQueryValidator()
        {
            RuleFor(a => a.Ids)
                .NotEmpty()
                .WithMessage("É necessário informar os Id(s) para obter o(s) fechamento(s) da turma");
        }
    }
}
