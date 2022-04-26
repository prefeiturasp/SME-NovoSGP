using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAulasPorTipoCommandHandler : AsyncRequestHandler<SalvarPendenciaAulasPorTipoCommand>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPendenciaAulasPorTipoCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task Handle(SalvarPendenciaAulasPorTipoCommand request, CancellationToken cancellationToken)
        {
            var aulasAgrupadas = request.Aulas.GroupBy(x => new { x.TurmaId, x.DisciplinaId });
            foreach (var item in aulasAgrupadas)
            {
                var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(item.First().TipoCalendarioId, item.First().DataAula));

                if (periodoEscolar != null)
                {
                    if (!item.First().AulaCJ)
                    {
                        var modalidadeTurma = await mediator.Send(new ObterModalidadeTurmaPorCodigoQuery(item.First().TurmaId));

                        if (modalidadeTurma != Modalidade.EducacaoInfantil || request.TipoPendenciaAula != TipoPendencia.Frequencia)
                        {
                            var professorTitularTurma = await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(item.First().TurmaId, item.First().DisciplinaId));

                            if (professorTitularTurma != null)
                            {
                                if (periodoEscolar != null)
                                    await SalvarPendenciaAulaUsuario(item.First().DisciplinaId, professorTitularTurma.ProfessorRf, periodoEscolar.Id, request.TipoPendenciaAula, item.Select(x => x.Id));
                            }
                        }
                        else
                        {
                            var professoresTitularesDaTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaQuery(item.First().TurmaId));

                            if (professoresTitularesDaTurma != null)
                            {
                                string[] professoresSeparados = professoresTitularesDaTurma.FirstOrDefault().Split(',');

                                foreach (var professor in professoresSeparados)
                                {
                                    string codigoRfProfessor = professor.Trim();

                                    if (!String.IsNullOrEmpty(codigoRfProfessor))
                                        await SalvarPendenciaAulaUsuario(item.First().DisciplinaId, codigoRfProfessor, periodoEscolar.Id, request.TipoPendenciaAula, item.Select(x => x.Id));
                                }
                            }
                        }
                    }
                    else
                        if (!String.IsNullOrEmpty(item.First().ProfessorRf))
                        await SalvarPendenciaAulaUsuario(item.First().DisciplinaId, item.First().ProfessorRf, periodoEscolar.Id, request.TipoPendenciaAula, item.Select(x => x.Id));
                }
            }
        }

        private async Task SalvarPendenciaAulaUsuario(string disciplinaId, string codigoRfProfessor, long periodoEscolarId, TipoPendencia tipoPendencia, IEnumerable<long> aulasIds)
        {
            long pendenciaIdExistente = await mediator.Send(new ObterPendenciaIdPorComponenteProfessorBimestreQuery(disciplinaId, codigoRfProfessor, periodoEscolarId, tipoPendencia));

            try
            {
                unitOfWork.IniciarTransacao();

                var pendenciaId = pendenciaIdExistente > 0
                    ? pendenciaIdExistente
                    : await mediator.Send(new SalvarPendenciaCommand(tipoPendencia));

                await mediator.Send(new SalvarPendenciasAulasCommand(pendenciaId, aulasIds));
                await SalvarPendenciaUsuario(pendenciaId, codigoRfProfessor);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task SalvarPendenciaUsuario(long pendenciaId, string professorRf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorRf));
            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
        }
    }
}
