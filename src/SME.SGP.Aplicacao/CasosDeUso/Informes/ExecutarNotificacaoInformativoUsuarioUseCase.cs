using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.IO;
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
            unitOfWork.IniciarTransacao();
            try
            {               
                var notificacaoId = await mediator.Send(new NotificarUsuarioCommand(notificacaoInformativoUsuario.Titulo,
                                                                                    notificacaoInformativoUsuario.Mensagem,
                                                                                    notificacaoInformativoUsuario.UsuarioRf,
                                                                                    NotificacaoCategoria.Informe,
                                                                                    NotificacaoTipo.Customizado,
                                                                                    notificacaoInformativoUsuario.DreCodigo,
                                                                                    notificacaoInformativoUsuario.UeCodigo,
                                                                                    string.Empty,
                                                                                    0,
                                                                                    notificacaoInformativoUsuario.InformativoId));

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
    }
}