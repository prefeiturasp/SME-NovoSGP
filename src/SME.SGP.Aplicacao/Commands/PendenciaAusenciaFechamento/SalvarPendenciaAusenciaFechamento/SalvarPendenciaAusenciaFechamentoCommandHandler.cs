using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAusenciaFechamentoCommandHandler : IRequestHandler<SalvarPendenciaAusenciaFechamentoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPendenciaAusenciaFechamentoCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(SalvarPendenciaAusenciaFechamentoCommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(Dominio.TipoPendencia.AusenciaFechamento,0, request.Mensagem, request.Instrucao, request.Titulo));
                await mediator.Send(new SalvarPendenciaProfessorCommand(pendenciaId, request.TurmaId, request.ComponenteCurricularId, request.ProfessorRf, request.PeriodoEscolarId));
                await GerarPendenciaUsuario(pendenciaId, request.ProfessorRf);

                unitOfWork.PersistirTransacao();
            }
            return true;
        }

        private async Task GerarPendenciaUsuario(long pendenciaId, string professorRf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorRf));
            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
        }
    }
}
