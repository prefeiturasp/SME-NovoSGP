using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
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
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IRepositorioTurmaConsulta repositorioTurma;


        public SalvarNotificacaoDevolutivaUseCase(IMediator mediator, IConfiguration configuration, IServicoNotificacao servicoNotificacao,
            IRepositorioNotificacaoDevolutiva repositorioNotificacaoDevolutiva, IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, IRepositorioTurmaConsulta repositorioTurma)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioNotificacaoDevolutiva = repositorioNotificacaoDevolutiva ?? throw new ArgumentNullException(nameof(repositorioNotificacaoDevolutiva));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<SalvarNotificacaoDevolutivaDto>();

            var turma = dadosMensagem.Turma;
            var usuarioLogado = dadosMensagem.Usuario;
            var devolutivaId = dadosMensagem.DevolutivaId;

            var devolutiva = await mediator.Send(new ObterDevolutivaPorIdQuery(devolutivaId));
            var titularEol = await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(turma.CodigoTurma, devolutiva.CodigoComponenteCurricular.ToString()));
            var componenteCurricular = await repositorioComponenteCurricular.ObterDisciplinaPorId(devolutiva.CodigoComponenteCurricular);

            Guid codigoRelatorio = await SolicitarRelatorioDevolutiva(devolutiva.Id, turma.UeId, turma.CodigoTurma, usuarioLogado);

            var botaoDownload = MontarBotaoDownload(codigoRelatorio);

            if (titularEol != null)
            {
                var mensagem = new StringBuilder($"O usuário {usuarioLogado.Nome} ({usuarioLogado.CodigoRf}) registrou a devolutiva dos diários de bordo de <strong>{componenteCurricular.NomeComponenteInfantil}</strong> da turma <strong>{turma.Nome}</strong> da <strong>{turma.Ue.TipoEscola}-{turma.Ue.Nome}</strong> " +
                    $"<strong>({turma.Ue.Dre.Abreviacao})</strong>. Esta devolutiva contempla os diários de bordo do período de <strong>{devolutiva.PeriodoInicio:dd/MM/yyyy}</strong> à <strong>{devolutiva.PeriodoFim:dd/MM/yyyy}</strong>.");

                mensagem.AppendLine($"<br/><br/>Clique no botão abaixo para fazer o download do arquivo com o conteúdo da devolutiva.");
                mensagem.AppendLine(botaoDownload);

                if (titularEol.ProfessorRf != usuarioLogado.CodigoRf)
                {
                    var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(titularEol.ProfessorRf));
                    if (usuario != null)
                    {
                        var notificacao = new Notificacao()
                        {
                            Ano = DateTime.Now.Year,
                            Categoria = NotificacaoCategoria.Aviso,
                            Tipo = NotificacaoTipo.Planejamento,
                            Titulo = $"Devolutiva do Diário de bordo da turma {turma.Nome} - {componenteCurricular.NomeComponenteInfantil}",
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
                return true;
            }
            return false;
        }

        private string MontarBotaoDownload(Guid codigoRelatorio)
        {
            var urlRedirecionamentoBase = configuration.GetSection("UrlServidorRelatorios").Value;
            var urlNotificacao = $"{urlRedirecionamentoBase}api/v1/downloads/sgp/pdfsincrono/RelatorioDevolutiva.pdf/{codigoRelatorio}";
            return $"<br/><br/><a href='{urlNotificacao}' target='_blank' class='btn-baixar-relatorio'><i class='fas fa-arrow-down mr-2'></i>Download</a>";
        }
        private async Task<Guid> SolicitarRelatorioDevolutiva(long devolutivaId, long ueId, string codigoTurma, Usuario usuarioLogado)
        {
            var turma = await repositorioTurma.ObterPorCodigo(codigoTurma);
            var filtro = new FiltroRelatorioDevolutivasSincrono()
            {
                DevolutivaId = devolutivaId,
                UsuarioNome = usuarioLogado.Nome,
                UsuarioRF = usuarioLogado.CodigoRf,
                UeId = ueId,
                TurmaId = turma.Id
            };
            return await mediator.Send(new SolicitaRelatorioDevolutivasCommand(filtro));
        }
    }
}
