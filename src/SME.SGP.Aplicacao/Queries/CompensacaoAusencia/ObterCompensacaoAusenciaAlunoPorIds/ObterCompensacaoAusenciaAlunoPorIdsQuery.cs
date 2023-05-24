using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using System.Collections.Generic;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoPorIdsQuery : IRequest<IEnumerable<CompensacaoAusenciaAluno>>
    {
        public ObterCompensacaoAusenciaAlunoPorIdsQuery(long[] ids)
        {
            Ids = ids;
        }

        public long[] Ids { get; }
    }

    public class ObterCompensacaoAusenciaAlunoPorIdsQueryValidator : AbstractValidator<ObterCompensacaoAusenciaAlunoPorIdsQuery>
    {
        public ObterCompensacaoAusenciaAlunoPorIdsQueryValidator()
        {
            RuleFor(t => t.Ids)
                .NotEmpty()
                .WithMessage("Os ids das compensações ausência alunos deve ser preenchidos");
        }
    }
}
