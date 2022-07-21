using System.Collections.Generic;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using Xunit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_reprocessar_componentes_nao_lancam_nota_pendencias_fechamento : NotaFechamentoTesteBase
    {
        public Ao_reprocessar_componentes_nao_lancam_nota_pendencias_fechamento(CollectionFixture collectionFixture) : base(collectionFixture)
        { }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaGeracaoPendenciasFechamentoCommand, bool>), typeof(IncluirFilaGeracaoPendenciasFechamentoCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessoresTurmaDisciplinaQuery, IEnumerable<ProfessorAtribuidoTurmaDisciplinaDTO>>), typeof(ProfessoresTurmaDisciplinaQueryHandlerFakeProfessorLeituraOSL), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_processar_para_resolver_pendencias_com_aula_sem_avaliacao()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_LEITURA_OSL_ID_1061.ToString());
        
            await ExecutarTesteComandosFechamentoTurmaDisciplina(filtroNotaFechamento);
        }
    }
}