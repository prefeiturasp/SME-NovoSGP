using MediatR;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Aula;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaWorkerUseCase : IExcluirAulaWorkerUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        private readonly IRepositorioAula repositorioAula;
        private readonly IServicoAula servicoAula;
        private readonly IServicoUsuario servicoUsuario;

        public ExcluirAulaWorkerUseCase(IMediator mediator, IUnitOfWork unitOfWork, IConfiguration configuration,
                            IRepositorioAula repositorio,
                            IServicoUsuario servicoUsuario,
                            IServicoAula servicoAula)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            this.repositorioAula = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoAula = servicoAula ?? throw new ArgumentNullException(nameof(servicoAula));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            using (SentrySdk.Init(configuration.GetValue<string>("Sentry:DSN")))
            {
                //var command = new ComandosAula(repositorioAula, servicoAula, servicoUsuario);

                SentrySdk.AddBreadcrumb($"Mensagem ExcluirAulaWorkerUseCase", "Rabbit - ExcluirAulaWorkerUseCase");
            }


            return await Task.FromResult(true);
        }
    }
}
