using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQuery : IRequest<IEnumerable<long>>
    {
        public ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQuery(long[] compensacaoAusenciaIds)
        {
            CompensacaoAusenciaIds = compensacaoAusenciaIds;
        }

        public long[] CompensacaoAusenciaIds { get; set; }   
    }

    public class ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQueryValidator : AbstractValidator<ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQuery>
    {
        public ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQueryValidator()
        {
            RuleFor(x => x.CompensacaoAusenciaIds)
                .NotEmpty()
                .WithMessage("Os ids das compensações de ausências devem ser informados para realizar a consulta de compensação sem aluno e aula");
        }
    }
}