using MediatR;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterTurmasInfantilNaoDeProgramaQueryHandlerTeste
    {
        private readonly ObterTurmasInfantilNaoDeProgramaQueryHandler query;
        private readonly Mock<IRepositorioTurmaConsulta> repositorioTurmaConsulta;

        public ObterTurmasInfantilNaoDeProgramaQueryHandlerTeste()
        {
            repositorioTurmaConsulta = new Mock<IRepositorioTurmaConsulta>();
            query = new ObterTurmasInfantilNaoDeProgramaQueryHandler(repositorioTurmaConsulta.Object);
        }

        [Fact]
        public async Task Deve_obter_somente_turmas_regulares_para_criar_aulas_automaticas()
        {
            //Arrange

            var turma1 = new Turma
            {
                Nome = "Turma Teste 1",
                CodigoTurma = "1",
                TipoTurma = TipoTurma.Regular,
                AnoLetivo = DateTime.Now.Year,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ano = "1",
                Historica = false,
                UeId = 1
            };

            var turma2 = new Turma
            {
                Nome = "Turma Teste 2",
                CodigoTurma = "2",
                TipoTurma = TipoTurma.Regular,
                AnoLetivo = DateTime.Now.Year,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ano = "1",
                Historica = false
            };

            var turma3 = new Turma
            {
                Nome = "Turma Teste 3",
                CodigoTurma = "3",
                TipoTurma = TipoTurma.Programa,
                AnoLetivo = DateTime.Now.Year,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ano = "1",
                Historica = false
            };

            repositorioTurmaConsulta.Setup(x => x.ObterTurmasInfantilNaoDeProgramaPorAnoLetivoAsync(
                                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                                .ReturnsAsync(new List<Turma> { turma1, turma2, turma3 });
            // Act
            var turmasConsulta = await query.Handle(new ObterTurmasInfantilNaoDeProgramaQuery(DateTime.Now.Year), new CancellationToken());

            //// Assert
            Assert.NotNull(turmasConsulta);

            Assert.True(turmasConsulta.Count() == 2, "O retorno deve conter duas turmas regulares");
            Assert.True(!turmasConsulta.Any(t=> t.TipoTurma == TipoTurma.Programa), "Somente deve retornar turmas regulares e não de programa");
        }
    }
}
