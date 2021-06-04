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
    public class SalvarNotificacaoAulaPrevistraCommandHandler : IRequestHandler<SalvarNotificacaoAulaPrevistaCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNotificacaoAulaPrevista repositorioNotificacaoAulaPrevista;

        public SalvarNotificacaoAulaPrevistraCommandHandler(IMediator mediator, IRepositorioNotificacaoAulaPrevista repositorioNotificacaoAulaPrevista)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNotificacaoAulaPrevista = repositorioNotificacaoAulaPrevista ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAulaPrevista));
        }

        public async Task<bool> Handle(SalvarNotificacaoAulaPrevistaCommand request, CancellationToken cancellationToken)
        {
            var notificacaoId = await mediator.Send(new NotificarUsuarioCommand(
                request.Titulo,
                request.Mensagem,
                request.ProfessorRF,
                NotificacaoCategoria.Alerta,
                NotificacaoTipo.Calendario,
                request.DreCodigo,
                request.UeCodigo,
                request.TurmaCodigo,
                usuarioId: request.UsuarioId));

            var notificacaoCodigo = await mediator.Send(new ObterCodigoNotificacaoPorIdQuery(notificacaoId));

            repositorioNotificacaoAulaPrevista.Salvar(new NotificacaoAulaPrevista()
            {
                Bimestre = request.Bimestre,
                NotificacaoCodigo = notificacaoCodigo,
                TurmaId = request.TurmaCodigo,
                DisciplinaId = request.ComponenteCurricularId
            });

            return true;
        }
    }
}
