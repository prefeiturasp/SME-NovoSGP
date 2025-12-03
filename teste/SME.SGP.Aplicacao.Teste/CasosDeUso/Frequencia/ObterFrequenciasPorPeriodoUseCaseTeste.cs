using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ObterFrequenciasPorPeriodoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterFrequenciasPorPeriodoUseCase useCase;

        public ObterFrequenciasPorPeriodoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterFrequenciasPorPeriodoUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve executar com sucesso quando dados válidos são fornecidos")]
        public async Task Deve_Executar_Com_Sucesso_Quando_Dados_Validos()
        {
            var filtro = CriarFiltroValido();
            var usuario = CriarUsuarioLogado();
            var turma = CriarTurma();
            var alunos = CriarAlunosDaTurma();
            var componenteCurricular = CriarComponenteCurricular();
            var aulas = CriarAulas();
            var tipoCalendarioId = 1L;
            var periodoEscolar = CriarPeriodoEscolar();
            var parametroPercentualCritico = CriarParametroSistema("75");
            var parametroPercentualAlerta = CriarParametroSistema("85");
            var registraFrequencia = true;
            var frequenciaAlunos = CriarFrequenciaAlunos();
            var turmaPossuiFrequenciaRegistrada = true;
            var registrosFrequenciaAlunos = CriarRegistrosFrequenciaAlunos();
            var compensacaoAusenciaAlunos = CriarCompensacaoAusenciaAlunos();
            var anotacoesTurma = CriarAnotacoesTurma();
            var frequenciaPreDefinida = CriarFrequenciaPreDefinida();
            var resultadoEsperado = CriarResultadoEsperado();

            ConfigurarMocks(filtro, usuario, turma, alunos, componenteCurricular, aulas, tipoCalendarioId,
                periodoEscolar, parametroPercentualCritico, parametroPercentualAlerta, registraFrequencia,
                frequenciaAlunos, turmaPossuiFrequenciaRegistrada, registrosFrequenciaAlunos,
                compensacaoAusenciaAlunos, anotacoesTurma, frequenciaPreDefinida, resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            VerificarChamadasMediator();
        }

        [Fact(DisplayName = "Deve lançar NegocioException quando componente curricular não é encontrado")]
        public async Task Deve_Lancar_Excecao_Quando_Componente_Curricular_Nao_Encontrado()
        {
            var filtro = CriarFiltroValido();
            var usuario = CriarUsuarioLogado();
            var turma = CriarTurma();
            var alunos = CriarAlunosDaTurma();

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DisciplinaDto)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("Componente curricular não localizado", exception.Message);
        }

        [Fact(DisplayName = "Deve lançar NegocioException quando turma não é encontrada")]
        public async Task Deve_Lancar_Excecao_Quando_Turma_Nao_Encontrada()
        {
            var filtro = CriarFiltroValido();
            var usuario = CriarUsuarioLogado();

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("Não foi encontrada a turma informada.", exception.Message);
        }

        [Fact(DisplayName = "Deve lançar NegocioException quando aulas não são encontradas")]
        public async Task Deve_Lancar_Excecao_Quando_Aulas_Nao_Encontradas()
        {
            var filtro = CriarFiltroValido();
            var usuario = CriarUsuarioLogado();
            var turma = CriarTurma();
            var alunos = CriarAlunosDaTurma();
            var componenteCurricular = CriarComponenteCurricular();

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componenteCurricular);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasPorDataPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Dominio.Aula>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("Aulas não encontradas para a turma no Período.", exception.Message);
        }

        [Fact(DisplayName = "Deve lançar NegocioException quando aulas são nulas")]
        public async Task Deve_Lancar_Excecao_Quando_Aulas_Sao_Nulas()
        {
            var filtro = CriarFiltroValido();
            var usuario = CriarUsuarioLogado();
            var turma = CriarTurma();
            var alunos = CriarAlunosDaTurma();
            var componenteCurricular = CriarComponenteCurricular();

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componenteCurricular);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasPorDataPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<Dominio.Aula>)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("Aulas não encontradas para a turma no Período.", exception.Message);
        }

        [Fact(DisplayName = "Deve lançar NegocioException quando período escolar não é encontrado")]
        public async Task Deve_Lancar_Excecao_Quando_Periodo_Escolar_Nao_Encontrado()
        {
            var filtro = CriarFiltroValido();
            var usuario = CriarUsuarioLogado();
            var turma = CriarTurma();
            var alunos = CriarAlunosDaTurma();
            var componenteCurricular = CriarComponenteCurricular();
            var aulas = CriarAulas();
            var tipoCalendarioId = 1L;

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componenteCurricular);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasPorDataPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulas);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendarioId);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoEscolar)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("Ocorreu um erro, esta aula está fora do período escolar.", exception.Message);
        }

        [Fact(DisplayName = "Deve lançar NegocioException quando parâmetro percentual crítico não é encontrado")]
        public async Task Deve_Lancar_Excecao_Quando_Parametro_Percentual_Critico_Nao_Encontrado()
        {
            var filtro = CriarFiltroValido();
            var usuario = CriarUsuarioLogado();
            var turma = CriarTurma();
            var alunos = CriarAlunosDaTurma();
            var componenteCurricular = CriarComponenteCurricular();
            var aulas = CriarAulas();
            var tipoCalendarioId = 1L;
            var periodoEscolar = CriarPeriodoEscolar();

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componenteCurricular);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasPorDataPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulas);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendarioId);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoEscolar);
            mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                    q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaCritico), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ParametrosSistema)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("Parâmetro de percentual de frequência em nível crítico/alerta não encontrado.", exception.Message);
        }

        [Fact(DisplayName = "Deve lançar NegocioException quando parâmetro percentual crítico tem valor vazio")]
        public async Task Deve_Lancar_Excecao_Quando_Parametro_Percentual_Critico_Tem_Valor_Vazio()
        {
            var filtro = CriarFiltroValido();
            var usuario = CriarUsuarioLogado();
            var turma = CriarTurma();
            var alunos = CriarAlunosDaTurma();
            var componenteCurricular = CriarComponenteCurricular();
            var aulas = CriarAulas();
            var tipoCalendarioId = 1L;
            var periodoEscolar = CriarPeriodoEscolar();
            var parametroPercentualCritico = new ParametrosSistema { Valor = string.Empty };

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componenteCurricular);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasPorDataPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulas);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendarioId);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoEscolar);
            mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                    q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaCritico), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroPercentualCritico);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("Parâmetro de percentual de frequência em nível crítico/alerta não encontrado.", exception.Message);
        }

        [Fact(DisplayName = "Deve funcionar com componente curricular de regência")]
        public async Task Deve_Funcionar_Com_Componente_Curricular_Regencia()
        {
            var filtro = CriarFiltroValido();
            var usuario = CriarUsuarioLogado();
            var turma = CriarTurma();
            var alunos = CriarAlunosDaTurma();
            var componenteCurricular = CriarComponenteCurricularRegencia();
            var aulas = CriarAulas();
            var tipoCalendarioId = 1L;
            var periodoEscolar = CriarPeriodoEscolar();
            var parametroPercentualCritico = CriarParametroSistema("75");
            var parametroPercentualAlerta = CriarParametroSistema("85");
            var registraFrequencia = true;
            var frequenciaAlunos = CriarFrequenciaAlunos();
            var turmaPossuiFrequenciaRegistrada = true;
            var registrosFrequenciaAlunos = CriarRegistrosFrequenciaAlunos();
            var compensacaoAusenciaAlunos = CriarCompensacaoAusenciaAlunos();
            var anotacoesTurma = CriarAnotacoesTurma();
            var frequenciaPreDefinida = CriarFrequenciaPreDefinida();
            var resultadoEsperado = CriarResultadoEsperado();

            ConfigurarMocks(filtro, usuario, turma, alunos, componenteCurricular, aulas, tipoCalendarioId,
                periodoEscolar, parametroPercentualCritico, parametroPercentualAlerta, registraFrequencia,
                frequenciaAlunos, turmaPossuiFrequenciaRegistrada, registrosFrequenciaAlunos,
                compensacaoAusenciaAlunos, anotacoesTurma, frequenciaPreDefinida, resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
        }

        [Fact(DisplayName = "Deve funcionar quando registros de frequência estão vazios")]
        public async Task Deve_Funcionar_Quando_Registros_Frequencia_Vazios()
        {
            var filtro = CriarFiltroValido();
            var usuario = CriarUsuarioLogado();
            var turma = CriarTurma();
            var alunos = CriarAlunosDaTurma();
            var componenteCurricular = CriarComponenteCurricular();
            var aulas = CriarAulas();
            var tipoCalendarioId = 1L;
            var periodoEscolar = CriarPeriodoEscolar();
            var parametroPercentualCritico = CriarParametroSistema("75");
            var parametroPercentualAlerta = CriarParametroSistema("85");
            var registraFrequencia = true;
            var frequenciaAlunos = CriarFrequenciaAlunos();
            var turmaPossuiFrequenciaRegistrada = true;
            var registrosFrequenciaAlunos = new List<RegistroFrequenciaAlunoPorAulaDto>(); // Vazio
            var compensacaoAusenciaAlunos = new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>();
            var anotacoesTurma = CriarAnotacoesTurma();
            var frequenciaPreDefinida = CriarFrequenciaPreDefinida();
            var resultadoEsperado = CriarResultadoEsperado();

            ConfigurarMocks(filtro, usuario, turma, alunos, componenteCurricular, aulas, tipoCalendarioId,
                periodoEscolar, parametroPercentualCritico, parametroPercentualAlerta, registraFrequencia,
                frequenciaAlunos, turmaPossuiFrequenciaRegistrada, registrosFrequenciaAlunos,
                compensacaoAusenciaAlunos, anotacoesTurma, frequenciaPreDefinida, resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
        }

        #region Métodos de Criação de Objetos

        private FiltroFrequenciaPorPeriodoDto CriarFiltroValido()
        {
            return new FiltroFrequenciaPorPeriodoDto
            {
                DisciplinaId = "1",
                TurmaId = "TURMA123",
                DataInicio = new DateTime(2023, 3, 1),
                DataFim = new DateTime(2023, 3, 31)
            };
        }

        private Usuario CriarUsuarioLogado()
        {
            return new Usuario
            {
                Id = 1,
                Login = "usuario.teste",
                Nome = "Usuário Teste",
                PerfilAtual = Guid.NewGuid(),
                CodigoRf = "1234567"
            };
        }

        private Turma CriarTurma()
        {
            return new Turma
            {
                Id = 1,
                CodigoTurma = "TURMA123",
                Nome = "1º A",
                AnoLetivo = 2023,
                ModalidadeCodigo = Modalidade.Fundamental
            };
        }

        private IEnumerable<AlunoPorTurmaResposta> CriarAlunosDaTurma()
        {
            return new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "123456",
                    NomeAluno = "Aluno Teste 1",
                    NumeroAlunoChamada = 1
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "123457",
                    NomeAluno = "Aluno Teste 2",
                    NumeroAlunoChamada = 2
                }
            };
        }

        private DisciplinaDto CriarComponenteCurricular()
        {
            return new DisciplinaDto
            {
                Id = 1,
                CodigoComponenteCurricular = 1,
                Regencia = false,
                CdComponenteCurricularPai = null
            };
        }

        private DisciplinaDto CriarComponenteCurricularRegencia()
        {
            return new DisciplinaDto
            {
                Id = 1,
                CodigoComponenteCurricular = 1,
                Regencia = true,
                CdComponenteCurricularPai = 2
            };
        }

        private IEnumerable<Dominio.Aula> CriarAulas()
        {
            return new List<Dominio.Aula>
            {
                new Dominio.Aula
                {
                    Id = 1,
                    DataAula = new DateTime(2023, 3, 1),
                    DisciplinaId = "1",
                    TurmaId = "TURMA123",
                    Quantidade = 2
                },
                new Dominio.Aula
                {
                    Id = 2,
                    DataAula = new DateTime(2023, 3, 2),
                    DisciplinaId = "1",
                    TurmaId = "TURMA123",
                    Quantidade = 1
                }
            };
        }

        private PeriodoEscolar CriarPeriodoEscolar()
        {
            return new PeriodoEscolar
            {
                Id = 1,
                Bimestre = 1,
                PeriodoInicio = new DateTime(2023, 2, 1),
                PeriodoFim = new DateTime(2023, 4, 30)
            };
        }

        private ParametrosSistema CriarParametroSistema(string valor)
        {
            return new ParametrosSistema
            {
                Id = 1,
                Valor = valor,
                Ano = 2023
            };
        }

        private IEnumerable<FrequenciaAluno> CriarFrequenciaAlunos()
        {
            return new List<FrequenciaAluno>
            {
                new FrequenciaAluno
                {
                    CodigoAluno = "123456",
                    TotalAulas = 10,
                    TotalAusencias = 2
                }
            };
        }

        private IEnumerable<RegistroFrequenciaAlunoPorAulaDto> CriarRegistrosFrequenciaAlunos()
        {
            return new List<RegistroFrequenciaAlunoPorAulaDto>
            {
                new RegistroFrequenciaAlunoPorAulaDto
                {
                    AulaId = 1,
                    AlunoCodigo = "123456",
                    TipoFrequencia = TipoFrequencia.C
                }
            };
        }

        private IEnumerable<CompensacaoAusenciaAlunoAulaSimplificadoDto> CriarCompensacaoAusenciaAlunos()
        {
            return new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>
            {
                new CompensacaoAusenciaAlunoAulaSimplificadoDto
                {
                    CodigoAluno = "123456",
                    AulaId = 1
                }
            };
        }

        private IEnumerable<AnotacaoAlunoAulaDto> CriarAnotacoesTurma()
        {
            return new List<AnotacaoAlunoAulaDto>
            {
                new AnotacaoAlunoAulaDto
                {
                    AlunoCodigo = "123456",
                    AulaId = 12
                }
            };
        }

        private IEnumerable<FrequenciaPreDefinidaDto> CriarFrequenciaPreDefinida()
        {
            return new List<FrequenciaPreDefinidaDto>
            {
                new FrequenciaPreDefinidaDto
                {
                    CodigoAluno = "123456",
                    Tipo = TipoFrequencia.C
                }
            };
        }

        private RegistroFrequenciaPorDataPeriodoDto CriarResultadoEsperado()
        {
            return new RegistroFrequenciaPorDataPeriodoDto
            {
                Aulas = new List<AulaFrequenciaDto>(),
                Alunos = new List<AlunoRegistroFrequenciaDto>()
            };
        }

        #endregion

        #region Configuração de Mocks

        private void ConfigurarMocks(
            FiltroFrequenciaPorPeriodoDto filtro,
            Usuario usuario,
            Turma turma,
            IEnumerable<AlunoPorTurmaResposta> alunos,
            DisciplinaDto componenteCurricular,
            IEnumerable<Dominio.Aula> aulas,
            long tipoCalendarioId,
            PeriodoEscolar periodoEscolar,
            ParametrosSistema parametroPercentualCritico,
            ParametrosSistema parametroPercentualAlerta,
            bool registraFrequencia,
            IEnumerable<FrequenciaAluno> frequenciaAlunos,
            bool turmaPossuiFrequenciaRegistrada,
            IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAlunos,
            IEnumerable<CompensacaoAusenciaAlunoAulaSimplificadoDto> compensacaoAusenciaAlunos,
            IEnumerable<AnotacaoAlunoAulaDto> anotacoesTurma,
            IEnumerable<FrequenciaPreDefinidaDto> frequenciaPreDefinida,
            RegistroFrequenciaPorDataPeriodoDto resultadoEsperado)
        {
            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componenteCurricular);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasPorDataPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulas);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendarioId);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoEscolar);

            mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                    q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaCritico), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroPercentualCritico);

            mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                    q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaAlerta), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroPercentualAlerta);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteRegistraFrequenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registraFrequencia);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciaAlunos);

            mediatorMock.Setup(m => m.Send(It.IsAny<ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaPossuiFrequenciaRegistrada);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterRegistrosFrequenciaAlunosPorPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registrosFrequenciaAlunos);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(compensacaoAusenciaAlunos);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosComAnotacaoPorPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacoesTurma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPreDefinidaPorTurmaComponenteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciaPreDefinida);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterListaFrequenciaAulasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);
        }

        private void VerificarChamadasMediator()
        {
            mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulasPorDataPeriodoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaCritico), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaAlerta), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterListaFrequenciaAulasQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}