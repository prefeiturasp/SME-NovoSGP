using MediatR;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificaFrequenciaPeriodoUeCommandHandler : IRequestHandler<NotificaFrequenciaPeriodoUeCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public NotificaFrequenciaPeriodoUeCommandHandler(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(NotificaFrequenciaPeriodoUeCommand request, CancellationToken cancellationToken)
        {
            var ues = await ObterUes();
            
            foreach(var ue in ues)
            {
                try
                {
                    var codigoRelatorio = await SolicitarRelatorioBimestral(request.PeriodoEscolarEncerrado.Bimestre, ue);
                    if (!codigoRelatorio.Equals(Guid.Empty))
                        await EnviarRelatorio(codigoRelatorio, ue, request.PeriodoEscolarEncerrado);
                }
                catch (Exception e)
                {
                    SentrySdk.CaptureException(e);
                }            
            }

            return true;
        }

        private async Task EnviarRelatorio(Guid codigoRelatorio, Ue ue, PeriodoEscolar periodoEscolarEncerrado)
        {
            var componentesUe = await VerificaComponentesUe(ue);

            var siglasComponentesUe = ObterSiglasComponentes(componentesUe);
            var nomesComponentesUe = OterNomesComponentes(componentesUe);

            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Validação bimestral de frequência {siglasComponentesUe} - {periodoEscolarEncerrado.Bimestre}º Bimestre - {descricaoUe}";
            var mensagem = $"Segue o relatório de frequência dos componentes de {nomesComponentesUe} do <b>{periodoEscolarEncerrado.Bimestre}º Bimestre</b> da <b>{descricaoUe}</b> para sua validação.<br/><br/>Clique no botão abaixo para fazer o download do arquivo.<br/><br/>";
            mensagem += await MontarBotaoDownload(codigoRelatorio);

            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem, NotificacaoCategoria.Workflow_Aprovacao, NotificacaoTipo.Frequencia, ObterCargos(), ue.Dre.CodigoDre, ue.CodigoUe));
        }

        private string OterNomesComponentes(IEnumerable<ComponenteCurricularDto> componentesUe)
        {
            return string.Join(", ",
                            componentesUe.Select(a => a.Codigo == "1060" ? "Sala de Leitura" :
                                                      a.Codigo == "1061" ? "Informática" :
                                                       "PAP"));
        }

        private string ObterSiglasComponentes(IEnumerable<ComponenteCurricularDto> componentesUe)
        {
            return string.Join(", ",
                            componentesUe.Select(a => a.Codigo == "1060" ? "OSL" :
                                                      a.Codigo == "1061" ? "OIE" :
                                                       "PAP"));
        }

        private async Task<IEnumerable<ComponenteCurricularDto>> VerificaComponentesUe(Ue ue)
        {
            var componentesUe = await mediator.Send(new ObterComponentesCurricularesPorUeQuery(ue.CodigoUe));
            var componentesNotificacao = new[] { "1060", "1061", "1322" };

            return componentesUe.Where(a => componentesNotificacao.Contains(a.Codigo.ToString()));
        }

        private Cargo[] ObterCargos()
            => new Cargo[] { Cargo.Diretor, Cargo.Supervisor };

        private async Task<string> MontarBotaoDownload(Guid codigoRelatorio)
        {
            var urlRedirecionamentoBase = configuration.GetSection("UrlServidorRelatorios").Value;
            var urlNotificacao = $"{urlRedirecionamentoBase}api/v1/downloads/sgp/pdf/RelatorioFaltasFrequencia.pdf/{codigoRelatorio}";
            return $"<a href='{urlNotificacao}' target='_blank' class='btn-baixar-relatorio'><i class='fas fa-arrow-down mr-2'></i>Download</a>";
        }

        private async Task<IEnumerable<Ue>> ObterUes()
            => await mediator.Send(new ObterUesComDrePorModalidadeTurmasQuery(Modalidade.Fundamental, DateTime.Now.Year));

        private async Task<Guid> SolicitarRelatorioBimestral(int bimestre, Ue ue)
        {
            var filtro = new FiltroRelatorioFrequenciaDto()
            {
                AnoLetivo = DateTime.Now.Year,
                AnosEscolares = new[] { "-99" },
                Bimestres = new List<int>() { bimestre },
                Modalidade = Modalidade.Fundamental,
                CodigoDre = ue.Dre.CodigoDre,
                CodigoUe = ue.CodigoUe,
                ComponentesCurriculares = new List<string>() { "1060", "1061", "1322" },
                TipoRelatorio = TipoRelatorioFaltasFrequencia.Ano,
                Condicao = CondicoesRelatorioFaltasFrequencia.TodosEstudantes,
                TipoFormatoRelatorio = TipoFormatoRelatorio.Pdf,
                NomeUsuario = "Processo automático",
                CodigoRf = " - ",
            };

            return await mediator.Send(new SolicitaRelatorioFaltasFrequenciaCommand(filtro));
        }
    }
}
