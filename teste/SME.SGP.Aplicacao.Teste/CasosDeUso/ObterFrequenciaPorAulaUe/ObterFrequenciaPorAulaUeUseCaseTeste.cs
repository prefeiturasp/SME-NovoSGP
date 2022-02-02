using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterFrequenciaPorAulaUeUseCaseTeste
    {
        private readonly ObterFrequenciaPorAulaUseCase obterFrequenciaPorAulaUeUseCase;
        private readonly Mock<IMediator> mediator;
        public ObterFrequenciaPorAulaUeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            obterFrequenciaPorAulaUeUseCase = new ObterFrequenciaPorAulaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Exibir_Aluno_Ativo_Bimestre()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = DateTime.Parse("2022-01-01"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-01"),
                DataSituacao = DateTime.Parse("2022-01-01"),
            };
            var aula = new Aula
            {
                DataAula = DateTime.Parse("2022-01-02"),
            };
            var periodo = new PeriodoEscolar
            {
                PeriodoInicio = DateTime.Parse("2021-12-31"),
            };

            var naoExibirAlunoFrequencia  = obterFrequenciaPorAulaUeUseCase.NaoExibirAlunoFrequencia(aluno,aula,periodo);

            Assert.False(naoExibirAlunoFrequencia,"Aluno não está ativo dentro do Bimestre");
        }

        [Fact]
        public async Task Deve_Exibir_Aluno_Ativo_Depois_Inicio_Bimestre()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido,
                DataMatricula = DateTime.Parse("2022-01-11"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-11"),
                DataSituacao = DateTime.Parse("2022-01-11"),
            };
            var aula = new Aula
            {
                DataAula = DateTime.Parse("2022-01-11"),
            };
            var periodo = new PeriodoEscolar
            {
                PeriodoInicio = DateTime.Parse("2022-01-10"),
            };

            var naoExibirAlunoFrequencia = obterFrequenciaPorAulaUeUseCase.NaoExibirAlunoFrequencia(aluno, aula, periodo);

            Assert.False(naoExibirAlunoFrequencia, "Aluno não está ativo depois do inicio do bimestre antes da aula");
        }

        [Fact]
        public async Task Deve_Exibir_Aluno_Inativo_Antes_Inicio_Aula()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                DataMatricula = DateTime.Parse("2022-01-01"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-10"),
                DataSituacao = DateTime.Parse("2022-01-10"),
                NumeroAlunoChamada = 1
            };
            var aula = new Aula
            {
                DataAula = DateTime.Parse("2022-01-11"),
            };
            var periodo = new PeriodoEscolar
            {
                PeriodoInicio = DateTime.Parse("2022-01-09"),
            };

            var naoExibirAlunoFrequencia = obterFrequenciaPorAulaUeUseCase.NaoExibirAlunoFrequencia(aluno, aula, periodo);

            Assert.False(naoExibirAlunoFrequencia, "Aluno está Ativo durante o bimestre");
        }

        [Fact]
        public async Task Nao_Deve_Exibir_Aluno_Ativo_Depois_Data_Aula()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = DateTime.Parse("2022-01-31"),
                DataSituacao = DateTime.Parse("2022-01-31"),
            };
            var aula = new Aula
            {
                DataAula = DateTime.Parse("2022-01-06"),
            };
            var periodo = new PeriodoEscolar
            {
                PeriodoInicio = DateTime.Parse("2022-01-01"),
            };

            var naoExibirAlunoFrequencia = obterFrequenciaPorAulaUeUseCase.NaoExibirAlunoFrequencia(aluno, aula, periodo);

            Assert.True(naoExibirAlunoFrequencia, "Aluno está Ativo depois da data da Aula");
        }

        [Fact]
        public async Task Nao_Deve_Exibir_Aluno_Inativo_Depois_Data_Aula()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                DataMatricula = DateTime.Parse("2022-01-30"),
                DataSituacao = DateTime.Parse("2022-01-31"),
            };
            var aula = new Aula
            {
                DataAula = DateTime.Parse("2022-01-06"),
            };
            var periodo = new PeriodoEscolar
            {
                PeriodoInicio = DateTime.Parse("2022-01-01"),
            };

            var naoExibirAlunoFrequencia = obterFrequenciaPorAulaUeUseCase.NaoExibirAlunoFrequencia(aluno, aula, periodo);

            Assert.True(naoExibirAlunoFrequencia, "Aluno está Ativo depois do inicio do bimestre e depois da data da aula");
        }

    }
}
