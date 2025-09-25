using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class SalvarCompensacaoAusenciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IDbTransaction> transactionMock;
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoOptions;
        private readonly SalvarCompensacaoAusenciaUseCase useCase;

        public SalvarCompensacaoAusenciaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            transactionMock = new Mock<IDbTransaction>();
            configuracaoOptions = Options.Create(new ConfiguracaoArmazenamentoOptions
            {
                BucketTemp = "temp",
                BucketArquivos = "arquivos"
            });

            unitOfWorkMock.Setup(u => u.IniciarTransacao()).Returns(transactionMock.Object);

            useCase = new SalvarCompensacaoAusenciaUseCase(
                mediatorMock.Object,
                unitOfWorkMock.Object,
                configuracaoOptions
            );
        }

        [Fact]
        public async Task Executar_Turma_Nao_Encontrada_Deve_Lancar_Negocio_Exception()
        {
            var compensacaoDto = CriarCompensacaoDto();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Turma)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(0, compensacaoDto));
            Assert.Equal("Turma não localizada!", exception.Message);
        }

        [Fact]
        public async Task Executar_Periodo_Fechado_Sem_Parametro_Permissao_Deve_Lancar_Negocio_Exception()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();

            ConfigurarMocksBasicos(turma, periodo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((ParametrosSistema)null);

            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(0, compensacaoDto));
            Assert.Equal($"Período do {periodo.Bimestre}º Bimestre não está aberto.", exception.Message);
        }

        [Fact]
        public async Task Executar_Parametro_Permissao_Inativo_Periodo_Fechado_Deve_Lancar_Negocio_Exception()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            var parametroInativo = new ParametrosSistema { Ativo = false };

            ConfigurarMocksBasicos(turma, periodo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametroInativo);

            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(0, compensacaoDto));
            Assert.Equal($"Período do {periodo.Bimestre}º Bimestre não está aberto.", exception.Message);
        }

        [Fact]
        public async Task Executar_Professor_Sem_Permissao_Deve_Lancar_Negocio_Exception()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            var usuario = CriarUsuarioProfessor();
            var parametroAtivo = new ParametrosSistema { Ativo = true };

            ConfigurarMocksBasicos(turma, periodo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametroAtivo);

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ProfessorPodePersistirTurmaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(0, compensacaoDto));
            Assert.Equal("Você não pode fazer alterações ou inclusões nesta turma e data.", exception.Message);
        }  

        [Fact]
        public async Task Executar_Disciplina_Nao_Encontrada_EOL_Deve_Lancar_Negocio_Exception()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            var usuario = CriarUsuarioGestor();

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaDto>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(0, compensacaoDto));
            Assert.Equal("Componente curricular não encontrado no EOL.", exception.Message);
        }

        [Fact]
        public async Task Executar_Regencia_Sem_Componentes_Informados_Deve_Lancar_Negocio_Exception()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.DisciplinasRegenciaIds = null;
            var usuario = CriarUsuarioGestor();
            var componenteRegencia = new DisciplinaDto { Id = 123, Regencia = true };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaDto> { componenteRegencia });

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(0, compensacaoDto));
            Assert.Equal("Regência de classe deve informar o(s) componente(s) curricular(es) relacionados a esta atividade.", exception.Message);
        }

        [Fact]
        public async Task Executar_Regencia_Componentes_Vazios_Deve_Lancar_Negocio_Exception()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.DisciplinasRegenciaIds = new List<string>();
            var usuario = CriarUsuarioGestor();
            var componenteRegencia = new DisciplinaDto { Id = 123, Regencia = true };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaDto> { componenteRegencia });

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(0, compensacaoDto));
            Assert.Equal("Regência de classe deve informar o(s) componente(s) curricular(es) relacionados a esta atividade.", exception.Message);
        }

        [Fact]
        public async Task Executar_Aluno_Sem_Ausencia_Deve_Lancar_Negocio_Exception()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>
            {
                new CompensacaoAusenciaAlunoDto { Id = "12345", QtdFaltasCompensadas = 2 }
            };
            var usuario = CriarUsuarioGestor();

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(0, compensacaoDto));
            Assert.Contains("O aluno(a) [12345] não possui ausência para compensar.", exception.Message);
        }

        [Fact]
        public async Task Executar_Compensacao_Maior_Que_Faltas_Disponiveis_Deve_Lancar_Negocio_Exception()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>
            {
                new CompensacaoAusenciaAlunoDto { Id = "12345", QtdFaltasCompensadas = 5 }
            };
            var usuario = CriarUsuarioGestor();
            var frequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = "12345",
                TotalAusencias = 6,
                TotalCompensacoes = 3,
                DisciplinaId = "123",
                PeriodoFim = periodo.PeriodoFim,
                TurmaId = turma.CodigoTurma
            };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno> { frequenciaAluno });

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(0, compensacaoDto));
            Assert.Contains("O aluno(a) [12345] possui apenas 3 faltas não compensadas.", exception.Message);
        }

        [Fact]
        public async Task Executar_NovaCompensacao_Sem_Alunos_Deve_Executar_Com_Sucesso()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>();
            var usuario = CriarUsuarioGestor();

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            await useCase.Executar(0, compensacaoDto);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
     
        [Fact]
        public async Task Executar_Alteracao_Compensacao_Com_Remocao_Alunos_Deve_Executar_Com_Sucesso()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>();
            var usuario = CriarUsuarioGestor();
            var compensacaoExistente = new SME.SGP.Dominio.CompensacaoAusencia { Id = 1, Descricao = "Desc Antiga" };
            var alunoExistente = new CompensacaoAusenciaAluno { Id = 1, CodigoAluno = "12345", CompensacaoAusenciaId = 1 };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(compensacaoExistente);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoPorCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<CompensacaoAusenciaAluno> { alunoExistente });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            await useCase.Executar(1, compensacaoDto);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Alteracao_Compensacao_Alterando_Faltas_Compensadas_Deve_Executar_Com_Sucesso()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>
            {
                new CompensacaoAusenciaAlunoDto { Id = "12345", QtdFaltasCompensadas = 3 }
            };
            var usuario = CriarUsuarioGestor();
            var compensacaoExistente = new SME.SGP.Dominio.CompensacaoAusencia { Id = 1, Descricao = "Desc Antiga" };
            var alunoExistente = new CompensacaoAusenciaAluno { Id = 1, CodigoAluno = "12345", CompensacaoAusenciaId = 1, QuantidadeFaltasCompensadas = 2 };
            var frequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = "12345",
                TotalAusencias = 6,
                TotalCompensacoes = 3,
                DisciplinaId = "123",
                PeriodoFim = periodo.PeriodoFim,
                TurmaId = turma.CodigoTurma
            };

            var faltasNaoCompensadas = new List<RegistroFaltasNaoCompensadaDto>
            {
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 1, DataAula = DateTime.Today, NumeroAula = 1, Sugestao = true },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 2, DataAula = DateTime.Today, NumeroAula = 2, Sugestao = true },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 3, DataAula = DateTime.Today, NumeroAula = 3, Sugestao = true },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 4, DataAula = DateTime.Today, NumeroAula = 4, Sugestao = false },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 5, DataAula = DateTime.Today, NumeroAula = 5, Sugestao = false }
            };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(compensacaoExistente);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoPorCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<CompensacaoAusenciaAluno> { alunoExistente });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno> { frequenciaAluno });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(faltasNaoCompensadas);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            await useCase.Executar(1, compensacaoDto);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Executar_Com_Disciplinas_Regencia_Deve_Gravar_Disciplinas()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.DisciplinasRegenciaIds = new List<string> { "456", "789" };
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>();
            var usuario = CriarUsuarioGestor();
            var componenteRegencia = new DisciplinaDto { Id = 123, Regencia = true };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaDto> { componenteRegencia });

            mediatorMock.Setup(m => m.Send(It.IsAny<InserirVariosCompensacaoAusenciaRegenciaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            await useCase.Executar(0, compensacaoDto);

            mediatorMock.Verify(m => m.Send(It.IsAny<InserirVariosCompensacaoAusenciaRegenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Alteracao_Disciplinas_Regencia_Deve_Atualizar_Individualmente()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.DisciplinasRegenciaIds = new List<string> { "456" };
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>();
            var usuario = CriarUsuarioGestor();
            var compensacaoExistente = new SME.SGP.Dominio.CompensacaoAusencia { Id = 1 };
            var componenteRegencia = new DisciplinaDto { Id = 123, Regencia = true };
            var disciplinaExistente = new CompensacaoAusenciaDisciplinaRegencia { DisciplinaId = "789", CompensacaoAusenciaId = 1 };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(compensacaoExistente);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaDto> { componenteRegencia });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<CompensacaoAusenciaDisciplinaRegencia> { disciplinaExistente });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaDiciplinaRegenciaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            await useCase.Executar(1, compensacaoDto);

            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaDiciplinaRegenciaCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Executar_Com_Alunos_Com_Compensacao_Aula_Ano_2023_Deve_Gravar_Aluno_Aulas()
        {
            var turma = CriarTurma();
            turma.AnoLetivo = 2023;
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>
            {
                new CompensacaoAusenciaAlunoDto
                {
                    Id = "12345",
                    QtdFaltasCompensadas = 2,
                    CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto>
                    {
                        new CompensacaoAusenciaAlunoAulaDto { RegistroFrequenciaAlunoId = 1 },
                        new CompensacaoAusenciaAlunoAulaDto { RegistroFrequenciaAlunoId = 2 }
                    }
                }
            };
            var usuario = CriarUsuarioGestor();
            var frequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = "12345",
                TotalAusencias = 6,
                TotalCompensacoes = 3,
                DisciplinaId = "123",
                PeriodoFim = periodo.PeriodoFim,
                TurmaId = turma.CodigoTurma
            };
            var faltasNaoCompensadas = new List<RegistroFaltasNaoCompensadaDto>
            {
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 1, DataAula = DateTime.Today, NumeroAula = 1 },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 2, DataAula = DateTime.Today, NumeroAula = 2 }
            };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno> { frequenciaAluno });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(faltasNaoCompensadas);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            await useCase.Executar(0, compensacaoDto);

            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Executar_Com_Notificacao_Compensacao_Deve_Publicar_Fila()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>
            {
                new CompensacaoAusenciaAlunoDto { Id = "12345", QtdFaltasCompensadas = 2 }
            };
            var usuario = CriarUsuarioGestor();
            var frequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = "12345",
                TotalAusencias = 7,
                TotalCompensacoes = 3,
                DisciplinaId = "123",
                PeriodoFim = periodo.PeriodoFim,
                TurmaId = turma.CodigoTurma
            };

            var faltasNaoCompensadas = new List<RegistroFaltasNaoCompensadaDto>
            {
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 1, DataAula = DateTime.Today, NumeroAula = 1, Sugestao = true },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 2, DataAula = DateTime.Today, NumeroAula = 2, Sugestao = true },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 3, DataAula = DateTime.Today, NumeroAula = 3, Sugestao = false },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 4, DataAula = DateTime.Today, NumeroAula = 4, Sugestao = false }
            };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno> { frequenciaAluno });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(faltasNaoCompensadas);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaCalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaSeExisteParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            await useCase.Executar(0, compensacaoDto);

            mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaCalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
      
        [Fact]
        public async Task Executar_Ano_Anterior_2023_Nao_Deve_Gravar_Aluno_Aulas_Deve_Executar_Com_Sucesso()
        {
            var turma = CriarTurma();
            turma.AnoLetivo = 2022; 
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>
            {
                new CompensacaoAusenciaAlunoDto { Id = "12345", QtdFaltasCompensadas = 2 }
            };
            var usuario = CriarUsuarioGestor();
            var frequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = "12345",
                TotalAusencias = 6,
                TotalCompensacoes = 3,
                DisciplinaId = "123",
                PeriodoFim = periodo.PeriodoFim,
                TurmaId = turma.CodigoTurma
            };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno> { frequenciaAluno });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            await useCase.Executar(0, compensacaoDto);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Compensacao_Aluno_Aula_Selecao_Automatica_Deve_Gravar_Sugestoes()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>
            {
                new CompensacaoAusenciaAlunoDto 
                { 
                    Id = "12345", 
                    QtdFaltasCompensadas = 2,
                    CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto>() 
                }
            };
            var usuario = CriarUsuarioGestor();
            var frequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = "12345",
                TotalAusencias = 6,
                TotalCompensacoes = 3,
                DisciplinaId = "123",
                PeriodoFim = periodo.PeriodoFim,
                TurmaId = turma.CodigoTurma
            };

            var faltasNaoCompensadas = new List<RegistroFaltasNaoCompensadaDto>
            {
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 1, DataAula = DateTime.Today, NumeroAula = 1, Sugestao = true },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 2, DataAula = DateTime.Today, NumeroAula = 2, Sugestao = true },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 3, DataAula = DateTime.Today, NumeroAula = 3, Sugestao = false }
            };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno> { frequenciaAluno });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(faltasNaoCompensadas);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            await useCase.Executar(0, compensacaoDto);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Compensacao_Migrada_Regencia_Sem_Componentes_Nao_Deve_Lancar_Exception()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.DisciplinasRegenciaIds = null;
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>();
            var usuario = CriarUsuarioGestor();
            var compensacaoExistente = new SME.SGP.Dominio.CompensacaoAusencia { Id = 1, Migrado = true }; 
            var componenteRegencia = new DisciplinaDto { Id = 123, Regencia = true };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarSalvamento();
            
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(compensacaoExistente);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaDto> { componenteRegencia });

            await useCase.Executar(1, compensacaoDto);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
        }

        [Fact]
        public async Task Executar_Professor_CJ_Sem_Atribuicao_Deve_Lancar_Negocio_Exception()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            var usuario = CriarUsuarioProfessorCJ();
            var parametroAtivo = new ParametrosSistema { Ativo = true };

            ConfigurarMocksBasicos(turma, periodo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametroAtivo);

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDisciplinasPerfilCJQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaResposta>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(0, compensacaoDto));
            Assert.Equal("Você não pode fazer alterações ou inclusões nesta turma e data.", exception.Message);
        }

        [Fact]
        public async Task Executar_Sem_Notificacao_Compensacao_Nao_Deve_Publicar_Fila()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>();
            var usuario = CriarUsuarioGestor();

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaSeExisteParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            await useCase.Executar(0, compensacaoDto);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Compensacao_Aluno_Aula_Quantidade_Maior_Que_Selecionadas_Deve_Completar_Automaticamente()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>
            {
                new CompensacaoAusenciaAlunoDto 
                { 
                    Id = "12345", 
                    QtdFaltasCompensadas = 3, 
                    CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto> 
                    {
                        new CompensacaoAusenciaAlunoAulaDto { RegistroFrequenciaAlunoId = 2 }
                    }
                }
            };
            var usuario = CriarUsuarioGestor();
            var frequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = "12345",
                TotalAusencias = 8,
                TotalCompensacoes = 3,
                DisciplinaId = "123",
                PeriodoFim = periodo.PeriodoFim,
                TurmaId = turma.CodigoTurma
            };

            var faltasNaoCompensadas = new List<RegistroFaltasNaoCompensadaDto>
            {
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 1, DataAula = DateTime.Today.AddDays(-3), NumeroAula = 1 },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 2, DataAula = DateTime.Today.AddDays(-2), NumeroAula = 2 },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 3, DataAula = DateTime.Today.AddDays(-1), NumeroAula = 3 },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 4, DataAula = DateTime.Today, NumeroAula = 4 },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 5, DataAula = DateTime.Today, NumeroAula = 5 }
            };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno> { frequenciaAluno });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(faltasNaoCompensadas);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            await useCase.Executar(0, compensacaoDto);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Executar_Compensacao_Aluno_Aula_Quantidade_Menor_Que_Selecionadas_Deve_Gravar_Apenas_Mais_Antigas()
        {

            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>
            {
                new CompensacaoAusenciaAlunoDto 
                { 
                    Id = "12345", 
                    QtdFaltasCompensadas = 2, 
                    CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto> 
                    {
                        new CompensacaoAusenciaAlunoAulaDto { RegistroFrequenciaAlunoId = 1 },
                        new CompensacaoAusenciaAlunoAulaDto { RegistroFrequenciaAlunoId = 2 },
                        new CompensacaoAusenciaAlunoAulaDto { RegistroFrequenciaAlunoId = 3 }
                    }
                }
            };
            var usuario = CriarUsuarioGestor();
            var frequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = "12345",
                TotalAusencias = 8,
                TotalCompensacoes = 3,
                DisciplinaId = "123",
                PeriodoFim = periodo.PeriodoFim,
                TurmaId = turma.CodigoTurma
            };

            var faltasNaoCompensadas = new List<RegistroFaltasNaoCompensadaDto>
            {
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 1, DataAula = DateTime.Today.AddDays(-3), NumeroAula = 1 },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 2, DataAula = DateTime.Today.AddDays(-2), NumeroAula = 2 },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 3, DataAula = DateTime.Today.AddDays(-1), NumeroAula = 3 },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 4, DataAula = DateTime.Today, NumeroAula = 4 },
                new RegistroFaltasNaoCompensadaDto { CodigoAluno = "12345", RegistroFrequenciaAlunoId = 5, DataAula = DateTime.Today, NumeroAula = 5 }
            };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno> { frequenciaAluno });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(faltasNaoCompensadas);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            await useCase.Executar(0, compensacaoDto);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Com_Mover_Arquivos_Deve_Executar_Comandos()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Descricao = "Nova descrição";
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>();
            var usuario = CriarUsuarioGestor();
            var compensacaoExistente = new SME.SGP.Dominio.CompensacaoAusencia { Id = 1, Descricao = "Descrição antiga" };

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(compensacaoExistente);

            mediatorMock.Setup(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(string.Empty);

            mediatorMock.Setup(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            await useCase.Executar(1, compensacaoDto);

            mediatorMock.Verify(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Professor_CJ_Com_Atribuicao_Deve_Permitir_Salvar()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            compensacaoDto.Alunos = new List<CompensacaoAusenciaAlunoDto>();
            var usuario = CriarUsuarioProfessorCJ();
            var parametroAtivo = new ParametrosSistema { Ativo = true };

            ConfigurarMocksBasicos(turma, periodo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametroAtivo);

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDisciplinasPerfilCJQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DisciplinaResposta> { new DisciplinaResposta { CodigoComponenteCurricular = 123 } });

            ConfigurarRestanteFluxoValido();

            await useCase.Executar(0, compensacaoDto);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterDisciplinasPerfilCJQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Erro_Na_Execucao_Deve_Fazer_Rollback()
        {
            var turma = CriarTurma();
            var periodo = CriarPeriodo();
            var compensacaoDto = CriarCompensacaoDto();
            var usuario = CriarUsuarioGestor();

            ConfigurarMocksFluxoBasicoValido(turma, periodo, usuario);
            ConfigurarComponenteCurricular();

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new Exception("Erro de teste"));

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(0, compensacaoDto));

            unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #region Métodos Auxiliares

        private CompensacaoAusenciaDto CriarCompensacaoDto()
        {
            return new CompensacaoAusenciaDto
            {
                TurmaId = "123456",
                Bimestre = 1,
                DisciplinaId = "123",
                Atividade = "Atividade Teste",
                Descricao = "Descrição teste",
                Alunos = new List<CompensacaoAusenciaAlunoDto>()
            };
        }

        private Turma CriarTurma()
        {
            return new Turma
            {
                Id = 1,
                CodigoTurma = "123456",
                AnoLetivo = 2025,
                ModalidadeCodigo = Modalidade.Fundamental,
                Semestre = 1
            };
        }

        private PeriodoEscolar CriarPeriodo()
        {
            return new PeriodoEscolar
            {
                Id = 1,
                Bimestre = 1,
                PeriodoInicio = DateTime.Today.AddDays(-30),
                PeriodoFim = DateTime.Today.AddDays(30),
                TipoCalendarioId = 1
            };
        }

        private Usuario CriarUsuarioGestor()
        {
            var usuario = new Usuario();
            usuario.DefinirPerfilAtual(new Guid("45E1E074-37D6-E911-ABD6-F81654FE895D"));
            return usuario;
        }

        private Usuario CriarUsuarioProfessor()
        {
            var usuario = new Usuario { CodigoRf = "12345" };
            usuario.DefinirPerfilAtual(new Guid("40E1E074-37D6-E911-ABD6-F81654FE895D"));
            return usuario;
        }

        private Usuario CriarUsuarioProfessorCJ()
        {
            var usuario = new Usuario { CodigoRf = "12345" };
            usuario.DefinirPerfilAtual(new Guid("41e1e074-37d6-e911-abd6-f81654fe895d"));
            return usuario;
        }

        private void ConfigurarMocksBasicos(Turma turma, PeriodoEscolar periodo)
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<PeriodoEscolar> { periodo });
        }

        private void ConfigurarUsuario(Usuario usuario)
        {
            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(usuario);
        }

        private void ConfigurarMocksFluxoBasicoValido(Turma turma, PeriodoEscolar periodo, Usuario usuario)
        {
            ConfigurarMocksBasicos(turma, periodo);
            ConfigurarUsuario(usuario);

            var parametroAtivo = new ParametrosSistema { Ativo = true };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametroAtivo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorAnoTurmaENomeQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((SME.SGP.Dominio.CompensacaoAusencia)null);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteRegistraFrequenciaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);
        }

        private void ConfigurarComponenteCurricular()
        {
            var componente = new DisciplinaDto { Id = 123, Regencia = false };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaDto> { componente });
        }

        private void ConfigurarSalvamento()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);
        }

        private void ConfigurarRestanteFluxoValido()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorAnoTurmaENomeQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((SME.SGP.Dominio.CompensacaoAusencia)null);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteRegistraFrequenciaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            ConfigurarComponenteCurricular();
            ConfigurarSalvamento();
        }

        #endregion
    }
}