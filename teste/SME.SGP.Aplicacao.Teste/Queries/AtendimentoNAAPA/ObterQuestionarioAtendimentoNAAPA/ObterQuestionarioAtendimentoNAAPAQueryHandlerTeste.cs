using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.AtendimentoNAAPA.ObterQuestionarioAtendimentoNAAPA
{
    public class ObterQuestionarioAtendimentoNAAPAQueryHandlerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioQuestaoAtendimentoNAAPA> _repositorioQuestaoMock;
        private readonly Mock<IRepositorioQuestionario> _repositorioQuestionarioMock;
        private readonly ObterQuestionarioAtendimentoNAAPAQueryHandler _handler;

        public ObterQuestionarioAtendimentoNAAPAQueryHandlerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioQuestaoMock = new Mock<IRepositorioQuestaoAtendimentoNAAPA>();
            _repositorioQuestionarioMock = new Mock<IRepositorioQuestionario>();

            _handler = new ObterQuestionarioAtendimentoNAAPAQueryHandler(
                _mediatorMock.Object,
                _repositorioQuestaoMock.Object,
                _repositorioQuestionarioMock.Object);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Mediator_Null()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ObterQuestionarioAtendimentoNAAPAQueryHandler(
                    null,
                    _repositorioQuestaoMock.Object,
                    _repositorioQuestionarioMock.Object));

            Assert.Equal("mediator", exception.ParamName);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_RepositorioQuestao_Null()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ObterQuestionarioAtendimentoNAAPAQueryHandler(
                    _mediatorMock.Object,
                    null,
                    _repositorioQuestionarioMock.Object));

            Assert.Equal("repositorioQuestaoEncaminhamento", exception.ParamName);
        }

        [Fact]
        public async Task Handle_Sem_EncaminhamentoId_Deve_Retornar_Respostas_Vazias()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(2, null, "123456", "1A");

            ConfigurarMocksBasicos();

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(1, "Questão Teste", TipoQuestao.Texto)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);

            _repositorioQuestaoMock.Verify(r => r.ObterRespostasEncaminhamento(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Com_EncaminhamentoId_Deve_Obter_Respostas()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(2, 100, "123456", "1A");
            var respostasEncaminhamento = new List<RespostaQuestaoAtendimentoNAAPADto>
            {
                new RespostaQuestaoAtendimentoNAAPADto
                {
                    Id = 1,
                    QuestaoId = 1,
                    Texto = "Resposta teste"
                }
            };

            ConfigurarMocksBasicos();

            _repositorioQuestaoMock.Setup(r => r.ObterRespostasEncaminhamento(100))
                .ReturnsAsync(respostasEncaminhamento);

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(1, "Questão Teste", TipoQuestao.Texto)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            _repositorioQuestaoMock.Verify(r => r.ObterRespostasEncaminhamento(100), Times.Once);
        }

        [Fact]
        public async Task Handle_Com_EncaminhamentoId_QuestionarioId_1_Deve_Aplicar_Regras_Frequencia()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(1, 100, "123456", "1A");
            var respostasEncaminhamento = new List<RespostaQuestaoAtendimentoNAAPADto>
            {
                new RespostaQuestaoAtendimentoNAAPADto
                {
                    Id = 1,
                    QuestaoId = 1,
                    Texto = "Resposta teste"
                }
            };

            ConfigurarMocksBasicos();
            ConfigurarMocksFrequencia();

            _repositorioQuestaoMock.Setup(r => r.ObterRespostasEncaminhamento(100))
                .ReturnsAsync(respostasEncaminhamento);

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(1, "Questão Teste", TipoQuestao.Texto),
                CriarQuestaoDto(2, "Questão Justificativa", TipoQuestao.Texto)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            _repositorioQuestaoMock.Verify(r => r.ObterRespostasEncaminhamento(100), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaGeralAlunoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Processar_Questao_PAP_Corretamente()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(2, null, "123456", "1A");
            var informacoesTurmasPrograma = new InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto
            {
                ComponentesPAP = new List<ComponenteCurricularSimplificadoDto>
                {
                    new ComponenteCurricularSimplificadoDto { Id = 1, Descricao = "Língua Portuguesa" }
                }
            };

            ConfigurarMocksBasicos();

            // Configurar ID específico para questão PAP
            _repositorioQuestaoMock.Setup(r => r.ObterIdQuestaoPorNomeComponenteQuestionario(2, "PAP"))
                .ReturnsAsync(10);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(informacoesTurmasPrograma);

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(10, "Questão PAP", TipoQuestao.ComboMultiplaEscolhaDinamico)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);

            var questao = resultado.First();
            Assert.Equal(10, questao.Id);
            Assert.NotNull(questao.Resposta);

            if (questao.Resposta.Any())
            {
                var resposta = questao.Resposta.First();
                Assert.NotNull(resposta.Texto);
            }

            _mediatorMock.Verify(m => m.Send(It.Is<ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery>(
                q => q.CodigoAluno == "123456"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Processar_Questao_Projeto_Corretamente()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(2, null, "123456", "1A");
            var informacoesTurmasPrograma = new InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto
            {
                ComponentesFortalecimentoAprendizagens = new List<ComponenteCurricularSimplificadoDto>
                {
                    new ComponenteCurricularSimplificadoDto { Id = 2, Descricao = "Matemática" }
                },
                ComponentesMaisEducacao = new List<ComponenteCurricularSimplificadoDto>
                {
                    new ComponenteCurricularSimplificadoDto { Id = 3, Descricao = "História" }
                }
            };

            ConfigurarMocksBasicos();

            // Configurar ID específico para questão PROJETO
            _repositorioQuestaoMock.Setup(r => r.ObterIdQuestaoPorNomeComponenteQuestionario(2, "PROJETO"))
                .ReturnsAsync(20);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(informacoesTurmasPrograma);

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(20, "Questão Projeto", TipoQuestao.ComboMultiplaEscolhaDinamico)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);

            var questao = resultado.First();
            Assert.Equal(20, questao.Id);
            Assert.NotNull(questao.Resposta);

            if (questao.Resposta.Any())
            {
                var resposta = questao.Resposta.First();
                Assert.NotNull(resposta.Texto);

                // Verificar se combinou corretamente os componentes de fortalecimento e mais educação
                var componentesCombinados = JsonConvert.DeserializeObject<List<ComponenteCurricularSimplificadoDto>>(resposta.Texto);
                Assert.Equal(2, componentesCombinados.Count);
                Assert.Contains(componentesCombinados, c => c.Descricao == "Matemática");
                Assert.Contains(componentesCombinados, c => c.Descricao == "História");
            }
        }

        [Fact]
        public async Task Handle_Deve_Processar_Questao_Tabela_Avaliacoes_Corretamente()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(2, null, "123456", "1A");
            var tabelaAvaliacoes = new AvaliacoesBimestraisAlunoDto
            {
                CodigoAluno = "123456",
                NomeAluno = "Aluno Teste",
                AvaliacoesBimestrais = new List<AvaliacaoBimestreDto>()
            };

            ConfigurarMocksBasicos();

            // Configurar ID específico para questão TABELA_AVALIACOES_BIMESTRAIS
            _repositorioQuestaoMock.Setup(r => r.ObterIdQuestaoPorNomeComponenteQuestionario(2, "TABELA_AVALIACOES_BIMESTRAIS"))
                .ReturnsAsync(30);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAvaliacoesBimestraisAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tabelaAvaliacoes);

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(30, "Questão Tabela", TipoQuestao.TabelaDinamica)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);

            var questao = resultado.First();
            Assert.Equal(30, questao.Id);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterAvaliacoesBimestraisAlunoQuery>(
                q => q.CodigoAluno == "123456"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_QuestionarioId_1_Com_Frequencia_Insuficiente_Deve_Tornar_Justificativa_Obrigatoria()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(1, null, "123456", "1A");

            ConfigurarMocksBasicos();
            ConfigurarMocksFrequencia();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaGeralAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(60.0);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterModalidadeTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Modalidade.Fundamental);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = "75" });

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(1, "Questão Normal", TipoQuestao.Texto),
                CriarQuestaoDto(2, "Justificativa", TipoQuestao.Texto)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            var questaoJustificativa = resultado.FirstOrDefault(q => q.Id == 2);
            Assert.NotNull(questaoJustificativa);
            Assert.True(questaoJustificativa.Obrigatorio);
        }

        [Fact]
        public async Task Handle_QuestionarioId_1_Com_Frequencia_Suficiente_Deve_Remover_Justificativa()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(1, null, "123456", "1A");

            ConfigurarMocksBasicos();
            ConfigurarMocksFrequencia();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaGeralAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(80.0);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterModalidadeTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Modalidade.Fundamental);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = "75" });

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(1, "Questão Normal", TipoQuestao.Texto),
                CriarQuestaoDto(2, "Justificativa", TipoQuestao.Texto)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.DoesNotContain(resultado, q => q.Id == 2);
        }

        [Fact]
        public async Task Handle_Modalidade_EducacaoInfantil_Deve_Usar_Parametro_FrequenciaMinima_Infantil()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(1, null, "123456", "1A");

            ConfigurarMocksBasicos();
            ConfigurarMocksFrequencia();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaGeralAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(70.0);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterModalidadeTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Modalidade.EducacaoInfantil);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = "60" });

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(1, "Questão Normal", TipoQuestao.Texto),
                CriarQuestaoDto(2, "Justificativa", TipoQuestao.Texto)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(
                q => q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaMinimaInfantil),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_QuestionarioId_Diferente_De_1_Nao_Deve_Aplicar_Regras_Frequencia()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(2, null, "123456", "1A");

            ConfigurarMocksBasicos();

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(1, "Questão Normal", TipoQuestao.Texto),
                CriarQuestaoDto(2, "Justificativa", TipoQuestao.Texto)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaGeralAlunoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Respostas_Existentes_Para_Questoes_Normais()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(2, 100, "123456", "1A");

            var respostasExistentes = new List<RespostaQuestaoAtendimentoNAAPADto>
            {
                new RespostaQuestaoAtendimentoNAAPADto
                {
                    Id = 1,
                    QuestaoId = 50,
                    RespostaId = 1,
                    Texto = "Resposta existente"
                }
            };

            ConfigurarMocksBasicos();

            _repositorioQuestaoMock.Setup(r => r.ObterRespostasEncaminhamento(100))
                .ReturnsAsync(respostasExistentes);

            // Criar questão com resposta já populada pelo mock do ObterQuestoesPorQuestionarioPorIdQuery
            var questoes = new List<QuestaoDto>
            {
                new QuestaoDto
                {
                    Id = 50,
                    Nome = "Questão Normal",
                    TipoQuestao = TipoQuestao.Texto,
                    Obrigatorio = false,
                    Resposta = new List<RespostaQuestaoDto>
                    {
                        new RespostaQuestaoDto
                        {
                            Id = 1,
                            OpcaoRespostaId = 1,
                            Texto = "Resposta existente",
                            QuestaoId = 50
                        }
                    }
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);

            var questao = resultado.First();
            Assert.Equal(50, questao.Id);
            Assert.NotNull(questao.Resposta);

            if (questao.Resposta.Any())
            {
                var resposta = questao.Resposta.First();
                Assert.Equal(1, resposta.Id);
                Assert.Equal(1, resposta.OpcaoRespostaId);
                Assert.Equal("Resposta existente", resposta.Texto);
            }

            _repositorioQuestaoMock.Verify(r => r.ObterRespostasEncaminhamento(100), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Questao_Sem_Respostas_Quando_Nao_Existe_Resposta_Encaminhamento()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(2, 100, "123456", "1A");

            var respostasExistentes = new List<RespostaQuestaoAtendimentoNAAPADto>(); // Lista vazia

            ConfigurarMocksBasicos();

            _repositorioQuestaoMock.Setup(r => r.ObterRespostasEncaminhamento(100))
                .ReturnsAsync(respostasExistentes);

            // Questão sem resposta pré-populada
            var questoes = new List<QuestaoDto>
            {
                new QuestaoDto
                {
                    Id = 60,
                    Nome = "Questão Sem Resposta",
                    TipoQuestao = TipoQuestao.Texto,
                    Obrigatorio = false,
                    Resposta = new List<RespostaQuestaoDto>() // Vazio - será populado pelo handler
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);

            var questao = resultado.First();
            Assert.Equal(60, questao.Id);
            Assert.NotNull(questao.Resposta);
            Assert.Empty(questao.Resposta); // Não deve ter respostas pois não existe no encaminhamento

            _repositorioQuestaoMock.Verify(r => r.ObterRespostasEncaminhamento(100), Times.Once);
        }

        [Theory]
        [InlineData("PAP")]
        [InlineData("PROJETO")]
        [InlineData("CLASSE_HOSPITALAR")]
        [InlineData("TABELA_AVALIACOES_BIMESTRAIS")]
        public async Task Handle_Deve_Obter_Ids_Questoes_Por_Nome_Componente(string nomeComponente)
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(2, null, "123456", "1A");

            ConfigurarMocksBasicos();

            _repositorioQuestaoMock.Setup(r => r.ObterIdQuestaoPorNomeComponenteQuestionario(2, nomeComponente))
                .ReturnsAsync(999);

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(1, "Questão Teste", TipoQuestao.Texto)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositorioQuestaoMock.Verify(r => r.ObterIdQuestaoPorNomeComponenteQuestionario(2, nomeComponente), Times.Once);
        }

        [Fact]
        public async Task Handle_Questao_Projeto_Sem_Componentes_Deve_Retornar_Lista_Vazia()
        {
            // Arrange
            var query = new ObterQuestionarioAtendimentoNAAPAQuery(2, null, "123456", "1A");
            var informacoesTurmasPrograma = new InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto
            {
                ComponentesFortalecimentoAprendizagens = new List<ComponenteCurricularSimplificadoDto>(),
                ComponentesMaisEducacao = new List<ComponenteCurricularSimplificadoDto>()
            };

            ConfigurarMocksBasicos();

            _repositorioQuestaoMock.Setup(r => r.ObterIdQuestaoPorNomeComponenteQuestionario(2, "PROJETO"))
                .ReturnsAsync(20);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(informacoesTurmasPrograma);

            var questoes = new List<QuestaoDto>
            {
                CriarQuestaoDto(20, "Questão Projeto", TipoQuestao.ComboMultiplaEscolhaDinamico)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);

            var questao = resultado.First();
            Assert.Equal(20, questao.Id);
            Assert.NotNull(questao.Resposta);

            if (questao.Resposta.Any())
            {
                var resposta = questao.Resposta.First();
                var componentesCombinados = JsonConvert.DeserializeObject<List<ComponenteCurricularSimplificadoDto>>(resposta.Texto);
                Assert.Empty(componentesCombinados);
            }
        }

        private void ConfigurarMocksBasicos()
        {
            // Mock para informações de turmas programa
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto
                {
                    ComponentesPAP = new List<ComponenteCurricularSimplificadoDto>(),
                    ComponentesFortalecimentoAprendizagens = new List<ComponenteCurricularSimplificadoDto>(),
                    ComponentesMaisEducacao = new List<ComponenteCurricularSimplificadoDto>()
                });

            // Mock para tabela de avaliações bimestrais
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAvaliacoesBimestraisAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AvaliacoesBimestraisAlunoDto());

            // Mock para IDs das questões por nome de componente (retornando 0 por padrão)
            _repositorioQuestaoMock.Setup(r => r.ObterIdQuestaoPorNomeComponenteQuestionario(It.IsAny<long>(), "PAP"))
                .ReturnsAsync(0);
            _repositorioQuestaoMock.Setup(r => r.ObterIdQuestaoPorNomeComponenteQuestionario(It.IsAny<long>(), "PROJETO"))
                .ReturnsAsync(0);
            _repositorioQuestaoMock.Setup(r => r.ObterIdQuestaoPorNomeComponenteQuestionario(It.IsAny<long>(), "CLASSE_HOSPITALAR"))
                .ReturnsAsync(0);
            _repositorioQuestaoMock.Setup(r => r.ObterIdQuestaoPorNomeComponenteQuestionario(It.IsAny<long>(), "TABELA_AVALIACOES_BIMESTRAIS"))
                .ReturnsAsync(0);

            // Mock para respostas vazias de encaminhamento
            _repositorioQuestaoMock.Setup(r => r.ObterRespostasEncaminhamento(It.IsAny<long>()))
                .ReturnsAsync(new List<RespostaQuestaoAtendimentoNAAPADto>());
        }

        private void ConfigurarMocksFrequencia()
        {
            // Mock para frequência geral do aluno
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaGeralAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(75.0); // Frequência padrão

            // Mock para modalidade da turma
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterModalidadeTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Modalidade.Fundamental);

            // Mock para parâmetro do sistema
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = "75" });
        }

        private QuestaoDto CriarQuestaoDto(long id, string nome, TipoQuestao tipo, bool obrigatorio = false)
        {
            return new QuestaoDto
            {
                Id = id,
                Nome = nome,
                TipoQuestao = tipo,
                Obrigatorio = obrigatorio,
                Resposta = new List<RespostaQuestaoDto>()
            };
        }
    }
}