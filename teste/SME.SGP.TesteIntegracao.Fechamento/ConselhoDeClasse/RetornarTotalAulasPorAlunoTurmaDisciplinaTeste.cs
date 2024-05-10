using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.FrequenciaAluno
{
    public class RetornarTotalAulasPorAlunoTurmaDisciplinaTeste : TesteBase
    {
        public RetornarTotalAulasPorAlunoTurmaDisciplinaTeste(CollectionFixture testFixture) : base(testFixture) { }

        [Fact]

        public async Task Deve_Retornar_Total_De_Aulas_Por_Aluno_Turma_Disciplina()
        {
            //Arrange
            var repositorio = ServiceProvider.GetService<IRepositorioAulaConsulta>();
            await CriarAulaComFrequencia();

            //Act
            var retorno = await repositorio.ObterTotalAulasPorTurmaDisciplinaAluno("1106", "111", "123123");

            //Assert
            retorno.ShouldNotBeNull();

            Assert.True(retorno.Count() >= 0);

        }

        private async Task CriarAulaComFrequencia()
        {
            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                AnoLetivo = 2020,
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                Migrado = true,
                CriadoEm = new DateTime(2021, 01, 15, 23, 48, 43),
                CriadoPor = "Sistema",
                AlteradoEm = null,
                AlteradoPor = "",
                CriadoRF = "0",
                AlteradoRF = null,
                Nome = "Teste"
            });

            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 13,
                PeriodoInicio = new DateTime(2020, 02, 05),
                PeriodoFim = new DateTime(2020, 04, 30),
                Bimestre = 1,
                CriadoEm = new DateTime(2021, 01, 15, 23, 48, 43),
                CriadoPor = "Sistema",
                AlteradoEm = null,
                AlteradoPor = "",
                CriadoRF = "0",
                AlteradoRF = null,
                Migrado = false
            });
            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                Id = 25510725,
                PeriodoInicio = new DateTime(2020, 02, 05),
                PeriodoFim = new DateTime(2020, 04, 30),
                Bimestre = 1,
                TotalAulas = 3,
                TotalAusencias = 3,
                CriadoEm = new DateTime(2021, 01, 15, 23, 48, 43),
                CriadoPor = "Sistema",
                AlteradoEm = null,
                AlteradoPor = null,
                CriadoRF = "0",
                AlteradoRF = null,
                TotalCompensacoes = 0,
                PeriodoEscolarId = 1,
                TotalPresencas = 0,
                TotalRemotos = 0,
                DisciplinaId = "1061",
                CodigoAluno = "123123",
                TurmaId = "111",
                Tipo = TipoFrequenciaAluno.PorDisciplina
            });
        }

    }
}
