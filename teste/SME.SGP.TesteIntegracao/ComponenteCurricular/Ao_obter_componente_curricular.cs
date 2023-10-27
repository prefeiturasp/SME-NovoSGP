using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ComponenteCurricular
{
    public class Ao_obter_componente_curricular : TesteBase
    {
        private const long ID_MATEMATICA = 1;
        private const string NOME_COMPONENTE = "MATEMATICA";
        public Ao_obter_componente_curricular(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_obter_descricao_obedecendo_nulos_e_com_espacos()
        {

            var mediator = ServiceProvider.GetService<IMediator>();

            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");
            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");
            //os dois últimos parâmetros são descricao_sgp e descricao_infantil (percebe-se que está com espaços (' '))
            await InserirNaBase("componente_curricular", "1", "512", "1", "1", "'MATEMATICA'", "false", "false", "true", "false", "false", "true", "' '", "''");


            var nomeComponente = await mediator.Send(new ObterDescricaoComponenteCurricularPorIdQuery(ID_MATEMATICA));

            nomeComponente.ShouldNotBeEmpty();
            nomeComponente.ShouldBe(NOME_COMPONENTE);
        }
    }
}
