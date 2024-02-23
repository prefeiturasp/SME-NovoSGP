using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterItensDeImprimirAnexosNAAPAQuery : IRequest<IEnumerable<ImprimirAnexoDto>>
    {
        public ObterItensDeImprimirAnexosNAAPAQuery(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; set; }
    }

    public class ObterItensDeImprimirAnexosNAAPAQueryValidator : AbstractValidator<ObterItensDeImprimirAnexosNAAPAQuery>
    {
        public ObterItensDeImprimirAnexosNAAPAQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoId)
                .NotEmpty()
                .WithMessage("O id do encaminhamento NAAPA deve ser informado para obter itens da impressão de anexo");
        }
    }
}
