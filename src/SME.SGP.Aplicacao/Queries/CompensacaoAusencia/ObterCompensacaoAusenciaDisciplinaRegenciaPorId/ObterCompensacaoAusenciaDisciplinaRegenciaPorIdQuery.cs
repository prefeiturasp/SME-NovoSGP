using System.Collections.Generic;
using System.Data;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQuery: IRequest<IEnumerable<CompensacaoAusenciaDisciplinaRegencia>>
    {
        public ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQuery(long compensacaoId)
        {
            CompensacaoId = compensacaoId;
        }

        public long CompensacaoId { get; set; }
    }

    class ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQueryValidator : AbstractValidator<ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQuery>
    {
        public ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQueryValidator()
        {
            RuleFor(x => x.CompensacaoId).GreaterThan(0).WithMessage("O Id da Compensação de deve ser informado para realizar a consulta");
        }
    }
}