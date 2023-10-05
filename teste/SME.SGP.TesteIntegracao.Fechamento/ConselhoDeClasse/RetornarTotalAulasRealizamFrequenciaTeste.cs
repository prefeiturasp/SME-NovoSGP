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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoClasseController
{
    public class RetornarAulasComESemFrequenciaTeste : TesteBase
    {
        public RetornarAulasComESemFrequenciaTeste(CollectionFixture testFixture) : base(testFixture) { }

        [Fact]
        public async Task Deve_Retornar_Total_Aulas_Que_Lancam_Frequencia()
        {
            //Arrange
            var useCase = ServiceProvider.GetService<IObterTotalAulasPorAlunoTurmaUseCase>();
            await CriarUsuarioLogado();
            CriarClaimFundamental();
            await CriarAulaComFrequencia();

            //Act
            var controller = new Api.Controllers.ConselhoClasseController();
            var retorno = await controller.ObterTotalAulasPorAlunoTurma("123123", "111", useCase);

            //Assert
            retorno.ShouldNotBeNull();

            Assert.IsType<OkObjectResult>(retorno);

        }
        private async Task CriarUsuarioLogado()
        {
            await InserirNaBase(new Usuario
            {
                Id = 27695,
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
                Id = 1,
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
