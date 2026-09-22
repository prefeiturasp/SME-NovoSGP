using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasEmAprovacaoQuery : IRequest<IEnumerable<NotaEmAprovacaoFechamentoDto>>
    {
        public ObterNotasEmAprovacaoQuery(NotaEmAprovacaoFechamentoDto[] filtros)
            => Filtros = filtros ?? Array.Empty<NotaEmAprovacaoFechamentoDto>();

        public ObterNotasEmAprovacaoQuery(string[] codigosAlunos, long[] turmaFechamentoIds)
        {
            CodigosAlunos = codigosAlunos;
            TurmaFechamentoIds = turmaFechamentoIds;
        }

        public NotaEmAprovacaoFechamentoDto[] Filtros { get; }
        public string[] CodigosAlunos { get; }
        public long[] TurmaFechamentoIds { get; }
    }

    public class ObterNotasEmAprovacaoQueryValidator : AbstractValidator<ObterNotasEmAprovacaoQuery>
    {
        public ObterNotasEmAprovacaoQueryValidator()
        {
            RuleForEach(q => q.Filtros).ChildRules(f =>
            {
                f.RuleFor(x => x.CodigoAluno).NotEmpty();
                f.RuleFor(x => x.TurmaFechamentoId).NotEmpty();
                f.RuleFor(x => x.DisciplinaId).NotEmpty();
            });
        }
    }
}
