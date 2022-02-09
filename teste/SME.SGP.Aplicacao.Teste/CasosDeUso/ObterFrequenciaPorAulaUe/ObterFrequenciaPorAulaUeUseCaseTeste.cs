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
        public async Task Deve_Exibir_Aluno_Ativo_No_Bimestre_Entrou_Antes_Aula()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = DateTime.Parse("2022-01-01"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-01"),
                DataSituacao = DateTime.Parse("2022-01-19"),
            };
            var aula = new Aula
            {
                DataAula = DateTime.Parse("2022-01-22"),
            };
            var periodo = new PeriodoEscolar
            {
                PeriodoInicio = DateTime.Parse("2021-12-31"),
            };

            var naoExibirAlunoFrequencia = obterFrequenciaPorAulaUeUseCase.NaoExibirAlunoFrequencia(aluno, aula, periodo);

            Assert.False(naoExibirAlunoFrequencia, "Aluno ativo dentro do Bimestre entrou antes da aula");
        }

        [Fact]
        public async Task Nao_Deve_Exibir_Aluno_Ativo_Bimestre_Entrou_Depois_Aula()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = DateTime.Parse("2022-01-01"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-01"),
                DataSituacao = DateTime.Parse("2022-01-19"),
            };
            var aula = new Aula
            {
                DataAula = DateTime.Parse("2022-01-18"),
            };
            var periodo = new PeriodoEscolar
            {
                PeriodoInicio = DateTime.Parse("2021-12-31"),
            };

            var naoExibirAlunoFrequencia = obterFrequenciaPorAulaUeUseCase.NaoExibirAlunoFrequencia(aluno, aula, periodo);

            Assert.True(naoExibirAlunoFrequencia, "Aluno está Ativo no bimestre e entrou depois da data da aula");
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

        [Fact]
        public async Task Deve_Exibir_Aluno_Transferido_No_Bimestre_Com_Numero_De_Chamada()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                DataMatricula = DateTime.Parse("2022-01-11"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-11"),
                DataSituacao = DateTime.Parse("2022-01-17"),
                NumeroAlunoChamada = 10,
            };
            var aula = new Aula
            {
                DataAula = DateTime.Parse("2022-01-14"),
            };
            var periodo = new PeriodoEscolar
            {
                PeriodoInicio = DateTime.Parse("2022-01-10"),
            };

            var naoExibirAlunoFrequencia = obterFrequenciaPorAulaUeUseCase.NaoExibirAlunoFrequencia(aluno, aula, periodo);

            Assert.False(naoExibirAlunoFrequencia, "Aluno transferido no bimestre com número de chamada");
        }

        [Fact]
        public async Task Nao_Deve_Exibir_Aluno_Transferido_No_Bimestre_Sem_Numero_De_Chamada()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                DataMatricula = DateTime.Parse("2022-01-11"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-11"),
                DataSituacao = DateTime.Parse("2022-01-17"),
                NumeroAlunoChamada = 0,
            };
            var aula = new Aula
            {
                DataAula = DateTime.Parse("2022-01-14"),
            };
            var periodo = new PeriodoEscolar
            {
                PeriodoInicio = DateTime.Parse("2022-01-10"),
            };

            var naoExibirAlunoFrequencia = obterFrequenciaPorAulaUeUseCase.NaoExibirAlunoFrequencia(aluno, aula, periodo);

            Assert.True(naoExibirAlunoFrequencia, "Aluno transferido no bimestre sem número de chamada");
        }

    }
}
