using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAusenciaDeAvaliacaoCPCommandHandler : IRequestHandler<SalvarPendenciaAusenciaDeAvaliacaoCPCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPendenciaAusenciaDeAvaliacaoCPCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(SalvarPendenciaAusenciaDeAvaliacaoCPCommand request, CancellationToken cancellationToken)
        {
            foreach (var pendenciaProfessor in request.PendenciasProfessores)
            {
                await mediator.Send(new SalvarPendenciaProfessorCommand(request.PendenciaId, request.TurmaId, pendenciaProfessor.componenteCurricularId, pendenciaProfessor.professorRf, request.PeriodoEscolarId));
            }
            await GerarPendenciaPerfil(request.PendenciaId, request.UeId);

            return true;
        }

        private async Task GerarPendenciaPerfil(long pendenciaId, long ueId)
        {
            await mediator.Send(new SalvarPendenciaPerfilCommand(pendenciaId, new List<PerfilUsuario>() { PerfilUsuario.CP }));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaTratarAtribuicaoPendenciaUsuarios,
                                                           new FiltroTratamentoAtribuicaoPendenciaDto(pendenciaId, ueId)));
        }
    }
}
