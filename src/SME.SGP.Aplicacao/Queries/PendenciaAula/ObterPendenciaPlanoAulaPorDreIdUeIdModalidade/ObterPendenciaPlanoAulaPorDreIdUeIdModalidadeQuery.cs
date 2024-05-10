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
    public class ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQuery : IRequest<IEnumerable<long>>
    {
        public long DreId { get; set; }
        public long UeId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
        public Modalidade Modalidade { get; set; }

        public ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQuery(long dreId, long ueId, TipoPendencia tipoPendencia, Modalidade modalidade)
        {
            DreId = dreId;
            UeId = ueId;
            TipoPendencia = tipoPendencia;
            Modalidade = modalidade;
        }
    }

    public class ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQueryValidator : AbstractValidator<ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQuery>
    {
        public ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQueryValidator()
        {
            RuleFor(a => a.DreId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da DRE para obter as pendências do plano de aula");

            RuleFor(a => a.UeId)
               .NotEmpty()
               .WithMessage("É necessário informar o id da UE para obter as pendências do plano de aula");

            RuleFor(a => a.TipoPendencia)
               .NotEmpty()
               .WithMessage("É necessário informar o tipo de pendência para obter as pendências do plano de aula");

            RuleFor(a => a.Modalidade)
               .NotEmpty()
               .WithMessage("É necessário informar a modalidade para obter as pendências do plano de aula");
        }
    }
}
