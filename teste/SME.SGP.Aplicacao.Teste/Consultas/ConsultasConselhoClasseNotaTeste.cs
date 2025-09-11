using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasConselhoClasseNotaTeste
    {
        private readonly Mock<IRepositorioConselhoClasseNotaConsulta> _repositorioMock;
        private readonly ConsultasConselhoClasseNota _consultasConselhoClasseNota;

        public ConsultasConselhoClasseNotaTeste()
        {
            _repositorioMock = new Mock<IRepositorioConselhoClasseNotaConsulta>();
            _consultasConselhoClasseNota = new ConsultasConselhoClasseNota(_repositorioMock.Object);
        }

        [Fact]
        public void Deve_Obter_Nota_Por_Id()
        {
            long id = 123;
            var notaEsperada = new ConselhoClasseNota { Id = id };

            _repositorioMock.Setup(r => r.ObterPorId(id))
                            .Returns(notaEsperada);

            var resultado = _consultasConselhoClasseNota.ObterPorId(id);

            Assert.Equal(notaEsperada, resultado);
            _repositorioMock.Verify(r => r.ObterPorId(id), Times.Once);
        }

        [Fact]
        public async Task Deve_Obter_Notas_Finais_Aluno_Async()
        {
            string alunoCodigo = "12345";
            string turmaCodigo = "67890";

            var notasEsperadas = new List<NotaConceitoBimestreComponenteDto>
               {
                  new NotaConceitoBimestreComponenteDto { ComponenteCurricularCodigo = 100, Nota = 9 }
               };

            _repositorioMock
                .Setup(r => r.ObterNotasAlunoAsync(alunoCodigo, turmaCodigo, null))
                .ReturnsAsync(notasEsperadas);

            var resultado = await _consultasConselhoClasseNota.ObterNotasFinaisAlunoAsync(alunoCodigo, turmaCodigo);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(9, resultado.First().Nota);

            _repositorioMock.Verify(r => r.ObterNotasAlunoAsync(alunoCodigo, turmaCodigo, null), Times.Once);
        }
    }
}
