using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotificacaoDevolutivaUseCase : ISalvarNotificacaoDevolutivaUseCase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private readonly IRepositorioNotificacaoDevolutiva repositorioNotificacaoDevolutiva;
        private readonly IServicoNotificacao servicoNotificacao;

        public SalvarNotificacaoDevolutivaUseCase(IMediator mediator, IConfiguration configuration, IServicoNotificacao servicoNotificacao,
            IRepositorioNotificacaoDevolutiva repositorioNotificacaoDevolutiva)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioNotificacaoDevolutiva = repositorioNotificacaoDevolutiva ?? throw new ArgumentNullException(nameof(repositorioNotificacaoDevolutiva));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<SalvarNotificacaoDevolutivaDto>();

            var turma = dadosMensagem.Turma;
            var usuarioLogado = dadosMensagem.Usuario;
            var devolutivaId = dadosMensagem.DevolutivaId;

            var titulares = await mediator.Send(new ObterProfessoresTitularesDaTurmaQuery(turma.CodigoTurma));
            var devolutiva = await mediator.Send(new ObterDevolutivaPorIdQuery(devolutivaId));

            if (titulares != null)
            {
                var mensagem = new StringBuilder($"O usuário {usuarioLogado.Nome} ({usuarioLogado.CodigoRf}) registrou a devolutiva dos diários de bordo da turma <strong>{turma.Nome}</strong> da <strong>{turma.Ue.TipoEscola}-{turma.Ue.Nome}</strong> ({turma.Ue.Dre.Abreviacao}). Esta devolutiva contempla os diários de bordo do período de <strong>{devolutiva.PeriodoInicio:dd/MM/yyyy}</strong> à <strong>{devolutiva.PeriodoFim:dd/MM/yyyy}</strong>.");
                                
                mensagem.AppendLine($"<br/><br/>Acesse o Diário de Bordo de uma das aulas para consultar o conteúdo da devolutiva.");

                if (titulares.Count() == 1)
                    titulares = titulares.FirstOrDefault().Split(',');

                foreach (var titular in titulares)
                {
                    var codigoRf = titular.Trim();

                    if (codigoRf != usuarioLogado.CodigoRf)
                    {
                        var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(codigoRf));
                        if (usuario != null)
                        {
                            var notificacao = new Notificacao()
                            {
                                Ano = DateTime.Now.Year,
                                Categoria = NotificacaoCategoria.Aviso,
                                Tipo = NotificacaoTipo.Planejamento,
                                Titulo = $"Devolutiva do Diário de bordo da turma {turma.Nome}",
                                Mensagem = mensagem.ToString(),
                                UsuarioId = usuario.Id,
                                TurmaId = "",
                                UeId = "",
                                DreId = "",
                            };

                            await servicoNotificacao.SalvarAsync(notificacao);

                            var notificacaoDevolutiva = new NotificacaoDevolutiva()
                            {
                                NotificacaoId = notificacao.Id,
                                DevolutivaId = devolutivaId
                            };

                            await repositorioNotificacaoDevolutiva.Salvar(notificacaoDevolutiva);

                        }

                    }
                }
                return true;
            }
            return false;
        }
    }
}
