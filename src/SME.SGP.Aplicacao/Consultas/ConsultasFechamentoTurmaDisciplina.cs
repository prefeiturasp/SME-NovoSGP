using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoTurmaDisciplina : IConsultasFechamentoTurmaDisciplina
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasTurma consultasTurma;
        private readonly IRepositorioConceitoConsulta repositorioConceito;
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioSintese repositorioSintese;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;
        private const int PRIMEIRO_BIMESTRE = 1;

        public ConsultasFechamentoTurmaDisciplina(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina,
            IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
            IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
            IConsultasPeriodoEscolar consultasPeriodoEscolar,
            IServicoUsuario servicoUsuario,
            IServicoAluno servicoAluno,
            IRepositorioConceitoConsulta repositorioConceito,
            IRepositorioSintese repositorioSintese,
            IConsultasDisciplina consultasDisciplina,
            IConsultasPeriodoFechamento consultasPeriodoFechamento,
            IConsultasTurma consultasTurma,
            IMediator mediator
            )
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoAluno = servicoAluno ?? throw new ArgumentNullException(nameof(servicoAluno));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.repositorioSintese = repositorioSintese ?? throw new ArgumentNullException(nameof(repositorioSintese));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public IEnumerable<Sintese> _sinteses { get; set; }

        public IEnumerable<Sintese> Sinteses
        {
            get
            {
                if (_sinteses.EhNulo())
                    _sinteses = repositorioSintese.Listar();

                return _sinteses;
            }
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> ObterDadosAlunos(string turmaCodigo, int anoLetivo, int semestre)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            var periodosAberto = await consultasPeriodoFechamento
                .ObterPeriodosComFechamentoEmAberto(turma.UeId, turma.AnoLetivo);

            var tipoCalendario = await mediator.Send(new ObterTipoDeCalendarioDaTurmaQuery { Turma = turma });

            if (tipoCalendario.EhNulo())
                throw new NegocioException("Não foi encontrado calendário cadastrado para a turma");

            var periodosEscolares = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));

            if (periodosEscolares.EhNulo())
                throw new NegocioException("Não foram encontrados periodos escolares cadastrados para a turma");

            DateTime primeiroPeriodoDoCalendario = periodosEscolares.Where(p => p.Bimestre == PRIMEIRO_BIMESTRE).Select(pe => pe.PeriodoInicio).FirstOrDefault();


            PeriodoEscolar periodoEscolar;
            if (periodosAberto.NaoEhNulo() && periodosAberto.Any())
            {
                // caso tenha mais de um periodo em aberto (abertura e reabertura) usa o ultimo bimestre
                periodoEscolar = periodosAberto.OrderBy(c => c.Bimestre).Last();
            }
            else
            {
                // Caso não esteja em periodo de fechamento ou escolar busca o ultimo existente

                periodoEscolar = consultasPeriodoEscolar
                    .ObterPeriodoPorData(periodosEscolares, DateTimeExtension.HorarioBrasilia().Date);

                if (periodoEscolar.EhNulo())
                    periodoEscolar = consultasPeriodoEscolar.ObterUltimoPeriodoPorData(periodosEscolares, DateTimeExtension.HorarioBrasilia().Date);
            }

            var dadosAlunos = await consultasTurma.ObterDadosAlunos(turmaCodigo, anoLetivo, periodoEscolar, turma.EhTurmaInfantil);

            var dadosAlunosFiltrados = dadosAlunos.Where(d => !d.EstaInativo() || d.EstaInativo() && d.DataSituacao >= primeiroPeriodoDoCalendario).OrderBy(d => d.Nome);
            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(dadosAlunosFiltrados.Select(x => x.CodigoEOL).ToArray(), anoLetivo);
            
            var listaRetorno = dadosAlunosFiltrados.OrderBy(aluno => aluno.CodigoEOL)
                                       .ThenByDescending(aluno => aluno.DataSituacao)
                                       .GroupBy(aluno => aluno.CodigoEOL)
                                       .Select(aluno => aluno.First())
                                       .OrderBy(aluno => aluno.Nome)
                                       .ToList();
            
            return MapearAlunoPap(listaRetorno,matriculadosTurmaPAP);
        }

        private IEnumerable<AlunoDadosBasicosDto> MapearAlunoPap(List<AlunoDadosBasicosDto> listaAlunos, IEnumerable<AlunosTurmaProgramaPapDto> matriculadosTurmaPAP)
        {
            foreach (var aluno in listaAlunos)
            {
                aluno.EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoEOL);
            }

            return listaAlunos;
        }

        public async Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int bimestre)
                    => await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(turmaId, disciplinaId, bimestre);

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplina(string turmaCodigo, string disciplinaId, int bimestre, long? tipoCalendario = null)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));

            return await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turma.Id, new long[] { Convert.ToInt64(disciplinaId) }, bimestre, tipoCalendario);
        }

        public async Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId)
            => await repositorioFechamentoTurmaDisciplina.ObterNotasBimestre(codigoAluno, fechamentoTurmaId);

        public async Task<FechamentoTurmaDisciplinaBimestreDto> ObterNotasFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int? bimestre, int semestre)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));

            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeCodigo.ObterModalidadeTipoCalendario(), semestre);
            if (tipoCalendario.EhNulo())
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            var bimestreAtual = bimestre;
            if (!bimestreAtual.HasValue || bimestre == 0)
                bimestreAtual = ObterBimestreAtual(periodosEscolares);

            var periodoAtual = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestreAtual);
            if (periodoAtual.EhNulo())
                throw new NegocioException("Não foi encontrado período escolar para o bimestre solicitado.");

            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turmaId, turma.AnoLetivo));
            if (alunos.EhNulo() || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");

            var fechamentoBimestre = new FechamentoTurmaDisciplinaBimestreDto()
            {
                Bimestre = bimestreAtual.Value,
                Periodo = tipoCalendario.Periodo,
                TotalAulasDadas = 0,
                TotalAulasPrevistas = 0,
                Alunos = new List<NotaConceitoAlunoBimestreDto>()
            };

            var codigosDisciplinas = new List<string>() { disciplinaId.ToString() };

            var usuarioRF = (string)null;

            var disciplina = await consultasDisciplina.ObterDisciplina(disciplinaId);
            IEnumerable<ComponenteCurricularEol> disciplinasRegenciaEOL = null;

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            if (disciplina.Regencia)
                disciplinasRegenciaEOL = await mediator.Send(new ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery(turmaId, usuarioLogado.Login, usuarioLogado.PerfilAtual));

            fechamentoBimestre.EhSintese = !disciplina.LancaNota;

            // Carrega fechamento da Turma x Disciplina x Bimestre  
            var fechamentosTurma = await ObterFechamentosTurmaDisciplina(turmaId, disciplinaId.ToString(), bimestreAtual.Value, tipoCalendario.Id);

            if ((fechamentosTurma.NaoEhNulo() && fechamentosTurma.Any()) || fechamentoBimestre.EhSintese)
            {
                if (fechamentosTurma.NaoEhNulo() && fechamentosTurma.Any())
                {
                    fechamentoBimestre.Situacao = fechamentosTurma.First().Situacao;
                    fechamentoBimestre.SituacaoNome = fechamentosTurma.First().Situacao.Name();
                    fechamentoBimestre.FechamentoId = fechamentosTurma.First().Id;
                    fechamentoBimestre.DataFechamento = fechamentosTurma.First().AlteradoEm ?? fechamentosTurma.First().CriadoEm;
                }

                fechamentoBimestre.Alunos = new List<NotaConceitoAlunoBimestreDto>();

                var exigeAprovacao = await mediator.Send(new ExigeAprovacaoDeNotaQuery(turma));

                var bimestreDoPeriodo = await consultasPeriodoEscolar.ObterPeriodoEscolarPorData(tipoCalendario.Id, periodoAtual.PeriodoFim);

                alunos = alunos.Where(a => a.VerificaSeMatriculaEstaDentroDoPeriodoSelecionado(bimestreDoPeriodo.PeriodoFim));

                var alunosValidosComOrdenacao = alunos.Where(a => a.EstaAtivo(bimestreDoPeriodo.PeriodoInicio, bimestreDoPeriodo.PeriodoFim))
                    .GroupBy(a => a.CodigoAluno)
                    .Select(a => a.OrderByDescending(i => i.DataSituacao).First())
                    .OrderBy(a => a.NomeAluno)
                    .ThenBy(a => a.NomeValido());

                var turmaPossuiFrequenciaRegistrada = await mediator
                    .Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, codigosDisciplinas.ToArray(), bimestreDoPeriodo.Id));

                var codigosAlunos = alunosValidosComOrdenacao.Select(c => c.CodigoAluno).Distinct().ToArray();

                var fechamentosTurmasAlunos = (from ft in fechamentosTurma
                                               from fa in ft.FechamentoAlunos
                                               where codigosAlunos.Contains(fa.AlunoCodigo)
                                               select new { ft.Id, fa.FechamentoTurmaDisciplinaId, ft.FechamentoTurmaId, ft.DisciplinaId, fa.AlunoCodigo });

                var fechamentosTurmasDisciplinasIds = fechamentosTurmasAlunos.Select(c => c.FechamentoTurmaDisciplinaId).Distinct().ToArray();

                var anotacoesAlunos = await mediator.Send(new ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQuery(fechamentosTurmasDisciplinasIds, codigosAlunos));

                var fechamentosIds = fechamentosTurmasAlunos.Select(x => x.Id).Distinct().ToArray();

                var notasConceitoBimestreRetorno = await mediator.Send(new ObterNotaBimestrePorCodigosAlunosIdsFechamentoQuery(codigosAlunos, fechamentosIds));

                var planosAEE = await mediator.Send(new VerificaPlanosAEEPorCodigosAlunosEAnoQuery(codigosAlunos, turma.AnoLetivo));
                var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(codigosAlunos, turma.AnoLetivo);
                foreach (var aluno in alunosValidosComOrdenacao)
                {
                    var fechamentoTurma = fechamentosTurmasAlunos.FirstOrDefault(c => c.AlunoCodigo == aluno.CodigoAluno);

                    var alunoDto = new NotaConceitoAlunoBimestreDto
                    {
                        CodigoAluno = aluno.CodigoAluno,
                        NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                        Nome = aluno.NomeAluno,
                        Ativo = aluno.EstaAtivo(periodoAtual.PeriodoFim),
                        EhAtendidoAEE = planosAEE.Any(x => x.CodigoAluno == aluno.CodigoAluno),
                        EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno)
                    };

                    var anotacaoAluno = anotacoesAlunos.FirstOrDefault(c => c.FechamentoAluno.FechamentoTurmaDisciplinaId == fechamentoTurma?.FechamentoTurmaDisciplinaId &&
                        c.FechamentoAluno.AlunoCodigo == aluno.CodigoAluno);

                    alunoDto.TemAnotacao = anotacaoAluno.NaoEhNulo() && anotacaoAluno.Anotacao.NaoEhNulo() &&
                        !string.IsNullOrEmpty(anotacaoAluno.Anotacao.Trim());

                    var marcador = servicoAluno.ObterMarcadorAluno(aluno, bimestreDoPeriodo);

                    if (marcador.NaoEhNulo())
                        alunoDto.Informacao = marcador.Descricao;                    

                    var frequenciaAluno = await mediator
                        .Send(new ObterPorAlunoDisciplinaDataQuery(aluno.CodigoAluno, codigosDisciplinas.ToArray(), periodoAtual.PeriodoFim, turmaId));

                    if (frequenciaAluno.NaoEhNulo())
                    {
                        alunoDto.QuantidadeFaltas = frequenciaAluno.TotalAusencias;
                        alunoDto.QuantidadeCompensacoes = frequenciaAluno.TotalCompensacoes;
                        alunoDto.PercentualFrequencia = frequenciaAluno.PercentualFrequenciaFormatado;
                    }
                    else
                    {
                        alunoDto.QuantidadeFaltas = 0;
                        alunoDto.QuantidadeCompensacoes = 0;
                        alunoDto.PercentualFrequencia = string.Empty;
                    }

                    // Carrega Frequencia do aluno
                    if (aluno.CodigoAluno.NaoEhNulo())
                    {
                        if (fechamentoBimestre.EhSintese && fechamentoTurma.EhNulo())
                        {
                            if (!turmaPossuiFrequenciaRegistrada)
                                throw new NegocioException("Não é possível registrar fechamento pois não há registros de frequência no bimestre.");

                            if(frequenciaAluno.NaoEhNulo())
                            {
                                var sinteseDto = await mediator.Send(new ObterSinteseAlunoQuery(frequenciaAluno.PercentualFrequencia, disciplina, turma.AnoLetivo));

                                alunoDto.SinteseId = sinteseDto.Id;
                                alunoDto.Sintese = sinteseDto.Valor;
                            }
                            else
                            {
                                alunoDto.Sintese = String.Empty;
                                alunoDto.SinteseId = null;
                            }                       
                        }
                        else
                        {
                            // Carrega notas do bimestre
                            var notasConceitoBimestre = notasConceitoBimestreRetorno.Where(x => x.CodigoAluno == aluno.CodigoAluno && x.FechamentoId == fechamentoTurma?.Id);

                            if (notasConceitoBimestre.Any())
                                alunoDto.Notas = new List<FechamentoNotaRetornoDto>();

                            // Excessão de disciplina ED. Fisica para modalidade EJA
                            if (turma.EhEJA() && notasConceitoBimestre.NaoEhNulo() && !turma.EhTurmaEdFisica())
                                notasConceitoBimestre = notasConceitoBimestre.Where(n => n.DisciplinaId != MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA);

                            if(turma.Ue.TipoEscola != TipoEscola.EMEBS && (TipoTurnoEOL)turma.TipoTurno != TipoTurnoEOL.Integral)
                                notasConceitoBimestre = notasConceitoBimestre.Where(n => n.DisciplinaId != MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_LIBRAS);

                            if (fechamentoBimestre.EhSintese)
                            {
                                var notaConceitoBimestre = notasConceitoBimestre.FirstOrDefault();

                                if (notaConceitoBimestre.NaoEhNulo() && (notaConceitoBimestre.SinteseId.HasValue && frequenciaAluno.NaoEhNulo()))
                                {
                                    alunoDto.SinteseId = (SinteseEnum)notaConceitoBimestre.SinteseId.Value;
                                    alunoDto.Sintese = ObterSintese(notaConceitoBimestre.SinteseId.Value);
                                }
                            }
                            else
                                foreach (var notaConceitoBimestre in notasConceitoBimestre)
                                {
                                    string nomeDisciplina;

                                    if (disciplina.Regencia)
                                        nomeDisciplina = disciplinasRegenciaEOL.FirstOrDefault(a => a.Codigo == notaConceitoBimestre.DisciplinaId)?.Descricao;
                                    else nomeDisciplina = disciplina.Nome;

                                    var nota = new FechamentoNotaRetornoDto()
                                    {
                                        DisciplinaId = notaConceitoBimestre.DisciplinaId,
                                        Disciplina = nomeDisciplina,
                                        NotaConceito = notaConceitoBimestre.ConceitoId.HasValue ? ObterConceito(notaConceitoBimestre.ConceitoId.Value) : notaConceitoBimestre.Nota,
                                        EhConceito = notaConceitoBimestre.ConceitoId.HasValue,
                                        ConceitoDescricao = notaConceitoBimestre.ConceitoId.HasValue ? ObterConceitoDescricao(notaConceitoBimestre.ConceitoId.Value) : string.Empty,
                                    };

                                    if (exigeAprovacao)
                                        await VerificaNotaEmAprovacao(aluno.CodigoAluno, fechamentoTurma.FechamentoTurmaId, fechamentoTurma.DisciplinaId, nota);

                                    ((List<FechamentoNotaRetornoDto>)alunoDto.Notas).Add(nota);
                                }
                        }

                        fechamentoBimestre.Alunos.Add(alunoDto);
                    }
                }
            }
            var codigosDisciplinasArray = Array.ConvertAll(codigosDisciplinas.ToArray(), long.Parse);
            var aulasPrevistas = await ObterAulasPrevistasAsync(turma, codigosDisciplinasArray, tipoCalendario.Id, bimestre);
            var aulasDadas = await mediator.Send(new ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(turma.CodigoTurma, codigosDisciplinasArray, tipoCalendario.Id, periodoAtual.Id, usuarioRF));

            var periodoAberto = await mediator.Send(new ObterTurmaEmPeriodoDeFechamentoQuery(turma, DateTimeExtension.HorarioBrasilia().Date, bimestreAtual.Value));
            
            fechamentoBimestre.Bimestre = bimestreAtual.Value;
            fechamentoBimestre.TotalAulasDadas = aulasDadas;
            fechamentoBimestre.TotalAulasPrevistas = aulasPrevistas;
            fechamentoBimestre.PodeProcessarReprocessar = UsuarioPossuiPermissaoNaTelaParaReprocessar() && periodoAberto;
            fechamentoBimestre.PeriodoAberto = periodoAberto;

            return fechamentoBimestre;
        }

        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }

        private async Task VerificaNotaEmAprovacao(string codigoAluno, long turmaFechamentoId, long disciplinaId, FechamentoNotaRetornoDto notasConceito)
        {
            double nota = await mediator.Send(new ObterNotaEmAprovacaoQuery(codigoAluno, turmaFechamentoId, disciplinaId));

            if (nota >= 0)
            {
                notasConceito.NotaConceito = nota;
                notasConceito.EmAprovacao = true;
            }
            else
            {
                notasConceito.EmAprovacao = false;
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

        private double ObterConceito(long id)
        {
            var conceito = repositorioConceito.ObterPorId(id);
            return conceito.NaoEhNulo() ? conceito.Id : 0;
        }

        private string ObterConceitoDescricao(long id)
        {
            var conceito = repositorioConceito.ObterPorId(id);
            return conceito.NaoEhNulo() ? conceito.Valor : "";
        }

        private string ObterSintese(long id)
        {
            var sintese = Sinteses.FirstOrDefault(c => c.Id == id);
            return sintese.NaoEhNulo() ? sintese.Descricao : "";
        }

        private bool UsuarioPossuiPermissaoNaTelaParaReprocessar()
        {
            var permissoesUsuario = servicoUsuario.ObterPermissoes();

            return permissoesUsuario.Any(p => p == Permissao.FB_A);
        }

        private async Task<int> ObterAulasPrevistasAsync(Turma turma, long[] componentesCurricularesId, long tipoCalendarioId, int? bimestre = null)
            => turma.ModalidadeCodigo != Modalidade.EducacaoInfantil
                ? await mediator.Send(new ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery(turma.CodigoTurma, tipoCalendarioId, componentesCurricularesId, bimestre))
                : default;

    }
}