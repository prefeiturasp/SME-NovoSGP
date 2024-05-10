using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Aulas.ReaverAulaDiarioBordoExcluida
{
    public class ReaverAulaDiarioBordoExcluidaCommandHandler : IRequestHandler<ReaverAulaDiarioBordoExcluidaCommand, long>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ReaverAulaDiarioBordoExcluidaCommandHandler(IUnitOfWork unitOfWork,
                                                           IRepositorioAula repositorioAula,
                                                           IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<long> Handle(ReaverAulaDiarioBordoExcluidaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                unitOfWork
                    .IniciarTransacao();

                var dataAtual = DateTime.Today.Date;
                var aula = await repositorioAula
                    .ObterPorIdAsync(request.AulaId);

                aula.Excluido = false;
                aula.AlteradoEm = dataAtual;
                aula.AlteradoPor = "Sistema";

                var id = await repositorioAula
                    .SalvarAsync(aula);

                var diarioBordo = await repositorioDiarioBordo
                    .ObterPorIdAsync(request.DiarioBordoId);

                diarioBordo.Excluido = false;
                diarioBordo.AlteradoEm = dataAtual;
                diarioBordo.AlteradoPor = "Sistema";

                await repositorioDiarioBordo
                    .SalvarAsync(diarioBordo);

                unitOfWork
                    .PersistirTransacao();

                return id;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
