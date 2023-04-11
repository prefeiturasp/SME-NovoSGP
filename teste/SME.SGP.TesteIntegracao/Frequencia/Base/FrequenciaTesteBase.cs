using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.AulaRecorrenteFake;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.ServicosFakes.Rabbit;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Integracoes.Respostas;

namespace SME.SGP.TesteIntegracao
{
    public abstract class FrequenciaTesteBase : TesteBaseComuns
    {
        protected long REGISTRO_FREQUENCIA_ID_1 = 1;
        protected long REGISTRO_FREQUENCIA_ID_2 = 2;
        protected long REGISTRO_FREQUENCIA_ID_3 = 3;
        protected long REGISTRO_FREQUENCIA_ID_4 = 4;

        private const int QUANTIDADE_3 = 3;
        protected const long AULA_ID_1 = 1;
        protected const long AULA_ID_2 = 2;
        protected const long AULA_ID_3 = 3;
        protected const long AULA_ID_4 = 4;
        protected const int NUMERO_AULAS_1 = 1;
        protected const int NUMERO_AULAS_2 = 2;
        protected const int NUMERO_AULAS_3 = 3;

        protected const int QTDE_1 = 1;
        protected const int QTDE_2 = 2;
        protected const int QTDE_3 = 3;

        protected const string TIPO_FREQUENCIA_COMPARECEU = "C";
        protected const string TIPO_FREQUENCIA_FALTOU = "F";
        protected const string TIPO_FREQUENCIA_REMOTO = "R";
        
        protected const int TIPO_FREQUENCIA_COMPARECEU_NUMERO = 1;
        protected const int TIPO_FREQUENCIA_FALTOU_NUMERO = 2;
        protected const int TIPO_FREQUENCIA_REMOTO_NUMERO = 3;

        protected const string CODIGO_ALUNO_99999 = "99999";
        protected const string CODIGO_ALUNO_77777 = "77777";
        protected const string CODIGO_ALUNO_CRIANCA_NAO_ATIVO_666666 = "666666";

        protected const int ZERO = 0;

        protected const decimal PERCENTUAL_100 = 100.0M;
        protected const decimal PERCENTUAL_ZERO = 0.00M;

        protected readonly DateTime DATA_03_08 = new(DateTimeExtension.HorarioBrasilia().Year - 6, 03, 07);

