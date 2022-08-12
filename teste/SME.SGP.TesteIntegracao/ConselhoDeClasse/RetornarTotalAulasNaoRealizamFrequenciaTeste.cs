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

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class RetornarTotalAulasNaoRealizamFrequenciaTeste : TesteBase
    {
        public RetornarTotalAulasNaoRealizamFrequenciaTeste(CollectionFixture testFixture) : base(testFixture) { }

        [Fact]
        public async Task Deve_Obter_Total_Aulas_Nao_Realizam_Frequencia()
        {
            //Arrange
            var useCase = ServiceProvider.GetService<IObterTotalAulasSemFrequenciaPorTurmaUseCase>();
            await CriarUsuarioLogado();
            CriarClaimFundamental();
            await CriarAulaSemFrequencia();

            //Act
            var controller = new Api.Controllers.ConselhoClasseController();
            var retorno = await controller.ObterTotalAulasSemFrequenciaPorTurma("2370993",useCase);

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

        private async Task CriarAulaSemFrequencia()
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

            await InserirNaBase(new Dominio.Aula
            {
                UeId = "1",
                DisciplinaId = "1106",
                TurmaId = "2370993",
                TipoCalendarioId = 1,
                ProfessorRf = "6737544",
                Quantidade = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                RecorrenciaAula = 0,
                TipoAula = TipoAula.Normal,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            });
        }
    }
}
