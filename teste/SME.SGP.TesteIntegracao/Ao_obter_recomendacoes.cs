using Microsoft.Extensions.DependencyInjection;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using MediatR;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_obter_recomendacoes : TesteBase
    {
        public Ao_obter_recomendacoes(TestFixture testFixture) : base(testFixture)
        {
        }

        [Fact]
        public async Task Deve_retornar_dados_para_recomendacao_aluno()
        {
            var useCase = ServiceProvider.GetService<IObterRecomendacoesAlunoFamiliaUseCase>();

            await InserirNaBase(new ConselhoClasseRecomendacao()
            {
                Id = 1,
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                Recomendacao = "Recomendação aluno teste 1",
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            }) ;

            await InserirNaBase(new ConselhoClasseRecomendacao()
            {
                Id = 2,
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                Recomendacao = "Recomendação aluno teste 2",
                CriadoEm = new DateTime(2022, 01, 01),
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

            await InserirNaBase(new ConselhoClasseRecomendacao()
            {
                Id = 1,
                Tipo = ConselhoClasseRecomendacaoTipo.Familia,
                Recomendacao = "Recomendação família teste 1",
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new ConselhoClasseRecomendacao()
            {
                Id = 2,
                Tipo = ConselhoClasseRecomendacaoTipo.Familia,
                Recomendacao = "Recomendação família teste 2",
                CriadoEm = new DateTime(2022, 01, 01),
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

            var retorno = await mediator.Send(new ObterRecomendacoesPorAlunoConselhoQuery("12345", 1, 1));

            retorno.ShouldNotBeEmpty();
            retorno.First().Tipo.ShouldBe((int)ConselhoClasseRecomendacaoTipo.Aluno);
        }

        [Fact]
        public async Task Deve_gravar_recomendacao_aluno()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await DadosConselhoClasse();

            var dadosGravados = ObterTodos<ConselhoClasseAlunoRecomendacao>();

            dadosGravados.ShouldNotBeNull();
            dadosGravados.First().ConselhoClasseAlunoId.ShouldBe(1);
        }

        public async Task DadosConselhoClasse()
        {
            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                Excluido = false,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Nome = "Calendário teste 2022",
                Periodo = Periodo.Anual,
                Situacao = true,
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                Bimestre = 1,
                PeriodoFim = new DateTime(2022, 01, 05),
                PeriodoInicio = new DateTime(2022, 01, 02),
                TipoCalendarioId = 1,
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new ConselhoClasseRecomendacao()
            {
                Id = 1,
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                Recomendacao = "Recomendação aluno teste",
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dre()
            {
                Id = 1,
                Nome = "Dre Teste",
                DataAtualizacao = new DateTime(2020, 1, 1),
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                DreId = 1,
                Nome = "Ue Teste",
                DataAtualizacao = new DateTime(2020, 1, 1),
            });

            await InserirNaBase(new Turma()
            {
                Id = 1,
                DataAtualizacao = new DateTime(2022, 1, 1),
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
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new ConselhoClasse()
            {
                Id = 1,
                FechamentoTurmaId = 1,
                Situacao = SituacaoConselhoClasse.EmAndamento,
                Excluido = false,
                CriadoEm = new DateTime(2022, 01, 01),
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
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new ConselhoClasseAlunoRecomendacao()
            {
                Id = 1,
                ConselhoClasseAlunoId = 1,
                ConselhoClasseRecomendacaoId = 1
            });

            await InserirNaBase(new ConselhoClasseRecomendacao()
            {
                Id = 2,
                Recomendacao = "recomendação família teste 2",
                Tipo = ConselhoClasseRecomendacaoTipo.Familia,
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new ConselhoClasseRecomendacao()
            {
                Id = 4,
                Recomendacao = "recomendação familia teste 1",
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new ConselhoClasseRecomendacao()
            {
                Id = 3,
                Recomendacao = "recomendação aluno teste 2",
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });
           
        }


    }
}