        protected FrequenciaTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionarioCoreSSOPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterSupervisorPorCodigoQuery, IEnumerable<SupervisoresRetornoDto>>), typeof(ObterSupervisorPorCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerComRegistroFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaInserirAulaRecorrenteCommand, bool>), typeof(IncluirFilaInserirAulaRecorrenteCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaExclusaoAulaRecorrenteCommand, bool>), typeof(IncluirFilaExclusaoAulaRecorrenteCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaAlteracaoAulaRecorrenteCommand, bool>), typeof(IncluirFilaAlteracaoAulaRecorrenteCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerAulaFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterValidacaoPodePersistirTurmaNasDatasQuery, List<PodePersistirNaDataRetornoEolDto>>), typeof(ObterValidacaoPodePersistirTurmaNasDatasQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaCalcularFrequenciaPorTurmaCommand, bool>), typeof(IncluirFilaCalcularFrequenciaPorTurmaCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaConsolidarDashBoardFrequenciaCommand, bool>), typeof(IncluirFilaConsolidarDashBoardFrequenciaCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>), typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(Nota.ServicosFakes.ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>), typeof(ObterDisciplinasPorCodigoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosDentroPeriodoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected async Task<AuditoriaDto> InserirFrequenciaUseCaseComValidacaoBasica(FrequenciaDto frequenciaDto)
        {
            var retorno = await ExecutarInserirFrequenciaUseCaseSemValidacaoBasica(frequenciaDto);

            retorno.ShouldNotBeNull();

            var registroFrequencia = ObterTodos<RegistroFrequencia>();
            registroFrequencia.ShouldNotBeEmpty();
            registroFrequencia.Count().ShouldBeGreaterThanOrEqualTo(1);

            var registroFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();
            registroFrequenciaAluno.ShouldNotBeEmpty();
            registroFrequenciaAluno.Count().ShouldBeGreaterThanOrEqualTo(1);

            var consolidacaoFrequenciaAlunoMensal = ObterTodos<Dominio.ConsolidacaoFrequenciaAlunoMensal>();
            consolidacaoFrequenciaAlunoMensal.ShouldNotBeEmpty();
            consolidacaoFrequenciaAlunoMensal.Count().ShouldBeGreaterThanOrEqualTo(1);

            var consolidacaoDashBoardFrequencias = ObterTodos<ConsolidacaoDashBoardFrequencia>();
            consolidacaoDashBoardFrequencias.ShouldNotBeEmpty();
            consolidacaoDashBoardFrequencias.Count().ShouldBeGreaterThanOrEqualTo(1);

            return retorno;
        }

        protected async Task<AuditoriaDto> ExecutarInserirFrequenciaUseCaseSemValidacaoBasica(FrequenciaDto frequenciaDto)
        {
            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();

            return await useCase.Executar(frequenciaDto);
        }

        protected async Task<AuditoriaDto> ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(SalvarAnotacaoFrequenciaAlunoDto salvarAnotacaoFrequenciaAlunoDto)
        {
            var useCase = ServiceProvider.GetService<ISalvarAnotacaoFrequenciaAlunoUseCase>();

            return await useCase.Executar(salvarAnotacaoFrequenciaAlunoDto);
        }

        protected async Task<bool> ExecutarExcluirAnotacaoFrequenciaAlunoUseCase(long id)
        {
            var useCase = ServiceProvider.GetService<IExcluirAnotacaoFrequenciaAlunoUseCase>();
            return await useCase.Executar(id);
        }

        protected async Task<bool> ExecutarAlterarAnotacaoFrequenciaAlunoUseCase(AlterarAnotacaoFrequenciaAlunoDto param)
        {
            var useCase = ServiceProvider.GetService<IAlterarAnotacaoFrequenciaAlunoUseCase>();
            return await useCase.Executar(param);
        }

        protected async Task CriarDadosBasicos(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, DateTime dataInicio, DateTime dataFim, int bimestre, DateTime dataAula, string componenteCurricular, bool criarPeriodo = true, long tipoCalendarioId = 1, bool criarPeriodoEscolarEAbertura = true, int quantidadeAula = QUANTIDADE_3)
        {
            await CriarDadosBaseSemTurma(perfil, tipoCalendario, dataInicio, dataFim, bimestre, tipoCalendarioId, criarPeriodo);
            await CriarTurma(modalidade);
            await CriarAula(componenteCurricular, dataAula, RecorrenciaAula.AulaUnica, quantidadeAula);
            if (criarPeriodoEscolarEAbertura)
                await CriarPeriodoEscolarEAbertura();
        }

        protected async Task CriarDadosBasicosAulaRecorrencia(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, DateTime dataInicio, DateTime dataFim, int bimestre, DateTime dataAula, string componenteCurricular, bool criarPeriodo = true, long tipoCalendarioId = 1, bool criarPeriodoEscolarEAbertura = true, int quantidadeAula = QUANTIDADE_3, int quantidadeRecorrencia = QUANTIDADE_AULA_2)
        {
            await CriarDadosBaseSemTurma(perfil, tipoCalendario, dataInicio, dataFim, bimestre, tipoCalendarioId, criarPeriodo);
            await CriarTurma(modalidade);
            await CriarAula(componenteCurricular, dataAula, RecorrenciaAula.RepetirBimestreAtual, quantidadeAula);
            await CriarAulaRecorrente(componenteCurricular, dataAula, RecorrenciaAula.RepetirBimestreAtual, quantidadeAula, quantidadeRecorrencia);
            if (criarPeriodoEscolarEAbertura)
                await CriarPeriodoEscolarEAbertura();
        }

        protected async Task CriarDadosBasicosSemPeriodoEscolar(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, DateTime dataAula, string componenteCurricular, int quantidadeAula = QUANTIDADE_3)
        {
            await CriarTipoCalendario(tipoCalendario);

            await CriarDreUePerfil();

            await CriarComponenteCurricular();

            CriarClaimUsuario(perfil);

            await CriarUsuarios();

            await CriarTurma(modalidade);

            await CriarAula(componenteCurricular, dataAula, RecorrenciaAula.AulaUnica, quantidadeAula);
        }

        protected async Task CriarDadosBaseSemTurma(string perfil, ModalidadeTipoCalendario tipoCalendario, DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1, bool criarPeriodo = true)
        {
            await CriarTipoCalendario(tipoCalendario);
            await CriarItensComuns(criarPeriodo, dataInicio, dataFim, bimestre, tipoCalendarioId);
            CriarClaimUsuario(perfil);
            await CriarUsuarios();
        }

        protected async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        private ComponenteCurricularDto ObterComponenteCurricular(long componenteCurricularId)
        {
            if (componenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_PORTUGUES_NOME
                };
            else if (componenteCurricularId == COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_DESCONHECIDO_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_NOME
                };

            return null;
        }

        protected async Task CriarAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula = QUANTIDADE_3, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            await InserirNaBase(ObterAula(componenteCurricularCodigo, dataAula, recorrencia, quantidadeAula, rf));
        }

