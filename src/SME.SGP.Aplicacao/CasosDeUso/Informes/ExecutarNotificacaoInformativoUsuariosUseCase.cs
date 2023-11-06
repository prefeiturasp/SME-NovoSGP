using MediatR;
using Minio.DataModel;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarNotificacaoInformativoUsuariosUseCase : AbstractUseCase, IExecutarNotificacaoInformativoUsuariosUseCase
    {
        public ExecutarNotificacaoInformativoUsuariosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var param = mensagem.ObterObjetoMensagem<string>();
            if (string.IsNullOrEmpty(param)) return false;
            var informativoId = long.Parse(param);

            var informativo = await mediator.Send(new ObterInformesPorIdQuery(informativoId));
            if (informativo.EhNulo())
                throw new NegocioException("Não foi possível encontrar o Informativo informado");

            var usuarios = new List<Usuario>() { new Usuario() { Login = "1111"}, new Usuario() { Login = "2222"} }; //await mediator.Send(new ObterResponsaveisPorDreUeNAAPAQuery("", ""));
            foreach (var usuario in usuarios)
            {
                var notificacaoInformativoUsuario = new NotificacaoInformativoUsuarioFiltro()
                {
                    InformativoId = informativoId,
                    UsuarioRf = usuario.Login,
                    Titulo = informativo.Titulo,
                    Mensagem = informativo.Texto,
                    DreCodigo = informativo.Dre?.CodigoDre,
                    UeCodigo = informativo.Ue?.CodigoUe
                };
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoInformativoUsuario, notificacaoInformativoUsuario, Guid.NewGuid(), null));  
            }               
            return true;
        }
    }
}