using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public class ServicoDeNotasConceitos : IServicoDeNotasConceitos
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly string hostAplicacao;
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;

        public ServicoDeNotasConceitos(IUnitOfWork unitOfWork,IConfiguration configuration,IMediator mediator, IRepositorioNotasConceitos repositorioNotasConceitos)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.hostAplicacao = configuration["UrlFrontEnd"];
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
        }

        public async Task Salvar(IEnumerable<NotaConceito> notasConceitos, string professorRf, string turmaId,
            string disciplinaId, bool consideraHistorico = false)
        {
            try
            {
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));

                if (turma.EhNulo())
                    throw new NegocioException($"Turma com código [{turmaId}] não localizada");

                var idsAtividadesAvaliativas = notasConceitos
                    .Select(x => x.AtividadeAvaliativaID);

                var atividadesAvaliativas =
                    await mediator.Send(new ObterListaDeAtividadesAvaliativasPorIdsQuery(idsAtividadesAvaliativas));

                var alunos = await mediator
                .Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(turma.CodigoTurma)));

                if (alunos.EhNulo() || !alunos.Any())
                    throw new NegocioException("Não foi encontrado nenhum aluno para a turma informada");

                var podeNotificar = await mediator.Send(new VerificaSeExisteParametroSistemaPorTipoQuery(TipoParametroSistema.GerarNotificacaoAlteracaoEmAtividadeAvaliativa));

                var usuario = await  mediator.Send(ObterUsuarioLogadoQuery.Instance);

                await ValidarAvaliacoes(idsAtividadesAvaliativas, atividadesAvaliativas, professorRf, disciplinaId,usuario.EhGestorEscolar(), turma);

                var entidadesSalvar = new List<NotaConceito>();

                var notasPorAvaliacoes = notasConceitos
                    .GroupBy(x => x.AtividadeAvaliativaID);

                var dataConsiderada = atividadesAvaliativas.Any()
                    ? atividadesAvaliativas.OrderBy(aa => aa.DataAvaliacao).Last().DataAvaliacao.Date
                    : DateTime.Today;
                alunos = (from a in alunos
                    join nc in notasConceitos
                        on a.CodigoAluno equals nc.AlunoId
                    join aa in atividadesAvaliativas
                        on nc.AtividadeAvaliativaID equals aa.Id
                    where a.EstaAtivo(aa.DataAvaliacao)
                    select a).Distinct();

                if (!usuario.EhGestorEscolar())
                    await VerificaSeProfessorPodePersistirTurmaDisciplina(professorRf, turmaId, disciplinaId,
                        dataConsiderada, usuario);

                foreach (var notasPorAvaliacao in notasPorAvaliacoes)
                {
                    var avaliacao = atividadesAvaliativas.FirstOrDefault(x => x.Id == notasPorAvaliacao.Key);
                    entidadesSalvar.AddRange(await ValidarEObter(notasPorAvaliacao.ToList(), avaliacao, alunos, disciplinaId, usuario, turma, podeNotificar));
                }

                await SalvarNoBanco(entidadesSalvar, turma.CodigoTurma);

                var alunosId = alunos
                    .Select(a => a.CodigoAluno)
                    .ToList();

                if (podeNotificar)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAvaliacao.RotaValidarMediaAlunos, new FiltroValidarMediaAlunosDto(idsAtividadesAvaliativas, alunosId, usuario, disciplinaId, turma.CodigoTurma, hostAplicacao, false, consideraHistorico), Guid.NewGuid(), usuario));

            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao Salvar as Notas de Conceito", LogNivel.Critico, LogContexto.Geral, ex.Message,excecaoInterna:ex.StackTrace));
                throw;
            }
        }

        public async Task<NotaTipoValor> TipoNotaPorAvaliacao(AtividadeAvaliativa atividadeAvaliativa,
            bool consideraHistorico = false)
        {
            var turmaEOL = await mediator.Send(new ObterDadosTurmaEolPorCodigoQuery(atividadeAvaliativa.TurmaId));

            if (turmaEOL.TipoTurma == Enumerados.TipoTurma.EdFisica)
            {
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(atividadeAvaliativa.TurmaId));
                return await mediator.Send(new ObterNotaTipoValorPorTurmaIdQuery(turma));
            }
            if (await ModalidadeTurmaEhCelp(turmaEOL))
                return new NotaTipoValor() { TipoNota = TipoNota.Conceito }; 

            var notaTipo = await ObterNotaTipo(atividadeAvaliativa.TurmaId, atividadeAvaliativa.DataAvaliacao,
                consideraHistorico);

            if (notaTipo.EhNulo())
                throw new NegocioException(MensagensNegocioLancamentoNota.Nao_foi_encontrado_tipo_de_nota_para_a_avaliacao);

            return notaTipo;
        }

        private async Task<bool> ModalidadeTurmaEhCelp(DadosTurmaEolDto turmaEOL)
        {
            if (turmaEOL.TipoTurma == TipoTurma.Programa)
            {
                var modalidade = await mediator.Send(new ObterModalidadeTurmaPorCodigoQuery(turmaEOL.Codigo.ToString()));

                return modalidade == Modalidade.CELP;
            }

            return false;
        }

        private static void ValidarSeAtividadesAvaliativasExistem(IEnumerable<long> avaliacoesAlteradasIds,
            IEnumerable<AtividadeAvaliativa> avaliacoes)
        {
            avaliacoesAlteradasIds.ToList().ForEach(avalicaoAlteradaId =>
            {
                var atividadeavaliativa = avaliacoes.FirstOrDefault(avaliacao => avaliacao.Id == avalicaoAlteradaId);

                if (atividadeavaliativa.EhNulo())
                    throw new NegocioException(
                        $"Não foi encontrada atividade avaliativa com o codigo {avalicaoAlteradaId}");
            });
        }

        private async Task<IEnumerable<PeriodoEscolar>> BuscarPeriodosEscolaresDaAtividade(
            AtividadeAvaliativa atividadeAvaliativa)
        {
            var dataFinal = atividadeAvaliativa.DataAvaliacao.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var aula = await mediator.Send(new ObterAulaIntervaloTurmaDisciplinaQuery(atividadeAvaliativa.DataAvaliacao,
                dataFinal, atividadeAvaliativa.TurmaId, atividadeAvaliativa.Id));

            if (aula.EhNulo())
                throw new NegocioException(
                    $"Não encontrada aula para a atividade avaliativa '{atividadeAvaliativa.NomeAvaliacao}' no dia {atividadeAvaliativa.DataAvaliacao.Date.ToString("dd/MM/yyyy")}");

            var periodosEscolares =
                await mediator.Send(new ObterPeriodoEscolarPorTipoCalendarioQuery(aula.TipoCalendarioId));
            return periodosEscolares;
        }

        public async Task<NotaTipoValor> ObterNotaTipo(string turmaCodigo, DateTime data,
            bool consideraHistorico = false)
        {
            var turma = await mediator.Send(
                new ObterAbrangenciaPorTurmaEConsideraHistoricoQuery(turmaCodigo, consideraHistorico));

            if (turma.EhNulo())
                throw new NegocioException(MensagensNegocioLancamentoNota.Nao_foi_encontrada_a_turma_informada);

            string anoCicloModalidade = string.Empty;

            if (!String.IsNullOrEmpty(turma?.Ano))
                anoCicloModalidade = turma.Ano == AnoCiclo.Alfabetizacao.Name() ? AnoCiclo.Alfabetizacao.Description() : turma.Ano;

            var ciclo = await mediator.Send(new ObterCicloPorAnoModalidadeQuery(anoCicloModalidade, turma.Modalidade));

            if (ciclo.EhNulo())
                throw new NegocioException(MensagensNegocioLancamentoNota.Nao_foi_encontrado_o_ciclo_da_turma_informada);

            var retorno = await mediator.Send(new ObterNotaTipoPorCicloIdDataAvalicacaoQuery(ciclo.Id, data));
            return retorno;
        }

        private async Task SalvarNoBanco(IEnumerable<NotaConceito> EntidadesSalvar, string codigoTurma)
        {
            var notaConceitoParaInserir = Enumerable.Empty<NotaConceito>();
            var notaConceitoParaRemover = Enumerable.Empty<NotaConceito>();
            var notaConceitoParaAtualizar = Enumerable.Empty<NotaConceito>();
            
            unitOfWork.IniciarTransacao();
            try
            {
                notaConceitoParaInserir = EntidadesSalvar.Where(x => x.Id == 0 && !String.IsNullOrEmpty(x.ObterNota())).ToList();
                notaConceitoParaRemover = EntidadesSalvar.Where(x => x.Id >= 0 && x.ObterNota().EhNulo()).ToList();
                notaConceitoParaAtualizar = EntidadesSalvar.Where(x => x.Id > 0 && x.ObterNota().NaoEhNulo()).ToList();

                foreach (var entidade in notaConceitoParaRemover)
                    await mediator.Send(new RemoverNotaConceitoCommand(entidade));

                if (notaConceitoParaInserir.Any())
                    await mediator.Send(new SalvarListaNotaConceitoCommand(notaConceitoParaInserir));

                foreach (var notaConceito in notaConceitoParaAtualizar)
                    repositorioNotasConceitos.Salvar(notaConceito);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
            await mediator.Send(new AtualizaCacheDeAtividadeAvaliativaPorTurmaCommand(codigoTurma, notaConceitoParaInserir, notaConceitoParaAtualizar, notaConceitoParaRemover));
            
        }

        private async Task ValidarAvaliacoes(IEnumerable<long> avaliacoesAlteradasIds,IEnumerable<AtividadeAvaliativa> atividadesAvaliativas, string professorRf, string disciplinaId,bool gestorEscolar, Turma turma)
        {
            if (atividadesAvaliativas.EhNulo() || !atividadesAvaliativas.Any())
                throw new NegocioException(MensagensNegocioLancamentoNota.Nao_foi_encontrada_nenhuma_da_avaliacao_informada);

            ValidarSeAtividadesAvaliativasExistem(avaliacoesAlteradasIds, atividadesAvaliativas);
            var disciplinasEol = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turma.CodigoTurma));

            await ValidarDataAvaliacaoECriador(atividadesAvaliativas, professorRf, disciplinaId, disciplinasEol,
                gestorEscolar);
        }


        private async Task ValidarDataAvaliacaoECriador(IEnumerable<AtividadeAvaliativa> atividadesAvaliativas,
            string professorRf, string disciplinaId, IEnumerable<ProfessorTitularDisciplinaEol> disciplinasEol,
            bool gestorEscolar)
        {
            if (atividadesAvaliativas.Any(x => x.DataAvaliacao.Date > DateTime.Today))
                throw new NegocioException(
                    "Não é possivel atribuir notas/conceitos para avaliação(es) com data(s) futura(s)");

            bool ehTitular = false;

            if (!gestorEscolar)
            {
                if (disciplinasEol.NaoEhNulo() && disciplinasEol.Any())
                    ehTitular = disciplinasEol.Any(d =>
                        d.DisciplinasId().Contains(long.Parse(disciplinaId))
                        && d.ProfessorRf == professorRf);

                var usuarioLogado = await mediator.Send(new ObterUsuarioPorRfQuery(professorRf));

                var atribuicaoEol =
                    await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery(
                        Convert.ToInt64(disciplinaId), atividadesAvaliativas.Select(x => x.TurmaId), usuarioLogado));

                var usuarioPossuiAtribuicaoNaTurmaNaData = atividadesAvaliativas.Any(a =>
                    atribuicaoEol.Any(b =>
                        b.CodigoTurma.Equals(a.TurmaId) && b.DataAtribuicaoAula <= a.DataAvaliacao &&
                        b.DataDisponibilizacaoAulas >= a.DataAvaliacao));

                if ((atividadesAvaliativas.Select(s => s.EhCj).Any() &&
                     !atividadesAvaliativas.Select(p => p.ProfessorRf.Equals(professorRf)).Any()) ||
                    (!atividadesAvaliativas.Select(s => s.EhCj).Any() && !ehTitular &&
                     !usuarioPossuiAtribuicaoNaTurmaNaData))
                    throw new NegocioException(
                        "Somente o professor que criou a avaliação e/ou titular, pode atribuir e/ou editar notas/conceitos");
            }
        }

        private async Task<IEnumerable<NotaConceito>> ValidarEObter(IEnumerable<NotaConceito> notasConceitos,
            AtividadeAvaliativa atividadeAvaliativa, IEnumerable<AlunoPorTurmaResposta> alunos, string disciplinaId,
            Usuario usuario, Turma turma, bool podeNotificar)
        {
                var notasMultidisciplina = new List<NotaConceito>();
                var alunosNotasExtemporaneas = new StringBuilder();
                var nota = notasConceitos.FirstOrDefault();
                var turmaHistorica =
                    await mediator.Send(new ObterAbrangenciaPorTurmaEConsideraHistoricoQuery(turma.CodigoTurma, true));
                var tipoNota = await TipoNotaPorAvaliacao(atividadeAvaliativa, turmaHistorica.NaoEhNulo());
                var notaParametro =
                    await mediator.Send(new ObterNotaParametroPorDataAvaliacaoQuery(atividadeAvaliativa.DataAvaliacao));
                var dataAtual = DateTime.Now;
                
                // Verifica Bimestre Atual
                var dataPesquisa = DateTime.Today;
                var periodosEscolares = await BuscarPeriodosEscolaresDaAtividade(atividadeAvaliativa);
                var periodoEscolarAtual = periodosEscolares.FirstOrDefault(x =>
                    x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);
                var periodoEscolarAvaliacao = periodosEscolares.FirstOrDefault(x =>
                    x.PeriodoInicio.Date <= atividadeAvaliativa.DataAvaliacao.Date &&
                    x.PeriodoFim.Date >= atividadeAvaliativa.DataAvaliacao.Date);
                if (periodoEscolarAvaliacao.EhNulo())
                    throw new NegocioException(MensagensNegocioLancamentoNota.Periodo_escolar_da_atividade_avaliativa_nao_encontrado);

                var bimestreAvaliacao = periodoEscolarAvaliacao.Bimestre;

                var fechamentoReabertura = await mediator.Send(new ObterTurmaEmPeriodoFechamentoReaberturaQuery(bimestreAvaliacao, DateTimeExtension.HorarioBrasilia().Date, periodoEscolarAvaliacao.TipoCalendarioId, atividadeAvaliativa.DreId, atividadeAvaliativa.UeId));

                var existePeriodoEmAberto = periodoEscolarAtual.NaoEhNulo() && periodoEscolarAtual.Bimestre == periodoEscolarAvaliacao.Bimestre
                                            || fechamentoReabertura.EhNulo();

                foreach (var notaConceito in notasConceitos)
                {
                    var aluno = alunos.FirstOrDefault(a => a.CodigoAluno.Equals(notaConceito.AlunoId));

                    if (aluno.EhNulo())
                        throw new NegocioException(String.Format(MensagensNegocioLancamentoNota.Nao_foi_encontrado_aluno_com_o_codigo, notaConceito.AlunoId));

                    if (tipoNota.TipoNota == TipoNota.Nota)
                    {
                        notaConceito.ValidarNota(notaParametro, aluno.NomeAluno);
                        if (notaParametro.EhNulo())
                            throw new NegocioException("Não foi possível localizar o parâmetro de nota.");
                    }
                    else
                    {
                        var conceitos =
                            await mediator.Send(new ObterConceitoPorDataQuery(atividadeAvaliativa.DataAvaliacao));

                        if (conceitos.EhNulo())
                            throw new NegocioException("Não foi possível localizar o parâmetro de conceito.");
                    }

                    notaConceito.TipoNota = tipoNota.TipoNota;
                    notaConceito.DisciplinaId = disciplinaId;
                    if (atividadeAvaliativa.Categoria.Equals(CategoriaAtividadeAvaliativa.Interdisciplinar) &&
                        notaConceito.Id.Equals(0))
                    {
                        var atividadeDisciplinas =
                            await mediator.Send(
                                new ObterListaDeAtividadeAvaliativaDisciplinaPorIdAtividadeQuery(atividadeAvaliativa.Id));
                        foreach (var atividade in atividadeDisciplinas)
                        {
                            if (!atividade.DisciplinaId.Equals(disciplinaId))
                            {
                                notasMultidisciplina.Add(new NotaConceito
                                {
                                    AlunoId = notaConceito.AlunoId,
                                    AtividadeAvaliativaID = notaConceito.AtividadeAvaliativaID,
                                    DisciplinaId = atividade.DisciplinaId,
                                    Nota = notaConceito.Nota,
                                    ConceitoId = notaConceito.ConceitoId,
                                    TipoNota = notaConceito.TipoNota,
                                    AlteradoEm = notaConceito.AlteradoEm,
                                    AlteradoPor = notaConceito.AlteradoPor,
                                });
                            }
                        }
                    }

                    if ((notaConceito.Id > 0) && (!existePeriodoEmAberto))
                    {
                        alunosNotasExtemporaneas.AppendLine($"<li>{aluno.CodigoAluno} - {aluno.NomeAluno}</li>");
                    }
                }

                if (podeNotificar && alunosNotasExtemporaneas.ToString().Length > 0)
                {
                    string mensagemNotificacao = $"<p>Os resultados da atividade avaliativa '{atividadeAvaliativa.NomeAvaliacao}' da turma {turma.Nome} da {turma.Ue.Nome} (DRE {turma.Ue.Dre.Nome}) no bimestre {bimestreAvaliacao} de {turma.AnoLetivo} foram alterados " +
                                                 $"pelo Professor {usuario.Nome} ({usuario.CodigoRf}) em {dataAtual.ToString("dd/MM/yyyy")} às {dataAtual.ToString("HH:mm")} para os seguintes alunos:</p><br/>{alunosNotasExtemporaneas.ToString()}" +
                                                 $"<a href='{hostAplicacao}diario-classe/notas/{nota.DisciplinaId}/{bimestreAvaliacao}'>Clique aqui para visualizar os detalhes.</a>";

                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAvaliacao.RotaNotificarUsuarioAlteracaoExtemporanea, new FiltroNotificarUsuarioAlteracaoExtemporaneaDto(atividadeAvaliativa, mensagemNotificacao, turma.Nome, turma.Ue.CodigoUe), Guid.NewGuid(), usuario));
                }

                var result = notasConceitos.ToList();
                result.AddRange(notasMultidisciplina);
                return result;
                
        }

        private async Task VerificaSeProfessorPodePersistirTurmaDisciplina(string codigoRf, string turmaId,
            string disciplinaId, DateTime dataAula, Usuario usuario = null)
        {
            if (usuario.EhNulo())
                usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var podePersistir =
                await mediator.Send(new ObterPodePersistirTurmaDisciplinaQuery(codigoRf, turmaId, disciplinaId, dataAula));

            if (!usuario.EhProfessorCj() && !podePersistir)
                throw new NegocioException(
                    MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }
    }
}