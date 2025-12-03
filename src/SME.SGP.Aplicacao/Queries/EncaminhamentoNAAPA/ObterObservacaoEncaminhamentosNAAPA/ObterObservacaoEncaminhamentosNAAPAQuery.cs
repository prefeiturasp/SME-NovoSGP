using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacaoEncaminhamentosNAAPAQuery: IRequest<PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>>
    {
        public ObterObservacaoEncaminhamentosNAAPAQuery(long encaminhamentoNAAPId,string usuarioLogadoRf)
        {
            EncaminhamentoNAAPId = encaminhamentoNAAPId;
            UsuarioLogadoRf = usuarioLogadoRf;
        }

        public long EncaminhamentoNAAPId { get; set; }
        public string UsuarioLogadoRf { get; set; }
    }

    public class ObterObservacaoEncaminhamentosNAAPAQueryValidator : AbstractValidator<ObterObservacaoEncaminhamentosNAAPAQuery>
    {
        public ObterObservacaoEncaminhamentosNAAPAQueryValidator()
        {
            RuleFor(x => x.EncaminhamentoNAAPId).GreaterThan(0).WithMessage("Informe o Id do Encaminhamento NAAPA para consultar as Observações");
            RuleFor(x => x.UsuarioLogadoRf).NotEmpty().WithMessage("Informe o Id do Usuario Logado para consultar as Observações");
        }
    }
}