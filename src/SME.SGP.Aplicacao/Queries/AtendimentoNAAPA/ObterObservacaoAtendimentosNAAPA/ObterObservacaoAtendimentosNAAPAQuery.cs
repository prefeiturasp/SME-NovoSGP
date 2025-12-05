using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacaoAtendimentosNAAPAQuery: IRequest<PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>>
    {
        public ObterObservacaoAtendimentosNAAPAQuery(long encaminhamentoNAAPId,string usuarioLogadoRf)
        {
            EncaminhamentoNAAPId = encaminhamentoNAAPId;
            UsuarioLogadoRf = usuarioLogadoRf;
        }

        public long EncaminhamentoNAAPId { get; set; }
        public string UsuarioLogadoRf { get; set; }
    }

    public class ObterObservacaoAtendimentosNAAPAQueryValidator : AbstractValidator<ObterObservacaoAtendimentosNAAPAQuery>
    {
        public ObterObservacaoAtendimentosNAAPAQueryValidator()
        {
            RuleFor(x => x.EncaminhamentoNAAPId).GreaterThan(0).WithMessage("Informe o Id do Atendimento NAAPA para consultar as Observações");
            RuleFor(x => x.UsuarioLogadoRf).NotEmpty().WithMessage("Informe o Id do Usuário Logado para consultar as Observações");
        }
    }
}