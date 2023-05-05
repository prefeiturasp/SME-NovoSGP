using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacaoEncaminhamentosNAAPAQuery: IRequest<PaginacaoResultadoDto<EncaminhamentoNAAPAObservacoesDto>>
    {
        public ObterObservacaoEncaminhamentosNAAPAQuery(long encaminhamentoNAAPId,long usuarioLogadoId)
        {
            EncaminhamentoNAAPId = encaminhamentoNAAPId;
            UsuarioLogadoId = usuarioLogadoId;
        }

        public long EncaminhamentoNAAPId { get; set; }
        public long UsuarioLogadoId { get; set; }
    }

    public class ObterObservacaoEncaminhamentosNAAPAQueryValidator : AbstractValidator<ObterObservacaoEncaminhamentosNAAPAQuery>
    {
        public ObterObservacaoEncaminhamentosNAAPAQueryValidator()
        {
            RuleFor(x => x.EncaminhamentoNAAPId).GreaterThan(0).WithMessage("Informe o Id do Encaminhamento NAAPA para consultar as Observações");
            RuleFor(x => x.UsuarioLogadoId).GreaterThan(0).WithMessage("Informe o Id do Usuario Logado para consultar as Observações");
        }
    }
}