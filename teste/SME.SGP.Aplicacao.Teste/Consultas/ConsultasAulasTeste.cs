using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasAulasTeste
    {
        private readonly ConsultasAula consultas;
        private readonly Mock<IRepositorioAula> repositorio;

        public ConsultasAulasTeste()
        {
            repositorio = new Mock<IRepositorioAula>();
            consultas = new ConsultasAula(repositorio.Object);

            Setup();
        }

        private void Setup()
        {
            // Mock para testar o metodo ObterPorId
            var aula = new Aula()
            {
                Id = 1,
                DataAula = new DateTime(2019, 11, 15),
                ProfessorId = 123,
                Quantidade = 3
            };

            repositorio.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(aula);

            // Mock das aulas por turma e disciplina
            IEnumerable<AulasPorTurmaDisciplinaDto> aulas = new List<AulasPorTurmaDisciplinaDto>()
            {
                new AulasPorTurmaDisciplinaDto() { ProfessorId = 1, Quantidade = 1, DataAula = new System.DateTime(2019,11,12) },
                new AulasPorTurmaDisciplinaDto() { ProfessorId = 1, Quantidade = 3, DataAula = new System.DateTime(2019,11,15) },
            };

            repositorio.Setup(c => c.ObterAulasTurmaDisciplina(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(aulas));
        }

        [Fact]
        public async Task DeveObterAulaPorId()
        {
            var aulaDto = consultas.BuscarPorId(1);

            Assert.NotNull(aulaDto);
            Assert.True(aulaDto.Id == 1);
            Assert.True(aulaDto.Quantidade == 3);
        }

        [Fact]
        public async Task DeveObterQuantidadeAulas()
        {
            var qtd = await consultas.ObterQuantidadeAulasTurma("123", "7");

            Assert.True(qtd == 4);
        }
    }
}
