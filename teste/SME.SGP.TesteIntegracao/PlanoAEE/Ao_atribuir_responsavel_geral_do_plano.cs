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

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_atribuir_responsavel_geral_do_plano : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;
        private const string RF_NOVO = "8888888";
        private const string NOME_NOVO_USUARIO = "Novo usuário";

        public Ao_atribuir_responsavel_geral_do_plano(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact(DisplayName = "Plano AEE - Deve atribuir um responsavel geral para o plano")]
        public async Task Deve_atribuir_responsavel_geral_do_plano()
        {
            await _builder.CriaItensComunsEja();
            await CriaPlanoAee();
            await CrieUsuarioNovoResponsavel();

            var useCase = ServiceProvider.GetService<IAtribuirResponsavelGeralDoPlanoUseCase>();
 
            var retorno = await useCase.Executar(1, RF_NOVO, string.Empty);

            retorno.ShouldBe(true);

            var lista = ObterTodos<Dominio.PlanoAEE>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().ResponsavelId.ShouldBe(2);
        }

        [Fact(DisplayName = "Plano AEE - Deve atribuir um responsavel e criando usuario")]
        public async Task Deve_atribuir_responsavel_criando_usuario()
        {
            await _builder.CriaItensComunsEja();
            await CriaPlanoAee();

            var useCase = ServiceProvider.GetService<IAtribuirResponsavelGeralDoPlanoUseCase>();

            var retorno = await useCase.Executar(1, RF_NOVO, NOME_NOVO_USUARIO);

            retorno.ShouldBe(true);

            var lista = ObterTodos<Dominio.PlanoAEE>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().ResponsavelId.ShouldBe(2);

            var listaUsuario = ObterTodos<Usuario>();

            listaUsuario.ShouldNotBeEmpty();
            listaUsuario.Exists(usuario => usuario.Nome == NOME_NOVO_USUARIO).ShouldBe(true);
        }

        private async Task CriaPlanoAee()
        {
            await InserirNaBase(new Dominio.PlanoAEE()
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
                Login = RF_NOVO,
                CodigoRf = RF_NOVO,
                Nome = NOME_NOVO_USUARIO,
                CriadoPor = "Sistema",
                CriadoRF = "0",
                AlteradoRF = "0"
            });
        }
    }
}
