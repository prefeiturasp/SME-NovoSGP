using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ExisteEncaminhamentoNAAPAAtivo
{
    public class ExisteEncaminhamentoNAAPAAtivoQuery : IRequest<bool>
    {
        public ExisteEncaminhamentoNAAPAAtivoQuery(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; }
    }

    public class ExisteEncaminhamentoNAAPAAtivoQueryValidator : AbstractValidator<ExisteEncaminhamentoNAAPAAtivoQuery>
    {
        public ExisteEncaminhamentoNAAPAAtivoQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoId)
                .GreaterThan(0)
                .WithMessage("O Id do encaminhamento NAAPA deve ser informado para a verificação de existência.");
        }
    }
}