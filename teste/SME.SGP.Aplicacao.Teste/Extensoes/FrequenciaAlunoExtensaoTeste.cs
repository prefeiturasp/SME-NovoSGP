using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        [Fact]
        public async Task ObterFrequenciaGeralPorAlunos_Sem_Codigo_Valido_Nao_Deve_Consultar_A_Base()
        {
            var mediatorMock = new Mock<IMediator>();

            var frequenciaPorAluno = await FrequenciaAlunoConsulta
                .ObterFrequenciaGeralPorAlunos(mediatorMock.Object, new[] { null, string.Empty, "   " }, "TURMA-1", "138");

            frequenciaPorAluno.Should().BeEmpty();
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ObterFrequenciaGeralPorAlunos_Deve_Consultar_Uma_Unica_Vez_Com_Os_Codigos_Distintos()
        {
            var mediatorMock = new Mock<IMediator>();
            var frequenciaAluno1 = ObterFrequencia(id: 1, codigoAluno: CODIGO_ALUNO_1);

            mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery>(q => q.AlunosCodigos.Length == 1 &&
                                                                                                         q.AlunosCodigos[0] == CODIGO_ALUNO_1 &&
                                                                                                         q.TurmaCodigo == "TURMA-1" &&
                                                                                                         q.ComponenteCurricularCodigo == "138"),
                                           It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno> { frequenciaAluno1 });

            var frequenciaPorAluno = await FrequenciaAlunoConsulta
                .ObterFrequenciaGeralPorAlunos(mediatorMock.Object, new[] { CODIGO_ALUNO_1, CODIGO_ALUNO_1, null }, "TURMA-1", "138");

            frequenciaPorAluno.Should().HaveCount(1);
            frequenciaPorAluno[CODIGO_ALUNO_1].Should().BeSameAs(frequenciaAluno1);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private static FrequenciaAluno ObterFrequencia(long id, string codigoAluno)
            => new FrequenciaAluno { Id = id, CodigoAluno = codigoAluno };
    }
}
