﻿using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_validar_grade_para_registro_de_aula : AulaTeste
    {
        public Ao_validar_grade_para_registro_de_aula(CollectionFixture collectionFixture) : base(collectionFixture)
        {

        }

        [Fact]
        public async Task Quantidade_aulas_superior_ao_limite_de_aulas_da_grade()
        {
            await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriarGrade();

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObtenhaDtoAula();
            dto.Quantidade = 2;

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Quantidade de aulas superior ao limíte de aulas da grade.");
        }

        [Fact]
        public async Task EJA_so_permite_criacao_de_5_aulas()
        {
            await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.EJA, ModalidadeTipoCalendario.EJA);
            await CriaAula();
            await CriarGrade(5);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObtenhaDtoAula();
            dto.EhRegencia = true;
      
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Para regência de EJA só é permitido a criação de 5 aulas por dia.");
        }

        [Fact]
        public async Task Regencia_classe_permite_criacao_de_uma_aula()
        {
            await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriaAula();
            await CriarGrade(5);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObtenhaDtoAula();
            dto.EhRegencia = true;

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Para regência de classe só é permitido a criação de 1 (uma) aula por dia.");
        }

        private async Task CriarGrade(int quantidadeAula = 1)
        {
            await InserirNaBase(new Grade
            {
                Nome = "Grade",
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new GradeFiltro
            {
                GradeId = 1,
                Modalidade = Modalidade.Fundamental,
                TipoEscola = TipoEscola.Nenhum,
                DuracaoTurno = 0,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new GradeDisciplina
            {
                GradeId=1,
                Ano=2,
                QuantidadeAulas= quantidadeAula,
                ComponenteCurricularId= 1106,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
        }
    }
}
