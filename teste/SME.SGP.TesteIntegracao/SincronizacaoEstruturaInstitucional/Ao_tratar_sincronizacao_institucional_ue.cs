using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Documento;
using SME.SGP.TesteIntegracao.Setup;
using SME.SGP.TesteIntegracao.SincronizacaoEstruturaInstitucional.ServicosFakes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace SME.SGP.TesteIntegracao.SincronizacaoEstruturaInstitucional
{
    public class Ao_tratar_sincronizacao_institucional_ue : DocumentoTesteBase
    {
        public Ao_tratar_sincronizacao_institucional_ue(CollectionFixture collectionFixture)
            : base(collectionFixture)
        {
        }

        protected override void RegistrarQueryFakes(IServiceCollection services)
        {
            base.RegistrarQueryFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUeDetalhesParaSincronizacaoInstitucionalQuery, UeDetalhesParaSincronizacaoInstituicionalDto>), typeof(ObterUeDetalhesParaSincronizacaoInstitucionalQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName ="Sincronização Institucional UE - Deve alterar a DRE da UE em caso de remanejamento")]
        public async Task Ao_tratar_sincronizacao_institucional_ue_alterar_dre_remanejamento()
        {            
            var dataHoraAtual = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new Dre()
            {
                CodigoDre = DRE_CODIGO_1,
                Nome = DRE_NOME_1,
                DataAtualizacao = dataHoraAtual
            });

            await InserirNaBase(new Dre()
            {
                CodigoDre = DRE_CODIGO_2,
                Nome = DRE_NOME_2,
                DataAtualizacao = dataHoraAtual
            });          

            await InserirNaBase(new Ue()
            {
                CodigoUe = UE_CODIGO_1,
                Nome = UE_NOME_2,
                DataAtualizacao = dataHoraAtual,
                DreId = ObterTodos<Dre>().Single(d => d.CodigoDre == DRE_CODIGO_1).Id
            });
            
            var useCase = ServiceProvider.GetService<IExecutarSincronizacaoInstitucionalUeTratarUseCase>();

            var retorno = await useCase.Executar(new MensagemRabbit(UE_CODIGO_1));
            retorno.ShouldBeTrue();

            var ueRemanejada = ObterTodos<Ue>()?.SingleOrDefault(u => u.Id == UE_ID_1);
            ueRemanejada.ShouldNotBeNull();
            ueRemanejada.DreId.ShouldBe(ObterTodos<Dre>().Single(d => d.CodigoDre == DRE_CODIGO_2).Id);
        }        
    }
}
