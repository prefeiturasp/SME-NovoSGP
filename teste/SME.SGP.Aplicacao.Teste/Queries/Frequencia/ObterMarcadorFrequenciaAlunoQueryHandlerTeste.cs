using SME.SGP.Aplicacao.Queries.Frequencia.ObterMarcadorFrequenciaAluno;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterMarcadorFrequenciaAlunoQueryHandlerTeste
    {
        private readonly ObterMarcadorFrequenciaAlunoQueryHandler _handler;

        public ObterMarcadorFrequenciaAlunoQueryHandlerTeste()
        {
            _handler = new ObterMarcadorFrequenciaAlunoQueryHandler();
        }

        [Fact(DisplayName = "Deve retornar marcador 'Novo' para aluno ativo dentro do período de 15 dias")]
        public async Task Handle_QuandoAlunoAtivoRecente_DeveRetornarMarcadorNovo()
        {
            // Arrange
            var hoje = DateTime.Now.Date;
            var periodoEscolar = new PeriodoEscolar { PeriodoInicio = hoje.AddDays(-20) };
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataSituacao = hoje.AddDays(-5) // Matrícula há 5 dias
            };
            var query = new ObterMarcadorFrequenciaAlunoQuery(aluno, periodoEscolar, Modalidade.Fundamental);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(TipoMarcadorFrequencia.Novo, resultado.Tipo);
            Assert.Contains("Estudante Novo", resultado.Descricao);
        }

        [Fact(DisplayName = "Não deve retornar marcador 'Novo' para aluno ativo há mais de 15 dias")]
        public async Task Handle_QuandoAlunoAtivoAntigo_NaoDeveRetornarMarcador()
        {
            // Arrange
            var hoje = DateTime.Now.Date;
            var periodoEscolar = new PeriodoEscolar { PeriodoInicio = hoje.AddDays(-30) };
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataSituacao = hoje.AddDays(-20) // Matrícula há 20 dias
            };
            var query = new ObterMarcadorFrequenciaAlunoQuery(aluno, periodoEscolar, Modalidade.Fundamental);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(resultado);
        }

        [Fact(DisplayName = "Deve retornar marcador 'Transferido' para aluno transferido")]
        public async Task Handle_QuandoAlunoTransferido_DeveRetornarMarcadorTransferido()
        {
            // Arrange
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                DataSituacao = DateTime.Now.Date,
                Transferencia_Interna = true,
                EscolaTransferencia = "EMEF TESTE",
                TurmaTransferencia = "9A"
            };
            var query = new ObterMarcadorFrequenciaAlunoQuery(aluno, new PeriodoEscolar(), Modalidade.EducacaoInfantil);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(TipoMarcadorFrequencia.Transferido, resultado.Tipo);
            Assert.Contains("Criança Transferida", resultado.Descricao);
            Assert.Contains("para escola EMEF TESTE e turma 9A", resultado.Descricao);
        }

        [Fact(DisplayName = "Deve retornar marcador 'Remanejado' para aluno remanejado")]
        public async Task Handle_QuandoAlunoRemanejado_DeveRetornarMarcadorRemanejado()
        {
            // Arrange
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.RemanejadoSaida,
                DataSituacao = DateTime.Now.Date,
                TurmaRemanejamento = "8B"
            };
            var query = new ObterMarcadorFrequenciaAlunoQuery(aluno, new PeriodoEscolar(), Modalidade.Fundamental);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(TipoMarcadorFrequencia.Remanejado, resultado.Tipo);
            Assert.Contains("Estudante Remanejado", resultado.Descricao);
            Assert.Contains("turma 8B", resultado.Descricao);
        }

        [Theory(DisplayName = "Deve retornar marcador 'Inativo' para diversas situações de inatividade")]
        [InlineData(SituacaoMatriculaAluno.Desistente)]
        [InlineData(SituacaoMatriculaAluno.VinculoIndevido)]
        [InlineData(SituacaoMatriculaAluno.Falecido)]
        [InlineData(SituacaoMatriculaAluno.NaoCompareceu)]
        [InlineData(SituacaoMatriculaAluno.Deslocamento)]
        [InlineData(SituacaoMatriculaAluno.Cessado)]
        [InlineData(SituacaoMatriculaAluno.ReclassificadoSaida)]
        public async Task Handle_QuandoAlunoInativo_DeveRetornarMarcadorInativo(SituacaoMatriculaAluno situacao)
        {
            // Arrange
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = situacao,
                DataSituacao = DateTime.Now.Date
            };
            var query = new ObterMarcadorFrequenciaAlunoQuery(aluno, new PeriodoEscolar(), Modalidade.Fundamental);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(TipoMarcadorFrequencia.Inativo, resultado.Tipo);
            Assert.Contains("Estudante Inativo", resultado.Descricao);
        }

        [Fact(DisplayName = "Não deve retornar marcador para situações não mapeadas")]
        public async Task Handle_QuandoSituacaoNaoMapeada_NaoDeveRetornarMarcador()
        {
            // Arrange
            var aluno = new AlunoPorTurmaResposta { CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido };
            var query = new ObterMarcadorFrequenciaAlunoQuery(aluno, new PeriodoEscolar(), Modalidade.Fundamental);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(resultado);
        }
    }
}
