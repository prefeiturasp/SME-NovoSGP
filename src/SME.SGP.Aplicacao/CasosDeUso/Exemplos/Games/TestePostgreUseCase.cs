using MediatR;
using SME.SGP.Aplicacao.Commands.Aulas;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Exemplos.Games
{
    public interface ITestePostgreUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }

    public class TestePostgreUseCase : ITestePostgreUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public TestePostgreUseCase(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            unitOfWork.IniciarTransacao();
            for (int i = 0; i < 10; i++)
            {
                await mediator.Send(new TestePostgreCommand(Guid.NewGuid()));
            }
            unitOfWork.Rollback();
            return await Task.FromResult(true);
        }
    }
}
