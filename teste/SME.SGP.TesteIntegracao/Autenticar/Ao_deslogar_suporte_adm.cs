using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao
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

            var comando = ServiceProvider.GetService<IComandosUsuario>();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.DeslogarSuporte());

            excecao.Message.ShouldBe("O usuário não está em suporte de um administrador!");
        }
    }
}
