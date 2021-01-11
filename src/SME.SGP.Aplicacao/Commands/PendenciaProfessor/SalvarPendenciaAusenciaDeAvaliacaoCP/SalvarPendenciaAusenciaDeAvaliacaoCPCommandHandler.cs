using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
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
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var pendenciaProfessor in request.PendenciasProfessores)
                {
                    await mediator.Send(new SalvarPendenciaProfessorCommand(request.PendenciaId, request.TurmaId, pendenciaProfessor.componenteCurricularId, pendenciaProfessor.professorRf, request.PeriodoEscolarId));
                }
                await GerarPendenciaUsuario(request.PendenciaId, request.UeCodigo);

                unitOfWork.PersistirTransacao();
            }
            return true;
        }

        private async Task GerarPendenciaUsuario(long pendenciaId, string codigoUe)
        {
            var usuariosIds = await mediator.Send(new ObterFuncionariosIdPorCodigoUeECargoQuery(codigoUe, Cargo.CP));

            foreach(var usuarioId in usuariosIds)
            {
                await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
            }
        }
    }
}
