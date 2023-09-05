using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery : IRequest<IEnumerable<EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>>
    {
        public ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }

    public class ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQueryValidator : AbstractValidator<ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery>
    {
        public ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQueryValidator()
        {
            RuleFor(c => c.UeId)
                .GreaterThan(0)
                .WithMessage("O Id da unidade escolar deve ser informada para obter as informações de inatividade de atendimento do naapa.");

        }
    }
}
