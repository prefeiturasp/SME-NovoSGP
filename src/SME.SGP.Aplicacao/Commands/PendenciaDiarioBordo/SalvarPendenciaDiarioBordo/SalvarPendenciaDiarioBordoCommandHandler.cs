using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaDiarioBordoCommandHandler : AsyncRequestHandler<SalvarPendenciaDiarioBordoCommand>
    {
        private readonly IRepositorioPendenciaDiarioBordo repositorioDiarioBordo;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPendenciaDiarioBordoCommandHandler(IRepositorioPendenciaDiarioBordo repositorioDiarioBordo, IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task Handle(SalvarPendenciaDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            unitOfWork.IniciarTransacao();

            try
            {
                var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.DiarioBordo));
                await SalvarPendenciaDiario(pendenciaId, request.Aulas, request.ProfessoresComponentes);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();

                throw;
            }
        }

        private async Task<bool> SalvarPendenciaDiario(long pendenciaId, IEnumerable<AulaComComponenteDto> aulas, List<ProfessorEComponenteInfantilDto> professoresEComponentes)
        {
            foreach (var aula in aulas)
            {
                var professoresParaGravar = new List<ProfessorEComponenteInfantilDto>();

                if(aula.ComponenteId > 0)
                {
                    foreach (var professor in professoresEComponentes.Distinct())
                        if (!professor.ComponentesCurricularesId.Contains(aula.ComponenteId))
                            professoresParaGravar.Add(professor);
                }
                else
                    professoresParaGravar.AddRange(professoresEComponentes);
                           

                foreach(var professorComPendencia in professoresParaGravar.DistinctBy(p=> p.CodigoRf))
                {
                    var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorComPendencia.CodigoRf));
                    await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));

                    var pendenciaDiarioBordo = new PendenciaDiarioBordo()
                    {
                        PendenciaId = pendenciaId,
                        ProfessorRf = professorComPendencia.CodigoRf,
                        ComponenteId = professorComPendencia.ComponentesCurricularesId.Any()? professorComPendencia.ComponentesCurricularesId.FirstOrDefault() : aula.ComponenteId,
                        AulaId = aula.Id
                    };
                    await repositorioDiarioBordo.SalvarAsync(pendenciaDiarioBordo);
                }
            }
            return true;
        }
    }
}
