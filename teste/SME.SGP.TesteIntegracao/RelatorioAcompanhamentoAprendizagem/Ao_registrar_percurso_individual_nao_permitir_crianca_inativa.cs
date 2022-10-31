using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_registrar_percurso_individual_nao_permitir_crianca_inativa : RelatorioAcompanhamentoAprendizagemTesteBase
    {
        public Ao_registrar_percurso_individual_nao_permitir_crianca_inativa(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>),
                typeof(RelatorioAcompanhamentoAprendizagem.ServicosFakes.ObterAlunoPorCodigoEolQueryHandlerAlunoInativoFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Não deve registrar o percurso individual no 1º semestre para criança inativa")]
        public async Task Nao_deve_registrar_o_percurso_individual_para_primeiro_semestre_para_crianca_inativa()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);
            
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = PRIMEIRO_SEMESTRE, 
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                AlunoCodigo = "77777"
            };
            
            await Assert.ThrowsAsync<NegocioException>(() => salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto));
        }
    }
}