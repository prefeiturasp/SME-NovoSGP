using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class NotaFechamentoBimestreTesteBase : TesteBaseComuns
    {
        private const string PARAMETRO_APROVACAO_ALTERACAO_NOTA_FECHAMENTO_NOME = "AprovacaoAlteracaoNotaFechamento";
        private const string PARAMETRO_APROVACAO_ALTERACAO_NOTA_FECHAMENTO_DESCRICAO = "Solicita aprovação nas alterações de notas de fechamento";
        private const string PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_REGENCIA_CLASSE_NOME = "CompensacaoAusenciaPercentualRegenciaClasse";
        private const string PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_REGENCIA_CLASSE_DESCRICAO = "Percentual de frequência onde a compensação de ausência considera abaixo do limite para regência de classe";
        private const string PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_REGENCIA_CLASSE_VALOR_75 = "75";
        private const string PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_FUND2_NOME = "CompensacaoAusenciaPercentualFund2";
        private const string PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_FUND2_DESCRICAO = "Percentual de frequência onde a compensação de ausência considera abaixo do limite para Fund2";
        private const string PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_FUND2_VALOR_50 = "50";
        private const string PARAMETRO_QUANTIDADE_DIAS_ALTERACAO_NOTA_FINAL_NOME = "QuantidadeDiasAlteracaoNotaFinal";
        private const string PARAMETRO_QUANTIDADE_DIAS_ALTERACAO_NOTA_FINAL_DESCRICAO = "Quantidade de dias para gerar notificação caso a nota final seja alterada";
        private const string PARAMETRO_QUANTIDADE_DIAS_ALTERACAO_NOTA_FINAL_VALOR_30 = "30";
        private const string PARAMETRO_MEDIA_BIMESTRE_NOME = "MediaBimestre";
        private const string PARAMETRO_MEDIA_BIMESTRE_DESCRICAO = "Média final para aprovação no bimestre";
        private const string PARAMETRO_MEDIA_BIMESTRE_VALOR_5 = "5";
        private const string PARAMETRO_APROVACAO_ALTERACAO_NOTA_CONSELHO_NOME = "AprovacaoAlteracaoNotaConselho";
        private const string PARAMETRO_APROVACAO_ALTERACAO_NOTA_CONSELHO_DESCRICAO = "Solicita aprovação nas alterações de notas do conselho";
        private const string PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_NOME = "PercentualAlunosInsuficientes";
        private const string PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_DESCRICAO = "Percentual de alunos com nota/conceito insuficientes para exigência de justificativa";
        private const string PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_VALOR_50 = "50";

        protected readonly long TIPO_AVALIACAO_CODIGO_1 = 1;
        protected readonly string AVALIACAO_NOME_1 = "Avaliação 1";

        public NotaFechamentoBimestreTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ConsolidacaoNotaAlunoCommand, bool>), typeof(ConsolidacaoNotaAlunoCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ServicosFakes.ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
               typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTodosAlunosNaTurmaQueryHandlerAnoAnteriorFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosDentroPeriodoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected class FiltroFechamentoNotaDto
        {
            public string Perfil { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public bool ConsiderarAnoAnterior { get; set; }
            public Modalidade Modalidade { get; set; }
            public string AnoTurma { get; set; }
            public TipoFrequenciaAluno TipoFrequenciaAluno { get; set; }
            public string ProfessorRf { get; set; }
            public string ComponenteCurricular { get; set; }
            public bool CriarPeriodoEscolar { get; set; } = true;
            public bool CriarPeriodoEscolarCustomizado { get; set; }
            public bool CriarPeriodoAbertura { get; set; } = true;
            public bool PeriodoEscolarValido { get; set; }
        }

        protected async Task CriarDadosBase(FiltroFechamentoNotaDto filtroFechamentoNota)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(filtroFechamentoNota.Perfil);
            await CriarUsuarios();

            await CriarTipoCalendario(filtroFechamentoNota.TipoCalendario, filtroFechamentoNota.ConsiderarAnoAnterior);
            await CriarTurma(filtroFechamentoNota.Modalidade, filtroFechamentoNota.AnoTurma, filtroFechamentoNota.ConsiderarAnoAnterior);

            await CriarParametrosNotaFechamento();

            if (filtroFechamentoNota.CriarPeriodoEscolar)
                await CriarPeriodoEscolar(filtroFechamentoNota.ConsiderarAnoAnterior);

            if (filtroFechamentoNota.CriarPeriodoEscolarCustomizado)
                await InserirPeriodoEscolarCustomizado(filtroFechamentoNota.PeriodoEscolarValido, filtroFechamentoNota.ConsiderarAnoAnterior);

            await CriarPeriodoFechamento();
            await CriarPeriodoReaberturaAnoAnterior(TIPO_CALENDARIO_1);
            await CriarFrequenciaAluno(filtroFechamentoNota.TipoFrequenciaAluno);

            await CriarSintese();
            await CrieConceitoValores();
            
            await CriarCiclo();
            await CriarNotasTipoEParametros(filtroFechamentoNota.ConsiderarAnoAnterior);
        }
        
        private async Task CriarNotasTipoEParametros(bool consideraAnoAnterior = false)
        {
            var dataBase = consideraAnoAnterior ? new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 01) : new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01);

            await InserirNaBase(new NotaTipoValor()
            {
                Ativo = true,
                InicioVigencia = dataBase,
                TipoNota = TipoNota.Nota,
                Descricao = NOTA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaTipoValor()
            {
                Ativo = true,
                InicioVigencia = dataBase,
                TipoNota = TipoNota.Conceito,
                Descricao = CONCEITO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 1,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 2,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 3,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 4,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 5,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 6,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 7,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 8,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaParametro()
            {
                Minima = 0,
                Media = 5,
                Maxima = 10,
                Incremento = 0.5,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task<NegocioException> ExecutarComandosFechamentoTurmaDisciplinaComExcecao(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma)
        {
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();

            return await Assert.ThrowsAsync<NegocioException>(async () => await comando.Salvar(fechamentoTurma));
        }

        protected async Task<NotasConceitosRetornoDto> ExecutarTeste(ListaNotasConceitosDto filtroListaNotasConceitos)
        {
            NotasConceitosRetornoDto retorno = null;

            var useCase = ServiceProvider.GetService<IObterNotasParaAvaliacoesUseCase>();

            if (useCase.NaoEhNulo())
                retorno = await useCase.Executar(filtroListaNotasConceitos);

            retorno.ShouldNotBeNull();

            return retorno;
        }

        protected async Task ExecutarTeste(IEnumerable<FechamentoTurmaDisciplinaDto> notasLancadas,
            bool gerarExcecao = false)
        {
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();

            if (gerarExcecao)
            {
                async Task doExecutarSalvar() { await comando.Salvar(notasLancadas); }
                await Should.ThrowAsync<NegocioException>(() => doExecutarSalvar());

                return;
            }

            await comando.Salvar(notasLancadas);

            var notasFechamento = ObterTodos<FechamentoTurmaDisciplina>();

            notasFechamento.ShouldNotBeNull();
            notasFechamento.ShouldNotBeEmpty();
            notasFechamento.Count.ShouldBeGreaterThanOrEqualTo(1);
        }

        protected async Task ExecutarTesteComExcecao(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma)
        {
            await ExecutarComandosFechamentoTurmaDisciplinaComExcecao(fechamentoTurma);
        }

        protected static FiltroFechamentoNotaDto ObterFiltroFechamentoNotaDto(string perfil, string anoTurma,
            bool consideraAnorAnterior = false)
        {
            return new FiltroFechamentoNotaDto()
            {
                Perfil = perfil,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = consideraAnorAnterior,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina
            };
        }

        protected async Task<IEnumerable<AuditoriaPersistenciaDto>> ExecutarTesteComValidacaoNota(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma, TipoNota tipoNota)
        {
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();
            var retorno = await comando.Salvar(fechamentoTurma);
            var fechamentoDto = fechamentoTurma.FirstOrDefault();
            var valorRetorno = retorno.FirstOrDefault();

            retorno.ShouldNotBeNull();
            valorRetorno.Mensagens.Any().ShouldBeFalse();
            (valorRetorno.MensagemConsistencia.Length > 0).ShouldBeTrue();

            ValidaFechamentoTurma(fechamentoDto, valorRetorno.Id);
            ValidaFechamentoAluno(fechamentoDto, valorRetorno.Id, tipoNota);

            return retorno;
        }

        protected FechamentoNotaDto ObterFechamentoNotaDto(string codigoAluno, long disciplina)
        {
            return new FechamentoNotaDto()
            {
                Anotacao = "",
                CodigoAluno = codigoAluno,
                DisciplinaId = disciplina,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRf = SISTEMA_CODIGO_RF
            };
        }

        protected FechamentoNotaDto ObterNotaConceito(string codigoAluno, long disciplina, long conceitoId)
        {
            var dto = ObterFechamentoNotaDto(codigoAluno, disciplina);
            dto.ConceitoId = conceitoId;

            return dto;
        }

        protected FechamentoNotaDto ObterNotaNumerica(string codigoAluno, long disciplina, long nota)
        {
            var dto = ObterFechamentoNotaDto(codigoAluno, disciplina);
            dto.Nota = nota;

            return dto;
        }

        protected void ValidaFechamentoTurma(FechamentoTurmaDisciplinaDto fechamentoDto, long id)
        {
            var listaTurmaFechamento = ObterTodos<FechamentoTurmaDisciplina>();
            listaTurmaFechamento.ShouldNotBeNull();
            var turmaFechamento = listaTurmaFechamento.FirstOrDefault(fechamento => fechamento.Id == id);
            turmaFechamento.DisciplinaId.ShouldBe(fechamentoDto.DisciplinaId);
        }

        protected List<FechamentoTurmaDisciplinaDto> ObterListaFechamentoTurma(List<FechamentoNotaDto> listaDeNota, long disciplina)
        {
            return new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = BIMESTRE_3,
                    DisciplinaId = disciplina,
                    Justificativa = "teste" ,
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = listaDeNota
                }
            };
        }

        private void ValidaFechamentoAluno(FechamentoTurmaDisciplinaDto fechamentoDto, long id, TipoNota tipoNota)
        {
            var fechamentosAlunos = ObterTodos<FechamentoAluno>().FindAll(alunos => alunos.FechamentoTurmaDisciplinaId == id);
            fechamentosAlunos.ShouldNotBeNull();
            var listaCodigoAlunoObjeto = ObterCodigosAlunosObjeto(fechamentosAlunos);
            var listaCodigoAlunoDto = ObterCodigosAlunosDto(fechamentoDto);

            listaCodigoAlunoObjeto.Except(listaCodigoAlunoDto).Count().ShouldBe(0);
            listaCodigoAlunoDto.Except(listaCodigoAlunoObjeto).Count().ShouldBe(0);

            ValidaNota(fechamentoDto, fechamentosAlunos, tipoNota);
            ValidaConsolidado(fechamentoDto, listaCodigoAlunoDto.ToList());
        }

        private void ValidaNota(FechamentoTurmaDisciplinaDto fechamentoDto, List<FechamentoAluno> fechamentosAlunos, TipoNota tipoNota)
        {
            var listaFechamentosNotas = ObterTodos<FechamentoNota>();
            listaFechamentosNotas.ShouldNotBeNull();

            foreach (var fechamentoNota in listaFechamentosNotas)
            {
                var alunoCodigo = fechamentosAlunos.FirstOrDefault(f => f.Id == fechamentoNota.FechamentoAlunoId).AlunoCodigo;
                var proposta = ObterFechamentoNotaDto(fechamentoDto, alunoCodigo);

                if (TipoNota.Nota == tipoNota)
                {
                    var atual = fechamentoNota.Nota;
                    (proposta.Nota == atual).ShouldBeTrue();
                }
                else
                {
                    var conceitoId = fechamentoNota.ConceitoId;
                    (proposta.ConceitoId == conceitoId).ShouldBeTrue();
                }
            }
        }

        private void ValidaConsolidado(FechamentoTurmaDisciplinaDto fechamentoDto, List<string> listaCodigoAlunoDto)
        {
            var listaConsolidacaoTurmaAluno = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            listaConsolidacaoTurmaAluno.ShouldNotBeNull();
            var listaConsolidadoCodigoAluno = listaConsolidacaoTurmaAluno.Select(s => s.AlunoCodigo).Distinct();
            listaConsolidadoCodigoAluno.Except(listaCodigoAlunoDto).Count().ShouldBe(0);
            listaCodigoAlunoDto.Except(listaConsolidadoCodigoAluno).Count().ShouldBe(0);

            var listaConsolidacaoTurmaAlunoNota = ObterTodos<ConselhoClasseConsolidadoTurmaAlunoNota>();
            listaConsolidacaoTurmaAlunoNota.ShouldNotBeNull();

            foreach (var consolidacaoTurmaAlunoNota in listaConsolidacaoTurmaAlunoNota.Where(w => w.ComponenteCurricularId == fechamentoDto.DisciplinaId))
            {
                var alunoRf = listaConsolidacaoTurmaAluno.FirstOrDefault(f => f.Id == consolidacaoTurmaAlunoNota.ConselhoClasseConsolidadoTurmaAlunoId).AlunoCodigo;
                var proposta = ObterFechamentoNotaDto(fechamentoDto, alunoRf);

                if (consolidacaoTurmaAlunoNota.Nota.HasValue)
                {
                    var atual = consolidacaoTurmaAlunoNota.Nota;
                    (proposta.Nota == atual).ShouldBeTrue();
                }
                else
                {
                    var conceitoId = consolidacaoTurmaAlunoNota.ConceitoId;
                    (proposta.ConceitoId == conceitoId).ShouldBeTrue();
                }
            }
        }

        private IEnumerable<string> ObterCodigosAlunosDto(FechamentoTurmaDisciplinaDto fechamentoTurma)
        {
            return fechamentoTurma.NotaConceitoAlunos.Select(aluno => aluno.CodigoAluno).Distinct();
        }

        private IEnumerable<string> ObterCodigosAlunosObjeto(List<FechamentoAluno> fechamentosAlunos)
        {
            return fechamentosAlunos.Select(s => s.AlunoCodigo).Distinct();
        }

        private FechamentoNotaDto ObterFechamentoNotaDto(FechamentoTurmaDisciplinaDto fechamentoTurma, string alunoCodigo)
        {
            return fechamentoTurma.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno.Equals(alunoCodigo));
        }

        protected async Task CriarTipoAvaliacao(TipoAvaliacaoCodigo tipoAvalicao, string descricaoAvaliacao)
        {
            await InserirNaBase(new TipoAvaliacao
            {
                Nome = descricaoAvaliacao,
                Descricao = descricaoAvaliacao,
                Situacao = true,
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = tipoAvalicao,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
        }

        protected async Task CriarAtividadeAvaliativaDisciplina(long atividadeAvaliativaId, string componenteCurricular)
        {
            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                AtividadeAvaliativaId = atividadeAvaliativaId,
                DisciplinaId = componenteCurricular,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
        }

        protected async Task CriarAtividadeAvaliativa(DateTime dataAvaliacao,
            long TipoAvaliacaoId, string nomeAvaliacao, bool ehRegencia = false,
            bool ehCj = false, string professorRf = USUARIO_PROFESSOR_CODIGO_RF_2222222)
        {
            await InserirNaBase(new AtividadeAvaliativa
            {
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                ProfessorRf = professorRf,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = TipoAvaliacaoId,
                NomeAvaliacao = nomeAvaliacao,
                DescricaoAvaliacao = nomeAvaliacao,
                DataAvaliacao = dataAvaliacao,
                EhRegencia = ehRegencia,
                EhCj = ehCj,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
        }

        private async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }

        protected async Task InserirPeriodoEscolarCustomizado(bool periodoEscolarValido = false, bool considerarAnoAnterior = false)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddYears(considerarAnoAnterior ? -1 : 0);

            await CriarPeriodoEscolar(periodoEscolarValido ? new DateTime(dataReferencia.Year, 02, 01) : dataReferencia.AddDays(-285), periodoEscolarValido ? new DateTime(dataReferencia.Year, 03, 31) : dataReferencia.AddDays(-210), BIMESTRE_1);
            await CriarPeriodoEscolar(periodoEscolarValido ? new DateTime(dataReferencia.Year, 04, 01) : dataReferencia.AddDays(-200), periodoEscolarValido ? new DateTime(dataReferencia.Year, 07, 15) : dataReferencia.AddDays(-125), BIMESTRE_2);
            await CriarPeriodoEscolar(periodoEscolarValido ? new DateTime(dataReferencia.Year, 08, 01) : dataReferencia.AddDays(-115), periodoEscolarValido ? new DateTime(dataReferencia.Year, 09, 30) : dataReferencia.AddDays(-40), BIMESTRE_3);
            await CriarPeriodoEscolar(periodoEscolarValido ? new DateTime(dataReferencia.Year, 10, 01) : dataReferencia.AddDays(-20), periodoEscolarValido ? new DateTime(dataReferencia.Year, 12, 15) : dataReferencia.AddDays(-5), BIMESTRE_4);
        }

        protected async Task InserirPeriodoAberturaCustomizado()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new PeriodoFechamento()
            { CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-209),
                FinalDoFechamento = dataReferencia.AddDays(-205)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-120),
                FinalDoFechamento = dataReferencia.AddDays(-116)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-120),
                FinalDoFechamento = dataReferencia.AddDays(-116)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_3,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-38),
                FinalDoFechamento = dataReferencia.AddDays(-34)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_4,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia,
                FinalDoFechamento = dataReferencia.AddDays(4)
            });
        }

        private async Task CriarParametrosNotaFechamento()
        {
            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_APROVACAO_ALTERACAO_NOTA_FECHAMENTO_NOME,
                Tipo = TipoParametroSistema.AprovacaoAlteracaoNotaFechamento,
                Descricao = PARAMETRO_APROVACAO_ALTERACAO_NOTA_FECHAMENTO_DESCRICAO,
                Valor = "",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });

            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_APROVACAO_ALTERACAO_NOTA_FECHAMENTO_NOME,
                Tipo = TipoParametroSistema.AprovacaoAlteracaoNotaFechamento,
                Descricao = PARAMETRO_APROVACAO_ALTERACAO_NOTA_FECHAMENTO_DESCRICAO,
                Valor = "",
                Ano = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });

            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_REGENCIA_CLASSE_NOME,
                Tipo = TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse,
                Descricao = PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_REGENCIA_CLASSE_DESCRICAO,
                Valor = PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_REGENCIA_CLASSE_VALOR_75,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });

            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_FUND2_NOME,
                Tipo = TipoParametroSistema.CompensacaoAusenciaPercentualFund2,
                Descricao = PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_FUND2_DESCRICAO,
                Valor = PARAMETRO_COMPENSACAO_AUSENCIA_PERCENTUAL_FUND2_VALOR_50,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });

            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_QUANTIDADE_DIAS_ALTERACAO_NOTA_FINAL_NOME,
                Tipo = TipoParametroSistema.QuantidadeDiasAlteracaoNotaFinal,
                Descricao = PARAMETRO_QUANTIDADE_DIAS_ALTERACAO_NOTA_FINAL_DESCRICAO,
                Valor = PARAMETRO_QUANTIDADE_DIAS_ALTERACAO_NOTA_FINAL_VALOR_30,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });

            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_MEDIA_BIMESTRE_NOME,
                Tipo = TipoParametroSistema.MediaBimestre,
                Descricao = PARAMETRO_MEDIA_BIMESTRE_DESCRICAO,
                Valor = PARAMETRO_MEDIA_BIMESTRE_VALOR_5,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });

            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_APROVACAO_ALTERACAO_NOTA_CONSELHO_NOME,
                Tipo = TipoParametroSistema.AprovacaoAlteracaoNotaConselho,
                Descricao = PARAMETRO_APROVACAO_ALTERACAO_NOTA_CONSELHO_DESCRICAO,
                Valor = string.Empty,
                Ano = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });

            //-> Ano anterior
            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_NOME,
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Descricao = PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_DESCRICAO,
                Valor = PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_VALOR_50,
                Ano = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });

            //-> Ano atual
            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_NOME,
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Descricao = PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_DESCRICAO,
                Valor = PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_VALOR_50,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });
        }

        private async Task CriarFrequenciaAluno(TipoFrequenciaAluno tipoFrequenciaAluno)
        {
            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_1,
                Tipo = tipoFrequenciaAluno,
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 05),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 30),
                Bimestre = BIMESTRE_1,
                TotalAulas = NUMERO_INTEIRO_20,
                TotalCompensacoes = NUMERO_INTEIRO_4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaId = TURMA_CODIGO_1,
                PeriodoEscolarId = NUMERO_LONGO_1,
                TotalPresencas = NUMERO_INTEIRO_16,
                TotalRemotos = NUMERO_INTEIRO_0
            });

            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_2,
                Tipo = tipoFrequenciaAluno,
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 05),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 30),
                Bimestre = BIMESTRE_1,
                TotalAulas = NUMERO_INTEIRO_20,
                TotalCompensacoes = NUMERO_INTEIRO_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaId = TURMA_CODIGO_1,
                PeriodoEscolarId = NUMERO_LONGO_1,
                TotalPresencas = NUMERO_INTEIRO_19,
                TotalRemotos = NUMERO_INTEIRO_0
            });

            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_3,
                Tipo = tipoFrequenciaAluno,
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 05),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 30),
                Bimestre = BIMESTRE_1,
                TotalAulas = NUMERO_INTEIRO_20,
                TotalCompensacoes = NUMERO_INTEIRO_5,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaId = TURMA_CODIGO_1,
                PeriodoEscolarId = NUMERO_LONGO_1,
                TotalPresencas = NUMERO_INTEIRO_15,
                TotalRemotos = NUMERO_INTEIRO_0
            });

            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_4,
                Tipo = tipoFrequenciaAluno,
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 05),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 30),
                Bimestre = BIMESTRE_1,
                TotalAulas = NUMERO_INTEIRO_20,
                TotalCompensacoes = NUMERO_INTEIRO_0,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaId = TURMA_CODIGO_1,
                PeriodoEscolarId = NUMERO_LONGO_1,
                TotalPresencas = NUMERO_INTEIRO_20,
                TotalRemotos = NUMERO_INTEIRO_0
            });
        }

        private async Task CriarPeriodoFechamento()
        {
            var periodoFechamento = new PeriodoFechamento
            {
                Id = 1,
                Migrado = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                Aplicacao = Dominio.Aplicacao.SGP,
                CriadoRF = SISTEMA_CODIGO_RF
            };

            var periodosEscolares = ObterTodos<PeriodoEscolar>();

            foreach (var periodoEscolar in periodosEscolares)
                periodoFechamento.AdicionarFechamentoBimestre(new PeriodoFechamentoBimestre(1, periodoEscolar, periodoEscolar.PeriodoFim, periodoEscolar.PeriodoFim.AddDays(10)));

            await InserirNaBase(periodoFechamento);
        }

        private async Task CriarSintese()
        {
            await InserirNaBase(new Sintese()
            {
                AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                Aprovado = true,
                Ativo = true,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Descricao = "",
                FimVigencia = DateTimeExtension.HorarioBrasilia().AddDays(1),
                Id = 1,
                InicioVigencia = DateTimeExtension.HorarioBrasilia(),
                Valor = SinteseEnum.Frequente.Name()
            });
        }

        protected async Task CriarCiclo()
        {
            await InserirNaBase(new Ciclo()
            {
                Descricao = ALFABETIZACAO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_1,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_2,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_3,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = INTERDISCIPLINAR,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_4,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_5,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_6,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = AUTORAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_7,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_8,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_9,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = MEDIO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_1,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_2,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_3,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_4,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_ALFABETIZACAO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 5,
                Ano = ANO_1,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_BASICA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 6,
                Ano = ANO_2,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_COMPLEMENTAR,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 7,
                Ano = ANO_3,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_FINAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 8,
                Ano = ANO_4,
                Modalidade = Modalidade.EJA
            });
        }

        private async Task CriarPeriodoReaberturaAnoAnterior(long tipoCalendarioId)
        {
            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = DATA_01_01_ANO_ANTERIOR,
                Fim = DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }
    }
}
