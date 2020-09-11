using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarDiarioBordoObservacaoUserCase : INotificarDiarioBordoObservacaoUserCase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public NotificarDiarioBordoObservacaoUserCase(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<NotificarDiarioBordoObservacaoDto>();

            var usuarioId = dadosMensagem.UsuarioId;
            var diarioBordoId = dadosMensagem.DiarioBordoId;
            var diarioBordo = await mediator.Send(new ObterDiarioBordoComAulaPorIdQuery(diarioBordoId));

            var dataAtual = DateTime.Now.ToString("MM/dd/yyyy");

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(turmaCodigo));

            var titulares = await mediator.Send(new ObterProfessoresTitularesDaTurmaQuery(turma.CodigoTurma));

            if (titulares != null)
            {
                var mensagem = new StringBuilder($"O usuário {usuarioLogado.Nome} ({usuarioLogado.CodigoRf}) inseriu uma nova observação no Diário de bordo do dia {dataAtual} da turma <strong>{turma.Nome}</strong> da <strong>{turma.Ue.Nome}</strong> ({turma.Ue.Dre.Abreviacao})");

                var hostAplicacao = configuration["UrlFrontEnd"];
                mensagem.AppendLine($"<br/><br/><a href='{hostAplicacao}diario-classe/diario-bordo'>Clique aqui para visualizar a observação.</a>");

                if (titulares.Count() == 1)
                    titulares = titulares.FirstOrDefault().Split(',');

                foreach (var titular in titulares)
                {
                    var codigoRf = titular.Trim();

                    if (codigoRf != usuarioLogado.CodigoRf)
                    {
                        var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(codigoRf));
                        if (usuario != null)
                            await mediator.Send(new NotificarUsuarioCommand($"Nova observação no Diário de bordo da turma {turma.Nome} ({dataAtual})",
                                                                            mensagem.ToString(),
                                                                            codigoRf,
                                                                            NotificacaoCategoria.Aviso,
                                                                            NotificacaoTipo.Planejamento));
                    }
                }
                return true;
            }
            return false;
        }
    }
}
