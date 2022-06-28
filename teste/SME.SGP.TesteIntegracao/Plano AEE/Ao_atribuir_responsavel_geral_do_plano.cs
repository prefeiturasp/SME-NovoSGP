using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Plano_AEE
{
    public class Ao_atribuir_responsavel_geral_do_plano : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;

        public Ao_atribuir_responsavel_geral_do_plano(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact]
        public async Task Deve_atribuir_responsavel_geral_do_plano()
        {
            await _builder.CriaItensComunsEja();
            await CriaPlanoAee();
            await CrieUsuarioNovoResponsavel();

            var useCase = ServiceProvider.GetService<IAtribuirResponsavelGeralDoPlanoUseCase>();
 
            var retorno = await useCase.Executar(1, "8888888");

            retorno.ShouldBe(true);

            var lista = ObterTodos<PlanoAEE>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().ResponsavelId.ShouldBe(2);
        }

        private async Task CriaPlanoAee()
        {
            await InserirNaBase(new PlanoAEE()
            {
                Id = 1,
                AlunoCodigo = "11223344",
                ResponsavelId = 1,
                TurmaId = 1,
                AlunoNome = "Maria Aluno teste",
                Questoes = new List<PlanoAEEQuestao>(),
                Situacao = SituacaoPlanoAEE.Validado,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(2022, 06, 08)
            });
        }

        private async Task CrieUsuarioNovoResponsavel()
        {
            await InserirNaBase(new Usuario
            {
                Id = 2,
                Login = "8888888",
                CodigoRf = "8888888",
                Nome = "Usuario CP",
                CriadoPor = "Sistema",
                CriadoRF = "0",
                AlteradoRF = "0"
            });
        }
    }
}
