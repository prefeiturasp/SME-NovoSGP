using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasNotasConceitos : IConsultasNotasConceitos
    {
        private readonly IConsultaAtividadeAvaliativa consultasAtividadeAvaliativa;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasFechamentoTurmaDisciplina consultasFechamentoTurmaDisciplina;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodoConsulta;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;
        private readonly IRepositorioNotaParametro repositorioNotaParametro;
        private readonly IRepositorioNotasConceitosConsulta repositorioNotasConceitos;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoDeNotasConceitos servicoDeNotasConceitos;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;
        private readonly IConsultasTurma consultasTurma;

        public ConsultasNotasConceitos(IConsultaAtividadeAvaliativa consultasAtividadeAvaliativa,
            IConsultasFechamentoTurmaDisciplina consultasFechamentoTurmaDisciplina, IConsultasDisciplina consultasDisciplina,
            IServicoDeNotasConceitos servicoDeNotasConceitos, IRepositorioNotasConceitosConsulta repositorioNotasConceitos,
            IRepositorioFrequenciaConsulta repositorioFrequencia, IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodoConsulta,
            IServicoUsuario servicoUsuario, IServicoAluno servicoAluno, IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
            IRepositorioNotaParametro repositorioNotaParametro, IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar, 
            IRepositorioTipoAvaliacao repositorioTipoAvaliacao, IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina,
            IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
            IMediator mediator,
            IConsultasTurma consultasTurma)
        {
            this.consultasAtividadeAvaliativa = consultasAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(consultasAtividadeAvaliativa));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasFechamentoTurmaDisciplina = consultasFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(consultasFechamentoTurmaDisciplina));
            this.servicoDeNotasConceitos = servicoDeNotasConceitos ?? throw new ArgumentNullException(nameof(servicoDeNotasConceitos));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioFrequenciaAlunoDisciplinaPeriodoConsulta = repositorioFrequenciaAlunoDisciplinaPeriodoConsulta ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodoConsulta));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoAluno = servicoAluno ?? throw new ArgumentNullException(nameof(servicoAluno));
            this.repositorioNotaParametro = repositorioNotaParametro ?? throw new ArgumentNullException(nameof(repositorioNotaParametro));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoAvaliacao = repositorioTipoAvaliacao ?? throw new ArgumentNullException(nameof(repositorioTipoAvaliacao));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
        }

        public async Task<NotasConceitosRetornoDto> ListarNotasConceitos(ListaNotasConceitosConsultaDto filtro)
        {
            var modalidadeTipoCalendario = filtro.Modalidade.ObterModalidadeTipoCalendario();
            
            var turma = await consultasTurma.ObterComUeDrePorCodigo(filtro.TurmaCodigo);

            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(filtro.AnoLetivo, modalidadeTipoCalendario, filtro.Semestre);
            if (tipoCalendario.EhNulo())
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            var bimestre = filtro.Bimestre;
            if (!bimestre.HasValue || bimestre == 0)
                bimestre = ObterBimestreAtual(periodosEscolares);

            var periodoAtual = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestre);
            if (periodoAtual.EhNulo())
                throw new NegocioException("Não foi encontrado período escolar para o bimestre solicitado.");

            List<AtividadeAvaliativa> atividadesAvaliativaEBimestres = new List<AtividadeAvaliativa>();
            // Carrega disciplinas filhas da disciplina passada como parametro
            var disciplinasProfessor = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(filtro.TurmaCodigo, true);
            var disciplinasFilha = disciplinasProfessor.Where(d => d.CdComponenteCurricularPai == long.Parse(filtro.DisciplinaCodigo));

            if (disciplinasFilha.Any())
            {
                foreach (var disciplinaFilha in disciplinasFilha)
                    atividadesAvaliativaEBimestres.AddRange(await consultasAtividadeAvaliativa.ObterAvaliacoesNoBimestre(filtro.TurmaCodigo, disciplinaFilha.CodigoComponenteCurricular.ToString(), periodoAtual.PeriodoInicio, periodoAtual.PeriodoFim));
            }
            else
                // Disciplina não tem disciplinas filhas então carrega avaliações da propria
                atividadesAvaliativaEBimestres.AddRange(await consultasAtividadeAvaliativa.ObterAvaliacoesNoBimestre(filtro.TurmaCodigo, filtro.DisciplinaCodigo, periodoAtual.PeriodoInicio, periodoAtual.PeriodoFim));

            var alunos = await mediator.Send(new ObterAlunosEolPorTurmaQuery(filtro.TurmaCodigo));
            if (alunos.EhNulo() || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");

            var retorno = new NotasConceitosRetornoDto();

            var tipoAvaliacaoBimestral = await repositorioTipoAvaliacao.ObterTipoAvaliacaoBimestral();

            retorno.BimestreAtual = bimestre.Value;
            retorno.MediaAprovacaoBimestre = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, DateTimeExtension.HorarioBrasilia().Year)));
            retorno.MinimoAvaliacoesBimestrais = tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre;
            retorno.PercentualAlunosInsuficientes = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTimeExtension.HorarioBrasilia().Year)));

            DateTime? dataUltimaNotaConceitoInserida = null;
            DateTime? dataUltimaNotaConceitoAlterada = null;
            var usuarioRfUltimaNotaConceitoInserida = string.Empty;
            var usuarioRfUltimaNotaConceitoAlterada = string.Empty;
            var nomeAvaliacaoAuditoriaInclusao = string.Empty;
            var nomeAvaliacaoAuditoriaAlteracao = string.Empty;

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            foreach (var periodoEscolar in periodosEscolares)
            {
                AtividadeAvaliativa atividadeAvaliativaParaObterTipoNota = null;
                var valorBimestreAtual = periodoEscolar.Bimestre;
                var bimestreParaAdicionar = new NotasConceitosBimestreRetornoDto()
                {
                    Descricao = $"{valorBimestreAtual}º Bimestre",
                    Numero = valorBimestreAtual,
                    PeriodoInicio = periodoEscolar.PeriodoInicio,
                    PeriodoFim = periodoEscolar.PeriodoFim
                };

                if (valorBimestreAtual == periodoAtual.Bimestre)
                {
                    var listaAlunosDoBimestre = new List<NotasConceitosAlunoRetornoDto>();

                    var atividadesAvaliativasdoBimestre = atividadesAvaliativaEBimestres
                        .Where(a => a.DataAvaliacao.Date >= periodoAtual.PeriodoInicio.Date && periodoAtual.PeriodoFim.Date >= a.DataAvaliacao.Date)
                        .OrderBy(a => a.DataAvaliacao)
                        .ToList();

                    var alunosIds = alunos.Select(a => a.CodigoAluno).Distinct();

                    IEnumerable<long> atividadesAvaliativas = atividadesAvaliativasdoBimestre.Select(a => a.Id)?.Distinct();
                    IEnumerable<NotaConceito> notas = null;

                    if (atividadesAvaliativaEBimestres.NaoEhNulo() && atividadesAvaliativaEBimestres.Any())
                        notas = repositorioNotasConceitos
                            .ObterNotasPorAlunosAtividadesAvaliativas(atividadesAvaliativas, alunosIds, filtro.DisciplinaCodigo);

                    var ausenciasAtividadesAvaliativas = await repositorioFrequencia
                        .ObterAusencias(filtro.TurmaCodigo, filtro.DisciplinaCodigo, atividadesAvaliativasdoBimestre.Select(a => a.DataAvaliacao).Distinct().ToArray(), alunosIds.ToArray());

                    var disciplinaEOL = await mediator.Send(new ObterComponenteCurricularPorIdQuery(long.Parse(filtro.DisciplinaCodigo)));
                    if (disciplinaEOL.EhNulo())
                        throw new NegocioException("Componente curricular informado não encontrado no EOL");
                    
                    IEnumerable<DisciplinaResposta> disciplinasRegencia = null;
                    if (disciplinaEOL.Regencia)
                    {
                        if (usuario.EhProfessorCj())
                        {
                            IEnumerable<DisciplinaDto> disciplinasRegenciaCJ = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(long.Parse(filtro.DisciplinaCodigo), filtro.TurmaCodigo, false, disciplinaEOL.Regencia);
                            if (disciplinasRegenciaCJ.EhNulo() || !disciplinasRegenciaCJ.Any())
                                throw new NegocioException("Não foram encontradas as disciplinas de regência");
                            disciplinasRegencia = MapearParaDto(disciplinasRegenciaCJ);
                        }
                        else
                        {
                            IEnumerable<ComponenteCurricularEol> disciplinasRegenciaEol = await mediator.Send(new ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery(filtro.TurmaCodigo, servicoUsuario.ObterLoginAtual(), servicoUsuario.ObterPerfilAtual()));
                            if (disciplinasRegenciaEol.EhNulo() || !disciplinasRegenciaEol.Any(x => x.Regencia))
                                throw new NegocioException("Não foram encontradas disciplinas de regência no EOL");
                            disciplinasRegencia = MapearParaDto(disciplinasRegenciaEol.Where(x => x.Regencia));
                        }
                    }

                    var fechamentosTurma = await consultasFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplina(filtro.TurmaCodigo, filtro.DisciplinaCodigo, valorBimestreAtual);

                    var alunosForeach = from a in alunos
                                        where (a.EstaAtivo(periodoAtual.PeriodoFim) ||
                                              (a.EstaInativo(periodoAtual.PeriodoFim) && a.DataSituacao.Date >= periodoAtual.PeriodoInicio.Date)) &&
                                              a.DataMatricula.Date <= periodoAtual.PeriodoFim.Date
                                        orderby a.NomeValido(), a.NumeroAlunoChamada
                                        select a;

                    foreach (var aluno in alunosForeach)
                    {
                        var alunoPossuiPlanoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, filtro.AnoLetivo));
                        var notaConceitoAluno = new NotasConceitosAlunoRetornoDto()
                        {
                            Id = aluno.CodigoAluno,
                            Nome = aluno.NomeValido(),
                            NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                            EhAtendidoAEE = alunoPossuiPlanoAEE
                        };
                        var notasAvaliacoes = new List<NotasConceitosNotaAvaliacaoRetornoDto>();

                        foreach (var atividadeAvaliativa in atividadesAvaliativasdoBimestre)
                        {
                            var notaDoAluno = ObterNotaParaVisualizacao(notas, aluno, atividadeAvaliativa);
                            var notaParaVisualizar = string.Empty;

                            if (notaDoAluno.NaoEhNulo())
                            {
                                notaParaVisualizar = notaDoAluno.ObterNota();

                                if (!dataUltimaNotaConceitoInserida.HasValue || notaDoAluno.CriadoEm > dataUltimaNotaConceitoInserida.Value)
                                {
                                    usuarioRfUltimaNotaConceitoInserida = $"{notaDoAluno.CriadoPor}({notaDoAluno.CriadoRF})";
                                    dataUltimaNotaConceitoInserida = notaDoAluno.CriadoEm;
                                    nomeAvaliacaoAuditoriaInclusao = atividadeAvaliativa.NomeAvaliacao;
                                }

                                if (notaDoAluno.AlteradoEm.HasValue && (!dataUltimaNotaConceitoAlterada.HasValue || notaDoAluno.AlteradoEm.Value > dataUltimaNotaConceitoAlterada.Value))
                                {
                                    usuarioRfUltimaNotaConceitoAlterada = $"{notaDoAluno.AlteradoPor}({notaDoAluno.AlteradoRF})";
                                    dataUltimaNotaConceitoAlterada = notaDoAluno.AlteradoEm;
                                    nomeAvaliacaoAuditoriaAlteracao = atividadeAvaliativa.NomeAvaliacao;
                                }
                            }

                            var ausente = ausenciasAtividadesAvaliativas.Any(a => a.AlunoCodigo == aluno.CodigoAluno && a.AulaData.Date == atividadeAvaliativa.DataAvaliacao.Date);
                            var notaAvaliacao = new NotasConceitosNotaAvaliacaoRetornoDto()
                            {
                                AtividadeAvaliativaId = atividadeAvaliativa.Id,
                                NotaConceito = notaParaVisualizar,
                                Ausente = ausente,
                                PodeEditar = aluno.EstaAtivo(atividadeAvaliativa.DataAvaliacao) ||
                                             (aluno.EstaInativo(atividadeAvaliativa.DataAvaliacao) && atividadeAvaliativa.DataAvaliacao.Date <= aluno.DataSituacao.Date)
                            };
                            notasAvaliacoes.Add(notaAvaliacao);
                        }

                        notaConceitoAluno.PodeEditar = notasAvaliacoes.Any(na => na.PodeEditar) || (atividadesAvaliativaEBimestres is null || !atividadesAvaliativaEBimestres.Any());

                        notaConceitoAluno.Marcador = servicoAluno.ObterMarcadorAluno(aluno, new PeriodoEscolar()
                        {
                            Bimestre = valorBimestreAtual,
                            PeriodoInicio = periodoAtual.PeriodoInicio,
                            PeriodoFim = periodoAtual.PeriodoFim
                        });

                        notaConceitoAluno.NotasAvaliacoes = notasAvaliacoes;

                        var fechamentoTurma = (from ft in fechamentosTurma
                                               from fa in ft.FechamentoAlunos
                                               where fa.AlunoCodigo.Equals(aluno.CodigoAluno)
                                               select ft).FirstOrDefault();

                        // Carrega Notas do Bimestre
                        if (fechamentoTurma.NaoEhNulo())
                        {
                            bimestreParaAdicionar.FechamentoTurmaId = fechamentoTurma.Id;
                            bimestreParaAdicionar.Situacao = fechamentoTurma.Situacao;

                            var notasConceitoBimestre = await consultasFechamentoTurmaDisciplina.ObterNotasBimestre(aluno.CodigoAluno, fechamentoTurma.Id);

                            retorno.AuditoriaBimestreInserido = $"Nota final do bimestre inserida por {fechamentoTurma.CriadoPor}({fechamentoTurma.CriadoRF}) em {fechamentoTurma.CriadoEm.ToString("dd/MM/yyyy")}, às {fechamentoTurma.CriadoEm.ToString("HH:mm")}.";
                            if (notasConceitoBimestre.Any())
                            {
                                var dadosAuditoriaAlteracaoBimestre = notasConceitoBimestre
                                    .OrderByDescending(nc => nc.AlteradoEm)
                                    .ThenBy(nc => nc.CriadoEm)
                                    .Select(nc => new
                                    {
                                        AlteradoPor = !string.IsNullOrWhiteSpace(nc.AlteradoPor) && !nc.AlteradoRf.Equals(0) ? nc.AlteradoPor : "",
                                        AlteradoRf = !string.IsNullOrWhiteSpace(nc.AlteradoRf) && !nc.AlteradoRf.Equals(0) ? nc.AlteradoRf : "",
                                        AlteradoEm = nc.AlteradoEm.HasValue && !nc.AlteradoRf.Equals(0) ? nc.AlteradoEm.Value : DateTime.MinValue,
                                    }).First();

                                if (!string.IsNullOrEmpty(dadosAuditoriaAlteracaoBimestre.AlteradoRf))
                                    retorno.AuditoriaBimestreAlterado = $"Nota final do bimestre alterada por {(dadosAuditoriaAlteracaoBimestre.AlteradoPor)}({dadosAuditoriaAlteracaoBimestre.AlteradoRf}) em {dadosAuditoriaAlteracaoBimestre.AlteradoEm.ToString("dd/MM/yyyy")}, às {dadosAuditoriaAlteracaoBimestre.AlteradoEm.ToString("HH:mm")}.";
                            }

                            if (disciplinaEOL.Regencia)
                            {
                                // Regencia carrega disciplinas mesmo sem nota de fechamento
                                foreach (var disciplinaRegencia in disciplinasRegencia)
                                {
                                    var nota = new FechamentoNotaRetornoDto()
                                    {
                                        DisciplinaId = disciplinaRegencia.CodigoComponenteCurricular,
                                        Disciplina = disciplinaRegencia.Nome,
                                    };
                                    var notaRegencia = notasConceitoBimestre?.FirstOrDefault(c => c.DisciplinaId == disciplinaRegencia.CodigoComponenteCurricular);
                                    if (notaRegencia.NaoEhNulo())
                                    {
                                        nota.NotaConceito = (notaRegencia.ConceitoId.HasValue ? notaRegencia.ConceitoId.Value : notaRegencia.Nota);
                                        nota.EhConceito = notaRegencia.ConceitoId.HasValue;
                                    }

                                    notaConceitoAluno.NotasBimestre.Add(nota);
                                }
                            }
                            else
                                foreach (var notaConceitoBimestre in notasConceitoBimestre)
                                    notaConceitoAluno.NotasBimestre.Add(new FechamentoNotaRetornoDto()
                                    {
                                        DisciplinaId = notaConceitoBimestre.DisciplinaId,
                                        Disciplina = disciplinaEOL.Nome,
                                        NotaConceito = notaConceitoBimestre.ConceitoId.HasValue ?
                                            notaConceitoBimestre.ConceitoId.Value :
                                            notaConceitoBimestre.Nota,
                                        EhConceito = notaConceitoBimestre.ConceitoId.HasValue
                                    });
                        }
                        else if (disciplinaEOL.Regencia && disciplinasRegencia.NaoEhNulo())
                        {
                            // Regencia carrega disciplinas mesmo sem nota de fechamento
                            foreach (var disciplinaRegencia in disciplinasRegencia)
                            {
                                notaConceitoAluno.NotasBimestre.Add(new FechamentoNotaRetornoDto()
                                {
                                    DisciplinaId = disciplinaRegencia.CodigoComponenteCurricular,
                                    Disciplina = disciplinaRegencia.Nome,
                                });
                            } 
                        }

                        // Carrega Frequencia Aluno
                        var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodoConsulta.ObterPorAlunoData(aluno.CodigoAluno, periodoAtual.PeriodoFim, TipoFrequenciaAluno.PorDisciplina, filtro.DisciplinaCodigo);
                        notaConceitoAluno.PercentualFrequencia = frequenciaAluno.EhNulo() ? null : frequenciaAluno.PercentualFrequenciaFormatado;

                        listaAlunosDoBimestre.Add(notaConceitoAluno);
                    }

                    foreach (var avaliacao in atividadesAvaliativasdoBimestre)
                    {
                        var avaliacaoDoBimestre = new NotasConceitosAvaliacaoRetornoDto()
                        {
                            Id = avaliacao.Id,
                            Data = avaliacao.DataAvaliacao,
                            Descricao = avaliacao.DescricaoAvaliacao,
                            Nome = avaliacao.NomeAvaliacao,
                            EhCJ = avaliacao.EhCj
                        };
                        if (avaliacao.Categoria.Equals(CategoriaAtividadeAvaliativa.Interdisciplinar))
                        {
                            avaliacaoDoBimestre.EhInterdisciplinar = true;
                            var atividadeDisciplinas = await repositorioAtividadeAvaliativaDisciplina.ListarPorIdAtividade(avaliacao.Id);
                            var idsDisciplinas = atividadeDisciplinas.Select(a => long.Parse(a.DisciplinaId)).ToArray();
                            var disciplinas = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(idsDisciplinas));
                            var nomesDisciplinas = disciplinas.Select(d => d.Nome).ToArray();
                            avaliacaoDoBimestre.Disciplinas = nomesDisciplinas;
                        }
                        bimestreParaAdicionar.Avaliacoes.Add(avaliacaoDoBimestre);

                        if (atividadeAvaliativaParaObterTipoNota.EhNulo())
                            atividadeAvaliativaParaObterTipoNota = avaliacao;
                    }
                    bimestreParaAdicionar.Alunos = listaAlunosDoBimestre;
                    bimestreParaAdicionar.QtdAvaliacoesBimestrais = atividadesAvaliativasdoBimestre.Count(x => x.TipoAvaliacaoId == tipoAvaliacaoBimestral.Id);
                    bimestreParaAdicionar.PodeLancarNotaFinal = await mediator.Send(new ObterTurmaEmPeriodoDeFechamentoQuery(turma, DateTimeExtension.HorarioBrasilia(), periodoAtual.Bimestre));

                    // Valida Avaliações Bimestrais
                    await ValidaMinimoAvaliacoesBimestrais(disciplinaEOL, disciplinasRegencia, tipoCalendario.Id, filtro.TurmaCodigo, valorBimestreAtual, tipoAvaliacaoBimestral, bimestreParaAdicionar);

                    if (atividadeAvaliativaParaObterTipoNota.NaoEhNulo())
                    {
                        var notaTipo = await servicoDeNotasConceitos.TipoNotaPorAvaliacao(atividadeAvaliativaParaObterTipoNota, filtro.TurmaHistorico);
                        if (notaTipo.EhNulo())
                            throw new NegocioException("Não foi possível obter o tipo de nota desta avaliação.");

                        retorno.NotaTipo = notaTipo.TipoNota;
                        CarregarValoresDeAuditoriaInserida(dataUltimaNotaConceitoInserida, usuarioRfUltimaNotaConceitoInserida, notaTipo.TipoNota, retorno, nomeAvaliacaoAuditoriaInclusao);
                        CarregarValoresAuditoriaAlterada(dataUltimaNotaConceitoAlterada, usuarioRfUltimaNotaConceitoAlterada, notaTipo.TipoNota, retorno, nomeAvaliacaoAuditoriaAlteracao);
                    }
                    else
                    {
                        var tipoNota = await ObterNotaTipo(long.Parse(filtro.TurmaCodigo), filtro.AnoLetivo, filtro.TurmaHistorico);
                        retorno.NotaTipo = tipoNota;
                    }
                }

                retorno.Bimestres.Add(bimestreParaAdicionar);
            }

            return retorno;
        }

        public async Task<IEnumerable<ConceitoDto>> ObterConceitos(DateTime data)
            => MapearParaDto(await mediator.Send(new ObterConceitoPorDataQuery(data)));

        public async Task<TipoNota> ObterNotaTipo(long turmaId, int anoLetivo, bool consideraHistorico)
        {
            var turmaEOL = await mediator.Send(new ObterDadosTurmaEolPorCodigoQuery(turmaId.ToString()));

            // Para turma tipo 2 o padrão é nota.
            if (turmaEOL.TipoTurma == TipoTurma.EdFisica)
                return TipoNota.Nota;

            if (await ModalidadeTurmaEhCelp(turmaEOL)) 
                return TipoNota.Conceito;

            var notaTipo = await servicoDeNotasConceitos.TipoNotaPorAvaliacao(new AtividadeAvaliativa()
            {
                TurmaId = turmaId.ToString(),
                DataAvaliacao = new DateTime(anoLetivo, 3, 1)
            }, consideraHistorico);

            return notaTipo.TipoNota;
        }

        public async Task<double> ObterValorArredondado(long atividadeAvaliativaId, double nota)
        {
            var atividadeAvaliativa = await repositorioAtividadeAvaliativa.ObterPorIdAsync(atividadeAvaliativaId);
            if (atividadeAvaliativa.EhNulo())
                throw new NegocioException("Não foi possível localizar a atividade avaliativa.");

            return await ObterValorArredondado(atividadeAvaliativa.DataAvaliacao, nota);
        }

        public async Task<double> ObterValorArredondado(DateTime data, double nota)
        {
            var notaParametro = await repositorioNotaParametro.ObterPorDataAvaliacao(data);
            if (notaParametro.EhNulo())
                throw new NegocioException("Não foi possível localizar o parâmetro da nota.");

            return notaParametro.Arredondar(nota);
        }

        private static NotaConceito ObterNotaParaVisualizacao(IEnumerable<NotaConceito> notas, AlunoPorTurmaResposta aluno, AtividadeAvaliativa atividadeAvaliativa)
        {
            var notaDoAluno = notas.FirstOrDefault(a => a.AlunoId == aluno.CodigoAluno && a.AtividadeAvaliativaID == atividadeAvaliativa.Id);

            return notaDoAluno;
        }

        private IEnumerable<ConceitoDto> MapearParaDto(IEnumerable<Conceito> conceitos)
        {
            foreach (var conceito in conceitos)
            {
                yield return new ConceitoDto()
                {
                    Id = conceito.Id,
                    Valor = conceito.Valor,
                    Descricao = conceito.Descricao,
                    Aprovado = conceito.Aprovado
                };
            }
        }

        private IEnumerable<DisciplinaResposta> MapearParaDto(IEnumerable<ComponenteCurricularEol> disciplinasRegenciaEol)
        {
            foreach (var disciplina in disciplinasRegenciaEol)
            {
                yield return new DisciplinaResposta()
                {
                    CodigoComponenteCurricular = disciplina.Codigo,
                    Compartilhada = disciplina.Compartilhada,
                    CodigoComponenteCurricularPai = disciplina.CodigoComponenteCurricularPai,
                    Nome = disciplina.Descricao,
                    Regencia = disciplina.Regencia,
                    RegistroFrequencia = disciplina.RegistraFrequencia,
                    TerritorioSaber = disciplina.TerritorioSaber,
                    LancaNota = disciplina.LancaNota,
                    BaseNacional = disciplina.BaseNacional,
                    GrupoMatriz = disciplina.GrupoMatriz.NaoEhNulo() ?
                        new Integracoes.Respostas.GrupoMatriz { Id = disciplina.GrupoMatriz.Id, Nome = disciplina.GrupoMatriz.Nome } :
                        new Integracoes.Respostas.GrupoMatriz()
                };
            }
        }

        private IEnumerable<DisciplinaResposta> MapearParaDto(IEnumerable<DisciplinaDto> disciplinasRegenciaCJ)
        {
            foreach (var disciplina in disciplinasRegenciaCJ)
            {
                yield return new DisciplinaResposta()
                {
                    CodigoComponenteCurricular = disciplina.CodigoComponenteCurricular,
                    Compartilhada = disciplina.Compartilhada,
                    CodigoComponenteCurricularPai = disciplina.CdComponenteCurricularPai,
                    Nome = disciplina.Nome,
                    Regencia = disciplina.Regencia,
                    RegistroFrequencia = disciplina.RegistraFrequencia,
                    TerritorioSaber = disciplina.TerritorioSaber,
                    LancaNota = disciplina.LancaNota,
                    GrupoMatriz = new Integracoes.Respostas.GrupoMatriz { Id = disciplina.GrupoMatrizId, Nome = disciplina.GrupoMatrizNome }
                };
            }
        }

        private int ObterBimestreAtual(IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dataPesquisa = DateTime.Now;

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);

            if (periodoEscolar.EhNulo())
                return 1;
            else return periodoEscolar.Bimestre;
        }

        private void CarregarValoresAuditoriaAlterada(DateTime? dataUltimaNotaConceitoAlterada, string usuarioAlterou, TipoNota tipoNota, NotasConceitosRetornoDto notasConceitosRetornoDto, string nomeAvaliacaoAlteracao)
        {
            if (dataUltimaNotaConceitoAlterada.HasValue)
                notasConceitosRetornoDto.AuditoriaAlterado = $"{ObterDescricaoNota(tipoNota)} da avaliação {nomeAvaliacaoAlteracao} alterados por {usuarioAlterou} em {dataUltimaNotaConceitoAlterada.Value.ToString("dd/MM/yyyy")}, às {dataUltimaNotaConceitoAlterada.Value.ToString("HH:mm")}.";
        }

        private void CarregarValoresDeAuditoriaInserida(DateTime? dataUltimaNotaConceitoInserida, string usuarioInseriu, TipoNota tipoNota, NotasConceitosRetornoDto notasConceitosRetornoDto, string nomeAvaliacaoInclusao)
        {
            if (dataUltimaNotaConceitoInserida.HasValue)
                notasConceitosRetornoDto.AuditoriaInserido = $"{ObterDescricaoNota(tipoNota)} da avaliação {nomeAvaliacaoInclusao} inseridos por {usuarioInseriu} em {dataUltimaNotaConceitoInserida.Value.ToString("dd/MM/yyyy")}, às {dataUltimaNotaConceitoInserida.Value.ToString("HH:mm")}.";
        }

        private string ObterDescricaoNota(TipoNota tipoNota)
        {
            return tipoNota == TipoNota.Conceito ? "Conceitos" : "Notas";
        }

        private async Task ValidaMinimoAvaliacoesBimestrais(DisciplinaDto disciplinaEOL, IEnumerable<DisciplinaResposta> disciplinasRegencia, long tipoCalendarioId, string turmaCodigo, int bimestre, TipoAvaliacao tipoAvaliacaoBimestral, NotasConceitosBimestreRetornoDto bimestreDto)
        {
            if (disciplinaEOL.Regencia)
            {
                var disciplinasObservacao = new List<string>();
                foreach (var disciplinaRegencia in disciplinasRegencia)
                {
                    var avaliacoes = await repositorioAtividadeAvaliativaRegencia.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaCodigo, disciplinaRegencia.CodigoComponenteCurricular.ToString(), bimestre);
                    if ((avaliacoes.EhNulo()) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                        disciplinasObservacao.Add(disciplinaRegencia.Nome);
                }
                if (disciplinasObservacao.Count > 0)
                    bimestreDto.Observacoes.Add($"O(s) componente(s) curricular(es) [{string.Join(",", disciplinasObservacao)}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
            else
            {
                var avaliacoes = await repositorioAtividadeAvaliativaDisciplina.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaCodigo, disciplinaEOL.CodigoComponenteCurricular.ToString(), bimestre);
                if ((avaliacoes.EhNulo()) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                    bimestreDto.Observacoes.Add($"O componente curricular [{disciplinaEOL.Nome}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
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
    }
}