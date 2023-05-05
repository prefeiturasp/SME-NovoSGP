using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery : IRequest<EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        public EncaminhamentoNAAPASecaoDto EncaminhamentoNAAPASecaoAlterado { get; set; }
        public EncaminhamentoNAAPA EncaminhamentoNAAPAExistente { get; set; }

        public ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery(EncaminhamentoNAAPASecaoDto encaminhamentoNAAPASecaoAlterado, EncaminhamentoNAAPA encaminhamentoNAAPAExistente)
        {
            EncaminhamentoNAAPASecaoAlterado = encaminhamentoNAAPASecaoAlterado;
            EncaminhamentoNAAPAExistente = encaminhamentoNAAPAExistente;
        }
    }

    public class ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery>
    {
        public ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPASecaoAlterado).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção alterado deve ser informado");
            RuleFor(c => c.EncaminhamentoNAAPAExistente).NotEmpty().WithMessage("O encaminhamentos NAAPA atual deve ser informado");
        }
    }
}
