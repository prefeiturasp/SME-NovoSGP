using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class RetornarTotalAulasNaoLancamNotaTeste : TesteBase
    {
        public RetornarTotalAulasNaoLancamNotaTeste(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public async Task Deve_Retornar_Total_Aulas_Que_Nao_Lancam_Nota()
        {
            //Arrange
            var useCase = ServiceProvider.GetService<IObterTotalAulasNaoLancamNotaUseCase>();
            await CriarUsuarioLogado();
            CriarClaimFundamental();
            await CriarAulaQueNaoLancaNota();

            //Act
            var controller = new ConselhoClasseController();
            var retorno = await controller.ObterTotalAulasNaoLancamNotasPorTurmaBimestre("2370993", 1, useCase);

            //Assert
            retorno.ShouldNotBeNull();

            Assert.IsType<OkObjectResult>(retorno);
        }

        private async Task CriarUsuarioLogado()
        {
            await InserirNaBase(new Usuario
            {
                Id = 27695,
                Login = "7495048",
                CodigoRf = "7495048",
                Nome = "FABIANA ROBERTA GUIMARAES REGO",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        private void CriarClaimFundamental()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", "FABIANA ROBERTA GUIMARAES REGO");
            variaveis.Add("UsuarioLogado", "7495048");
            variaveis.Add("RF", "7495048");
            variaveis.Add("login", "7495048");
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = "7495048", Type = "rf" },
                new InternalClaim { Value = "40e1e074-37d6-e911-abd6-f81654fe895d", Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }

        private async Task CriarAulaQueNaoLancaNota()
        {
            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
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

            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                PeriodoInicio = new DateTime(2022, 01, 03),
                PeriodoFim = new DateTime(2022, 04, 29),
                Bimestre = 1,
                CriadoEm = new DateTime(2021, 01, 15, 23, 48, 43),
                CriadoPor = "Sistema",
                AlteradoEm = null,
                AlteradoPor = "",
                CriadoRF = "0",
                AlteradoRF = null,
                Migrado = false
            });
            await InserirNaBase(new FrequenciaAluno
            {
                Id = 2084687593,
                PeriodoInicio = new DateTime(2022, 01, 03),
                PeriodoFim = new DateTime(2022, 04, 29),
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = new DateTime(2022, 04, 21, 12, 46, 29),
                CriadoPor = "Sistema",
                AlteradoEm = new DateTime(2022, 04, 23, 21, 52, 51),
                AlteradoPor = "Sistema",
                CriadoRF = "0",
                AlteradoRF = "0",
                TotalCompensacoes = 0,
                PeriodoEscolarId = 1,
                TotalPresencas = 1,
                TotalRemotos = 0,
                DisciplinaId = "1060",
                CodigoAluno = "6579272",
                TurmaId = "2370993",
                Tipo = TipoFrequenciaAluno.PorDisciplina
                
            });
        }

    }
}
