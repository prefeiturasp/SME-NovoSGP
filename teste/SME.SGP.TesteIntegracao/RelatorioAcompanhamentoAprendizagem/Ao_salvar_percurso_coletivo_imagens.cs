using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_salvar_percurso_coletivo_imagens : RelatorioAcompanhamentoAprendizagemTesteBase
    {
        public Ao_salvar_percurso_coletivo_imagens(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<TurmaEmPeriodoAbertoQuery,bool>),typeof(TurmaEmPeriodoAbertoQueryHandlerFake),ServiceLifetime.Scoped));
        }
        

        [Fact(DisplayName = "Relatório do Acompanhamento da Aprendizagem - Deve Registrar o percurso coletivo inserindo duas imagens")]
        public async Task Registrar_o_percurso_coletivo_inserindo_duas_imagens()
        {
            await CriarDadosBasicos();
            var useCase = ObterSalvarAcompanhamentoUseCase();
            
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);
            
            var dto = new AcompanhamentoTurmaDto
            {
                TurmaId = 1,
                Semestre = 1,
                ApanhadoGeral = @"<html><body>
                                        teste
                                    <img src='http://www.localhost.com.br/imagem.png'>
                                    <img src='http://www.localhost.com.br/imagem.png'>
                                    </body><html/>"
            };
            var salvar = await useCase.Executar(dto);
            salvar.Id.ShouldBeEquivalentTo((long)1);
            
            var obterTodos = ObterTodos<AcompanhamentoTurma>();
            obterTodos.ShouldNotBeNull();
            obterTodos.Count.ShouldBeEquivalentTo(1);
        }
        [Fact(DisplayName = "Relatório do Acompanhamento da Aprendizagem - Não deve Registrar o percurso coletivo inserindo mais de duas imagens")]
        public async Task Registrar_o_percurso_coletivo_inserindo_mais_de_duas_imagens()
        {
            await CriarDadosBasicos();
            var useCase = ObterSalvarAcompanhamentoUseCase();
            
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);
            
            var dto = new AcompanhamentoTurmaDto
            {
                TurmaId = TURMA_ID_1,
                Semestre = PRIMEIRO_SEMESTRE,
                ApanhadoGeral = TEXTO_PADRAO_COM_4_IMAGENS
            };
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
            ex.ShouldNotBeNull();
            
            var obterTodos = ObterTodos<AcompanhamentoTurma>();
            obterTodos.ShouldNotBeNull();
            obterTodos.Count.ShouldBeEquivalentTo(0);
        }

    }
}