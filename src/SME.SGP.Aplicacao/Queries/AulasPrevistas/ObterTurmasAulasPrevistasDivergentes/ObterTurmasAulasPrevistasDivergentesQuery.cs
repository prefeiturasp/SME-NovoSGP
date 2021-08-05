using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAulasPrevistasDivergentesQuery : IRequest<IEnumerable<RegistroAulaPrevistaDivergenteDto>>
    {
        public ObterTurmasAulasPrevistasDivergentesQuery(int quantidadeDiasBimestreNotificacao)
        {
            QuantidadeDiasBimestreNotificacao = quantidadeDiasBimestreNotificacao;
        }

        public int QuantidadeDiasBimestreNotificacao { get; }
    }

    public class ObterTurmasAulasPrevistasDivergentesQueryValidator : AbstractValidator<ObterTurmasAulasPrevistasDivergentesQuery>
    {
        public ObterTurmasAulasPrevistasDivergentesQueryValidator()
        {
            RuleFor(a => a.QuantidadeDiasBimestreNotificacao)
                .NotEmpty()
                .WithMessage("É necessário informar a quantidade de dias para a notificação de divergência de aulas previstas x dadas");
        }
    }
}
