using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCaseTeste
    {
        private readonly ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase consolidacaoTurmaConselhoClasseAlunoUseCase;
        private readonly Mock<IMediator> mediator;

        public ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            consolidacaoTurmaConselhoClasseAlunoUseCase = new ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_retornar_false_se_a_mensagem_estiver_vazia()
        {
            var retorno = await consolidacaoTurmaConselhoClasseAlunoUseCase.Executar(new MensagemRabbit(""));

            Assert.False(retorno);
        }

        [Fact]
        public async Task Deve_Salvar_Nota_Ativo_Bimestre_Final_Consolidado_Conselho_Classe_Aluno_Turma()
        {
            var alunoCodigo = "9";
            var bimestre = 0;
            var turmaId = 101;
            var inativo = false;
            var componenteCurricularId = 2;

            var mensagemConsolidacaoConselhoClasseAlunoDto = ObterMensagemConsolidacaoConselhoClasseAlunoDto(alunoCodigo, turmaId, inativo,bimestre, componenteCurricularId);

            var jsonMensagem = JsonSerializer.Serialize(mensagemConsolidacaoConselhoClasseAlunoDto);

            MediatorRepositorioSetup(alunoCodigo, turmaId);

            //Act
            var retorno = await consolidacaoTurmaConselhoClasseAlunoUseCase.Executar(new MensagemRabbit(jsonMensagem));

            //Asert
            Assert.True(retorno);
        }

        

        [Fact]
        public async Task Deve_Salvar_Nota_Ativo_Bimestre_1_Consolidado_Conselho_Classe_Aluno_Turma()
        {
            var alunoCodigo = "9";
            var bimestre = 1;
            var turmaId = 101;
            var inativo = false;
            var componenteCurricularId = 2;

            var mensagemConsolidacaoConselhoClasseAlunoDto = ObterMensagemConsolidacaoConselhoClasseAlunoDto(alunoCodigo, turmaId, inativo,bimestre, componenteCurricularId);

            var jsonMensagem = JsonSerializer.Serialize(mensagemConsolidacaoConselhoClasseAlunoDto);

            MediatorRepositorioSetup(alunoCodigo, turmaId);

            //Act
            var retorno = await consolidacaoTurmaConselhoClasseAlunoUseCase.Executar(new MensagemRabbit(jsonMensagem));

            //Asert
            Assert.True(retorno);
        }

        [Fact]
        public async Task Deve_Salvar_Nota_Inativo_Consolidado_Conselho_Classe_Aluno_Turma()
        {
            var alunoCodigo = "9";
            var bimestre = 1;
            var turmaId = 101;
            var inativo = true;
            var componenteCurricularId = 2;

            var mensagemConsolidacaoConselhoClasseAlunoDto = ObterMensagemConsolidacaoConselhoClasseAlunoDto(alunoCodigo, turmaId, inativo, bimestre, componenteCurricularId);

            var jsonMensagem = JsonSerializer.Serialize(mensagemConsolidacaoConselhoClasseAlunoDto);

            MediatorRepositorioSetup(alunoCodigo, turmaId);

            //Act
            var retorno = await consolidacaoTurmaConselhoClasseAlunoUseCase.Executar(new MensagemRabbit(jsonMensagem));

            //Asert
            Assert.True(retorno);
        }

        [Fact]
        public async Task Deve_Salvar_Conceito_Ativo_Bimestre_Final_Consolidado_Conselho_Classe_Aluno_Turma()
        {
            var alunoCodigo = "9";
            var bimestre = 0;
            var turmaId = 101;
            var conceito = 1;
            var inativo = false;
            var componenteCurricularId = 2;

            var mensagemConsolidacaoConselhoClasseAlunoDto = ObterMensagemConsolidacaoConselhoClasseAlunoDto(alunoCodigo, turmaId, inativo, bimestre, componenteCurricularId);

            var jsonMensagem = JsonSerializer.Serialize(mensagemConsolidacaoConselhoClasseAlunoDto);

            var consolidadoTurmaAluno = ObterConselhoClasseConsolidadoTurmaAluno(alunoCodigo, turmaId, null, conceito);

            MediatorRepositorioSetup(alunoCodigo, turmaId);

            //Act
            var retorno = await consolidacaoTurmaConselhoClasseAlunoUseCase.Executar(new MensagemRabbit(jsonMensagem));

            //Asert
            Assert.True(retorno);
        }

        [Fact]
        public async Task Deve_Salvar_Conceito_Ativo_Bimestre_1_Consolidado_Conselho_Classe_Aluno_Turma()
        {
            var alunoCodigo = "9";
            var bimestre = 1;
            var turmaId = 101;
            var conceito = 1;
            var inativo = false;
            var componenteCurricularId = 2;

            var mensagemConsolidacaoConselhoClasseAlunoDto = ObterMensagemConsolidacaoConselhoClasseAlunoDto(alunoCodigo, turmaId, inativo, bimestre, componenteCurricularId);

            var jsonMensagem = JsonSerializer.Serialize(mensagemConsolidacaoConselhoClasseAlunoDto);

            var consolidadoTurmaAluno = ObterConselhoClasseConsolidadoTurmaAluno(alunoCodigo, turmaId, null, conceito);

            MediatorRepositorioSetup(alunoCodigo, turmaId);

            //Act
            var retorno = await consolidacaoTurmaConselhoClasseAlunoUseCase.Executar(new MensagemRabbit(jsonMensagem));

            //Asert
            Assert.True(retorno);
        }

        [Fact]
        public async Task Deve_Salvar_Conceito_Inativo_Consolidado_Conselho_Classe_Aluno_Turma()
        {
            var alunoCodigo = "9";
            var bimestre = 1;
            var turmaId = 101;
            var conceito = 1;
            var inativo = true;
            var componenteCurricularId = 2;

            var mensagemConsolidacaoConselhoClasseAlunoDto = ObterMensagemConsolidacaoConselhoClasseAlunoDto(alunoCodigo, turmaId, inativo, bimestre, componenteCurricularId);

            var jsonMensagem = JsonSerializer.Serialize(mensagemConsolidacaoConselhoClasseAlunoDto);

            var consolidadoTurmaAluno = ObterConselhoClasseConsolidadoTurmaAluno(alunoCodigo, turmaId, null, conceito);

            MediatorRepositorioSetup(alunoCodigo, turmaId);

            //Act
            var retorno = await consolidacaoTurmaConselhoClasseAlunoUseCase.Executar(new MensagemRabbit(jsonMensagem));

            //Asert
            Assert.True(retorno);
        }
        
        private void MediatorRepositorioSetup(string alunoCodigo, int turmaId)
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<long>() { 1000 });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Turma());

            mediator.Setup(a => a.Send(It.IsAny<ObterFechamentoPorTurmaPeriodoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FechamentoTurma());

            mediator.Setup(a => a.Send(It.IsAny<ObterConselhoClassePorFechamentoIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ConselhoClasse());

            mediator.Setup(a => a.Send(It.IsAny<ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ConselhoClasseAluno());

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new string[1] { "" });

            mediator.Setup(a => a.Send(It.IsAny<ObterComponentesComNotaDeFechamentoOuConselhoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ComponenteCurricularDto>());

            mediator.Setup(a => a.Send(It.IsAny<ObterComponentesCurricularesEOLPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ComponenteCurricularEol>());
        }

        private static ConselhoClasseConsolidadoTurmaAluno ObterConselhoClasseConsolidadoTurmaAluno(string alunoCodigo, int turmaId, double? nota, long? conceito)
        {
            return new ConselhoClasseConsolidadoTurmaAluno
            {
                AlunoCodigo = alunoCodigo,
                //Bimestre = bimestre,
                TurmaId = turmaId,
                //Nota = nota,
                //ConceitoId = conceito,
                Status = SituacaoConselhoClasse.NaoIniciado
            };
        }

        private static MensagemConsolidacaoConselhoClasseAlunoDto ObterMensagemConsolidacaoConselhoClasseAlunoDto(string alunoCodigo, int turmaId, bool inativo, int? bimestre =0, long? componenteCurricularId = null)
        {
            return new MensagemConsolidacaoConselhoClasseAlunoDto()
            {
                AlunoCodigo = alunoCodigo,
                TurmaId = turmaId,
                Inativo = inativo,
                ComponenteCurricularId = componenteCurricularId,
                Bimestre = bimestre,
            };
        }
    }
}
