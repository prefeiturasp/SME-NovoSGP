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


    }
}
