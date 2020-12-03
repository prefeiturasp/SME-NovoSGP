using MediatR;
using Microsoft.Extensions.Configuration;
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
                var codigoRelatorio = await SolicitarRelatorioBimestral(request.PeriodoEscolarEncerrado.Bimestre, ue);
                await EnviarRelatorio(codigoRelatorio, ue, request.PeriodoEscolarEncerrado);
            }

            return true;
        }

        private async Task EnviarRelatorio(Guid codigoRelatorio, Ue ue, PeriodoEscolar periodoEscolarEncerrado)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao}";
            var titulo = $"Validação bimestral de frequência OSL, OIE e PAP - {periodoEscolarEncerrado.Bimestre}º Bimestre - {descricaoUe})";
            var mensagem = $"Segue o relatório de frequência dos componentes de Sala de Leitura, Informática e PAP do <b>{periodoEscolarEncerrado.Bimestre}º Bimestre</b> da <b>{descricaoUe}</b> para sua validação.<br/><br/>Clique no botão abaixo para fazer o download do arquivo.";
            mensagem += await MontarBotaoDownload(codigoRelatorio);

            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem, NotificacaoCategoria.Workflow_Aprovacao, NotificacaoTipo.Frequencia, ObterCargos()));
        }

        private Cargo[] ObterCargos()
            => new Cargo[] { Cargo.Diretor, Cargo.Supervisor };

        private async Task<string> MontarBotaoDownload(Guid codigoRelatorio)
        {
            var urlRedirecionamentoBase = configuration.GetSection("UrlServidorRelatorios").Value;
            var urlNotificacao = $"{urlRedirecionamentoBase}api/v1/downloads/sgp/pdf/RelatorioFaltasFrequencia/{codigoRelatorio}";
            return $"<a href='{urlNotificacao}' target='_blank' class='btn-baixar-relatorio'><i class='fas fa-arrow-down mr-2'></i>Download</a>";
        }

        private async Task<IEnumerable<Ue>> ObterUes()
            => await mediator.Send(new ObterUesComDrePorModalidadeTurmasQuery(Modalidade.Fundamental, DateTime.Now.Year));

        private async Task<Guid> SolicitarRelatorioBimestral(int bimestre, Ue ue)
        {
            var filtro = new FiltroRelatorioFaltasFrequenciaDto()
            {
                AnoLetivo = DateTime.Now.Year,
                AnosEscolares = Enumerable.Range(1, 9).Cast<string>(),
                Bimestres = new List<int>() { bimestre },
                Modalidade = Modalidade.Fundamental,
                CodigoDre = ue.Dre.CodigoDre,
                CodigoUe = ue.CodigoUe,
                ComponentesCurriculares = new List<long>() { 1060, 1061, 1322 },
                TodosEstudantes = true,
                TipoRelatorio = TipoRelatorioFaltasFrequencia.Ambos,
                TipoFormatoRelatorio = TipoFormatoRelatorio.Pdf
            };

            return await mediator.Send(new SolicitaRelatorioFaltasFrequenciaCommand(filtro));
        }
    }
}
