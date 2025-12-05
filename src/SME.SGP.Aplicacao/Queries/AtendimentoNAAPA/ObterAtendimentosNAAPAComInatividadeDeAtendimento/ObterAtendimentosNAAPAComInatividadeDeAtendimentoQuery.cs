using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery : IRequest<IEnumerable<AtendimentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>>
    {
        public ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }

    public class ObterAtendimentosNAAPAComInatividadeDeAtendimentoQueryValidator : AbstractValidator<ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery>
    {
        public ObterAtendimentosNAAPAComInatividadeDeAtendimentoQueryValidator()
        {
            RuleFor(c => c.UeId)
                .GreaterThan(0)
                .WithMessage("O id da unidade escolar deve ser informado para obter as informações de inatividade do atendimento do naapa.");

        }
    }
}
