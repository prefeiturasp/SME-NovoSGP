using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_salvar_percurso_coletivo_imagens : RelatorioAcompanhamentoAprendizagemTesteBase
    {
        public Ao_salvar_percurso_coletivo_imagens(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }


        [Fact(DisplayName = "Relatório do Acompanhamento da Aprendizagem - Não Deve Registrar o percurso coletivo para semestre e ano anterior sem reabertura")]
        public async Task Registrar_percurso_coletivo_para_semestre_e_ano_anterior_sem_reabertura()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            var useCase = SalvarAcompanhamentoTurmaUseCase();
            
            var dto = new AcompanhamentoTurmaDto
            {
                TurmaId = 1,
                Semestre = 1,
                ApanhadoGeral = "<html><body>teste</body><html/>"
            };
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
            ex.ShouldNotBeNull();
            
            var obterTodos = ObterTodos<AcompanhamentoTurma>();
            obterTodos.ShouldNotBeNull();
            obterTodos.Count.ShouldBeEquivalentTo(0);
        }

        [Fact(DisplayName = "Relatório do Acompanhamento da Aprendizagem - Deve Registrar o percurso coletivo inserindo duas imagens")]
        public async Task Registrar_o_percurso_coletivo_inserindo_duas_imagens()
        {
            await CriarDadosBasicos();
            var useCase = SalvarAcompanhamentoTurmaUseCase();
            
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
            var useCase = SalvarAcompanhamentoTurmaUseCase();
            
            var dto = new AcompanhamentoTurmaDto
            {
                TurmaId = 1,
                Semestre = 1,
                ApanhadoGeral = $@"<html><body>
                                         teste
                                    <img src='http://www.localhost.com.br/imagem.png'>
                                    <img src='http://www.localhost.com.br/imagem.png'>
                                    <img src='http://www.localhost.com.br/imagem.png'>
                                  <body/><html/>"
            };
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
            ex.ShouldNotBeNull();
            
            var obterTodos = ObterTodos<AcompanhamentoTurma>();
            obterTodos.ShouldNotBeNull();
            obterTodos.Count.ShouldBeEquivalentTo(0);
        }
        
    }
}