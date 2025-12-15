using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesApresentacaoNovoEncaminhamentoNAAPAQuery : IRequest<PaginacaoResultadoDto<NovoEncaminhamentoNAAPAHistoricoDeAlteracaoDto>>
    {
        public ObterHistoricosDeAlteracoesApresentacaoNovoEncaminhamentoNAAPAQuery(long encaminhamentoNAAPId)
        {
            EncaminhamentoNAAPId = encaminhamentoNAAPId;
        }

        public long EncaminhamentoNAAPId { get; set; }
    }

    public class ObterHistoricosDeAlteracoesApresentacaoNovoEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterHistoricosDeAlteracoesApresentacaoNovoEncaminhamentoNAAPAQuery>
    {
        public ObterHistoricosDeAlteracoesApresentacaoNovoEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(x => x.EncaminhamentoNAAPId).GreaterThan(0).WithMessage("Informe o Id do Encaminhamento NAAPA para consultar as Observações");
        }
    }
}
