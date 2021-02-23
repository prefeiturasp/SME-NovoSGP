using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaEncaminhamentoAEECommandHandler : IRequestHandler<GerarPendenciaEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public ExecutarVerificacaoPendenciaAusenciaFechamentoCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(GerarPendenciaEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(Dominio.TipoPendencia.AEE, request.Mensagem, request.Instrucao, request.Titulo));

                await mediator.Send(new SalvarPendenciaProfessorCommand(pendenciaId, request.TurmaId, request.ComponenteCurricularId, request.ProfessorRf, request.PeriodoEscolarId));

                await GerarPendenciaUsuario(pendenciaId, request.ProfessorRf);

                unitOfWork.PersistirTransacao();
            }
            return true;
        }
    }
}
