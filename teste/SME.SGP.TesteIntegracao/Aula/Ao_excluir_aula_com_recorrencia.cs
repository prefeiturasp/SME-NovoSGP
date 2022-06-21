using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaRecorrencia
{
    public class Ao_excluir_aula_com_recorrencia : AulaMockComponentePortugues
    {
        private DateTime dataInicio = new DateTime(2022, 05, 02);
        private DateTime dataFim = new DateTime(2022, 07, 08);

        public Ao_excluir_aula_com_recorrencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Excluir_aula_com_recorrencia_no_bimestre()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataInicio, RecorrenciaAula.RepetirBimestreAtual);
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual);

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            var dto = ObtenhaDto(RecorrenciaAula.RepetirBimestreAtual);
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Aula>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().Excluido.ShouldBe(true);
        }
    }
}
