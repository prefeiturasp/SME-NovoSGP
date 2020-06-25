using MediatR;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Aplicacao.Commands.Aulas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaRecorrenteUseCase : IAlterarAulaRecorrenteUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        private readonly IRepositorioAula repositorioAula;

        public AlterarAulaRecorrenteUseCase(IMediator mediator, IUnitOfWork unitOfWork, IConfiguration configuration, IRepositorioAula repositorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            repositorioAula = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem AlterarAulaRecorrenteUseCase", "Rabbit - AlterarAulaRecorrenteUseCase");

            AlterarAulaRecorrenteCommand filtro = mensagemRabbit.ObterObjetoFiltro<AlterarAulaRecorrenteCommand>();


            unitOfWork.IniciarTransacao();
            for (int i = 0; i < 10; i++)
            {
                await mediator.Send(new TestePostgreCommand(Guid.NewGuid()));
            }

            unitOfWork.PersistirTransacao();
            // TODO chamar command

            return true;
        }
    }
}
