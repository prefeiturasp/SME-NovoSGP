using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands
{
    public class InserirWorkflowReposicaoAulaCommandHandlerTeste
    {
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IComandosWorkflowAprovacao> _comandosWorkflowAprovacao;
        private readonly InserirWorkflowReposicaoAulaCommandHandler _handler;

        public InserirWorkflowReposicaoAulaCommandHandlerTeste()
        {
            _configuration = new Mock<IConfiguration>();
            _comandosWorkflowAprovacao = new Mock<IComandosWorkflowAprovacao>();
            _handler = new InserirWorkflowReposicaoAulaCommandHandler(_configuration.Object, _comandosWorkflowAprovacao.Object);
        }

        [Fact]
        public async Task Deve_Inserir_Workflow_Com_Sucesso()
        {
            var command = CriarCommand();
            _configuration.Setup(c => c["UrlFrontEnd"]).Returns("http://localhost/");
            _comandosWorkflowAprovacao.Setup(c => c.Salvar(It.IsAny<WorkflowAprovacaoDto>())).ReturnsAsync(1);

            var resultado = await _handler.Handle(command, CancellationToken.None);

            Assert.True(resultado > 0);
            _comandosWorkflowAprovacao.Verify(c => c.Salvar(It.IsAny<WorkflowAprovacaoDto>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Configurar_Workflow_Com_Dados_Corretos()
        {
            var command = CriarCommand();
            _configuration.Setup(c => c["UrlFrontEnd"]).Returns("http://localhost/");
            WorkflowAprovacaoDto workflowCapturado = null;
            _comandosWorkflowAprovacao.Setup(c => c.Salvar(It.IsAny<WorkflowAprovacaoDto>()))
                .Callback<WorkflowAprovacaoDto>(w => workflowCapturado = w)
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(workflowCapturado);
            Assert.Equal(command.Ano, workflowCapturado.Ano);
            Assert.Equal(NotificacaoCategoria.Workflow_Aprovacao, workflowCapturado.NotificacaoCategoria);
            Assert.Equal(command.AulaId, workflowCapturado.EntidadeParaAprovarId);
            Assert.Equal(WorkflowAprovacaoTipo.ReposicaoAula, workflowCapturado.Tipo);
            Assert.Equal(command.UeCodigo, workflowCapturado.UeId);
            Assert.Equal(command.DreCodigo, workflowCapturado.DreId);
            Assert.Equal(NotificacaoTipo.Calendario, workflowCapturado.NotificacaoTipo);
        }

        [Fact]
        public async Task Deve_Configurar_Titulo_Notificacao_Corretamente()
        {
            var command = CriarCommand();
            _configuration.Setup(c => c["UrlFrontEnd"]).Returns("http://localhost/");
            WorkflowAprovacaoDto workflowCapturado = null;
            _comandosWorkflowAprovacao.Setup(c => c.Salvar(It.IsAny<WorkflowAprovacaoDto>()))
                .Callback<WorkflowAprovacaoDto>(w => workflowCapturado = w)
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal($"Criação de Aula de Reposição na turma {command.TurmaNome}", workflowCapturado.NotificacaoTitulo);
        }

        [Fact]
        public async Task Deve_Configurar_Mensagem_Notificacao_Com_Link_Correto()
        {
            var command = CriarCommand();
            var urlFrontEnd = "http://localhost/";
            _configuration.Setup(c => c["UrlFrontEnd"]).Returns(urlFrontEnd);
            WorkflowAprovacaoDto workflowCapturado = null;
            _comandosWorkflowAprovacao.Setup(c => c.Salvar(It.IsAny<WorkflowAprovacaoDto>()))
                .Callback<WorkflowAprovacaoDto>(w => workflowCapturado = w)
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            var linkEsperado = $"{urlFrontEnd}calendario-escolar/calendario-professor/cadastro-aula/editar/{command.AulaId}/true?anoLetivo={command.Ano}&modalidade={(int)command.TurmaModalidade}&semestre={command.TurmaSemestre}&dre={command.DreCodigo}&ue={command.UeCodigo}&turma={command.TurmaCodigo}&turmaDescricao={command.TurmaDescricao}";
            Assert.Contains(linkEsperado, workflowCapturado.NotificacaoMensagem);
            Assert.Contains($"Foram criadas {command.Quantidade} aula(s) de reposição", workflowCapturado.NotificacaoMensagem);
            Assert.Contains(command.ComponenteCurricularNome, workflowCapturado.NotificacaoMensagem);
            Assert.Contains(command.TurmaNome, workflowCapturado.NotificacaoMensagem);
            Assert.Contains(command.UeNome, workflowCapturado.NotificacaoMensagem);
            Assert.Contains(command.DreNome, workflowCapturado.NotificacaoMensagem);
        }

        [Fact]
        public async Task Deve_Construir_Link_Com_Todos_Parametros_Query_String()
        {
            var command = CriarCommand();
            var urlFrontEnd = "http://localhost/";
            _configuration.Setup(c => c["UrlFrontEnd"]).Returns(urlFrontEnd);
            WorkflowAprovacaoDto workflowCapturado = null;
            _comandosWorkflowAprovacao.Setup(c => c.Salvar(It.IsAny<WorkflowAprovacaoDto>()))
                .Callback<WorkflowAprovacaoDto>(w => workflowCapturado = w)
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            Assert.Contains($"?anoLetivo={command.Ano}", workflowCapturado.NotificacaoMensagem);
            Assert.Contains($"&modalidade={(int)command.TurmaModalidade}", workflowCapturado.NotificacaoMensagem);
            Assert.Contains($"&semestre={command.TurmaSemestre}", workflowCapturado.NotificacaoMensagem);
            Assert.Contains($"&dre={command.DreCodigo}", workflowCapturado.NotificacaoMensagem);
            Assert.Contains($"&ue={command.UeCodigo}", workflowCapturado.NotificacaoMensagem);
            Assert.Contains($"&turma={command.TurmaCodigo}", workflowCapturado.NotificacaoMensagem);
            Assert.Contains($"&turmaDescricao={command.TurmaDescricao}", workflowCapturado.NotificacaoMensagem);
        }

        [Fact]
        public async Task Deve_Adicionar_Nivel_Diretor_Ao_Workflow()
        {
            var command = CriarCommand();
            _configuration.Setup(c => c["UrlFrontEnd"]).Returns("http://localhost/");
            WorkflowAprovacaoDto workflowCapturado = null;
            _comandosWorkflowAprovacao.Setup(c => c.Salvar(It.IsAny<WorkflowAprovacaoDto>()))
                .Callback<WorkflowAprovacaoDto>(w => workflowCapturado = w)
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(workflowCapturado);
            Assert.Contains(workflowCapturado.Niveis, n => n.Cargo == Cargo.Diretor);
        }

        [Fact]
        public async Task Deve_Retornar_Id_Do_Workflow_Criado()
        {
            var command = CriarCommand();
            var workflowIdEsperado = 123L;
            _configuration.Setup(c => c["UrlFrontEnd"]).Returns("http://localhost/");
            _comandosWorkflowAprovacao.Setup(c => c.Salvar(It.IsAny<WorkflowAprovacaoDto>())).ReturnsAsync(workflowIdEsperado);

            var resultado = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(workflowIdEsperado, resultado);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Configuration_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new InserirWorkflowReposicaoAulaCommandHandler(null, _comandosWorkflowAprovacao.Object));
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_ComandosWorkflowAprovacao_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new InserirWorkflowReposicaoAulaCommandHandler(_configuration.Object, null));
        }

        [Fact]
        public void Command_Deve_Ter_Propriedades_Derivadas_Corretas()
        {
            var command = CriarCommand();

            Assert.NotNull(command.UeTipoNome);
            Assert.NotNull(command.TurmaModalidadeNome);
            Assert.NotNull(command.TurmaDescricao);
            Assert.Contains(command.TurmaModalidadeNome, command.TurmaDescricao);
            Assert.Contains(command.TurmaNome, command.TurmaDescricao);
            Assert.Contains(command.UeNome, command.TurmaDescricao);
        }

        [Fact]
        public void Command_Deve_Inicializar_Com_Valores_Corretos()
        {
            var aula = new Dominio.Aula { Id = 1, Quantidade = 3 };
            var dre = new Dominio.Dre { Nome = "Dre teste", CodigoDre = "321" };
            var ue = new Dominio.Ue { Nome = "Ue Teste", CodigoUe = "123", Dre = dre, TipoEscola = TipoEscola.EMEF };
            var turma = new Dominio.Turma { Id = 2, Nome = "Turma Teste", CodigoTurma = "T123", Ue = ue, ModalidadeCodigo = Dominio.Modalidade.Fundamental, Semestre = 1 };
            var componenteCurricular = "Matemática";
            var perfilAtual = Guid.NewGuid();

            var command = new InserirWorkflowReposicaoAulaCommand(2024, aula, turma, componenteCurricular, perfilAtual);

            Assert.Equal(2024, command.Ano);
            Assert.Equal(1, command.AulaId);
            Assert.Equal(3, command.Quantidade);
            Assert.Equal("321", command.DreCodigo);
            Assert.Equal("Dre teste", command.DreNome);
            Assert.Equal("123", command.UeCodigo);
            Assert.Equal(TipoEscola.EMEF, command.UeTipo);
            Assert.Equal("Ue Teste", command.UeNome);
            Assert.Equal("Turma Teste", command.TurmaNome);
            Assert.Equal("T123", command.TurmaCodigo);
            Assert.Equal(1, command.TurmaSemestre);
            Assert.Equal(Dominio.Modalidade.Fundamental, command.TurmaModalidade);
            Assert.Equal("Matemática", command.ComponenteCurricularNome);
            Assert.Equal(perfilAtual, command.PerfilAtual);
        }

        private InserirWorkflowReposicaoAulaCommand CriarCommand()
        {
            var aula = new Dominio.Aula { Id = 1, Quantidade = 3 };
            var dre = new Dominio.Dre { Nome = "Dre teste", CodigoDre = "321" };
            var ue = new Dominio.Ue { Nome = "Ue Teste", CodigoUe = "123", Dre = dre, TipoEscola = TipoEscola.EMEF };
            var turma = new Dominio.Turma { Id = 2, Nome = "Turma Teste", CodigoTurma = "T123", Ue = ue, ModalidadeCodigo = Dominio.Modalidade.Fundamental, Semestre = 1 };

            return new InserirWorkflowReposicaoAulaCommand(2024, aula, turma, "Matemática", Guid.NewGuid());
        }
    }
}
