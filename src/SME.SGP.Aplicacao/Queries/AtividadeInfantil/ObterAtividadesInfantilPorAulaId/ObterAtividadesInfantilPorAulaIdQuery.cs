using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{

    public class ObterAtividadesInfantilPorAulaIdQuery : IRequest<IEnumerable<AtividadeInfantilDto>>
    {
        public long AulaId { get; set; }

        public ObterAtividadesInfantilPorAulaIdQuery(long aulaId)
        {
            AulaId=aulaId;
        }
    }

    public class ObterAtividadesInfantilPorAulaIdQueryValidator : AbstractValidator<ObterAtividadesInfantilPorAulaIdQuery>
    {
        public ObterAtividadesInfantilPorAulaIdQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("É necessário informar o identificador da aula para consulta as Atividades do Infantil");
        }
    }
}
