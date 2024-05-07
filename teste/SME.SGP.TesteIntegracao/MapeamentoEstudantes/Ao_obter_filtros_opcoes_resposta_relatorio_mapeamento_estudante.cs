using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{

    public class Ao_obter_filtros_opcoes_resposta_relatorio_mapeamento_estudante : MapeamentoBase
    {
        public Ao_obter_filtros_opcoes_resposta_relatorio_mapeamento_estudante(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override async Task CriarDadosBase()
        {
            await base.CriarDadosBase();
            CarregarDadosBase();
        }

        [Fact(DisplayName = "Relatório Mapeamento de estudante - Obter opções de respostas para filtro de relatório mapeamento estudante")]
        public async Task Ao_obter_opcoes_resposta()
        {
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<IObterFiltrosOpcoesRespostaRelatorioMapeamentoUseCase>();
            var retorno = await useCase.Executar();
            retorno.OpcoesRespostaAcompanhadoNAAPA.Any(op => op.Nome.Equals("Sim")).ShouldBeTrue();
            retorno.OpcoesRespostaAcompanhadoNAAPA.Any(op => op.Nome.Equals("Não")).ShouldBeTrue();

            retorno.OpcoesRespostaDistorcaoIdadeAnoSerie.Any(op => op.Nome.Equals("Sim")).ShouldBeTrue();
            retorno.OpcoesRespostaDistorcaoIdadeAnoSerie.Any(op => op.Nome.Equals("Não")).ShouldBeTrue();

            retorno.OpcoesRespostaPossuiPlanoAEE.Any(op => op.Nome.Equals("Sim")).ShouldBeTrue();
            retorno.OpcoesRespostaPossuiPlanoAEE.Any(op => op.Nome.Equals("Não")).ShouldBeTrue();

            retorno.OpcoesRespostaProgramaSPIntegral.Any(op => op.Nome.Equals("Sim")).ShouldBeTrue();
            retorno.OpcoesRespostaProgramaSPIntegral.Any(op => op.Nome.Equals("Não")).ShouldBeTrue();

            retorno.OpcoesRespostaFrequencia.Any(op => op.Nome.Equals("Frequente")).ShouldBeTrue();
            retorno.OpcoesRespostaFrequencia.Any(op => op.Nome.Equals("Não Frequente")).ShouldBeTrue();

            retorno.OpcoesRespostaHipoteseEscritaEstudante.Any(op => op.Equals("Pré-Silábico")).ShouldBeTrue();
            retorno.OpcoesRespostaHipoteseEscritaEstudante.Any(op => op.Equals("Silábico sem valor")).ShouldBeTrue();
            retorno.OpcoesRespostaHipoteseEscritaEstudante.Any(op => op.Equals("Silábico com valor")).ShouldBeTrue();
            retorno.OpcoesRespostaHipoteseEscritaEstudante.Any(op => op.Equals("Silábico alfabético")).ShouldBeTrue();
            retorno.OpcoesRespostaHipoteseEscritaEstudante.Any(op => op.Equals("Alfabético")).ShouldBeTrue();
            
            retorno.OpcoesRespostaAvaliacoesExternasProvaSP.Any(op => op.Equals("Abaixo do básico")).ShouldBeTrue();
            retorno.OpcoesRespostaAvaliacoesExternasProvaSP.Any(op => op.Equals("Básico")).ShouldBeTrue();
            retorno.OpcoesRespostaAvaliacoesExternasProvaSP.Any(op => op.Equals("Adequado")).ShouldBeTrue();
            retorno.OpcoesRespostaAvaliacoesExternasProvaSP.Any(op => op.Equals("Avançado")).ShouldBeTrue();
        }
    }
}
