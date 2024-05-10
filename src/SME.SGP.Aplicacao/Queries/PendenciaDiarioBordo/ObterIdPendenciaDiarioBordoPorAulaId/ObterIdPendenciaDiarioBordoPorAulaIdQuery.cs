using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdPendenciaDiarioBordoPorAulaIdQuery : IRequest<IEnumerable<PendenciaUsuarioDto>>
    {
        public long AulaId { get; set; }

        public ObterIdPendenciaDiarioBordoPorAulaIdQuery(long aulaId)
        {
            AulaId = aulaId;
        }
    }

    public class ObterIdPendenciaDiarioBordoPorAulaIdQueryValidator : AbstractValidator<ObterIdPendenciaDiarioBordoPorAulaIdQuery>
    {
        public ObterIdPendenciaDiarioBordoPorAulaIdQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da aula para obter o id da pendência do diário de bordo");
        }
    }
}
