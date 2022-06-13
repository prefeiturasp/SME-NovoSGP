using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Existe_responsavel_atribuido_a_ue_por_codigoUe_tipo : TesteBase
    {
        private const string CODIGO_UE_1 = "1";
        private const string CODIGO_UE_2 = "2";
        private const string CODIGO_DRE_1 = "1";
        private const string RESPONSAVEL_PAAI_10 = "10";
        private const string RESPONSAVEL_PAAI_11 = "11";
        private const string RESPONSAVEL_PAAI_12 = "12";
        private const string SISTEMA = "Sistema";

        public Existe_responsavel_atribuido_a_ue_por_codigoUe_tipo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_retornar_true_quando_existir_atribuicao_por_tipo_paai_para_ue()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_10, TipoResponsavelAtribuicao.PAAI);
            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_11, TipoResponsavelAtribuicao.PAAI);
            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_12, TipoResponsavelAtribuicao.PAAI);

            var retorno = await mediator.Send(new ObterResponsavelAtribuidoUePorUeTipoQuery(CODIGO_UE_1, TipoResponsavelAtribuicao.PAAI));

            retorno.Any().ShouldBeTrue();            
        }

        [Fact]
        public async Task Deve_retornar_true_quando_existir_atribuicao_por_tipo_assistente_social_para_ue()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_10, TipoResponsavelAtribuicao.AssistenteSocial);
            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_11, TipoResponsavelAtribuicao.AssistenteSocial);
            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_12, TipoResponsavelAtribuicao.AssistenteSocial);

            var retorno = await mediator.Send(new ObterResponsavelAtribuidoUePorUeTipoQuery(CODIGO_UE_1, TipoResponsavelAtribuicao.AssistenteSocial));

            retorno.Any().ShouldBeTrue();
        }

        [Fact]
        public async Task Deve_retornar_false_quando_existir_atribuicao_por_tipo_assistente_social_quando_desejado_tipo_paai_para_ue()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_10, TipoResponsavelAtribuicao.AssistenteSocial);
            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_11, TipoResponsavelAtribuicao.AssistenteSocial);
            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_12, TipoResponsavelAtribuicao.AssistenteSocial);

            var retorno = await mediator.Send(new ObterResponsavelAtribuidoUePorUeTipoQuery(CODIGO_UE_1, TipoResponsavelAtribuicao.PAAI));

            retorno.Any().ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_retornar_false_quando_nao_existir_atribuicao_por_tipo_paai_para_ue()
        {
            var mediator = ServiceProvider.GetService<IMediator>();
                        
            var retorno = await mediator.Send(new ObterResponsavelAtribuidoUePorUeTipoQuery(CODIGO_UE_2, TipoResponsavelAtribuicao.PAAI));

            retorno.Any().ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_retornar_false_quando_existir_atribuicao_para_ue_diferente_por_tipo_paai_e_ue()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_10, TipoResponsavelAtribuicao.PAAI);
            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_11, TipoResponsavelAtribuicao.PAAI);
            await InserirResponsavel(CODIGO_UE_1, RESPONSAVEL_PAAI_12, TipoResponsavelAtribuicao.PAAI);

            var retorno = await mediator.Send(new ObterResponsavelAtribuidoUePorUeTipoQuery(CODIGO_UE_2, TipoResponsavelAtribuicao.PAAI));

            retorno.Any().ShouldBeFalse();
        }

        public async Task InserirResponsavel(string codigoUe, string responsavel,  TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            await InserirNaBase(new SupervisorEscolaDre()
            {
                DreId = CODIGO_DRE_1,
                EscolaId = codigoUe,
                SupervisorId = responsavel,
                Tipo = (int)tipoResponsavelAtribuicao,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA,
                Excluido = false
            });
        }    
    }
}