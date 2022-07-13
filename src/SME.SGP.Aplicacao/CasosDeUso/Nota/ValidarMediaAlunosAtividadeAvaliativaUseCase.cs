using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ValidarMediaAlunosAtividadeAvaliativaUseCase : AbstractUseCase, IValidarMediaAlunosAtividadeAvaliativaUseCase
    {
        private readonly IServicoEol servicoEOL;
        private readonly IRepositorioConceitoConsulta repositorioConceito;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioNotaParametro repositorioNotaParametro;
        private readonly IRepositorioAulaConsulta repositorioAula;
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IRepositorioCiclo repositorioCiclo;

        public ValidarMediaAlunosAtividadeAvaliativaUseCase(IMediator mediator, IServicoEol servicoEOL, IRepositorioConceitoConsulta repositorioConceito, 
                                                            IConsultasAbrangencia consultasAbrangencia, IRepositorioNotaParametro repositorioNotaParametro,
                                                            IRepositorioAulaConsulta repositorioAula, IRepositorioTurmaConsulta repositorioTurma,
                                                            IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor, IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                                            IServicoNotificacao servicoNotificacao, IRepositorioCiclo repositorioCiclo) : base(mediator)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioNotaParametro = repositorioNotaParametro ?? throw new ArgumentNullException(nameof(repositorioNotaParametro));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioCiclo = repositorioCiclo ?? throw new ArgumentNullException(nameof(repositorioCiclo));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroValidarMediaAlunosAtividadeAvaliativaDto>();

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var atividadeAvaliativa = filtro.AtividadesAvaliativas.FirstOrDefault(x => x.Id == filtro.NotasPorAvaliacao.Key);
            
            var valoresConceito = await repositorioConceito.ObterPorData(atividadeAvaliativa.DataAvaliacao);
            
            var turmaHistorica = await consultasAbrangencia.ObterAbrangenciaTurma(atividadeAvaliativa.TurmaId, true);
            
            var tipoNota = await TipoNotaPorAvaliacao(atividadeAvaliativa, turmaHistorica != null);
            
            var ehTipoNota = tipoNota.TipoNota == TipoNota.Nota;
            
            var notaParametro = await repositorioNotaParametro.ObterPorDataAvaliacao(atividadeAvaliativa.DataAvaliacao);
            
            var quantidadeAlunos = filtro.NotasPorAvaliacao.Count();
            
            var quantidadeAlunosSuficientes = 0;
            
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(atividadeAvaliativa.TurmaId);

            var periodosEscolares = await BuscarPeriodosEscolaresDaAtividade(atividadeAvaliativa);
            var periodoAtividade = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= atividadeAvaliativa.DataAvaliacao.Date && x.PeriodoFim.Date >= atividadeAvaliativa.DataAvaliacao.Date);

            foreach (var nota in filtro.NotasPorAvaliacao)
            {
                var valorConceito = ehTipoNota ? valoresConceito.FirstOrDefault(a => a.Id == nota.ConceitoId) : null;

                quantidadeAlunosSuficientes += ehTipoNota ? nota.Nota >= notaParametro.Media ? 1 : 0 
                                                          : valorConceito != null && valorConceito.Aprovado ? 1 : 0;
            }
            string mensagemNotasConceitos = $"<p>Os resultados da atividade avaliativa '{atividadeAvaliativa.NomeAvaliacao}' da turma {turma.Nome} da {turma.Ue.Nome} (DRE {turma.Ue.Dre.Nome}) no bimestre {periodoAtividade.Bimestre} de {turma.AnoLetivo} foram alterados " +
          $"pelo Professor {filtro.Usuario.Nome} ({filtro.Usuario.CodigoRf}) em {dataAtual.ToString("dd/MM/yyyy")} às {dataAtual.ToString("HH:mm")} estão abaixo da média.</p>" +
          $"<a href='{filtro.FiltroAtividadeAvaliativa.HostAplicacao}diario-classe/notas/{filtro.DisciplinaId}/{periodoAtividade.Bimestre}'>Clique aqui para visualizar os detalhes.</a>";

            // Avalia se a quantidade de alunos com nota/conceito suficientes esta abaixo do percentual parametrizado para notificação
            if (quantidadeAlunosSuficientes < (quantidadeAlunos * filtro.PercentualAlunosInsuficientes / 100))
            {
                // Notifica todos os CPs da UE
                foreach (var usuarioCP in filtro.FiltroAtividadeAvaliativa.UsuariosCPs)
                {

                    servicoNotificacao.Salvar(new Notificacao()
                    {
                        Ano = atividadeAvaliativa.CriadoEm.Year,
                        Categoria = NotificacaoCategoria.Alerta,
                        DreId = atividadeAvaliativa.DreId,
                        Mensagem = mensagemNotasConceitos,
                        UsuarioId = usuarioCP.Id,
                        Tipo = NotificacaoTipo.Notas,
                        Titulo = $"Resultados de Atividade Avaliativa - Turma {turma.Nome}",
                        TurmaId = atividadeAvaliativa.TurmaId,
                        UeId = atividadeAvaliativa.UeId,
                    });
                }
            }

            return true;
        }

        private async Task<NotaTipoValor> TipoNotaPorAvaliacao(AtividadeAvaliativa atividadeAvaliativa, bool consideraHistorico = false)
        {
            var turmaEOL = await servicoEOL.ObterDadosTurmaPorCodigo(atividadeAvaliativa.TurmaId.ToString());

            if (turmaEOL.TipoTurma == Dominio.Enumerados.TipoTurma.EdFisica)
                return repositorioNotaTipoValor.ObterPorTurmaId(Convert.ToInt64(atividadeAvaliativa.TurmaId), Dominio.Enumerados.TipoTurma.EdFisica);

            var notaTipo = await ObterNotaTipo(atividadeAvaliativa.TurmaId, atividadeAvaliativa.DataAvaliacao, consideraHistorico);

            if (notaTipo == null)
                throw new NegocioException("Não foi encontrado tipo de nota para a avaliação informada");

            return notaTipo;
        }

        private async Task<IEnumerable<PeriodoEscolar>> BuscarPeriodosEscolaresDaAtividade(AtividadeAvaliativa atividadeAvaliativa)
        {
            var dataFinal = atividadeAvaliativa.DataAvaliacao.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var aula = await repositorioAula.ObterAulaIntervaloTurmaDisciplina(atividadeAvaliativa.DataAvaliacao, dataFinal, atividadeAvaliativa.TurmaId, atividadeAvaliativa.Id);

            if (aula == null)
                throw new NegocioException($"Não encontrada aula para a atividade avaliativa '{atividadeAvaliativa.NomeAvaliacao}' no dia {atividadeAvaliativa.DataAvaliacao.Date.ToString("dd/MM/yyyy")}");

            IEnumerable<PeriodoEscolar> periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(aula.TipoCalendarioId);
            return periodosEscolares;
        }

        public async Task<NotaTipoValor> ObterNotaTipo(string turmaCodigo, DateTime data, bool consideraHistorico = false)
        {
            var turma = await consultasAbrangencia.ObterAbrangenciaTurma(turmaCodigo, consideraHistorico);

            if (turma == null)
                throw new NegocioException("Não foi encontrada a turma informada");

            string anoCicloModalidade = !String.IsNullOrEmpty(turma?.Ano) ? turma.Ano == AnoCiclo.Alfabetizacao.Name() ? AnoCiclo.Alfabetizacao.Description() : turma.Ano : string.Empty;
            var ciclo = repositorioCiclo.ObterCicloPorAnoModalidade(anoCicloModalidade, turma.Modalidade);

            if (ciclo == null)
                throw new NegocioException("Não foi encontrado o ciclo da turma informada");

            return repositorioNotaTipoValor.ObterPorCicloIdDataAvalicacao(ciclo.Id, data);
        }
    }
}
