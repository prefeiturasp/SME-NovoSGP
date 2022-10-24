using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoClasseRecomendacao
{
    public class Ao_obter_recomendacoes : TesteBase
    {
        public Ao_obter_recomendacoes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_retornar_dados_para_recomendacao_aluno()
        {
            var useCase = ServiceProvider.GetService<IObterRecomendacoesAlunoFamiliaUseCase>();

            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Id = 1,
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                Recomendacao = "Recomendação aluno teste 1",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            }) ;

            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Id = 2,
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                Recomendacao = "Recomendação aluno teste 2",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            var retorno = await useCase.Executar();

            retorno.ShouldNotBeEmpty();
            retorno.First().Tipo.ShouldBe((int)ConselhoClasseRecomendacaoTipo.Aluno);
        }

        [Fact]
        public async Task Deve_retornar_dados_para_recomendacao_familia()
        {
            var useCase = ServiceProvider.GetService<IObterRecomendacoesAlunoFamiliaUseCase>();

            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Id = 1,
                Tipo = ConselhoClasseRecomendacaoTipo.Familia,
                Recomendacao = "Recomendação família teste 1",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Id = 2,
                Tipo = ConselhoClasseRecomendacaoTipo.Familia,
                Recomendacao = "Recomendação família teste 2",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            var retorno = await useCase.Executar();

            retorno.ShouldNotBeEmpty();
            retorno.First().Tipo.ShouldBe((int)ConselhoClasseRecomendacaoTipo.Familia);
        }

        [Fact]
        public async Task Deve_retornar_dados_para_aluno_especifico_conselho()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await DadosConselhoClasse();

            var retorno = await mediator.Send(new ObterRecomendacoesPorAlunoConselhoQuery("12345", 1, 1, new long[] { }));

            retorno.ShouldNotBeEmpty();
            retorno.First().Tipo.ShouldBe((int)ConselhoClasseRecomendacaoTipo.Aluno);
        }
  
        public async Task Deve_inserir_recomendacao_aluno()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await DadosConselhoClasse();

            var listaRecomendacoesAluno = new List<long> { 1, 3 };
            var listaRecomendacoesFamilia = new List<long> { 2, 4 };

            await mediator.Send(new SalvarConselhoClasseAlunoRecomendacaoCommand(listaRecomendacoesAluno, listaRecomendacoesFamilia, 1));

            var dadosGravados = ObterTodos<ConselhoClasseAlunoRecomendacao>();

            dadosGravados.ShouldNotBeNull();
            dadosGravados.First().ConselhoClasseAlunoId.ShouldBe(1);
        }


        [Fact]
        public async Task Deve_obter_recomendacoes_por_conselho_aluno_id()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await DadosConselhoClasse();

            var recomendacoesAluno = await mediator.Send(new ObterRecomendacoesPorConselhoAlunoIdQuery(1));

            recomendacoesAluno.ShouldNotBeNull();
            recomendacoesAluno.First().ShouldBe(1);
        }

        public async Task DadosConselhoClasse()
        {
            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                Excluido = false,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Nome = "Calendário teste ano atual",
                Periodo = Periodo.Anual,
                Situacao = true,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                Bimestre = 1,
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 05),
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 02),
                TipoCalendarioId = 1,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Id = 1,
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                Recomendacao = "Recomendação aluno teste",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dre()
            {
                Id = 1,
                Nome = "Dre Teste",
                DataAtualizacao = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-2).Year, 1, 1),
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                DreId = 1,
                Nome = "Ue Teste",
                DataAtualizacao = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-2).Year, 1, 1),
            });

            await InserirNaBase(new Turma()
            {
                Id = 1,
                DataAtualizacao = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1),
                Historica = false,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                UeId = 1
            });


            await InserirNaBase(new FechamentoTurma()
            {
                Id = 1,
                PeriodoEscolarId = 1,
                TurmaId = 1,
                Excluido = false,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new ConselhoClasse()
            {
                Id = 1,
                FechamentoTurmaId = 1,
                Situacao = SituacaoConselhoClasse.EmAndamento,
                Excluido = false,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new ConselhoClasseAluno()
            {
                Id = 1,
                ConselhoClasseId = 1,
                AlunoCodigo = "12345",
                RecomendacoesAluno = "",
                RecomendacoesFamilia = "",
                AnotacoesPedagogicas = "",
                Migrado = false,
                Excluido = false,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new ConselhoClasseAlunoRecomendacao()
            {
                Id = 1,
                ConselhoClasseAlunoId = 1,
                ConselhoClasseRecomendacaoId = 1
            });

            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Id = 2,
                Recomendacao = "recomendação família teste 2",
                Tipo = ConselhoClasseRecomendacaoTipo.Familia,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Id = 4,
                Recomendacao = "recomendação familia teste 1",
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Id = 3,
                Recomendacao = "recomendação aluno teste 2",
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });
           
        }




    }
}
