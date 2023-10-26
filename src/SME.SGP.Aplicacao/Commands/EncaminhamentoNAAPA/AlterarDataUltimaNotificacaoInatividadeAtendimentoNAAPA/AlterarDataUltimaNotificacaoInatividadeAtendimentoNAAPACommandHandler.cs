using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarDataUltimaNotificacaoInatividadeAtendimentoNAAPACommandHandler : IRequestHandler<AlterarDataUltimaNotificacaoInatividadeAtendimentoNAAPACommand, bool>
    {
        public readonly IMediator mediator;
        public readonly IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA;

        public AlterarDataUltimaNotificacaoInatividadeAtendimentoNAAPACommandHandler(IMediator mediator, IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(AlterarDataUltimaNotificacaoInatividadeAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var encaminhamento = await mediator.Send(new ObterEncaminhamentoNAAPAPorIdQuery(request.EncaminhamentoId));

            if (encaminhamento.NaoEhNulo()) 
            {
                encaminhamento.DataUltimaNotificacaoSemAtendimento = DateTimeExtension.HorarioBrasilia();
                await repositorioEncaminhamentoNAAPA.SalvarAsync(encaminhamento);
                
                return true;
            }

            return false;
        }
    }
}
