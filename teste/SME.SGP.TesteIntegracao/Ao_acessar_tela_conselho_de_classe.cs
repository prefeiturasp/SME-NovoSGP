using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_acessar_tela_conselho_de_classe : TesteBase
    {
        public Ao_acessar_tela_conselho_de_classe(CollectionFixture collectionFixture) : base(collectionFixture) { }

        [Fact]
        public async Task Deve_Exibir_Notas_Alunos_Inativos_Com_Matricula_E_Situacao_Dentro_Do_PeriodoEscolar()
        {
            //Arrange
            var inicioPeriodoEscolar = new DateTime(2022, 01, 03);
            var fimPeriodoEscolar = new DateTime(2022, 04, 29);

            await Criar_Nota_Fechamento(inicioPeriodoEscolar, fimPeriodoEscolar);

            string[] codigoTurmas = { "1" };
            var codigoAluno = "4853818";
            var dataMatricula = new DateTime(2021, 10, 06);
            var dataSituacao = new DateTime(2022, 03, 09);
            const int BIMESTRE = 1;

            var repositorio = ServiceProvider.GetService<IRepositorioFechamentoNotaConsulta>();
            //Act
            var retorno = await repositorio.ObterNotasAlunoPorTurmasCodigosBimestreAsync(codigoTurmas, codigoAluno, BIMESTRE, dataMatricula, dataSituacao);

            //Assert
            retorno.ShouldNotBeNull();
            Assert.True(retorno.Any());
        }

        [Fact]
        public async Task Nao_Deve_Exibir_Notas_Alunos_Inativos_Com_Situacao_Antes_do_PeriodoEscolar()
        {
            //Arrange
            var inicioPeriodoEscolar = new DateTime(2022, 01, 03);
            var fimPeriodoEscolar = new DateTime(2022, 04, 29);

            await Criar_Nota_Fechamento(inicioPeriodoEscolar, fimPeriodoEscolar);

            string[] codigoTurmas = { "1" };
            var codigoAluno = "4853818";
            var dataMatricula = new DateTime(2021, 10, 06);
            var dataSituacao = new DateTime(2021, 03, 09);
            const int BIMESTRE = 1;

            var repositorio = ServiceProvider.GetService<IRepositorioFechamentoNotaConsulta>();
            //Act
            var retorno = await repositorio.ObterNotasAlunoPorTurmasCodigosBimestreAsync(codigoTurmas, codigoAluno, BIMESTRE, dataMatricula, dataSituacao, dataMatricula.Year);

            //Assert
            retorno.ShouldNotBeNull();
            Assert.True(!retorno.Any());
        }

        private async Task Criar_Nota_Fechamento(DateTime inicioPeriodoEscolar, DateTime fimPeriodoEscolar)
        {
            await InserirNaBase(new Dre()
            {
                CodigoDre = "1",
                DataAtualizacao = DateTime.Now
            });

            await InserirNaBase(new Ue()
            {
                CodigoUe = "1",
                DreId = 1,
                DataAtualizacao = DateTime.Now

            });

            await InserirNaBase(new TipoCalendario
            {
                AnoLetivo = 2022,
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



            await InserirNaBase(new PeriodoEscolar()
            {
                TipoCalendarioId = 1,
                //PeriodoInicio = new DateTime(2022, 01, 03),
                PeriodoInicio = inicioPeriodoEscolar,
                //PeriodoFim = new DateTime(2022, 04, 29),
                PeriodoFim = fimPeriodoEscolar,
                CriadoPor = "",
                AlteradoPor = "",
                CriadoRF = "",
                Bimestre = 1,
                CriadoEm = DateTime.Now,
                Migrado = false

            });


            await InserirNaBase(new Dominio.Turma()
            {
                CodigoTurma = "1",
                UeId = 1,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                DataAtualizacao = DateTime.Now,
                Historica = false
            });

            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = 1,
                PeriodoEscolarId = 1,
                CriadoPor = "",
                AlteradoPor = "",
                CriadoRF = "",
                CriadoEm = DateTime.Now,
                Migrado = false,
                Excluido = false,
                AlteradoEm = DateTime.Now,
                AlteradoRF = ""
            });



            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = 1,
                FechamentoTurmaId = 1,
                CriadoPor = "",
                AlteradoPor = "",
                CriadoRF = "",

            });

            await InserirNaBase(new FechamentoAluno()
            {
                AlunoCodigo = "4853818",
                FechamentoTurmaDisciplinaId = 1,
                CriadoPor = "",
                AlteradoPor = "",
                CriadoRF = "",
                Excluido = false,
                CriadoEm = DateTime.Now

            });

            await InserirNaBase(new FechamentoNota()
            {
                FechamentoAlunoId = 1,
                DisciplinaId = 1,
                Migrado = false,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                AlteradoPor = "",
                CriadoRF = "",
                Nota = 10
            });

            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");

            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");

            await InserirNaBase("componente_curricular", "1", "null", "1", "1", "'ARTE'", "false", "false", "false", "true", "true", "true", "'Arte'", "null");

        }
    }
}
