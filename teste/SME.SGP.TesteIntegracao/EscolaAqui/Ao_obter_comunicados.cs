using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EscolaAqui
{
    public class Ao_obter_comunicados : TesteBaseComuns
    {
        
        public Ao_obter_comunicados(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);     
        }

        [Fact(DisplayName = "Retornar comunicados do ano atual")]
        public async Task Deve_retornar_comunicados_ano_atual()
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarItensComuns(false, DateTime.Now.Date, DateTime.Now.Date, 1);
            await CriarTurma(Modalidade.Fundamental);
            await CriarComunicadoAluno("Comunicado 2023 1", DateTime.Now.Year, DRE_CODIGO_1, UE_CODIGO_1, TURMA_CODIGO_1, CODIGO_ALUNO_1, 1);
            await CriarComunicadoAluno("Comunicado 2023 2", DateTime.Now.Year, DRE_CODIGO_2, UE_CODIGO_2, TURMA_CODIGO_2, CODIGO_ALUNO_2, 2);
            await CriarComunicadoAluno("Comunicado 2022 1", DateTime.Now.Year-1, DRE_CODIGO_1, UE_CODIGO_1, TURMA_CODIGO_1, CODIGO_ALUNO_1, 3);

            var useCase = ServiceProvider.GetService<IObterComunicadosAnoAtualUseCase>();

            var retorno = await useCase.Executar();
            retorno.ShouldNotBeEmpty();
            retorno.Count().ShouldBe(2);
            retorno.Where(e => e.CodigoDre == DRE_CODIGO_1 && e.CodigoUe == UE_CODIGO_1 && e.TurmaCodigo == TURMA_CODIGO_1 && e.AlunoCodigo == CODIGO_ALUNO_1).Count().ShouldBe(1);
            retorno.Where(e => e.CodigoDre == DRE_CODIGO_2 && e.CodigoUe == UE_CODIGO_2 && e.TurmaCodigo == TURMA_CODIGO_2 && e.AlunoCodigo == CODIGO_ALUNO_2).Count().ShouldBe(1);
        }
    }
}