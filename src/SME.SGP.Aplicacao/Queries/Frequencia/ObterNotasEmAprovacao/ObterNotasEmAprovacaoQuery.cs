using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasEmAprovacaoQuery : IRequest<IEnumerable<NotaEmAprovacaoFechamentoDto>>
    {
        public ObterNotasEmAprovacaoQuery(NotaEmAprovacaoFechamentoDto[] filtros) => Filtros = filtros;
        public NotaEmAprovacaoFechamentoDto[] Filtros { get; }
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
