using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQuery : IRequest<IEnumerable<FechamentoReaberturaBimestre>>
    {
        public long FechamentoReaberturaId { get; set; }

        public ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQuery(long fechamentoReaberturaId)
        {
            FechamentoReaberturaId = fechamentoReaberturaId;
        }
    }

    public class ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQueryValidator : AbstractValidator<ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQuery>
    {
        public ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQueryValidator()
        {
            RuleFor(a => a.FechamentoReaberturaId)
                .NotEmpty()
                .WithMessage("É necessário informar o id do fechamento reabertura para consultar os bimestres abertos.");
        }
    }
}
