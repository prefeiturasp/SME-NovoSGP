﻿using MediatR;
using Polly;
using Polly.Registry;
using SME.SGP.Infra;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using Elasticsearch.Net.Specification.CrossClusterReplicationApi;

namespace SME.SGP.Aplicacao
{
    public class InserirAulaRecorrenteCommandHandler : IRequestHandler<InserirAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioNotificacaoAula repositorioNotificacaoAula;
        private readonly IUnitOfWork unitOfWork;
        public InserirAulaRecorrenteCommandHandler(IMediator mediator,
                                                   IRepositorioAula repositorioAula,
                                                   IRepositorioNotificacaoAula repositorioNotificacaoAula,
                                                   IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioNotificacaoAula = repositorioNotificacaoAula ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAula));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(InserirAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            AtribuicaoEsporadica atribuicao = new AtribuicaoEsporadica();
            if (!request.Usuario.EhGestorEscolar())
                atribuicao = await ValidarComponentesProfessor(request, request.Usuario, atribuicao);
            await GerarRecorrencia(request, request.Usuario, atribuicao);
            return true;
        }

        private async Task<AtribuicaoEsporadica> ValidarComponentesProfessor(InserirAulaRecorrenteCommand aulaRecorrente,
            Usuario usuarioLogado,
            AtribuicaoEsporadica atribuicao)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aulaRecorrente.CodigoTurma));

            if (usuarioLogado.EhProfessorCj())
            {
                var possuiAtribuicaoCJ = await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, usuarioLogado.CodigoRf));

                var atribuicoesEsporadica = await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(usuarioLogado.CodigoRf, false, aulaRecorrente.DataAula.Year, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

                if (possuiAtribuicaoCJ && (atribuicoesEsporadica != null && atribuicoesEsporadica.Any()))
                {
                    var verificaAtribuicao = atribuicoesEsporadica.FirstOrDefault(a => a.DataInicio <= aulaRecorrente.DataAula.Date && a.DataFim >= aulaRecorrente.DataAula.Date && a.DreId == turma.Ue.Dre.CodigoDre && a.UeId == turma.Ue.CodigoUe);
                    if (verificaAtribuicao == null)
                        throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma);

                    atribuicao = verificaAtribuicao;
                }

                await ValidaComponentesQuandoCj(aulaRecorrente, usuarioLogado);
                return atribuicao;
            }

            var obterComponentesQuery = new ObterComponentesCurricularesDoProfessorNaTurmaQuery(aulaRecorrente.CodigoTurma,
                                                                                                usuarioLogado.Login,
                                                                                                usuarioLogado.PerfilAtual,
                                                                                                turma.EhTurmaInfantil);

            var componentes = await mediator.Send(obterComponentesQuery);
            var podeCriarAulaTurma = PodeCadastarAulaNaTurma(componentes, aulaRecorrente);

            if (!podeCriarAulaTurma)
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma);

            var obterUsuarioQuery = new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(aulaRecorrente.ComponenteCurricularId,
                                                                                           aulaRecorrente.CodigoTurma, aulaRecorrente.DataAula, usuarioLogado);
            var usuarioPodePersistirTurmaNaData = await mediator.Send(obterUsuarioQuery);

            if (!usuarioPodePersistirTurmaNaData)
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);

            return atribuicao;
        }

        private bool PodeCadastarAulaNaTurma(IEnumerable<ComponenteCurricularEol> componentes, InserirAulaRecorrenteCommand aulaRecorrente)
        {
            if (componentes == null || !componentes.Any())
                return false;

            var componente = componentes.FirstOrDefault(c => c.Codigo == aulaRecorrente.ComponenteCurricularId);

            if (componente == null)
                componente = componentes.FirstOrDefault(c => c.CodigoComponenteTerritorioSaber == aulaRecorrente.ComponenteCurricularId);

            if (componente == null) return false;

            if (!componente.TerritorioSaber && (componente.Codigo != aulaRecorrente.ComponenteCurricularId)
                || componente.TerritorioSaber && (componente.CodigoComponenteTerritorioSaber != aulaRecorrente.CodigoTerritorioSaber))
                return false;

            return true;
        }

        private async Task ValidaComponentesQuandoCj(InserirAulaRecorrenteCommand aulaRecorrente,
            Usuario usuarioLogado)
        {
            var obterComponentesQuery = new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login);
            var componentes = await mediator.Send(obterComponentesQuery);
            bool FilterComponentesCompativeis(AtribuicaoCJ c) =>
                c.TurmaId == aulaRecorrente.CodigoTurma && (c.DisciplinaId == aulaRecorrente.ComponenteCurricularId || (aulaRecorrente.CodigoTerritorioSaber.HasValue && aulaRecorrente.CodigoTerritorioSaber.Value > 0 && c.DisciplinaId.Equals(aulaRecorrente.CodigoTerritorioSaber.Value)));

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(aulaRecorrente.CodigoTurma));
            var componentesAtribuicaoEol = await mediator
                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(aulaRecorrente.CodigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual, turma.EhTurmaInfantil));
            var componenteAtribuicaoEolCorrespondente = componentesAtribuicaoEol?
                .FirstOrDefault(ca => ca.Codigo.Equals(aulaRecorrente.ComponenteCurricularId) || ca.CodigoComponenteTerritorioSaber.Equals(aulaRecorrente.ComponenteCurricularId));

            if ((componentes == null || !componentes.Any(FilterComponentesCompativeis)) && componenteAtribuicaoEolCorrespondente == null)
            {
                var componenteTerritorioParaAula = await mediator
                    .Send(new DefinirComponenteCurricularParaAulaQuery(aulaRecorrente.CodigoTurma, aulaRecorrente.ComponenteCurricularId, usuarioLogado));

                if (componenteTerritorioParaAula == default)
                    throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma);

                if (!componenteTerritorioParaAula.codigoTerritorio.HasValue || componenteTerritorioParaAula.codigoTerritorio.Value == 0 || !componentes.Select(c => c.DisciplinaId).Contains(componenteTerritorioParaAula.codigoTerritorio.Value))
                    throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma);
            }
        }

        private async Task GerarRecorrencia(InserirAulaRecorrenteCommand aulaRecorrente, Usuario usuario, AtribuicaoEsporadica atribuicao)
        {
            var inicioRecorrencia = aulaRecorrente.DataAula;
            var obterFimPeriodoQuery = new ObterFimPeriodoRecorrenciaQuery(
                aulaRecorrente.TipoCalendarioId,
                aulaRecorrente.DataAula,
                aulaRecorrente.RecorrenciaAula);
            var fimRecorrencia = await mediator.Send(obterFimPeriodoQuery);

            if (fimRecorrencia == DateTime.MinValue)
                fimRecorrencia = inicioRecorrencia;

            await GerarRecorrenciaParaPeriodos(aulaRecorrente, inicioRecorrencia, fimRecorrencia, usuario, atribuicao);
        }

        private async Task GerarRecorrenciaParaPeriodos(InserirAulaRecorrenteCommand aulaRecorrente, DateTime inicioRecorrencia, DateTime fimRecorrencia, Usuario usuario, AtribuicaoEsporadica atribuicao)
        {
            var diasParaIncluirRecorrencia = ObterDiasDaRecorrencia(inicioRecorrencia, fimRecorrencia);

            if (diasParaIncluirRecorrencia == null || !diasParaIncluirRecorrencia.Any())
                throw new NegocioException("Não foi possível obter dias para incluir aulas recorrentes.");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aulaRecorrente.CodigoTurma));

            if (turma == null)
                throw new NegocioException("Não foi possível obter a turma para inclusão de aulas recorrentes.");

            var codigosTerritorioEquivalentes = await mediator
                .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(aulaRecorrente.ComponenteCurricularId, turma.CodigoTurma, usuario.EhProfessor() ? usuario.Login : null));

            var codigosComponentesConsiderados = new List<long>() { aulaRecorrente.ComponenteCurricularId };

            if (codigosTerritorioEquivalentes != default)
                codigosComponentesConsiderados.AddRange(codigosTerritorioEquivalentes.Select(c => long.Parse(c.codigoComponente)).Except(codigosComponentesConsiderados));

            var validacaoDatas = await ValidarDatasAula(diasParaIncluirRecorrencia, aulaRecorrente.CodigoTurma,
                codigosComponentesConsiderados.ToArray(), aulaRecorrente.TipoCalendarioId, aulaRecorrente.EhRegencia,
                aulaRecorrente.Quantidade, usuario, turma, atribuicao);
            var datasPersistencia = validacaoDatas.datasPersistencia;
            var mensagensValidacao = validacaoDatas.mensagensValidacao;

            var geracaoRecorrencia = await GerarAulaDeRecorrenciaParaDias(aulaRecorrente, usuario, datasPersistencia, aulaRecorrente.EhRegencia, turma, codigosTerritorioEquivalentes);

            // Notificar usuário da conclusão da geração de aulas
            await NotificarUsuario(geracaoRecorrencia.aula, geracaoRecorrencia.aulasQueDeramErro, mensagensValidacao, usuario, datasPersistencia.Count(), aulaRecorrente.NomeComponenteCurricular, turma);
        }

        private async Task NotificarUsuario(Aula aula, IEnumerable<(DateTime dataAula, string errorMessage)> aulasQueDeramErro, IEnumerable<string> mensagensValidacao, Usuario usuario, int quantidadeAulasCriadas, string componenteCurricularNome, Turma turma)
        {
            var perfilAtual = usuario.PerfilAtual;
            if (perfilAtual == Guid.Empty)
                throw new NegocioException($"Não foi encontrado o perfil do usuário informado.");

            var tituloMensagem = $"Criação de Aulas de {componenteCurricularNome} na turma {turma.Nome}";
            StringBuilder mensagemUsuario = new StringBuilder();

            mensagemUsuario.Append($"Foram criadas {quantidadeAulasCriadas} aulas do componente curricular {componenteCurricularNome} para a turma {turma.Nome} da {turma.Ue?.Nome} ({turma.Ue?.Dre?.Nome}).");

            if (mensagensValidacao.Any())
            {
                mensagemUsuario
                    .Append($"<br><br>Não foi possível criar aulas nas seguintes datas:<br>")
                    .Append(mensagensValidacao
                        .OrderBy(data => data)
                        .Aggregate((mensagem, aggregation) => aggregation + $"<br /> {mensagem}")
                    );
            }

            if (aulasQueDeramErro.Any())
            {
                mensagemUsuario.Append($"<br><br>Ocorreram erros na criação das seguintes aulas:<br>");
                foreach (var aulaComErro in aulasQueDeramErro.OrderBy(data => data))
                {
                    mensagemUsuario.Append($"<br /> {aulaComErro.dataAula.ToString("dd/MM/yyyy")} - {aulaComErro.errorMessage};");
                }
            }

            unitOfWork.IniciarTransacao();
            try
            {
                // Salva Notificação
                var notificacaoId = await mediator.Send(new NotificarUsuarioCommand(tituloMensagem,
                                                               mensagemUsuario.ToString(),
                                                               usuario.CodigoRf,
                                                               NotificacaoCategoria.Aviso,
                                                               NotificacaoTipo.Calendario,
                                                               turma.Ue.Dre.CodigoDre,
                                                               turma.Ue.CodigoUe,
                                                               turma.CodigoTurma,
                                                               DateTime.Now.Year));

                // Gera vinculo Notificacao x Aula
                await repositorioNotificacaoAula.Inserir(notificacaoId, aula.Id);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task<(IEnumerable<DateTime> datasPersistencia, IEnumerable<string> mensagensValidacao)> ValidarDatasAula(IEnumerable<DateTime> diasParaIncluirRecorrencia, string turmaCodigo, long[] componentesCurricularesCodigos, long tipoCalendarioId, bool ehRegencia, int quantidade, Usuario usuario, Turma turma, AtribuicaoEsporadica atribuicao)
        {
            // Aulas Existentes
            var validacaoAulasExistentes = await ValidarAulaExistenteNaData(diasParaIncluirRecorrencia, turmaCodigo, componentesCurricularesCodigos, usuario.EhProfessorCj());
            var datasValidas = validacaoAulasExistentes.datasValidas;

            if (datasValidas == null || !datasValidas.Any())
            {
                var mensagem = validacaoAulasExistentes.mensagensValidacao.Any() ?
                    string.Join("<br/>", validacaoAulasExistentes.mensagensValidacao) :
                    $"{string.Join("<br/>", diasParaIncluirRecorrencia)} Não foi possível validar essas datas para a inclusão de aulas recorrentes.";
                throw new NegocioException(mensagem);
            }

            // Grade Curricular
            var validacaoGradeCurricular = await ValidarGradeCurricular(datasValidas, turmaCodigo, componentesCurricularesCodigos, ehRegencia, quantidade, usuario.CodigoRf);

            // Dias Letivos
            var validacaoDiasLetivos = await ValidarDiasLetivos(validacaoGradeCurricular.datasValidas, turma, tipoCalendarioId);

            if (validacaoDiasLetivos.diasLetivos == null || !validacaoDiasLetivos.diasLetivos.Any())
                throw new NegocioException($"{string.Join("<br/>", validacaoDiasLetivos.mensagensValidacao)}");

            // Atribuição Professor
            var validacaoAtribuicaoProfessor = await ValidarAtribuicaoProfessor(validacaoDiasLetivos.diasLetivos, turmaCodigo, componentesCurricularesCodigos.OrderBy(cc => cc).Last(), usuario, atribuicao); ;

            return (validacaoAtribuicaoProfessor.datasAtribuicao,
                    validacaoAtribuicaoProfessor.mensagensValidacao
                        .Union(validacaoDiasLetivos.mensagensValidacao
                        .Union(validacaoGradeCurricular.mensagensValidacao)
                        .Union(validacaoAulasExistentes.mensagensValidacao)));
        }

        private async Task<(IEnumerable<DateTime> diasLetivos, IEnumerable<string> mensagensValidacao)> ValidarDiasLetivos(IEnumerable<DateTime> datasConsulta, Turma turma, long tipoCalendarioId)
        {
            var diasLetivos = new List<DateTime>();
            var mensagensValidacao = new List<string>();

            foreach (var dataConsulta in datasConsulta)
            {
                var consultaPodeCadastrarAula = await mediator.Send(new ObterPodeCadastrarAulaPorDataQuery()
                {
                    UeCodigo = turma.Ue.CodigoUe,
                    DreCodigo = turma.Ue.Dre.CodigoDre,
                    TipoCalendarioId = tipoCalendarioId,
                    DataAula = dataConsulta,
                    Turma = turma
                });

                if (consultaPodeCadastrarAula.PodeCadastrar)
                    diasLetivos.Add(dataConsulta);
                else
                    IncluirMensagemValidacao(dataConsulta, "Não é possível cadastrar essa aula pois a data informada está fora do período letivo.", ref mensagensValidacao);
            }

            return (diasLetivos, mensagensValidacao);
        }

        private void IncluirMensagemValidacao(DateTime data, string mensagem, ref List<string> mensagensValidacao)
        {
            mensagensValidacao.Add($"<b>{data.ToString("dd/MM/yyyy")}</b> - {mensagem};");
        }

        private async Task<(IEnumerable<DateTime> datasValidas, IEnumerable<string> mensagensValidacao)> ValidarGradeCurricular(IEnumerable<DateTime> datasConsulta, string turmaCodigo, long[] componentesCurricularesCodigos, bool ehRegencia, int quantidade, string codigoRf)
        {
            var datasValidas = new List<DateTime>();
            var mensagensValidacao = new List<string>();

            foreach (var dataConsulta in datasConsulta)
            {
                try
                {
                    var gradeAulas = await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(turmaCodigo, componentesCurricularesCodigos, dataConsulta, codigoRf, ehRegencia));
                    var quantidadeAulasRestantes = gradeAulas == null ? int.MaxValue : gradeAulas.QuantidadeAulasRestante;

                    if (gradeAulas != null)
                    {
                        if (quantidadeAulasRestantes < quantidade)
                            throw new NegocioException("Quantidade de aulas superior ao limíte de aulas da grade.");
                        if (!gradeAulas.PodeEditar && (quantidade != gradeAulas.QuantidadeAulasRestante))
                            throw new NegocioException("Quantidade de aulas não pode ser diferente do valor da grade curricular.");

                    }
                    datasValidas.Add(dataConsulta);
                }
                catch (Exception e)
                {
                    IncluirMensagemValidacao(dataConsulta, e.Message, ref mensagensValidacao);
                }
            }

            return (datasValidas, mensagensValidacao);
        }

        private async Task<(IEnumerable<DateTime> datasValidas, IEnumerable<string> mensagensValidacao)> ValidarAulaExistenteNaData(IEnumerable<DateTime> diasParaIncluirRecorrencia, string turmaCodigo, long[] componentesCurricularesCodigos, bool professorCJ)
        {
            var mensagensValidacao = new List<string>();
            var datasComRegistro = await mediator.Send(new ObterAulaPorDataAulasExistentesQuery(diasParaIncluirRecorrencia.ToList(), turmaCodigo, componentesCurricularesCodigos.Select(c => c.ToString()).ToArray(), professorCJ));

            if (datasComRegistro != null && datasComRegistro.Any())
            {
                diasParaIncluirRecorrencia.Where(a => datasComRegistro.Any(d => d == a)).ToList()
                                                .ForEach(a => IncluirMensagemValidacao(a, "Já existe aula cadastrada nesta data", ref mensagensValidacao));
                return (diasParaIncluirRecorrencia.Where(a => !datasComRegistro.Any(d => d == a)),
                        mensagensValidacao);
            }

            return (datasValidas: diasParaIncluirRecorrencia, mensagensValidacao: Enumerable.Empty<string>());
        }

        private async Task<(IEnumerable<DateTime> datasAtribuicao, IEnumerable<string> mensagensValidacao)> ValidarAtribuicaoProfessor(IEnumerable<DateTime> datasValidas, string turmaCodigo, long componenteCurricularCodigo, Usuario usuario, AtribuicaoEsporadica atribuicao)
        {
            var mensagensValidacao = new List<string>();

            if (usuario.EhProfessorCj() || usuario.EhGestorEscolar())
            {
                if (String.IsNullOrEmpty(atribuicao.DreId))
                    return (datasValidas, Enumerable.Empty<string>());

                datasValidas
                    .Where(d => d.Date > atribuicao.DataFim)
                    .OrderBy(a => a.Date)
                    .ToList()
                    .ForEach(dataInvalida => IncluirMensagemValidacao(
                        dataInvalida,
                        "Este professor não pode persistir nesta turma neste dia pois não possui abrangência",
                        ref mensagensValidacao)
                    );

                var datasAtribuicaoCJ = datasValidas.Where(d => d.Date <= atribuicao.DataFim);

                return (datasAtribuicaoCJ, mensagensValidacao);
            }

            if (datasValidas == null || !datasValidas.Any())
                throw new NegocioException("Não foi possível obter datas validas para a atribuição do professor no EOL.");

            var datasAtribuicaoEOL = await mediator.Send(new ObterValidacaoPodePersistirTurmaNasDatasQuery(
                usuario.Login,
                turmaCodigo,
                datasValidas.Select(a => a.Date).ToArray(),
                componenteCurricularCodigo));


            if (datasAtribuicaoEOL == null || !datasAtribuicaoEOL.Any())
                throw new NegocioException("Não foi possível validar datas para a atribuição do professor no EOL.");

            var datasAtribuicao = datasAtribuicaoEOL
                .Where(a => a.PodePersistir)
                .Select(a => a.Data);

            datasValidas
                .Where(d => !datasAtribuicao.Any(a => a.Date == d))
                .ToList()
                .ForEach(dataInvalida => IncluirMensagemValidacao(
                    dataInvalida,
                    "Este professor não pode persistir nesta turma neste dia.",
                    ref mensagensValidacao)
                );

            return (datasAtribuicao, mensagensValidacao);
        }

        private async Task<(Aula aula, IEnumerable<(DateTime dataAula, string mensagemDeErro)> aulasQueDeramErro)> GerarAulaDeRecorrenciaParaDias(InserirAulaRecorrenteCommand aulaRecorrente, Usuario usuario, IEnumerable<DateTime> datasParaPersistencia, bool ehRegencia, Turma turma, (string codigoComponente, string professor)[] codigosTerritorioEquivalentes = default)
        {
            var aulasQueDeramErro = new List<(DateTime dataAula, string errorMessage)>();

            var aula = GerarNovaAula(aulaRecorrente, usuario);

            foreach (var dia in datasParaPersistencia)
            {
                if (aula.Id == 0)
                {
                    var codigosComponentesConsiderados = new List<long>() { long.Parse(aula.DisciplinaId) };

                    if (codigosTerritorioEquivalentes != default)
                        codigosComponentesConsiderados.AddRange(codigosTerritorioEquivalentes.Select(c => long.Parse(c.codigoComponente)).Except(codigosComponentesConsiderados));

                    var retornoPodeCadastrarAula = await PodeCadastrarAula(0, aula.TurmaId, codigosComponentesConsiderados.ToArray(), dia, ehRegencia, aulaRecorrente.TipoAula, usuario.CodigoRf);

                    if (retornoPodeCadastrarAula.PodeCadastrarAula)
                    {
                        if (codigosTerritorioEquivalentes != default && long.Parse(codigosTerritorioEquivalentes.First().codigoComponente) > long.Parse(aula.DisciplinaId))
                            aula.DisciplinaId = codigosTerritorioEquivalentes.First().codigoComponente;

                        aula.ProfessorRf = codigosTerritorioEquivalentes.FirstOrDefault().professor ?? usuario.Login;

                        await repositorioAula.SalvarAsync(aula);
                    }
                    else
                        aulasQueDeramErro.Add((dia, "Já existe aula cadastrada nesta data"));

                    continue;
                }

                var aulaParaAdicionar = (Aula)aula.Clone();
                aulaParaAdicionar.DataAula = dia;
                aulaParaAdicionar.AdicionarAulaPai(aula);

                try
                {
                    await repositorioAula.SalvarAsync(aulaParaAdicionar);
                    await mediator.Send(new ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommand(turma,
                                                                                                       long.Parse(aulaParaAdicionar.DisciplinaId),
                                                                                                       aulaParaAdicionar.DataAula));
                }
                catch (Exception ex)
                {
                    aulasQueDeramErro.Add((dia, $"Erro Interno: {ex.Message}"));
                }
            }

            return (aula, aulasQueDeramErro);
        }

        private async Task<CadastroAulaDto> PodeCadastrarAula(int aulaId, string turmaCodigo, long[] disciplinasId, DateTime dataAula, bool ehRegencia, TipoAula tipoAula, string codigoRf)
        {
            if (CriandoAula(aulaId) || await AlterandoDataAula(aulaId, dataAula))
            {
                if (!await mediator.Send(new PodeCadastrarAulaNoDiaQuery(dataAula, turmaCodigo, disciplinasId, codigoRf, tipoAula)))
                    throw new NegocioException($"Não é possível cadastrar aula do tipo '{tipoAula.Name()}' para o dia selecionado!");
            }

            return new CadastroAulaDto()
            {
                PodeCadastrarAula = true,
                Grade = tipoAula == TipoAula.Reposicao ? null : await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(turmaCodigo, disciplinasId, dataAula, ehRegencia: ehRegencia))
            };
        }

        private static bool CriandoAula(long aulaId) => aulaId == 0;

        private async Task<bool> AlterandoDataAula(long aulaId, DateTime dataAula)
        {
            var dataOriginalAula = await mediator.Send(new ObterDataAulaQuery(aulaId));
            return dataAula != dataOriginalAula;
        }

        private Aula GerarNovaAula(InserirAulaRecorrenteCommand aulaRecorrente, Usuario usuario)
        {
            var entidadeAula = new Aula();

            entidadeAula.ProfessorRf = usuario.CodigoRf;
            entidadeAula.RecorrenciaAula = aulaRecorrente.RecorrenciaAula;

            entidadeAula.UeId = aulaRecorrente.CodigoUe;
            entidadeAula.DisciplinaId = aulaRecorrente.ComponenteCurricularId.ToString();
            entidadeAula.DisciplinaNome = aulaRecorrente.NomeComponenteCurricular;
            entidadeAula.TurmaId = aulaRecorrente.CodigoTurma;
            entidadeAula.TipoCalendarioId = aulaRecorrente.TipoCalendarioId;
            entidadeAula.DataAula = aulaRecorrente.DataAula.Date;
            entidadeAula.Quantidade = aulaRecorrente.Quantidade;
            entidadeAula.TipoAula = aulaRecorrente.TipoAula;
            entidadeAula.AulaCJ = usuario.EhProfessorCj();
            return entidadeAula;
        }

        private IEnumerable<DateTime> ObterDiasDaRecorrencia(DateTime inicioRecorrencia, DateTime fimRecorrencia)
        {
            if (inicioRecorrencia.Date == fimRecorrencia.Date)
                return new List<DateTime>() { inicioRecorrencia };

            return ObterDiaEntreDatas(inicioRecorrencia, fimRecorrencia);
        }

        private IEnumerable<DateTime> ObterDiaEntreDatas(DateTime inicio, DateTime fim)
        {
            for (DateTime i = inicio; i <= fim; i = i.AddDays(7))
            {
                yield return i;
            }
        }
    }
}
