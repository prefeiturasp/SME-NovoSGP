using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class NotificarCompensacaoAusenciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioCompensacaoAusenciaAlunoConsulta> repositorioCompensacaoAusenciaAlunoConsultaMock;
        private readonly Mock<IRepositorioCompensacaoAusenciaAluno> repositorioCompensacaoAusenciaAlunoMock;
        private readonly Mock<IRepositorioCompensacaoAusencia> repositorioCompensacaoAusenciaMock;
        private readonly Mock<IRepositorioTurmaConsulta> repositorioTurmaConsultaMock;
        private readonly NotificarCompensacaoAusenciaUseCase useCase;

        public NotificarCompensacaoAusenciaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repositorioCompensacaoAusenciaAlunoConsultaMock = new Mock<IRepositorioCompensacaoAusenciaAlunoConsulta>();
            repositorioCompensacaoAusenciaAlunoMock = new Mock<IRepositorioCompensacaoAusenciaAluno>();
            repositorioCompensacaoAusenciaMock = new Mock<IRepositorioCompensacaoAusencia>();
            repositorioTurmaConsultaMock = new Mock<IRepositorioTurmaConsulta>();

            useCase = new NotificarCompensacaoAusenciaUseCase(
                mediatorMock.Object,
                repositorioCompensacaoAusenciaAlunoConsultaMock.Object,
                repositorioCompensacaoAusenciaAlunoMock.Object,
                repositorioCompensacaoAusenciaMock.Object,
                repositorioTurmaConsultaMock.Object
            );
        }

        [Fact]
        public void Construtor_Deve_Lancar_Exception_Quando_Mediator_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new NotificarCompensacaoAusenciaUseCase(
                null,
                repositorioCompensacaoAusenciaAlunoConsultaMock.Object,
                repositorioCompensacaoAusenciaAlunoMock.Object,
                repositorioCompensacaoAusenciaMock.Object,
                repositorioTurmaConsultaMock.Object
            ));
        }

        [Fact]
        public void Construtor_Deve_Lancar_Exception_Quando_Repositorio_Compensacao_Ausencia_Aluno_Consulta_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new NotificarCompensacaoAusenciaUseCase(
                mediatorMock.Object,
                null,
                repositorioCompensacaoAusenciaAlunoMock.Object,
                repositorioCompensacaoAusenciaMock.Object,
                repositorioTurmaConsultaMock.Object
            ));
        }

        [Fact]
        public void Construtor_Deve_Lancar_Exception_Quando_Repositorio_Compensacao_Ausencia_Aluno_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new NotificarCompensacaoAusenciaUseCase(
                mediatorMock.Object,
                repositorioCompensacaoAusenciaAlunoConsultaMock.Object,
                null,
                repositorioCompensacaoAusenciaMock.Object,
                repositorioTurmaConsultaMock.Object
            ));
        }

        [Fact]
        public void Construtor_Deve_Lancar_Exception_Quando_Repositorio_Compensacao_Ausencia_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new NotificarCompensacaoAusenciaUseCase(
                mediatorMock.Object,
                repositorioCompensacaoAusenciaAlunoConsultaMock.Object,
                repositorioCompensacaoAusenciaAlunoMock.Object,
                null,
                repositorioTurmaConsultaMock.Object
            ));
        }

        [Fact]
        public void Construtor_Deve_Lancar_Exception_Quando_Repositorio_Turma_Consulta_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new NotificarCompensacaoAusenciaUseCase(
                mediatorMock.Object,
                repositorioCompensacaoAusenciaAlunoConsultaMock.Object,
                repositorioCompensacaoAusenciaAlunoMock.Object,
                repositorioCompensacaoAusenciaMock.Object,
                null
            ));
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Compensacao_Nao_Possui_Alunos()
        {
            var filtro = new FiltroNotificacaoCompensacaoAusenciaDto(1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            repositorioCompensacaoAusenciaAlunoConsultaMock
                .Setup(r => r.ObterPorCompensacao(1))
                .ReturnsAsync((IEnumerable<CompensacaoAusenciaAluno>)null);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Compensacao_Possui_Alunos_Vazios()
        {
            var filtro = new FiltroNotificacaoCompensacaoAusenciaDto(1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            repositorioCompensacaoAusenciaAlunoConsultaMock
                .Setup(r => r.ObterPorCompensacao(1))
                .ReturnsAsync(new List<CompensacaoAusenciaAluno>());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Todos_Alunos_Ja_Foram_Notificados()
        {
            var filtro = new FiltroNotificacaoCompensacaoAusenciaDto(1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var alunos = new List<CompensacaoAusenciaAluno>
            {
                new CompensacaoAusenciaAluno { Notificado = true, QuantidadeFaltasCompensadas = 1 },
                new CompensacaoAusenciaAluno { Notificado = true, QuantidadeFaltasCompensadas = 2 }
            };

            repositorioCompensacaoAusenciaAlunoConsultaMock
                .Setup(r => r.ObterPorCompensacao(1))
                .ReturnsAsync(alunos);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Alunos_Nao_Possuem_Faltas_Compensadas()
        {
            var filtro = new FiltroNotificacaoCompensacaoAusenciaDto(1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var alunos = new List<CompensacaoAusenciaAluno>
            {
                new CompensacaoAusenciaAluno { Notificado = false, QuantidadeFaltasCompensadas = 0 },
                new CompensacaoAusenciaAluno { Notificado = false, QuantidadeFaltasCompensadas = 0 }
            };

            repositorioCompensacaoAusenciaAlunoConsultaMock
                .Setup(r => r.ObterPorCompensacao(1))
                .ReturnsAsync(alunos);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Notificar_Compensacao_Normal_Quando_Periodo_Aberto()
        {
            await Configurar_Cenario_Notificacao(true, false);

            var filtro = new FiltroNotificacaoCompensacaoAusenciaDto(1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            Verificar_Notificacao_Normal();
        }

        [Fact]
        public async Task Executar_Deve_Notificar_Compensacao_Extemporanea_Quando_Periodo_Fechado_E_Parametro_Ativo()
        {
            await Configurar_Cenario_Notificacao(false, true);

            var filtro = new FiltroNotificacaoCompensacaoAusenciaDto(1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            Verificar_Notificacao_Extemporanea();
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Exception_Quando_Periodo_Fechado_E_Parametro_Inativo()
        {
            await Configurar_Cenario_Notificacao(false, false);

            var filtro = new FiltroNotificacaoCompensacaoAusenciaDto(1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(mensagem));

            Assert.Equal("Compensação de ausência não permitida, É necessário que o período esteja aberto", excecao.Message);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Exception_Quando_Disciplina_Nao_Encontrada()
        {
            await Configurar_Cenario_Base_Com_Disciplina_Invalida();

            var filtro = new FiltroNotificacaoCompensacaoAusenciaDto(1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(mensagem));

            Assert.Equal("Componente curricular não encontrado no EOL.", excecao.Message);
        }

        [Fact]
        public async Task Executar_Deve_Marcar_Alunos_Como_Notificados()
        {
            await Configurar_Cenario_Notificacao(true, false);

            var filtro = new FiltroNotificacaoCompensacaoAusenciaDto(1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            repositorioCompensacaoAusenciaAlunoMock.Verify(r => r.Salvar(It.Is<CompensacaoAusenciaAluno>(a => a.Notificado == true)), Times.Exactly(2));
        }

        private async Task Configurar_Cenario_Notificacao(bool periodoAberto, bool parametroAtivo)
        {
            var alunos = Criar_Alunos_Nao_Notificados();
            var compensacao = Criar_Compensacao();
            var turma = Criar_Turma();
            var professor = Criar_Professor();
            var disciplina = Criar_Disciplina();
            var parametro = parametroAtivo ? new ParametrosSistema { Ativo = true } : null;
            var alunosEol = Criar_Alunos_Eol();

            repositorioCompensacaoAusenciaAlunoConsultaMock
                .Setup(r => r.ObterPorCompensacao(1))
                .ReturnsAsync(alunos);

            repositorioCompensacaoAusenciaMock
                .Setup(r => r.ObterPorId(1))
                .Returns(compensacao);

            repositorioTurmaConsultaMock
                .Setup(r => r.ObterTurmaComUeEDrePorId(1))
                .ReturnsAsync(turma);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(disciplina);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioCoreSSOQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(professor);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoAberto);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametro);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAlunosEolPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosEol);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirNotificacaoCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<EnviarNotificacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);
        }

        private async Task Configurar_Cenario_Base_Com_Disciplina_Invalida()
        {
            var alunos = Criar_Alunos_Nao_Notificados();
            var compensacao = Criar_Compensacao();
            var turma = Criar_Turma();

            repositorioCompensacaoAusenciaAlunoConsultaMock
                .Setup(r => r.ObterPorCompensacao(1))
                .ReturnsAsync(alunos);

            repositorioCompensacaoAusenciaMock
                .Setup(r => r.ObterPorId(1))
                .Returns(compensacao);

            repositorioTurmaConsultaMock
                .Setup(r => r.ObterTurmaComUeEDrePorId(1))
                .ReturnsAsync(turma);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DisciplinaDto)null);
        }

        private void Verificar_Notificacao_Normal()
        {
            mediatorMock.Verify(m => m.Send(It.Is<EnviarNotificacaoCommand>(
                cmd => cmd.Titulo.Contains("Atividade de compensação da turma") &&
                       cmd.CategoriaNotificacao == NotificacaoCategoria.Aviso &&
                       cmd.TipoNotificacao == NotificacaoTipo.Frequencia
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        private void Verificar_Notificacao_Extemporanea()
        {
            mediatorMock.Verify(m => m.Send(It.Is<EnviarNotificacaoCommand>(
                cmd => cmd.Titulo.Contains("Atividade de compensação de ausência extemporânea") &&
                       cmd.CategoriaNotificacao == NotificacaoCategoria.Alerta &&
                       cmd.TipoNotificacao == NotificacaoTipo.Frequencia
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        private List<CompensacaoAusenciaAluno> Criar_Alunos_Nao_Notificados()
        {
            return new List<CompensacaoAusenciaAluno>
            {
                new CompensacaoAusenciaAluno
                {
                    CodigoAluno = "123",
                    Notificado = false,
                    QuantidadeFaltasCompensadas = 2
                },
                new CompensacaoAusenciaAluno
                {
                    CodigoAluno = "456",
                    Notificado = false,
                    QuantidadeFaltasCompensadas = 3
                }
            };
        }

        private SME.SGP.Dominio.CompensacaoAusencia Criar_Compensacao()
        {
            return new SME.SGP.Dominio.CompensacaoAusencia
            {
                Id = 1,
                TurmaId = 1,
                DisciplinaId = "138",
                Bimestre = 2,
                Nome = "Compensação Teste",
                CriadoRF = "12345"
            };
        }

        private Turma Criar_Turma()
        {
            return new Turma
            {
                Id = 1,
                CodigoTurma = "T123",
                Nome = "1A",
                AnoLetivo = 2023,
                ModalidadeCodigo = Modalidade.Fundamental,
                Ue = new Ue
                {
                    CodigoUe = "U123",
                    Nome = "Escola Teste",
                    TipoEscola = Dominio.TipoEscola.EMEF,
                    Dre = new SME.SGP.Dominio.Dre
                    {
                        CodigoDre = "D123",
                        Nome = "DRE Teste"
                    }
                }
            };
        }

        private MeusDadosDto Criar_Professor()
        {
            return new MeusDadosDto
            {
                Nome = "Professor Teste",
                CodigoRf = "12345"
            };
        }

        private DisciplinaDto Criar_Disciplina()
        {
            return new DisciplinaDto
            {
                CodigoComponenteCurricular = 138,
                Nome = "Português"
            };
        }

        private List<AlunoPorTurmaResposta> Criar_Alunos_Eol()
        {
            return new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "123",
                    NomeAluno = "Aluno 1",
                    NumeroAlunoChamada = 1
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "456",
                    NomeAluno = "Aluno 2",
                    NumeroAlunoChamada = 2
                }
            };
        }
    }
}