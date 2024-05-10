using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterFrequenciaPorAulaUeUseCaseTeste
    {
        public ObterFrequenciaPorAulaUeUseCaseTeste()
        {}

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

            var naoExibirAlunoFrequencia  = aluno.DeveMostrarNaChamada(DateTime.Parse("2022-01-02"), DateTime.Parse("2021-12-31"));

            Assert.True(naoExibirAlunoFrequencia,"Aluno não está ativo dentro do Bimestre");
        }

        [Fact]
        public Task Deve_Exibir_Aluno_Ativo_No_Bimestre_Entrou_Antes_Aula()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = DateTime.Parse("2022-01-01"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-01"),
                DataSituacao = DateTime.Parse("2022-01-19"),
            };

            var naoExibirAlunoFrequencia = aluno.DeveMostrarNaChamada(DateTime.Parse("2022-01-22"), DateTime.Parse("2021-12-31"));

            Assert.True(naoExibirAlunoFrequencia, "Aluno ativo dentro do Bimestre entrou antes da aula");
            
            return Task.CompletedTask;
        }

        [Fact]
        public Task Nao_Deve_Exibir_Aluno_Ativo_Bimestre_Entrou_Depois_Aula()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = DateTime.Parse("2022-01-19"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-01"),
                DataSituacao = DateTime.Parse("2022-12-15"),
            };

            var naoExibirAlunoFrequencia = aluno.DeveMostrarNaChamada(DateTime.Parse("2022-01-18"), DateTime.Parse("2021-12-31"));

            Assert.False(naoExibirAlunoFrequencia, "Aluno está Ativo no bimestre e entrou depois da data da aula");

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Exibir_Aluno_Ativo_Depois_Inicio_Bimestre()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido,
                DataMatricula = DateTime.Parse("2022-01-11"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-11"),
                DataSituacao = DateTime.Parse("2022-01-11"),
            };

            var naoExibirAlunoFrequencia = aluno.DeveMostrarNaChamada(DateTime.Parse("2022-01-12"), DateTime.Parse("2022-06-01"));

            Assert.True(naoExibirAlunoFrequencia, "Aluno não está ativo depois do inicio do bimestre antes da aula");

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Exibir_Aluno_Inativo_Antes_Inicio_Aula()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                DataMatricula = DateTime.Parse("2022-01-01"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-10"),
                DataSituacao = DateTime.Parse("2022-01-10"),
                NumeroAlunoChamada = 1
            };

            var naoExibirAlunoFrequencia = aluno.DeveMostrarNaChamada(DateTime.Parse("2022-01-11"), DateTime.Parse("2022-01-09"));

            Assert.True(naoExibirAlunoFrequencia, "Aluno está Ativo durante o bimestre");

            return Task.CompletedTask;
        }

        [Fact]
        public Task Nao_Deve_Exibir_Aluno_Ativo_Depois_Data_Aula()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = DateTime.Parse("2022-01-31"),
                DataSituacao = DateTime.Parse("2022-01-31"),
            };

            var naoExibirAlunoFrequencia = aluno.DeveMostrarNaChamada(DateTime.Parse("2022-01-06"), DateTime.Parse("2022-01-01"));

            Assert.False(naoExibirAlunoFrequencia, "Aluno está Ativo depois da data da Aula");

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Exibir_Aluno_Inativo_Depois_Data_Aula()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                DataMatricula = DateTime.Parse("2022-01-30"),
                DataSituacao = DateTime.Parse("2022-01-31"),
            };

            var naoExibirAlunoFrequencia = aluno.DeveMostrarNaChamada(DateTime.Parse("2022-01-06"),DateTime.Parse("2022-01-01"));

            Assert.True(naoExibirAlunoFrequencia, "Aluno está Ativo depois do inicio do bimestre e depois da data da aula");

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Exibir_Aluno_Transferido_No_Bimestre()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                DataMatricula = DateTime.Parse("2022-01-11"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-11"),
                DataSituacao = DateTime.Parse("2022-01-17"),
            };

            var naoExibirAlunoFrequencia = aluno.DeveMostrarNaChamada(DateTime.Parse("2022-01-14"), DateTime.Parse("2022-01-10"));

            Assert.True(naoExibirAlunoFrequencia, "Aluno transferido no bimestre com número de chamada");

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Exibir_Aluno_Excepcionalmente_Ativo_Em_Bimestre_Ano_Anterior()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido,
                DataMatricula = DateTime.Parse("2022-01-07"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-07"),
                DataSituacao = DateTime.Parse("2022-01-07"),
            };

            var exibirAlunoFrequencia = aluno.DeveMostrarNaChamada(DateTime.Parse("2021-05-05"), DateTime.Parse("2022-04-01"));

            Assert.True(exibirAlunoFrequencia, "Aluno concluiu turma com data de matricula e situação no ano posterir");

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Exibir_Aluno_Transferido_Dentro_Bimestre_Ano_Anterior()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                DataMatricula = DateTime.Parse("2021-04-20"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-07"),
                DataSituacao = DateTime.Parse("2022-01-07"),
            };

            var exibirAlunoFrequencia = aluno.DeveMostrarNaChamada(DateTime.Parse("2021-05-05"), DateTime.Parse("2022-04-01"));

            Assert.True(exibirAlunoFrequencia, "Aluno transferido no ano anterior dentro do bimestre");

            return Task.CompletedTask;
        }

        [Fact]
        public Task Nao_Deve_Exibir_Aluno_Concluido_Data_Matricula_Posterior_Data_Base_Ano_Anterior()
        {
            var aluno = new AlunoPorTurmaResposta
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido,
                DataMatricula = DateTime.Parse("2021-05-06"),
                DataAtualizacaoContato = DateTime.Parse("2022-01-07"),
                DataSituacao = DateTime.Parse("2022-01-07"),
            };

            var exibirAlunoFrequencia = aluno.DeveMostrarNaChamada(DateTime.Parse("2021-05-05"), DateTime.Parse("2022-04-01"));

            Assert.False(exibirAlunoFrequencia, "Aluno transferido no ano anterior fora do bimestre");

            return Task.CompletedTask;
        }
    }
}
