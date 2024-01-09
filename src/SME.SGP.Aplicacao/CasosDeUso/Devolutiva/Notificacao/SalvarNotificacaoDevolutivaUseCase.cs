using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotificacaoDevolutivaUseCase : ISalvarNotificacaoDevolutivaUseCase
    {
        private const int ANO_LETIVO_INICIO_DEVOLUTIVA_UNIFICADA = 2024;

        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private readonly IRepositorioNotificacaoDevolutiva repositorioNotificacaoDevolutiva;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IConsultasDisciplina consultasDisciplina;

        private Turma turma;
        private Devolutiva devolutiva;

        public SalvarNotificacaoDevolutivaUseCase(
                                                  IMediator mediator, 
                                                  IConfiguration configuration, 
                                                  IServicoNotificacao servicoNotificacao,
                                                  IRepositorioNotificacaoDevolutiva repositorioNotificacaoDevolutiva,
                                                  IConsultasDisciplina consultasDisciplina)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioNotificacaoDevolutiva = repositorioNotificacaoDevolutiva ?? throw new ArgumentNullException(nameof(repositorioNotificacaoDevolutiva));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<SalvarNotificacaoDevolutivaDto>();

            turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(dadosMensagem.TurmaId));
            if (turma.EhNulo())
                throw new NegocioException("Não foi possível obter a turma");

            var usuarioNome = dadosMensagem.UsuarioNome;
            var usuarioRf = dadosMensagem.UsuarioRF;

            devolutiva = await mediator.Send(new ObterDevolutivaPorIdQuery(dadosMensagem.DevolutivaId));

            if (devolutiva.EhNulo())
                throw new NegocioException("Não foi possível obter a devolutiva");

            if (turma.AnoLetivo >= ANO_LETIVO_INICIO_DEVOLUTIVA_UNIFICADA)
                return await ExecuteNotificacaoDevolutivaUnificada(usuarioNome, usuarioRf);

            return await ExecuteNotificacaoDevolutiva(devolutiva.CodigoComponenteCurricular, usuarioNome, usuarioRf);
        }

        private async Task<bool> ExecuteNotificacaoDevolutiva(long codigoComponenteCurricular, string usuarioNome, string usuarioRf)
        {
            var professorTitularEol = await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(turma.CodigoTurma, codigoComponenteCurricular.ToString()));

            if (professorTitularEol.EhNulo())
                throw new NegocioException("Não foi possível obter o professor titular da turma");

            var componenteCurricular = await mediator.Send(new ObterComponenteCurricularPorIdQuery(codigoComponenteCurricular));

            if (componenteCurricular.EhNulo())
                throw new NegocioException("Não foi possível obter o componente curricular");

            var codigoRelatorio = await mediator.Send(new SolicitaRelatorioDevolutivasCommand(devolutiva.Id, usuarioNome, usuarioRf, turma.UeId, turma.Id));

            if (codigoRelatorio == Guid.Empty)
                throw new NegocioException("Não foi possível obter o relatório de devolutiva");

            var botaoDownload = MontarBotaoDownload(codigoRelatorio);

            if (professorTitularEol.NaoEhNulo())
            {
                var mensagem = new StringBuilder($"O usuário {usuarioNome} ({usuarioRf}) registrou a devolutiva dos diários de bordo de <strong>{componenteCurricular.NomeComponenteInfantil}</strong> da turma <strong>{turma.Nome}</strong> da <strong>{turma.Ue.TipoEscola}-{turma.Ue.Nome}</strong> " +
                    $"<strong>({turma.Ue.Dre.Abreviacao})</strong>. Esta devolutiva contempla os diários de bordo do período de <strong>{devolutiva.PeriodoInicio:dd/MM/yyyy}</strong> à <strong>{devolutiva.PeriodoFim:dd/MM/yyyy}</strong>.");

                mensagem.AppendLine($"<br/><br/>Clique no botão abaixo para fazer o download do arquivo com o conteúdo da devolutiva.");
                mensagem.AppendLine(botaoDownload);

                if (professorTitularEol.ProfessorRf != usuarioRf)
                {
                    var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(professorTitularEol.ProfessorRf));
                    if (usuario.NaoEhNulo())
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

                        await servicoNotificacao.Salvar(notificacao);

                        var notificacaoDevolutiva = new NotificacaoDevolutiva()
                        {
                            NotificacaoId = notificacao.Id,
                            DevolutivaId = devolutiva.Id
                        };
                        await repositorioNotificacaoDevolutiva.Salvar(notificacaoDevolutiva);
                    }
                }
                return true;
            }
            return false;
        }

        private async Task<bool> ExecuteNotificacaoDevolutivaUnificada(string usuarioNome, string usuarioRf)
        {
            var disciplinas = await ObterComponentesCurricularesTurma(turma.CodigoTurma, devolutiva.CodigoComponenteCurricular);

            foreach(var disciplina in disciplinas)
                await ExecuteNotificacaoDevolutiva(disciplina.CodigoComponenteCurricular, usuarioNome, usuarioRf);
            
            return true;
        }

        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesCurricularesTurma(string turmaCodigo, long componentePai)
        {
            var disciplinas = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(turmaCodigo, false);

            return disciplinas.FindAll(item => item.CdComponenteCurricularPai == componentePai);
        }

        private string MontarBotaoDownload(Guid codigoRelatorio)
        {
            var urlRedirecionamentoBase = configuration.GetSection("UrlServidorRelatorios").Value;
            var urlNotificacao = $"{urlRedirecionamentoBase}api/v1/downloads/sgp/pdfsincrono/Devolutivas.pdf/{codigoRelatorio}";
            return $"<br/><br/><a href='{urlNotificacao}' target='_blank' class='btn-baixar-relatorio'><i class='fas fa-arrow-down mr-2'></i>Download</a>";
        }        
    }
}
