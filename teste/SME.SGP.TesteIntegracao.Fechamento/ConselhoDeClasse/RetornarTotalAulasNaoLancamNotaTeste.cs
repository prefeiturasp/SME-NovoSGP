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

namespace SME.SGP.TesteIntegracao.ConselhoClasseController
{
    public class RetornarTotalAulasNaoLancamNotaTeste : TesteBase
    {
        public RetornarTotalAulasNaoLancamNotaTeste(CollectionFixture testFixture) : base(testFixture) { }

        [Fact]
        public async Task Deve_Retornar_Total_Aulas_Que_Nao_Lancam_Nota()
        {
            //Arrange
            var useCase = ServiceProvider.GetService<IObterTotalAulasNaoLancamNotaUseCase>();
            await CriarUsuarioLogado();
            CriarClaimFundamental();
            await CriarAulaQueNaoLancaNota();

            //Act
            var controller = new Api.Controllers.ConselhoClasseController();
            var retorno = await controller.ObterTotalAulasNaoLancamNotasPorTurmaBimestre("111", 1, "123123", useCase);

            //Assert
            retorno.ShouldNotBeNull();

            Assert.IsType<OkObjectResult>(retorno);
        }

        private async Task CriarUsuarioLogado()
        {
            await InserirNaBase(new Usuario
            {
                Id = 1,
                Login = TesteBaseComuns.USUARIO_LOGADO_RF,
                CodigoRf = TesteBaseComuns.USUARIO_LOGADO_RF,
                Nome = TesteBaseComuns.USUARIO_LOGADO_NOME,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        private void CriarClaimFundamental()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", TesteBaseComuns.USUARIO_LOGADO_NOME);
            variaveis.Add("UsuarioLogado", TesteBaseComuns.USUARIO_LOGADO_RF);
            variaveis.Add("RF", TesteBaseComuns.USUARIO_LOGADO_RF);
            variaveis.Add("login", TesteBaseComuns.USUARIO_LOGADO_RF);
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = TesteBaseComuns.USUARIO_LOGADO_RF, Type = "rf" },
                new InternalClaim { Value = Perfis.PERFIL_PROFESSOR.ToString(), Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }

        private async Task CriarAulaQueNaoLancaNota()
        {
            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                Migrado = true,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 15, 23, 48, 43),
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
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 03),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 29),
                Bimestre = 1,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 15, 23, 48, 43),
                CriadoPor = "Sistema",
                AlteradoEm = null,
                AlteradoPor = "",
                CriadoRF = "0",
                AlteradoRF = null,
                Migrado = false
            });
            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                Id = 2084687593,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 03),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 29),
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 21, 12, 46, 29),
                CriadoPor = "Sistema",
                AlteradoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 23, 21, 52, 51),
                AlteradoPor = "Sistema",
                CriadoRF = "0",
                AlteradoRF = "0",
                TotalCompensacoes = 0,
                PeriodoEscolarId = 1,
                TotalPresencas = 1,
                TotalRemotos = 0,
                DisciplinaId = "1060",
                CodigoAluno = "123123",
                TurmaId = "111",
                Tipo = TipoFrequenciaAluno.PorDisciplina

            });
        }

    }
}
