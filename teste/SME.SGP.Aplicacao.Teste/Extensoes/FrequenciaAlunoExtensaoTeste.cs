using FluentAssertions;
using SME.SGP.Dominio;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Extensoes
{
    public class FrequenciaAlunoExtensaoTeste
    {
        private const string CODIGO_ALUNO_1 = "1000";
        private const string CODIGO_ALUNO_2 = "2000";

        [Fact]
        public void ToDicionarioPorAluno_Quando_Colecao_Nula_Deve_Retornar_Dicionario_Vazio()
        {
            IEnumerable<FrequenciaAluno> frequenciasAlunos = null;

            var dicionario = frequenciasAlunos.ToDicionarioPorAluno();

            dicionario.Should().NotBeNull();
            dicionario.Should().BeEmpty();
        }

        [Fact]
        public void ToDicionarioPorAluno_Quando_Colecao_Vazia_Deve_Retornar_Dicionario_Vazio()
        {
            var frequenciasAlunos = new List<FrequenciaAluno>();

            var dicionario = frequenciasAlunos.ToDicionarioPorAluno();

            dicionario.Should().BeEmpty();
        }

        [Fact]
        public void ToDicionarioPorAluno_Quando_Aluno_Possui_Mais_De_Um_Registro_Deve_Prevalecer_O_De_Maior_Id()
        {
            var frequenciaAntiga = ObterFrequencia(id: 1, codigoAluno: CODIGO_ALUNO_1);
            var frequenciaRecente = ObterFrequencia(id: 2, codigoAluno: CODIGO_ALUNO_1);

            var dicionario = new List<FrequenciaAluno> { frequenciaAntiga, frequenciaRecente }.ToDicionarioPorAluno();

            dicionario.Should().HaveCount(1);
            dicionario[CODIGO_ALUNO_1].Should().BeSameAs(frequenciaRecente);
        }

        [Fact]
        public void ToDicionarioPorAluno_Independe_Da_Ordem_De_Entrada_Dos_Registros()
        {
            var frequenciaAntiga = ObterFrequencia(id: 1, codigoAluno: CODIGO_ALUNO_1);
            var frequenciaRecente = ObterFrequencia(id: 2, codigoAluno: CODIGO_ALUNO_1);

            var dicionario = new List<FrequenciaAluno> { frequenciaRecente, frequenciaAntiga }.ToDicionarioPorAluno();

            dicionario[CODIGO_ALUNO_1].Should().BeSameAs(frequenciaRecente);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ToDicionarioPorAluno_Deve_Descartar_Registro_Sem_Codigo_De_Aluno(string codigoAlunoInvalido)
        {
            var frequenciaValida = ObterFrequencia(id: 1, codigoAluno: CODIGO_ALUNO_1);
            var frequenciaInvalida = ObterFrequencia(id: 2, codigoAluno: codigoAlunoInvalido);

            var dicionario = new List<FrequenciaAluno> { frequenciaValida, frequenciaInvalida }.ToDicionarioPorAluno();

            dicionario.Should().HaveCount(1);
            dicionario.Should().ContainKey(CODIGO_ALUNO_1);
        }

        [Fact]
        public void ObterFrequenciaAlunoOuNulo_Quando_Dicionario_Nulo_Deve_Retornar_Nulo()
        {
            IDictionary<string, FrequenciaAluno> frequenciaPorAluno = null;

            frequenciaPorAluno.ObterFrequenciaAlunoOuNulo(CODIGO_ALUNO_1).Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ObterFrequenciaAlunoOuNulo_Quando_Codigo_Invalido_Deve_Retornar_Nulo(string codigoAlunoInvalido)
        {
            var frequenciaPorAluno = new List<FrequenciaAluno> { ObterFrequencia(id: 1, codigoAluno: CODIGO_ALUNO_1) }.ToDicionarioPorAluno();

            frequenciaPorAluno.ObterFrequenciaAlunoOuNulo(codigoAlunoInvalido).Should().BeNull();
        }

        [Fact]
        public void ObterFrequenciaAlunoOuNulo_Quando_Aluno_Nao_Consta_No_Dicionario_Deve_Retornar_Nulo()
        {
            var frequenciaPorAluno = new List<FrequenciaAluno> { ObterFrequencia(id: 1, codigoAluno: CODIGO_ALUNO_1) }.ToDicionarioPorAluno();

            frequenciaPorAluno.ObterFrequenciaAlunoOuNulo(CODIGO_ALUNO_2).Should().BeNull();
        }

        [Fact]
        public void ObterFrequenciaAlunoOuNulo_Quando_Aluno_Consta_No_Dicionario_Deve_Retornar_A_Frequencia_Correspondente()
        {
            var frequenciaAluno1 = ObterFrequencia(id: 1, codigoAluno: CODIGO_ALUNO_1);
            var frequenciaAluno2 = ObterFrequencia(id: 2, codigoAluno: CODIGO_ALUNO_2);

            var frequenciaPorAluno = new List<FrequenciaAluno> { frequenciaAluno1, frequenciaAluno2 }.ToDicionarioPorAluno();

            frequenciaPorAluno.ObterFrequenciaAlunoOuNulo(CODIGO_ALUNO_2).Should().BeSameAs(frequenciaAluno2);
        }

        private static FrequenciaAluno ObterFrequencia(long id, string codigoAluno)
            => new FrequenciaAluno { Id = id, CodigoAluno = codigoAluno };
    }
}
