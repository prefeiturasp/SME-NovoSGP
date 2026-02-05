using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries.Informes.VerificaSeExisteNotificacaoInformePorIdUsuarioRf;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarNotificacaoInformativoUsuarioUseCase : AbstractUseCase, IExecutarNotificacaoInformativoUsuarioUseCase
    {
        private readonly IUnitOfWork unitOfWork;

        public ExecutarNotificacaoInformativoUsuarioUseCase(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var notificacaoInformativoUsuario = param.ObterObjetoMensagem<NotificacaoInformativoUsuarioFiltro>();
            var informeExcluido = await mediator.Send(new InformeFoiExcluidoQuery(notificacaoInformativoUsuario.InformativoId));
            
            if (informeExcluido)
                return false;

            var existeNotificacaoUsuario = await mediator.Send(new VerificaSeExisteNotificacaoInformePorIdUsuarioRfQuery(notificacaoInformativoUsuario.InformativoId, notificacaoInformativoUsuario.UsuarioRf));
            if (existeNotificacaoUsuario)
                return false;

            unitOfWork.IniciarTransacao();
            try
            {
                var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(notificacaoInformativoUsuario.UsuarioRf));
                var notificacaoId = await mediator.Send(new NotificarUsuarioCommand(notificacaoInformativoUsuario.Titulo,
                                                                                    notificacaoInformativoUsuario.Mensagem,
                                                                                    notificacaoInformativoUsuario.UsuarioRf,
                                                                                    NotificacaoCategoria.Informe,
                                                                                    NotificacaoTipo.Customizado,
                                                                                    notificacaoInformativoUsuario.DreCodigo,
                                                                                    notificacaoInformativoUsuario.UeCodigo,
                                                                                    string.Empty,
                                                                                    0,
                                                                                    ObterCodigoNotificacao(notificacaoInformativoUsuario.InformativoId, usuarioId),
                                                                                    null,
                                                                                    string.Empty,
                                                                                    usuarioId));

                await mediator.Send(new SalvarInformativoNotificacaoCommand(notificacaoInformativoUsuario.InformativoId, notificacaoId));
                unitOfWork.PersistirTransacao();
                return true;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private long ObterCodigoNotificacao(long informativoId, long usuarioId)
        {
            return long.Parse(string.Concat(informativoId, usuarioId.ToString("000000")));
        }
    }
}