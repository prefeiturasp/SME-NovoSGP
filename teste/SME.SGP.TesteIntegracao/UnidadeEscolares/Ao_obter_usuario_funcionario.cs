using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_obter_usuario_funcionario : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;
        public Ao_obter_usuario_funcionario(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact]
        public async Task Deve_retornar_usuario_por_login()
        {
            await _builder.CriaItensComunsEja();

            var useCase = ServiceProvider.GetService<IObterUsuarioFuncionarioUseCase>();

            var dto = new FiltroFuncionarioDto()
            {
                CodigoRF = "6926886"
            };

            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeEmpty();
            retorno.First().CodigoRf.ShouldBe("6926886");
        }
    }
}
