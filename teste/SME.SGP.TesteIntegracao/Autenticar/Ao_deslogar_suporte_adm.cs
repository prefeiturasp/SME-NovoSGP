using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.SuporteAdm
{
    public class Ao_deslogar_suporte_adm : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;
        public Ao_deslogar_suporte_adm(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact]
        public async Task Ao_deslogar_usuario_nao_e_suporte_adm()
        {
            await _builder.CriaItensComunsEja();

            var comando = ServiceProvider.GetService<IDeslogarSuporteUsuarioUseCase>();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Executar());

            excecao.Message.ShouldBe("O usuário não está em suporte de um administrador!");
        }
    }
}
