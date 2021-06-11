using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoEncerramentoEncaminhamentoAEECommandHandler : IRequestHandler<NotificacaoEncerramentoEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public NotificacaoEncerramentoEncaminhamentoAEECommandHandler(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(NotificacaoEncerramentoEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {

            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(request.EncaminhamentoAEEId));
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId));

            var usuario = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(encaminhamentoAEE.CriadoRF));

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.InfantilPreEscola ? "da criança" : "do estudante";

            var titulo = $"Encaminhamento AEE - {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) - {ueDre}";
            var mensagem = $"O usuário {request.UsuarioNome} ({request.UsuarioRF}) <b>encerrou</b> o encaminhamento {estudanteOuCrianca} {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) " +
                $"da turma {turma.ModalidadeCodigo.ShortName()}-{turma.Nome} da {ueDre}. Motivo: {encaminhamentoAEE.MotivoEncerramento}. <br/><br/>" +
                $"<a href='{hostAplicacao}aee/encaminhamento/editar/{encaminhamentoAEE.Id}'>Clique aqui para acessar o encaminhamento.</a>";




            await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo,
                                                                     mensagem,
                                                                     NotificacaoCategoria.Alerta,
                                                                     NotificacaoTipo.AEE,
                                                                     new List<long> { usuario })); ;

            return true;
        }

    }
}