        protected async Task CriarAulaRecorrente(string componenteCurricularCodigo, DateTime dataAulaBase, RecorrenciaAula recorrencia, int quantidadeAula = QUANTIDADE_3, int qdadeRecorrencia = QUANTIDADE_AULA_2, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            var dataAulaRecorrente = dataAulaBase;
            for (int i = 0; i < qdadeRecorrencia; i++)
            {
                dataAulaRecorrente = dataAulaRecorrente.AddDays(7);
                var aula = ObterAula(componenteCurricularCodigo, dataAulaRecorrente, recorrencia, quantidadeAula, rf);
                aula.AulaPaiId = 1;
                await InserirNaBase(aula);
            }
        }

        private Dominio.Aula ObterAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            return new Dominio.Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = componenteCurricularCodigo,
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = rf,
                Quantidade = quantidadeAula,
                DataAula = dataAula,
                RecorrenciaAula = recorrencia,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            };
        }

        protected async Task CriarPeriodoEscolarEAberturaPadrao()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        protected async Task CriarMotivoAusencia(string descricao)
        {
            await InserirNaBase(new MotivoAusencia() { Descricao = descricao });
        }

        protected async Task CriarPeriodoReabertura(long tipoCalendarioId)
        {
            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = DATA_01_01,
                Fim = DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        protected FrequenciaDto ObterFrequenciaDto()
        {
            var frenquencia = new FrequenciaDto(AULA_ID_1);

            frenquencia.ListaFrequencia = ObterListaDeFrequenciaAluno();

            return frenquencia;
        }

        protected async Task CrieFrenquenciaAluno(string codigoAluno, string disciplina, DateTime dataInicio, DateTime dataFim, int bimestre)
        {
            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                CodigoAluno = codigoAluno,
                DisciplinaId = disciplina,
                PeriodoEscolarId = TIPO_CALENDARIO_1,
                TurmaId = TURMA_CODIGO_1,
                PeriodoInicio = dataInicio,
                PeriodoFim = dataFim,
                Bimestre = bimestre,
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME
            });
        }

        protected async Task CrieRegistroDeFrenquencia()
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID_1,
                CriadoPor = "",
                CriadoRF = ""
            });

            await RegistroFrequenciaAluno(CODIGO_ALUNO_1, QUANTIDADE_AULA);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_1, QUANTIDADE_AULA_2);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_1, QUANTIDADE_AULA_3);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_2, QUANTIDADE_AULA);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_2, QUANTIDADE_AULA_2);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_2, QUANTIDADE_AULA_3);
        }

        protected async Task CrieRegistroDeFrenquenciaTodasAulas(string[] codigoAlunos, int qdadeAula)
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID_1,
                CriadoPor = "",
                CriadoRF = ""
            });

            var aulas = ObterTodos<Dominio.Aula>();
            foreach (var aula in aulas)
                foreach (var aluno in codigoAlunos)
                    for (int indexAula = 1; indexAula <= (qdadeAula > 0 ? qdadeAula : aula.Quantidade); indexAula++)
                        await RegistroFrequenciaAluno(aluno, indexAula, aula.Id);

        }

        protected async Task CriarRegistrosConsolidacaoFrequenciaAlunoMensal()
        {
            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "1",
                Mes = 5,
                Percentual = 50,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 5,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "2",
                Mes = 5,
                Percentual = 80,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 8,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "3",
                Mes = 5,
                Percentual = 40,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 6,
                QuantidadeCompensacoes = 0
            });
        }


        private List<RegistroFrequenciaAlunoDto> ObterListaDeFrequenciaAluno()
        {
            var lista = new List<RegistroFrequenciaAlunoDto>();
            var aulas = ObterFrenquenciaAula();

            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "1", Aulas = aulas, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName() });
            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "2", Aulas = aulas, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName() });
            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "3", Aulas = aulas, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName() });
            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "4", Aulas = aulas, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName() });

            return lista;
        }

        private List<FrequenciaAulaDto> ObterFrenquenciaAula()
        {
            var lista = new List<FrequenciaAulaDto>();

            lista.Add(new FrequenciaAulaDto() { NumeroAula = QUANTIDADE_AULA, TipoFrequencia = TipoFrequencia.C.ShortName() });

            return lista;
        }

        protected async Task InserirFrequenciaUseCaseComValidacaoCompleta(FrequenciaDto frequencia, TipoFrequencia tipoFrequenciaPreDefinida, TipoFrequencia tipoFrequenciaRegistrada, decimal percentualFrequencia, int numeroAulas, int qtdeAusencias, int qtdeCompensacoes)
        {
            await ExecutarInserirFrequenciaUseCaseSemValidacaoBasica(frequencia);

            var frequenciaPreDefinida = ObterTodos<FrequenciaPreDefinida>();
            frequenciaPreDefinida.ShouldNotBeEmpty();
            frequenciaPreDefinida.FirstOrDefault().TipoFrequencia.Equals(tipoFrequenciaPreDefinida).ShouldBeTrue();

            var registroFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();
            registroFrequenciaAluno.ShouldNotBeEmpty();
            (registroFrequenciaAluno.FirstOrDefault().Valor == (int)tipoFrequenciaRegistrada).ShouldBeTrue();

            var consolidacaoFrequenciaAlunoMensal = ObterTodos<Dominio.ConsolidacaoFrequenciaAlunoMensal>();
            consolidacaoFrequenciaAlunoMensal.ShouldNotBeEmpty();
            (consolidacaoFrequenciaAlunoMensal.FirstOrDefault().Percentual == percentualFrequencia).ShouldBeTrue();
            (consolidacaoFrequenciaAlunoMensal.FirstOrDefault().QuantidadeAulas == numeroAulas).ShouldBeTrue();
            (consolidacaoFrequenciaAlunoMensal.FirstOrDefault().QuantidadeAusencias == qtdeAusencias).ShouldBeTrue();
            (consolidacaoFrequenciaAlunoMensal.FirstOrDefault().QuantidadeCompensacoes == qtdeCompensacoes).ShouldBeTrue();
        }

        protected async Task CriarPredefinicaoAluno(string codigoAluno, TipoFrequencia tipoFrequencia, long componenteCurricularId, long turmaId)
        {
            await InserirNaBase(new FrequenciaPreDefinida()
            {
                CodigoAluno = codigoAluno,
                TipoFrequencia = tipoFrequencia,
                ComponenteCurricularId = componenteCurricularId,
                TurmaId = turmaId
            });
        }

        private async Task RegistroFrequenciaAluno(string codigoAluno, int numeroAula, long aulaid = 1)
        {
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 1,
                CodigoAluno = codigoAluno,
                RegistroFrequenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                Valor = (int)TipoFrequencia.F,
                NumeroAula = numeroAula,
                AulaId = aulaid
            });
        }

        protected async Task<long> Criar_Justificativa_Para_Exclusao_Alteracao_Motivo_Descricao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());

            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08,
                BIMESTRE_2, DATA_07_08, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, quantidadeAula: NUMERO_AULAS_2);

            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            return retorno.Id;
        }

        protected async Task<long> Criar_Justificativa_Para_Exclusao_Alteracao_Somente_Com_Motivo()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());

            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08,
                BIMESTRE_2, DATA_07_08, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, quantidadeAula: NUMERO_AULAS_2);

            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            return retorno.Id;
        }

        protected async Task<long> Criar_Justificativa_Para_Exclusao_Alteracao_Somente_Com_Descricao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());

            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08,
                BIMESTRE_2, DATA_07_08, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, quantidadeAula: NUMERO_AULAS_2);

            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            return retorno.Id;
        }

        protected async Task<long> Criar_Justificativa_Para_Exclusao_Alteracao_Somente_Com_Anotacao_Possui_Atribuicao_Na_Turma_Na_Data()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());

            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08,
                BIMESTRE_2, DATA_07_08, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), false, quantidadeAula: NUMERO_AULAS_2);

            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            return retorno.Id;
        }


        protected async Task InserirParametroSistema(bool inserirParametrosAnoAnterior = false)
        {
            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "PercentualFrequenciaCritico",
                Tipo = TipoParametroSistema.PercentualFrequenciaCritico,
                Descricao = "",
                Valor = "75",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "PercentualFrequenciaAlerta",
                Tipo = TipoParametroSistema.PercentualFrequenciaAlerta,
                Descricao = "",
                Valor = "80",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            if (inserirParametrosAnoAnterior)
            {
                await InserirNaBase(new ParametrosSistema()
                {
                    Nome = "PercentualFrequenciaCritico",
                    Tipo = TipoParametroSistema.PercentualFrequenciaCritico,
                    Descricao = "",
                    Valor = "75",
                    Ano = DateTimeExtension.HorarioBrasilia().Year - 1,
                    Ativo = true,
                    CriadoEm = DateTime.Now,
                    CriadoPor = "",
                    CriadoRF = ""
                });

                await InserirNaBase(new ParametrosSistema()
                {
                    Nome = "PercentualFrequenciaAlerta",
                    Tipo = TipoParametroSistema.PercentualFrequenciaAlerta,
                    Descricao = "",
                    Valor = "80",
                    Ano = DateTimeExtension.HorarioBrasilia().Year - 1,
                    Ativo = true,
                    CriadoEm = DateTime.Now,
                    CriadoPor = "",
                    CriadoRF = ""
                });
            }
        }

        protected async Task CriarDadosFrenqueciaAluno(string codigoAluno, TipoFrequenciaAluno tipoFrequenciaAluno, int totalAusencia = 2)
        {
            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                PeriodoInicio = DATA_02_05,
                PeriodoFim = DATA_07_08,
                Bimestre = 2,
                TotalAulas = 12,
                TotalAusencias = totalAusencia,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 21, 12, 46, 29),
                CriadoPor = "Sistema",
                AlteradoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 23, 21, 52, 51),
                AlteradoPor = "Sistema",
                CriadoRF = "0",
                AlteradoRF = "0",
                TotalCompensacoes = 0,
                PeriodoEscolarId = 1,
                TotalPresencas = 1,
                TotalRemotos = 0,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                CodigoAluno = codigoAluno,
                TurmaId = TURMA_CODIGO_1,
                Tipo = tipoFrequenciaAluno
            });
        }
    }
}