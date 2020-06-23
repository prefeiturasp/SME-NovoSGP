using MediatR;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirAulaRecorrenteUseCase : IInserirAulaRecorrenteUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        private readonly IRepositorioAula repositorioAula;

        public InserirAulaRecorrenteUseCase(IMediator mediator, IUnitOfWork unitOfWork, IConfiguration configuration, IRepositorioAula repositorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            repositorioAula = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem InserirAulaRecorrenteUseCase", "Rabbit - InserirAulaRecorrenteUseCase");

            InserirAulaRecorrenteCommand filtro = mensagemRabbit.ObterObjetoFiltro<InserirAulaRecorrenteCommand>();

            // TODO chamar command

            return true;
        }
    }
}
