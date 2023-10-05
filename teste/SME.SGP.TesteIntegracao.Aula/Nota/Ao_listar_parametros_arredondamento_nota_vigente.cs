using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class Ao_listar_parametros_arredondamento_nota_vigente : NotaTesteBase
    {
        
        public Ao_listar_parametros_arredondamento_nota_vigente(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Parametros arredondamento nota - Deve ser encontrado vigente para ano atual e ano anterior conforme data base informada")]
        public async Task Deve_encontrar_parametros_ano_atual_e_anterior()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await CriarDadosBase_ParametrosNota(0, 10, 0.5, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01), new DateTime(DateTimeExtension.HorarioBrasilia().Year, 12, 31));
            await CriarDadosBase_ParametrosNota(4, 9, 1, new DateTime(DateTimeExtension.HorarioBrasilia().Year - 1, 01, 01), new DateTime(DateTimeExtension.HorarioBrasilia().Year - 1, 12, 31));

            var retorno = await mediator.Send(new ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery(DateTimeExtension.HorarioBrasilia().Date));
            retorno.ShouldNotBeNull();
            retorno.Minima.ShouldBe(0);
            retorno.Maxima.ShouldBe(10);
            retorno.Incremento.ShouldBe(0.5);

            retorno = await mediator.Send(new ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery(DateTimeExtension.HorarioBrasilia().Date.AddYears(-1)));
            retorno.ShouldNotBeNull();
            retorno.Minima.ShouldBe(4);
            retorno.Maxima.ShouldBe(9);
            retorno.Incremento.ShouldBe(1);

        }

        private async Task CriarDadosBase_ParametrosNota(double Minima, double Maxima, double Incremento, DateTime inicioVigencia, DateTime finalVigencia)
        {
            await InserirNaBase(new NotaParametro()
            {
                Minima = Minima,
                Media = 5,
                Maxima = Maxima,
                Incremento = Incremento,
                Ativo = true,
                InicioVigencia = inicioVigencia,
                FimVigencia = finalVigencia,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

    }
}
