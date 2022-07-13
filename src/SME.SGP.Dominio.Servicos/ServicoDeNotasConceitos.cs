using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public class ServicoDeNotasConceitos : IServicoDeNotasConceitos
    {
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;

        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly string hostAplicacao;

        public Turma turma { get; set; }

        private IEnumerable<Usuario> _usuariosCPs;

        public async Task<IEnumerable<Usuario>> ObterUsuariosCPs()
        {
            if (_usuariosCPs == null)
            {
                var listaCPsUe =
                    await mediator.Send(new ObterFuncionariosPorCargoUeQuery(turma.Ue.CodigoUe, (long) Cargo.CP));
                var cpsUsuarios = await CarregaUsuariosPorRFs(listaCPsUe);
                _usuariosCPs = cpsUsuarios;
            }

            return _usuariosCPs;
        }

        private Usuario _usuarioDiretor;

        public async Task<Usuario> ObterUsuarioDiretor()
        {
            if (_usuarioDiretor == null)
            {
                var diretor =
                    await mediator.Send(new ObterFuncionariosPorCargoUeQuery(turma.Ue.CodigoUe, (long) Cargo.Diretor));
                var usrDiretor = await CarregaUsuariosPorRFs(diretor);
                _usuarioDiretor = usrDiretor.First();
            }

            return _usuarioDiretor;
        }

        public ServicoDeNotasConceitos(
            IUnitOfWork unitOfWork,
            IServicoNotificacao servicoNotificacao,
            IServicoUsuario servicoUsuario,
            IConfiguration configuration,
            IMediator mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.hostAplicacao = configuration["UrlFrontEnd"];
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Salvar(IEnumerable<NotaConceito> notasConceitos, string professorRf, string turmaId,
            string disciplinaId)
        {
            turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));

            if (turma == null)
                throw new NegocioException($"Turma com código [{turmaId}] não localizada");

            var idsAtividadesAvaliativas = notasConceitos
                .Select(x => x.AtividadeAvaliativaID);

            var atividadesAvaliativas =
                await mediator.Send(new ObterListaDeAtividadesAvaliativasPorIdsQuery(idsAtividadesAvaliativas));

            var alunos = await mediator.Send(new ObterAlunosEolPorTurmaQuery(turmaId, true));

            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado nenhum aluno para a turma informada");

            var usuario = await servicoUsuario
                .ObterUsuarioLogado();

            await ValidarAvaliacoes(idsAtividadesAvaliativas, atividadesAvaliativas, professorRf, disciplinaId,
                usuario.EhGestorEscolar());

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
                entidadesSalvar.AddRange(await ValidarEObter(notasPorAvaliacao.ToList(), avaliacao, alunos, professorRf,
                    disciplinaId, usuario, turma));
            }

            var criadoPor = await mediator.Send(new ObterUsuarioLogadoQuery());

            SalvarNoBanco(entidadesSalvar, criadoPor);

            var alunosId = alunos
                .Select(a => a.CodigoAluno)
                .ToList();

            await validarMediaAlunos(idsAtividadesAvaliativas, alunosId, usuario, disciplinaId, turma.CodigoTurma);
        }

        public async Task<NotaTipoValor> TipoNotaPorAvaliacao(AtividadeAvaliativa atividadeAvaliativa,
            bool consideraHistorico = false)
        {
            var turmaEOL = await mediator.Send(new ObterDadosTurmaEolPorCodigoQuery(atividadeAvaliativa.TurmaId));

            if (turmaEOL.TipoTurma == Enumerados.TipoTurma.EdFisica)
                return await mediator.Send(
                    new ObterNotaTipoValorPorTurmaIdQuery(Convert.ToInt64(atividadeAvaliativa.TurmaId),
                        Enumerados.TipoTurma.EdFisica));

            var notaTipo = await ObterNotaTipo(atividadeAvaliativa.TurmaId, atividadeAvaliativa.DataAvaliacao,
                consideraHistorico);

            if (notaTipo == null)
                throw new NegocioException("Não foi encontrado tipo de nota para a avaliação informada");

            return notaTipo;
        }

        public async Task validarMediaAlunos(IEnumerable<long> idsAtividadesAvaliativas, IEnumerable<string> alunosId,
            Usuario usuario, string disciplinaId, string codigoTurma)
        {
            var dataAtual = DateTime.Now;
            var notasConceitos =
                await mediator.Send(new ObterNotasPorAlunosAtividadesAvaliativasQuery(
                    idsAtividadesAvaliativas.ToArray(), alunosId.ToArray(), disciplinaId, codigoTurma));

            var atividadesAvaliativas =
                await mediator.Send(new ObterListaDeAtividadesAvaliativasPorIdsQuery(idsAtividadesAvaliativas));

            var notasPorAvaliacoes = notasConceitos.GroupBy(x => x.AtividadeAvaliativaID);
            var percentualAlunosInsuficientes = double.Parse(await mediator.Send(
                new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes,
                    DateTime.Today.Year)));

            foreach (var notasPorAvaliacao in notasPorAvaliacoes)
            {
                var atividadeAvaliativa = atividadesAvaliativas.FirstOrDefault(x => x.Id == notasPorAvaliacao.Key);
                var valoresConceito =
                    await mediator.Send(new ObterConceitoPorDataQuery(atividadeAvaliativa.DataAvaliacao));
                var turmaHistorica =
                    await mediator.Send(
                        new ObterAbrangenciaPorTurmaEConsideraHistoricoQuery(atividadeAvaliativa.TurmaId, true));
                var tipoNota = await TipoNotaPorAvaliacao(atividadeAvaliativa, turmaHistorica != null);
                var ehTipoNota = tipoNota.TipoNota == TipoNota.Nota;
                var notaParametro =
                    await mediator.Send(new ObterNotaParametroPorDataAvaliacaoQuery(atividadeAvaliativa.DataAvaliacao));
                var quantidadeAlunos = notasPorAvaliacao.Count();
                var quantidadeAlunosSuficientes = 0;
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(atividadeAvaliativa.TurmaId));

                var periodosEscolares = await BuscarPeriodosEscolaresDaAtividade(atividadeAvaliativa);
                var periodoAtividade = periodosEscolares.FirstOrDefault(x =>
                    x.PeriodoInicio.Date <= atividadeAvaliativa.DataAvaliacao.Date &&
                    x.PeriodoFim.Date >= atividadeAvaliativa.DataAvaliacao.Date);

                foreach (var nota in notasPorAvaliacao)
                {
                    var valorConceito =
                        ehTipoNota ? valoresConceito.FirstOrDefault(a => a.Id == nota.ConceitoId) : null;
                    quantidadeAlunosSuficientes += ehTipoNota ? nota.Nota >= notaParametro.Media ? 1 : 0 :
                        valorConceito != null && valorConceito.Aprovado ? 1 : 0;
                }

                string mensagemNotasConceitos =
                    $"<p>Os resultados da atividade avaliativa '{atividadeAvaliativa.NomeAvaliacao}' da turma {turma.Nome} da {turma.Ue.Nome} (DRE {turma.Ue.Dre.Nome}) no bimestre {periodoAtividade.Bimestre} de {turma.AnoLetivo} foram alterados " +
                    $"pelo Professor {usuario.Nome} ({usuario.CodigoRf}) em {dataAtual.ToString("dd/MM/yyyy")} às {dataAtual.ToString("HH:mm")} estão abaixo da média.</p>" +
                    $"<a href='{hostAplicacao}diario-classe/notas/{disciplinaId}/{periodoAtividade.Bimestre}'>Clique aqui para visualizar os detalhes.</a>";

                // Avalia se a quantidade de alunos com nota/conceito suficientes esta abaixo do percentual parametrizado para notificação
                if (quantidadeAlunosSuficientes < (quantidadeAlunos * percentualAlunosInsuficientes / 100))
                {
                    _usuariosCPs = null;
                    // Notifica todos os CPs da UE
                    var usuarioCPs = await ObterUsuariosCPs();
                    foreach (var usuarioCP in usuarioCPs)
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
            }
        }

        private async Task<IEnumerable<Usuario>> CarregaUsuariosPorRFs(IEnumerable<UsuarioEolRetornoDto> listaCPsUe)
        {
            var usuarios = new List<Usuario>();
            foreach (var cpUe in listaCPsUe)
                usuarios.Add(await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(cpUe.CodigoRf));
            return usuarios;
        }

        private static void ValidarSeAtividadesAvaliativasExistem(IEnumerable<long> avaliacoesAlteradasIds,
            IEnumerable<AtividadeAvaliativa> avaliacoes)
        {
            avaliacoesAlteradasIds.ToList().ForEach(avalicaoAlteradaId =>
            {
                var atividadeavaliativa = avaliacoes.FirstOrDefault(avaliacao => avaliacao.Id == avalicaoAlteradaId);

                if (atividadeavaliativa == null)
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

            if (aula == null)
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

            if (turma == null)
                throw new NegocioException("Não foi encontrada a turma informada");

            string anoCicloModalidade = !String.IsNullOrEmpty(turma?.Ano)
                ? turma.Ano == AnoCiclo.Alfabetizacao.Name() ? AnoCiclo.Alfabetizacao.Description() : turma.Ano
                : string.Empty;
            var ciclo = await mediator.Send(new ObterCicloPorAnoModalidadeQuery(anoCicloModalidade, turma.Modalidade));

            if (ciclo == null)
                throw new NegocioException("Não foi encontrado o ciclo da turma informada");

            var retorno = await mediator.Send(new ObterNotaTipoPorCicloIdDataAvalicacaoQuery(ciclo.Id, data));
            return retorno;
        }

        private async Task SalvarNoBanco(List<NotaConceito> EntidadesSalvar, Usuario criadoPor)
        {
            unitOfWork.IniciarTransacao();

            var registroComIdZero = EntidadesSalvar.Where(x => x.Id == 0 && x.ObterNota() != null).ToList();
            var registroSemIdZero = EntidadesSalvar.Where(x => x.Id >= 0 && x.ObterNota() == null).ToList();

            foreach (var entidade in registroSemIdZero)
            {
                await mediator.Send(new RemoverNotaConceitoCommand(entidade));
            }

            if (registroComIdZero.Any())
                await mediator.Send(new SalvarListaNotaConceitoCommand(registroComIdZero, criadoPor));

            unitOfWork.PersistirTransacao();
        }

        private async Task ValidarAvaliacoes(IEnumerable<long> avaliacoesAlteradasIds,
            IEnumerable<AtividadeAvaliativa> atividadesAvaliativas, string professorRf, string disciplinaId,
            bool gestorEscolar)
        {
            if (atividadesAvaliativas == null || !atividadesAvaliativas.Any())
                throw new NegocioException("Não foi encontrada nenhuma da(s) avaliação(es) informada(s)");

            ValidarSeAtividadesAvaliativasExistem(avaliacoesAlteradasIds, atividadesAvaliativas);
            var disciplinasEol = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turma.CodigoTurma));

            await ValidarDataAvaliacaoECriador(atividadesAvaliativas, professorRf, disciplinaId, disciplinasEol,
                gestorEscolar);
        }


        private async Task ValidarDataAvaliacaoECriador(IEnumerable<AtividadeAvaliativa> atividadesAvaliativas,
            string professorRf, string disciplinaId, IEnumerable<ProfessorTitularDisciplinaEol> disciplinasEol,
            bool gestorEscolar)
        {
            if (atividadesAvaliativas.Where(x => x.DataAvaliacao.Date > DateTime.Today).Any())
                throw new NegocioException(
                    "Não é possivel atribuir notas/conceitos para avaliação(es) com data(s) futura(s)");

            bool ehTitular = false;

            if (!gestorEscolar)
            {
                if (disciplinasEol != null && disciplinasEol.Any())
                    ehTitular = disciplinasEol.Any(d =>
                        d.DisciplinaId.ToString() == disciplinaId && d.ProfessorRf == professorRf);

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
            AtividadeAvaliativa atividadeAvaliativa, IEnumerable<AlunoPorTurmaResposta> alunos, string professorRf,
            string disciplinaId,
            Usuario usuario, Turma turma)
        {
            var notasMultidisciplina = new List<NotaConceito>();
            var alunosNotasExtemporaneas = new StringBuilder();
            var nota = notasConceitos.FirstOrDefault();
            var turmaHistorica =
                await mediator.Send(new ObterAbrangenciaPorTurmaEConsideraHistoricoQuery(turma.CodigoTurma, true));
            var tipoNota = await TipoNotaPorAvaliacao(atividadeAvaliativa, turmaHistorica != null);
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
            if (periodoEscolarAvaliacao == null)
                throw new NegocioException("Período escolar da atividade avaliativa não encontrado");

            var bimestreAvaliacao = periodoEscolarAvaliacao.Bimestre;

            var existePeriodoEmAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma,
                DateTimeExtension.HorarioBrasilia().Date, periodoEscolarAvaliacao.Bimestre, !turma.EhAnoAnterior()));

            foreach (var notaConceito in notasConceitos)
            {
                var aluno = alunos.FirstOrDefault(a => a.CodigoAluno.Equals(notaConceito.AlunoId));

                if (aluno == null)
                    throw new NegocioException($"Não foi encontrado aluno com o codigo {notaConceito.AlunoId}");

                if (tipoNota.TipoNota == TipoNota.Nota)
                {
                    notaConceito.ValidarNota(notaParametro, aluno.NomeAluno);
                    if (notaParametro == null)
                        throw new NegocioException("Não foi possível localizar o parâmetro de nota.");
                }
                else
                {
                    var conceitos =
                        await mediator.Send(new ObterConceitoPorDataQuery(atividadeAvaliativa.DataAvaliacao));

                    if (conceitos == null)
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
                                TipoNota = notaConceito.TipoNota
                            });
                        }
                    }
                }

                if ((notaConceito.Id > 0) && (!existePeriodoEmAberto))
                {
                    alunosNotasExtemporaneas.AppendLine($"<li>{aluno.CodigoAluno} - {aluno.NomeAluno}</li>");
                }
            }

            if (alunosNotasExtemporaneas.ToString().Length > 0)
            {
                string mensagem =
                    $"<p>Os resultados da atividade avaliativa '{atividadeAvaliativa.NomeAvaliacao}' da turma {turma.Nome} da {turma.Ue.Nome} (DRE {turma.Ue.Dre.Nome}) no bimestre {bimestreAvaliacao} de {turma.AnoLetivo} foram alterados " +
                    $"pelo Professor {usuario.Nome} ({usuario.CodigoRf}) em {dataAtual.ToString("dd/MM/yyyy")} às {dataAtual.ToString("HH:mm")} para os seguintes alunos:</p><br/>{alunosNotasExtemporaneas.ToString()}" +
                    $"<a href='{hostAplicacao}diario-classe/notas/{nota.DisciplinaId}/{bimestreAvaliacao}'>Clique aqui para visualizar os detalhes.</a>";

                var usuariosCPs = await ObterUsuariosCPs();
                foreach (var usuarioCP in usuariosCPs)
                {
                    NotificarUsuarioAlteracaoExtemporanea(atividadeAvaliativa, mensagem, usuarioCP.Id, turma.Nome);
                }

                var diretor = await ObterUsuarioDiretor();
                NotificarUsuarioAlteracaoExtemporanea(atividadeAvaliativa, mensagem, diretor.Id, turma.Nome);
            }

            var result = notasConceitos.ToList();
            result.AddRange(notasMultidisciplina);
            return result;
        }

        private void NotificarUsuarioAlteracaoExtemporanea(AtividadeAvaliativa atividadeAvaliativa, string mensagem,
            long usuarioId, string turmaNome)
        {
            servicoNotificacao.Salvar(new Notificacao()
            {
                Ano = atividadeAvaliativa.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Alerta,
                DreId = atividadeAvaliativa.DreId,
                Mensagem = mensagem,
                UsuarioId = usuarioId,
                Tipo = NotificacaoTipo.Notas,
                Titulo = $"Alteração em Atividade Avaliativa - Turma {turmaNome}",
                TurmaId = atividadeAvaliativa.TurmaId,
                UeId = atividadeAvaliativa.UeId,
            });
        }

        private async Task VerificaSeProfessorPodePersistirTurmaDisciplina(string codigoRf, string turmaId,
            string disciplinaId, DateTime dataAula, Usuario usuario = null)
        {
            if (usuario == null)
                usuario = await servicoUsuario.ObterUsuarioLogado();

            var podePersistir =
                await servicoUsuario.PodePersistirTurmaDisciplina(codigoRf, turmaId, disciplinaId, dataAula);

            if (!usuario.EhProfessorCj() && !podePersistir)
                throw new NegocioException(
                    "Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        }
    }
}