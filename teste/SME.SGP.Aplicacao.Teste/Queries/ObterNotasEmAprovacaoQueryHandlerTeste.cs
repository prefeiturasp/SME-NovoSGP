using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterNotasEmAprovacaoQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_preservar_consulta_por_alunos_e_fechamentos_do_listao()
        {
            var alunos = new[] { "1", "2" };
            var fechamentos = new long[] { 10, 20 };
            var query = new ObterNotasEmAprovacaoQuery(alunos, fechamentos);
            IEnumerable<NotaEmAprovacaoFechamentoDto> esperado = new[]
            {
                new NotaEmAprovacaoFechamentoDto { CodigoAluno = "1", TurmaFechamentoId = 10, DisciplinaId = 138, Nota = -1 }
            };
            var repositorio = new Mock<IRepositorioNotasConceitosConsulta>(MockBehavior.Strict);
            repositorio.Setup(r => r.ObterNotasEmAprovacao(alunos, fechamentos)).ReturnsAsync(esperado);

            Assert.True(new ObterNotasEmAprovacaoQueryValidator().Validate(query).IsValid);
            var resultado = await new ObterNotasEmAprovacaoQueryHandler(repositorio.Object)
                .Handle(query, CancellationToken.None);

            Assert.Same(esperado, resultado);
            repositorio.Verify(r => r.ObterNotasEmAprovacao(alunos, fechamentos), Times.Once);
            repositorio.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Deve_preservar_filtros_por_aluno_fechamento_e_disciplina_da_feature()
        {
            var filtros = new[]
            {
                new NotaEmAprovacaoFechamentoDto { CodigoAluno = "1", TurmaFechamentoId = 10, DisciplinaId = 138 },
                new NotaEmAprovacaoFechamentoDto { CodigoAluno = "1", TurmaFechamentoId = 20, DisciplinaId = 1105 }
            };
            var query = new ObterNotasEmAprovacaoQuery(filtros);
            IEnumerable<NotaEmAprovacaoFechamentoDto> esperado = filtros;
            var repositorio = new Mock<IRepositorioNotasConceitosConsulta>(MockBehavior.Strict);
            repositorio.Setup(r => r.ObterNotasEmAprovacaoAsync(filtros)).ReturnsAsync(esperado);

            Assert.True(new ObterNotasEmAprovacaoQueryValidator().Validate(query).IsValid);
            var resultado = await new ObterNotasEmAprovacaoQueryHandler(repositorio.Object)
                .Handle(query, CancellationToken.None);

            Assert.Same(esperado, resultado);
            repositorio.Verify(r => r.ObterNotasEmAprovacaoAsync(filtros), Times.Once);
            repositorio.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Filtros_nulos_ou_vazios_nao_devem_acionar_consulta_do_listao(bool nulos)
        {
            var query = new ObterNotasEmAprovacaoQuery(nulos ? null : Array.Empty<NotaEmAprovacaoFechamentoDto>());
            var repositorio = new Mock<IRepositorioNotasConceitosConsulta>(MockBehavior.Strict);
            repositorio.Setup(r => r.ObterNotasEmAprovacaoAsync(It.Is<NotaEmAprovacaoFechamentoDto[]>(f => f.Length == 0)))
                .ReturnsAsync(Array.Empty<NotaEmAprovacaoFechamentoDto>());

            var resultado = await new ObterNotasEmAprovacaoQueryHandler(repositorio.Object)
                .Handle(query, CancellationToken.None);

            Assert.Empty(resultado);
            repositorio.Verify(r => r.ObterNotasEmAprovacaoAsync(It.IsAny<NotaEmAprovacaoFechamentoDto[]>()), Times.Once);
            repositorio.VerifyNoOtherCalls();
        }

        [Fact]
        public void Deve_manter_validacao_dos_filtros_da_feature()
        {
            var query = new ObterNotasEmAprovacaoQuery(new[] { new NotaEmAprovacaoFechamentoDto() });
            var resultado = new ObterNotasEmAprovacaoQueryValidator().Validate(query);

            Assert.False(resultado.IsValid);
            Assert.Equal(3, resultado.Errors.Count);
        }
    }
}
