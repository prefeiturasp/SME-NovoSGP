using MediatR;
using SME.SGP.Dominio;
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
            var aulasAgrupadas = request.Aulas.GroupBy(x => new { x.TurmaId, x.DisciplinaId });
            foreach (var item in aulasAgrupadas)
            {
                unitOfWork.IniciarTransacao();

                var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.DiarioBordo));
                await SalvarPendenciaDiario(pendenciaId, item);

                unitOfWork.PersistirTransacao();
            }
        }


        private async Task<bool> SalvarPendenciaDiario(long pendenciaId, IEnumerable<Aula> aulas)
        {
            var professor = await mediator.Send(new ObterProfessorDaTurmaPorAulaIdQuery(aulas.First().Id));
            var aulaExemplo = aulas.First();
            Guid perfilProfessorInfantil = Guid.Parse(PerfilUsuario.PROFESSOR_INFANTIL.ObterNome());
            var componenteProfessorEol = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(aulaExemplo.TurmaId, professor.CodigoRf, perfilProfessorInfantil));
            try
            {
                foreach (var aula in aulas)
                {
                    var pendenciaDiarioBordo = new PendenciaDiarioBordo()
                    {
                        PendenciaId = pendenciaId,
                        ProfessorRf = professor.CodigoRf,
                        ComponenteId = componenteProfessorEol != null ? componenteProfessorEol.First().Codigo : Convert.ToInt64(aula.DisciplinaId),
                        AulaId = aula.Id
                    };
                    await repositorioDiarioBordo.SalvarAsync(pendenciaDiarioBordo);
                }
            }
            catch (Exception ex)
            {

            }

            return true;
        }
    }
}
