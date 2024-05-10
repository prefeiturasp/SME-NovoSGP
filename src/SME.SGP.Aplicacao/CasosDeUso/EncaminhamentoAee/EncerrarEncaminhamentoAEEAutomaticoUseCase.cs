using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarEncaminhamentoAEEAutomaticoUseCase : AbstractUseCase, IEncerrarEncaminhamentoAEEAutomaticoUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EncerrarEncaminhamentoAEEAutomaticoUseCase(IMediator mediator,
            IUnitOfWork unitOfWork) : base(mediator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto>();

            _unitOfWork.IniciarTransacao();
            try
            {
                await mediator.Send(new AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand(filtro.EncaminhamentoId));

                var pendenciasEncaminhamentoAEE = await mediator.Send(new ObterPendenciasDoEncaminhamentoAEEPorIdQuery(filtro.EncaminhamentoId));

                if (pendenciasEncaminhamentoAEE.NaoEhNulo())
                {
                    foreach (var pendenciaEncaminhamentoAEE in pendenciasEncaminhamentoAEE)
                        await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendenciaEncaminhamentoAEE.PendenciaId));
                }

                _unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }

            return true;
        }
    }
}
