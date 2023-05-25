using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesRegenciaPorAnoQuery : IRequest<IEnumerable<DisciplinaResposta>>
    {
        public ObterComponentesRegenciaPorAnoQuery(int anoTurma)
        {
            AnoTurma = anoTurma;
        }

        public int AnoTurma { get; set; }
    }

    public class ObterComponentesRegenciaPorAnoQueryValidator : AbstractValidator<ObterComponentesRegenciaPorAnoQuery>
    {
        public ObterComponentesRegenciaPorAnoQueryValidator()
        {
            RuleFor(c => c.AnoTurma)
               .GreaterThanOrEqualTo(0)
               .WithMessage("O ano da turma deve ser informado para a busca de componentes de regência.");
        }
    }
}
