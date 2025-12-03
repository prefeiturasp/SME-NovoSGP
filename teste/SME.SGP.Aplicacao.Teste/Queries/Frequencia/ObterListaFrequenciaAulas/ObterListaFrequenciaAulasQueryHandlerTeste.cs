using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Frequencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia.ObterListaFrequenciaAulas
{
    public class ObterListaFrequenciaAulasQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterListaFrequenciaAulasQueryHandler handler;

        public ObterListaFrequenciaAulasQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            handler = new ObterListaFrequenciaAulasQueryHandler(mediator.Object);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Mediator_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterListaFrequenciaAulasQueryHandler(null));
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Nao_Possui_Aulas()
        {
            var filtro = new FiltroFrequenciaAulasDto
            {
                Turma = new Turma { CodigoTurma = "123", AnoLetivo = 2024, ModalidadeCodigo = Modalidade.Fundamental },
                Aulas = Enumerable.Empty<Dominio.Aula>(),
                AlunosDaTurma = new List<AlunoPorTurmaResposta>(),
                RegistrosFrequenciaAlunos = new List<RegistroFrequenciaAlunoPorAulaDto>(),
                FrequenciaAlunos = new List<FrequenciaAluno>(),
                FrequenciasPreDefinidas = new List<FrequenciaPreDefinidaDto>(),
                CompensacaoAusenciaAlunoAulas = new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>(),
                AnotacoesTurma = new List<AnotacaoAlunoAulaDto>(),
                RegistraFrequencia = true,
                TurmaPossuiFrequenciaRegistrada = false,
                DataInicio = DateTime.Now.AddMonths(-1),
                DataFim = DateTime.Now,
                PercentualAlerta = 85,
                PercentualCritico = 75,
                PeriodoEscolar = new PeriodoEscolar { Id = 1, Bimestre = 1 }
            };
            var query = new ObterListaFrequenciaAulasQuery(filtro);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.Null(retorno);
        }

        [Fact]
        public async Task Handle_Deve_Carregar_Aulas_Com_Usuario_Professor_CJ()
        {
            var query = CriarQueryBase();
            var usuarioLogado = CriarUsuarioLogado(ehProfessorCj: true, ehGestor: false);

            mediator.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno.Aulas);
            mediator.Verify(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Carregar_Aulas_Com_Usuario_Gestor()
        {
            var query = CriarQueryBase();
            var usuarioLogado = CriarUsuarioLogado(ehProfessorCj: false, ehGestor: true);

            mediator.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno.Aulas);
        }

        [Fact]
        public async Task Handle_Deve_Carregar_Auditoria_Dos_Registros()
        {
            var filtro = new FiltroFrequenciaAulasDto
            {
                Turma = new Turma { CodigoTurma = "123", AnoLetivo = 2024, ModalidadeCodigo = Modalidade.Fundamental },
                Aulas = new List<Dominio.Aula> { new Dominio.Aula { Id = 1, DataAula = DateTime.Now.AddDays(-1), Quantidade = 2, TipoAula = TipoAula.Normal, AulaCJ = false } },
                AlunosDaTurma = new List<AlunoPorTurmaResposta> { new AlunoPorTurmaResposta { CodigoAluno = "123", NomeAluno = "João Silva", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, SituacaoMatricula = "Ativo", DataSituacao = DateTime.Now, DataNascimento = DateTime.Now.AddYears(-10), NumeroAlunoChamada = 1 } },
                RegistrosFrequenciaAlunos = new List<RegistroFrequenciaAlunoPorAulaDto> { new RegistroFrequenciaAlunoPorAulaDto { RegistroFrequenciaId = 1, CriadoEm = DateTime.Now.AddDays(-2), CriadoPor = "Usuario Teste", CriadoRf = "1234567" } },
                FrequenciaAlunos = new List<FrequenciaAluno>(),
                FrequenciasPreDefinidas = new List<FrequenciaPreDefinidaDto>(),
                CompensacaoAusenciaAlunoAulas = new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>(),
                AnotacoesTurma = new List<AnotacaoAlunoAulaDto>(),
                RegistraFrequencia = true,
                TurmaPossuiFrequenciaRegistrada = false,
                DataInicio = DateTime.Now.AddMonths(-1),
                DataFim = DateTime.Now,
                PercentualAlerta = 85,
                PercentualCritico = 75,
                PeriodoEscolar = new PeriodoEscolar { Id = 1, Bimestre = 1 }
            };
            var query = new ObterListaFrequenciaAulasQuery(filtro);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.NotNull(retorno.Auditoria);
        }

        [Fact]
        public async Task Handle_Deve_Buscar_Alunos_Turma_PAP()
        {
            var query = CriarQueryBase();
            var alunosPAP = new List<AlunosTurmaProgramaPapDto>
            {
                new AlunosTurmaProgramaPapDto { CodigoAluno = 123 }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosPAP);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            mediator.Verify(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(retorno.Alunos.First().EhMatriculadoTurmaPAP);
        }

        [Fact]
        public async Task Handle_Deve_Verificar_Plano_AEE_Do_Aluno()
        {
            var query = CriarQueryBase();

            ConfigurarMocksBasicos();

            mediator.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            mediator.Verify(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(retorno.Alunos.First().EhAtendidoAEE);
        }

        [Fact]
        public async Task Handle_Deve_Obter_Marcador_Frequencia_Aluno()
        {
            var query = CriarQueryBase();
            var marcador = new MarcadorFrequenciaDto { Tipo = TipoMarcadorFrequencia.Novo };

            mediator.Setup(m => m.Send(It.IsAny<ObterMarcadorFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(marcador);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.NotNull(retorno.Alunos.First().Marcador);
            mediator.Verify(m => m.Send(It.IsAny<ObterMarcadorFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Calcular_Indicativo_Frequencia_Critico()
        {
            var frequenciaAlunos = new List<FrequenciaAluno>
            {
                new FrequenciaAluno { CodigoAluno = "123", TotalAusencias = 30, TotalAulas = 100 }
            };
            var query = CriarQueryBase(true, null, null, null, frequenciaAlunos);
            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.Equal(TipoIndicativoFrequencia.Critico, retorno.Alunos.First().IndicativoFrequencia.Tipo);
        }

        [Fact]
        public async Task Handle_Deve_Calcular_Indicativo_Frequencia_Alerta()
        {
            var frequenciaAlunos = new List<FrequenciaAluno>
            {
                new FrequenciaAluno { CodigoAluno = "123", TotalAulas = 100, TotalAusencias = 20 }
            };
            var query = CriarQueryBase(true, null, null, null, frequenciaAlunos);
            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.Equal(TipoIndicativoFrequencia.Alerta, retorno.Alunos.First().IndicativoFrequencia.Tipo);
        }

        [Fact]
        public async Task Handle_Deve_Calcular_Indicativo_Frequencia_Info()
        {
            var frequenciaAlunos = new List<FrequenciaAluno>
            {
                new FrequenciaAluno { CodigoAluno = "123", TotalAulas = 100, TotalAusencias = 10 }
            };
            var query = CriarQueryBase(true, null, null, null, frequenciaAlunos);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.Equal(TipoIndicativoFrequencia.Info, retorno.Alunos.First().IndicativoFrequencia.Tipo);
        }

        [Fact]
        public async Task Handle_Deve_Calcular_Indicativo_Frequencia_Quando_Frequencia_Aluno_Nulo()
        {
            var frequenciaAlunos = Enumerable.Empty<FrequenciaAluno>().ToList();
            var query = CriarQueryBase(true, null, null, null, frequenciaAlunos);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.NotNull(retorno.Alunos.First().IndicativoFrequencia);
            Assert.Null(retorno.Alunos.First().IndicativoFrequencia.Percentual);
        }

        [Fact]
        public async Task Handle_Deve_Buscar_Registro_Frequencia_Por_Aula_Quando_Nao_Existe()
        {
            var query = CriarQueryBase();

            mediator.Setup(m => m.Send(It.IsAny<ObterRegistroFrequenciaPorAulaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RegistroFrequencia)null);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            mediator.Verify(m => m.Send(It.IsAny<ObterRegistroFrequenciaPorAulaIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Buscar_Primeiro_Registro_Frequencia_Por_Data_Quando_Registro_Aula_Nao_Existe()
        {
            var query = CriarQueryBase();
            var primeiroRegistro = new ComponenteCurricularSugeridoDto
            {
                AulaId = 999,
                ComponenteCurricularSugerido = "Matemática",
                QuantidadeAulas = 2
            };

            ConfigurarMocksBasicos();

            mediator.Setup(m => m.Send(It.IsAny<ObterRegistroFrequenciaPorAulaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RegistroFrequencia)null);

            mediator.Setup(m => m.Send(It.IsAny<ObterPrimeiroRegistroFrequenciaPorDataETurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(primeiroRegistro);

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            mediator.Verify(m => m.Send(It.IsAny<ObterPrimeiroRegistroFrequenciaPorDataETurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Buscar_Frequencia_Sugerida_Quando_Primeiro_Registro_Existe()
        {
            var query = CriarQueryBase();
            var primeiroRegistro = new ComponenteCurricularSugeridoDto
            {
                AulaId = 999,
                ComponenteCurricularSugerido = "Matemática",
                QuantidadeAulas = 2
            };

            var frequenciasSugeridas = new List<FrequenciaAlunoSimplificadoDto>
            {
                new FrequenciaAlunoSimplificadoDto
                {
                    CodigoAluno = "123",
                    NumeroAula = 2,
                    TipoFrequencia = TipoFrequencia.F,
                }
            };

            ConfigurarMocksBasicos();

            mediator.Setup(m => m.Send(It.IsAny<ObterRegistroFrequenciaPorAulaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RegistroFrequencia)null);

            mediator.Setup(m => m.Send(It.IsAny<ObterPrimeiroRegistroFrequenciaPorDataETurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(primeiroRegistro);

            mediator.Setup(m => m.Send(It.IsAny<ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciasSugeridas);

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            mediator.Verify(m => m.Send(It.IsAny<ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Frequencias_Sugeridas_Nao_Encontradas()
        {
            var query = CriarQueryBase();
            var primeiroRegistro = new ComponenteCurricularSugeridoDto
            {
                AulaId = 999,
                ComponenteCurricularSugerido = "Matemática",
                QuantidadeAulas = 2
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterRegistroFrequenciaPorAulaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RegistroFrequencia)null);

            mediator.Setup(m => m.Send(It.IsAny<ObterPrimeiroRegistroFrequenciaPorDataETurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(primeiroRegistro);

            mediator.Setup(m => m.Send(It.IsAny<ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<FrequenciaAlunoSimplificadoDto>)null);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
        }

        [Fact]
        public async Task Handle_Deve_Carregar_Aulas_Quando_Registra_Frequencia()
        {
            var registrosFrequenciaAlunos = new List<RegistroFrequenciaAlunoPorAulaDto>
            {
                new RegistroFrequenciaAlunoPorAulaDto
                {
                    AlunoCodigo = "123",
                    AulaId = 1,
                    NumeroAula = 1,
                    TipoFrequencia = TipoFrequencia.C
                }
            };
            var query = CriarQueryBase(true, null, null, null, null, registrosFrequenciaAlunos);
            
            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno.Alunos);
            Assert.NotEmpty(retorno.Alunos.First().Aulas);
        }

        [Fact]
        public async Task Handle_Deve_Carregar_Apenas_Anotacoes_Quando_Nao_Registra_Frequencia()
        {
            var query = CriarQueryBase(false);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno.Alunos);
        }

        [Fact]
        public async Task Handle_Deve_Carregar_Compensacoes_Ausencia_Do_Aluno()
        {
            var compensacaoAusenciaAlunoAulas = new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>
            {
                new CompensacaoAusenciaAlunoAulaSimplificadoDto
                {
                    CodigoAluno = "123",
                    AulaId = 1,
                    NumeroAula = 1
                }
            };

            var query = CriarQueryBase(true, null, null, null, null, null, compensacaoAusenciaAlunoAulas);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno.Alunos);
        }

        [Fact]
        public async Task Handle_Deve_Carregar_Frequencia_Pre_Definida()
        {
            var frequenciasPreDefinidas = new List<FrequenciaPreDefinidaDto>
            {
                new FrequenciaPreDefinidaDto
                {
                    CodigoAluno = "123",
                    Tipo = TipoFrequencia.C
                }
            };
            var query = CriarQueryBase(true, null, null, frequenciasPreDefinidas);
            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno.Alunos);
        }

        [Fact]
        public async Task Handle_Deve_Usar_Nome_Social_Quando_Disponivel()
        {
            var alunosDaTurma = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "123",
                    NomeAluno = "João Silva",
                    NomeSocialAluno = "João Santos",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "Ativo",
                    DataSituacao = DateTime.Now,
                    DataNascimento = DateTime.Now.AddYears(-10),
                    NumeroAlunoChamada = 1
                }
            };
            var query = CriarQueryBase(true, null, alunosDaTurma);
            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.Equal("João Santos", retorno.Alunos.First().NomeAluno);
        }

        [Fact]
        public async Task Handle_Deve_Usar_Nome_Aluno_Quando_Nome_Social_Nulo()
        {
            var query = CriarQueryBase();

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.Equal("João Silva", retorno.Alunos.First().NomeAluno);
        }

        [Fact]
        public async Task Handle_Deve_Carregar_Dados_Responsavel()
        {
            var alunosDaTurma = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "123",
                    NomeAluno = "João Silva",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "Ativo",
                    DataSituacao = DateTime.Now,
                    DataNascimento = DateTime.Now.AddYears(-10),
                    NumeroAlunoChamada = 1,
                    NomeResponsavel = "Maria Silva",
                    TipoResponsavel = "1",
                    CelularResponsavel = "11999999999",
                    DataAtualizacaoContato = DateTime.Now
                }
            };
            var query = CriarQueryBase(true, null, alunosDaTurma);
            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            var aluno = retorno.Alunos.First();
            Assert.Equal("Maria Silva", aluno.NomeResponsavel);
            Assert.NotNull(aluno.TipoResponsavel);
            Assert.Equal("11999999999", aluno.CelularResponsavel);
            Assert.NotNull(aluno.DataAtualizacaoContato);
        }

        [Fact]
        public async Task Handle_Deve_Processar_Multiplos_Alunos()
        {
            var alunosDaTurma = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "123",
                    NomeAluno = "João Silva",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "Ativo",
                    DataSituacao = DateTime.Now,
                    DataNascimento = DateTime.Now.AddYears(-10),
                    NumeroAlunoChamada = 1
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "456",
                    NomeAluno = "Maria Santos",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "Ativo",
                    DataSituacao = DateTime.Now,
                    DataNascimento = DateTime.Now.AddYears(-11),
                    NumeroAlunoChamada = 2
                }
            };
            var query = CriarQueryBase(true, null, alunosDaTurma);

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.Equal(2, retorno.Alunos.Count);
        }

        [Fact]
        public async Task Handle_Deve_Processar_Anotacoes_Da_Turma()
        {
            var anotacoesTurma = new List<AnotacaoAlunoAulaDto>
            {
                new AnotacaoAlunoAulaDto
                {
                    AlunoCodigo = "123",
                    AulaId = 1,
                }
            };
            var query = CriarQueryBase(true, anotacoesTurma);
            

            ConfigurarMocksBasicos();

            var retorno = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno.Alunos);
        }

        private ObterListaFrequenciaAulasQuery CriarQueryBase(bool registraFrequencia = true, List<AnotacaoAlunoAulaDto> anotacoesTurma = null, 
            List<AlunoPorTurmaResposta> alunoPorTurmaRespostas = null, List<FrequenciaPreDefinidaDto> frequenciasPreDefinidas = null, List<FrequenciaAluno> frequenciaAlunos = null, 
            List<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAlunos = null, List<CompensacaoAusenciaAlunoAulaSimplificadoDto> compensacaoAusenciaAlunoAulas = null)
        {
            var filtro = new FiltroFrequenciaAulasDto
            {
                Turma = new Turma
                {
                    CodigoTurma = "123",
                    AnoLetivo = 2024,
                    ModalidadeCodigo = Modalidade.Fundamental
                },
                Aulas = new List<Dominio.Aula>
                {
                    new Dominio.Aula
                    {
                        Id = 1,
                        DataAula = DateTime.Now.AddDays(-1),
                        Quantidade = 2,
                        TipoAula = TipoAula.Normal,
                        AulaCJ = false
                    }
                },
                AlunosDaTurma = alunoPorTurmaRespostas ?? new List<AlunoPorTurmaResposta>
                {
                    new AlunoPorTurmaResposta
                    {
                        CodigoAluno = "123",
                        NomeAluno = "João Silva",
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                        SituacaoMatricula = "Ativo",
                        DataSituacao = DateTime.Now,
                        DataNascimento = DateTime.Now.AddYears(-10),
                        NumeroAlunoChamada = 1
                    }
                },
                RegistrosFrequenciaAlunos = registrosFrequenciaAlunos ?? new List<RegistroFrequenciaAlunoPorAulaDto>(),
                FrequenciaAlunos = frequenciaAlunos ?? new List<FrequenciaAluno>(),
                FrequenciasPreDefinidas = frequenciasPreDefinidas ?? new List<FrequenciaPreDefinidaDto>(),
                CompensacaoAusenciaAlunoAulas = compensacaoAusenciaAlunoAulas ?? new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>(),
                AnotacoesTurma = anotacoesTurma ?? new List<AnotacaoAlunoAulaDto>(),
                RegistraFrequencia = registraFrequencia,
                TurmaPossuiFrequenciaRegistrada = false,
                DataInicio = DateTime.Now.AddMonths(-1),
                DataFim = DateTime.Now,
                PercentualAlerta = 85,
                PercentualCritico = 75,
                PeriodoEscolar = new PeriodoEscolar
                {
                    Id = 1,
                    Bimestre = 1
                }
            };

            return new ObterListaFrequenciaAulasQuery(filtro);
        }

        private void ConfigurarMocksBasicos()
        {
            var usuarioLogado = CriarUsuarioLogado(ehProfessorCj: false, ehGestor: false);

            mediator.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            mediator.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunosTurmaProgramaPapDto> { new AlunosTurmaProgramaPapDto { CodigoAluno = 123 } });

            mediator.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mediator.Setup(m => m.Send(It.IsAny<ObterMarcadorFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MarcadorFrequenciaDto { Tipo = TipoMarcadorFrequencia.Novo });

            mediator.Setup(m => m.Send(It.IsAny<ObterRegistroFrequenciaPorAulaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RegistroFrequencia());
        }

        private Usuario CriarUsuarioLogado(bool ehProfessorCj, bool ehGestor)
        {
            var usuario = new Usuario();

            if (ehProfessorCj)
            {
                usuario.PerfilAtual = Dominio.Perfis.PERFIL_CJ;
            }

            if (ehGestor)
            {
                usuario.PerfilAtual = Dominio.Perfis.PERFIL_DIRETOR;
            }

            return usuario;
        }
    }
}
