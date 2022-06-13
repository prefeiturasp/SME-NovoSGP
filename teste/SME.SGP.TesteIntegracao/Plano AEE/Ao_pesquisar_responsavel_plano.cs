﻿using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.Plano_AEE
{
    public class Ao_pesquisar_responsavel_plano : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;

        public Ao_pesquisar_responsavel_plano(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact]
        public async Task Deve_retornar_responsavel_pelo_planoaee_por_ue()
        {
            await _builder.CriaItensComunsEja(); 

            var useCase = ServiceProvider.GetService<IPesquisaResponsavelPlanoPorDreUEUseCase>();
            var filtro = new FiltroPesquisaFuncionarioDto()
            {
                CodigoTurma = "1"
            };

            var pagina = await useCase.Executar(filtro);

            pagina.ShouldNotBeNull();
            pagina.Items.ShouldNotBeNull();
            pagina.Items.Count().ShouldBeGreaterThanOrEqualTo(1);
        }
    }
}
