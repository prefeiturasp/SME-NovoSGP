using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Turma
{
    public class Ao_obter_turma_sondagem : TesteBaseComuns
    {
        public Ao_obter_turma_sondagem(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Turma - Ao obter turma sondagem da ue")]
        public async Task Ao_obter_turma_sondagem_ue()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilPAP());

            await CriarUsuarios();

            await CriarTurma(Modalidade.Fundamental, "7", TURMA_CODIGO_1, TipoTurma.Regular);
            await CriarTurma(Modalidade.Fundamental, "6", TURMA_CODIGO_2, TipoTurma.Regular);
            await CriarTurma(Modalidade.Medio, "7", TURMA_CODIGO_3, TipoTurma.Regular);
            await CriarTurma(Modalidade.Fundamental, "1", TURMA_CODIGO_4, TipoTurma.Programa);

            var useCase = ServiceProvider.GetService<IObterTurmaSondagemUseCase>();

            var retorno = await useCase.Executar(UE_CODIGO_1, DateTime.Now.Year);

            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
        }
    }
}
