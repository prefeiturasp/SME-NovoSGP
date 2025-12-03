using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQuery : IRequest<PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>>
    {
        public ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQuery(long encaminhamentoNAAPId)
        {
            EncaminhamentoNAAPId = encaminhamentoNAAPId;
        }

        public long EncaminhamentoNAAPId { get; set; }
    }

    public class ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQuery>
    {
        public ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(x => x.EncaminhamentoNAAPId).GreaterThan(0).WithMessage("Informe o Id do Encaminhamento NAAPA para consultar as Observações");
        }
    }
}
