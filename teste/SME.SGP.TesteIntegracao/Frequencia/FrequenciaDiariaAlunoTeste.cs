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
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.FrequenciaDiaria
{
    public class FrequenciaDiariaAlunoTeste : TesteBase
    {
        public FrequenciaDiariaAlunoTeste(CollectionFixture collectionFixture) : base(collectionFixture)
        {

        }

        [Fact(DisplayName = "Frequência - Deve retornar lista com frequencia")]
        public async Task Deve_Retornar_Lista_Com_Frequencia()
        {
            await CriarUsuarioLogado();
            CriarClaimProfessorFundamental();
            await CriarItensBasicos();
            var useCase = ServiceProvider.GetService<IObterFrequenciaDiariaAlunoUseCase>();

            var controller = new FrequenciaAcompanhamentoController();

            var retorno = await controller.ObterFrequenciaDiariaAluno(1, 1, 1, 2, 1, useCase);

            retorno.ShouldNotBeNull();
            Assert.IsType<OkObjectResult>(retorno);
        }

        [Fact(DisplayName = "Frequência - Dev retornar código 200 - sem dados")]
        public async Task Deve_Retornar_200_Sem_Dados()
        {
            await CriarUsuarioLogado();
            CriarClaimProfessorFundamental();
            await CriarItensBasicos();
            var useCase = ServiceProvider.GetService<IObterFrequenciaDiariaAlunoUseCase>();

            var controller = new FrequenciaAcompanhamentoController();

            var retorno = await controller.ObterFrequenciaDiariaAluno(1, 138, 1, 2,1, useCase);

            retorno.ShouldNotBeNull();

            Assert.IsType<NoContentResult>(retorno);
        }


        private void CriarClaimProfessorFundamental()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", "Professor Nome");
            variaveis.Add("UsuarioLogado", "1");
            variaveis.Add("RF", "1");
            variaveis.Add("login", "1");
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = "1", Type = "rf" },
                new InternalClaim { Value = Perfis.PERFIL_ADMUE.ToString(), Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }

        private async Task CriarUsuarioLogado()
        {
            await InserirNaBase(new Usuario
            {
                Id = 1,
                Login = "1",
                CodigoRf = "1",
                Nome = "ADALGISA GONCALVES",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1
            });

            await InserirNaBase(new Turma
            {
                Id = 1,
                UeId = 1,
                Ano = "1",
                CodigoTurma = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year
            });

            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                Nome = "",
                CriadoPor = "",
                CriadoRF = ""
            });
            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 03),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 29),
                Bimestre = 2,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 15, 23, 48, 43),
                CriadoPor = "Sistema",
                AlteradoEm = null,
                AlteradoPor = "",
                CriadoRF = "0",
                AlteradoRF = null,
                Migrado = false
            });
            await InserirNaBase(new Dominio.Aula
            {
                Id = 1,
                CriadoPor = "",
                CriadoRF = "",
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                ProfessorRf = "",
                TipoCalendarioId = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 26),
                Quantidade = 1,
                
            });
            await InserirNaBase(new RegistroFrequencia
            {
                Id = 1,
                AulaId = 1,
                CriadoPor = "",
                CriadoRF = ""
            });
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 1,
                CodigoAluno = "1",
                RegistroFrequenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 1,
                NumeroAula = 1,
                AulaId = 1
            });
        }
    }
}
