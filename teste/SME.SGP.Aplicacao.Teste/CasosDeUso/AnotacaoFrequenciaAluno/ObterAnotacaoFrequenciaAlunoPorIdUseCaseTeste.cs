using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AnotacaoFrequenciaAluno
{
    public class ObterAnotacaoFrequenciaAlunoPorIdUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterAnotacaoFrequenciaAlunoPorIdUseCase _useCase;

        public ObterAnotacaoFrequenciaAlunoPorIdUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterAnotacaoFrequenciaAlunoPorIdUseCase(_mediatorMock.Object);
        }

        private Dominio.AnotacaoFrequenciaAluno CriarAnotacao(long id, long aulaId, string codigoAluno, long? motivoAusenciaId = null, bool excluido = false)
        {
            var anotacao = new Dominio.AnotacaoFrequenciaAluno(aulaId, "Anotacao Teste", codigoAluno, motivoAusenciaId)
            {
                Id = id,
                Excluido = excluido
            };
            return anotacao;
        }

        private MotivoAusencia CriarMotivoAusencia(long id, string descricao)
        {
            return new MotivoAusencia { Id = id, Descricao = descricao };
        }

        private Dominio.Aula CriarAula(long id, string turmaId)
        {
            return new Dominio.Aula { Id = id, TurmaId = turmaId };
        }

        private Turma CriarTurma(string codigoTurma, int anoLetivo)
        {
            return new Turma { CodigoTurma = codigoTurma, AnoLetivo = anoLetivo, ModalidadeCodigo = Modalidade.Fundamental, TipoTurno = 1 };
        }

        private AlunoPorTurmaResposta CriarAluno(string codigoAluno)
        {
            return new AlunoPorTurmaResposta
            {
                CodigoAluno = codigoAluno,
                NomeAluno = "Nome Aluno Teste",
                NomeResponsavel = "Responsavel Teste",
                CelularResponsavel = "999999999",
                DataNascimento = DateTime.Now.AddYears(-10),
                DataAtualizacaoContato = DateTime.Now,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                SituacaoMatricula = "Ativo",
                TipoResponsavel = "1"
            };
        }

        [Fact]
        public async Task Executar_Quando_Anotacao_Valida_Deve_Retornar_AnotacaoFrequenciaAlunoCompletoDto()
        {
            long anotacaoId = 1;
            long aulaId = 10;
            string codigoTurma = "T100";
            string codigoAluno = "A10";
            long motivoId = 5;
            int anoLetivo = 2024;
            double frequencia = 0.85;

            var anotacaoDominio = CriarAnotacao(anotacaoId, aulaId, codigoAluno, motivoId);
            var motivoDominio = CriarMotivoAusencia(motivoId, "Motivo Teste");
            var aulaDominio = CriarAula(aulaId, codigoTurma);
            var turmaDominio = CriarTurma(codigoTurma, anoLetivo);
            var alunoResposta = CriarAluno(codigoAluno);
            var listaAlunos = new List<AlunoPorTurmaResposta> { alunoResposta };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterMotivoAusenciaPorIdQuery>(q => q.Id == motivoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(motivoDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosPorTurmaEAnoLetivoQuery>(q => q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunos);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaGeralAlunoQuery>(q => q.CodigoAluno == codigoAluno && q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequencia);

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(anotacaoId);

            Assert.NotNull(resultado);
            Assert.Equal(anotacaoId, resultado.Id);
            Assert.Equal(motivoId, resultado.MotivoAusenciaId);
            Assert.Equal(motivoDominio.Descricao, resultado.MotivoAusencia.Descricao);
            Assert.Equal(codigoAluno, resultado.CodigoAluno);
            Assert.True(resultado.Aluno.EhAtendidoAEE);
        }

        [Fact]
        public async Task Executar_Quando_Anotacao_Com_MotivoAusenciaId_Nulo_Deve_Retornar_AnotacaoFrequenciaAlunoCompletoDto()
        {
            long anotacaoId = 2;
            long aulaId = 11;
            string codigoTurma = "T101";
            string codigoAluno = "A11";
            int anoLetivo = 2024;
            double frequencia = 0.90;

            var anotacaoDominio = CriarAnotacao(anotacaoId, aulaId, codigoAluno, null);
            var aulaDominio = CriarAula(aulaId, codigoTurma);
            var turmaDominio = CriarTurma(codigoTurma, anoLetivo);
            var alunoResposta = CriarAluno(codigoAluno);
            var listaAlunos = new List<AlunoPorTurmaResposta> { alunoResposta };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoDominio);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MotivoAusencia)null);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosPorTurmaEAnoLetivoQuery>(q => q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunos);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaGeralAlunoQuery>(q => q.CodigoAluno == codigoAluno && q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequencia);

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await _useCase.Executar(anotacaoId);

            Assert.NotNull(resultado);
            Assert.Equal(anotacaoId, resultado.Id);
            Assert.Equal(0, resultado.MotivoAusenciaId);
            Assert.Null(resultado.MotivoAusencia);
            Assert.False(resultado.Aluno.EhAtendidoAEE);
        }

        [Fact]
        public async Task Executar_Quando_Anotacao_Nao_Encontrada_Deve_Lancar_NegocioException()
        {
            long anotacaoId = 3;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Dominio.AnotacaoFrequenciaAluno)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anotacaoId));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Anotacao_Excluida_Deve_Lancar_NegocioException()
        {
            long anotacaoId = 4;
            var anotacaoDominio = CriarAnotacao(anotacaoId, 0, "", null, true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoDominio);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anotacaoId));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Aula_Nao_Encontrada_Deve_Lancar_NegocioException()
        {
            long anotacaoId = 5;
            long aulaId = 12;
            string codigoAluno = "A12";

            var anotacaoDominio = CriarAnotacao(anotacaoId, aulaId, codigoAluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoDominio);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MotivoAusencia)null);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Dominio.Aula)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anotacaoId));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Nao_Encontrada_Deve_Lancar_NegocioException()
        {
            long anotacaoId = 6;
            long aulaId = 13;
            string codigoTurma = "T103";
            string codigoAluno = "A13";

            var anotacaoDominio = CriarAnotacao(anotacaoId, aulaId, codigoAluno);
            var aulaDominio = CriarAula(aulaId, codigoTurma);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoDominio);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MotivoAusencia)null);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anotacaoId));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Alunos_Da_Turma_Nao_Encontrados_Deve_Lancar_NegocioException()
        {
            long anotacaoId = 7;
            long aulaId = 14;
            string codigoTurma = "T104";
            string codigoAluno = "A14";
            int anoLetivo = 2024;

            var anotacaoDominio = CriarAnotacao(anotacaoId, aulaId, codigoAluno);
            var aulaDominio = CriarAula(aulaId, codigoTurma);
            var turmaDominio = CriarTurma(codigoTurma, anoLetivo);
            IEnumerable<AlunoPorTurmaResposta> listaAlunosNula = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoDominio);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MotivoAusencia)null);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosPorTurmaEAnoLetivoQuery>(q => q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunosNula);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anotacaoId));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Alunos_Da_Turma_Vazio_Deve_Lancar_NegocioException()
        {
            long anotacaoId = 8;
            long aulaId = 15;
            string codigoTurma = "T105";
            string codigoAluno = "A15";
            int anoLetivo = 2024;

            var anotacaoDominio = CriarAnotacao(anotacaoId, aulaId, codigoAluno);
            var aulaDominio = CriarAula(aulaId, codigoTurma);
            var turmaDominio = CriarTurma(codigoTurma, anoLetivo);
            var listaAlunosVazia = new List<AlunoPorTurmaResposta>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoDominio);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MotivoAusencia)null);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosPorTurmaEAnoLetivoQuery>(q => q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunosVazia);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anotacaoId));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Aluno_Nao_Encontrado_Na_Turma_Deve_Lancar_NegocioException()
        {
            long anotacaoId = 9;
            long aulaId = 16;
            string codigoTurma = "T106";
            string codigoAlunoAnotacao = "A16";
            string codigoAlunoInexistente = "A999";
            int anoLetivo = 2024;

            var anotacaoDominio = CriarAnotacao(anotacaoId, aulaId, codigoAlunoInexistente);
            var aulaDominio = CriarAula(aulaId, codigoTurma);
            var turmaDominio = CriarTurma(codigoTurma, anoLetivo);
            var alunoResposta = CriarAluno(codigoAlunoAnotacao);
            var listaAlunos = new List<AlunoPorTurmaResposta> { alunoResposta };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoDominio);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MotivoAusencia)null);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosPorTurmaEAnoLetivoQuery>(q => q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunos);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anotacaoId));
        }

        [Theory]
        [InlineData("1", "Filiação 1")]
        [InlineData("2", "Filiação 2")]
        [InlineData("3", "Responsável Legal")]
        [InlineData("4", "Próprio estudante")]
        [InlineData("99", "Filiacao1")]
        public async Task Executar_Quando_MapearParaDto_Deve_Mapear_TipoResponsavel_Corretamente(string tipoResponsavelCodigo, string tipoResponsavelNome)
        {
            long anotacaoId = 10;
            long aulaId = 17;
            string codigoTurma = "T107";
            string codigoAluno = "A17";
            int anoLetivo = 2024;
            double frequencia = 0.95;

            var anotacaoDominio = CriarAnotacao(anotacaoId, aulaId, codigoAluno);
            var aulaDominio = CriarAula(aulaId, codigoTurma);
            var turmaDominio = CriarTurma(codigoTurma, anoLetivo);
            var alunoResposta = CriarAluno(codigoAluno);
            alunoResposta.TipoResponsavel = tipoResponsavelCodigo;
            var listaAlunos = new List<AlunoPorTurmaResposta> { alunoResposta };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoDominio);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MotivoAusencia)null);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaDominio);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosPorTurmaEAnoLetivoQuery>(q => q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunos);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaGeralAlunoQuery>(q => q.CodigoAluno == codigoAluno && q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequencia);

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await _useCase.Executar(anotacaoId);

            Assert.NotNull(resultado);
            Assert.Equal(tipoResponsavelNome, resultado.Aluno.TipoResponsavel);
        }
    }
}
