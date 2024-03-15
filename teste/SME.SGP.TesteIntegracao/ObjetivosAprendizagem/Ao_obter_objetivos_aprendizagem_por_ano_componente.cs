using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao;
using SME.SGP.TesteIntegracao.Informe.ServicosFake;
using SME.SGP.TesteIntegracao.RelatorioPAP;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ObjetivosAprendizagem
{
    public class Ao_obter_objetivos_aprendizagem_por_ano_componente : TesteBaseComuns
    {
        private const long ID_JUREMA_COMPONENTE_CURRICULAR_ARTE = 1;
        public Ao_obter_objetivos_aprendizagem_por_ano_componente(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterJuremaIdsPorComponentesCurricularIdQuery, long[]>), typeof(ObterJuremaIdsPorComponentesCurricularIdQueryHandlerFake), ServiceLifetime.Scoped));
        }


        [Fact(DisplayName = "Deve obter objetivos de aprendizagem por ano e componente curricular EF")]
        public async Task Deve_obter_objetivos_de_aprendizagem_por_ano_e_componente_curricular_EF()
        {
            await CriarDreUePerfilComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarTurma(Modalidade.Fundamental, "5", TURMA_CODIGO_1, TipoTurma.Regular);
            await CriarObjetivosAprendizagem();

            var useCase = ServiceProvider.GetService<IListarObjetivoAprendizagemPorAnoTurmaEComponenteCurricularUseCase>();
            var retorno = await useCase.Executar(ANO_5, COMPONENTE_CURRICULAR_ARTES_ID_139, false, TURMA_ID_1);
            retorno.Count().ShouldBe(2);
            retorno.Any(r => r.Codigo.Equals("(EF05A01)")).ShouldBeTrue();
            retorno.Any(r => r.Codigo.Equals("(EF05A02)")).ShouldBeTrue();
        }

        [Fact(DisplayName = "Deve obter objetivos de aprendizagem por ano e componente curricular EJA")]
        public async Task Deve_obter_objetivos_de_aprendizagem_por_ano_e_componente_curricular_EJA()
        {
            await CriarDreUePerfilComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarTurma(Modalidade.EJA, "3", TURMA_CODIGO_1, TipoTurma.Regular);
            await CriarObjetivosAprendizagem();

            var useCase = ServiceProvider.GetService<IListarObjetivoAprendizagemPorAnoTurmaEComponenteCurricularUseCase>();
            var retorno = await useCase.Executar(ANO_3, COMPONENTE_CURRICULAR_ARTES_ID_139, false, TURMA_ID_1);
            retorno.Count().ShouldBe(2);
            retorno.Any(r => r.Codigo.Equals("(EFEJAECA02)")).ShouldBeTrue();
            retorno.Any(r => r.Codigo.Equals("(EFEJAECA03)")).ShouldBeTrue();
        }

        private async Task CriarObjetivosAprendizagem()
        {
            await InserirNaBase("objetivo_aprendizagem", new string[] { "id", "descricao", "codigo", "ano_turma", "componente_curricular_id", "criado_em", "atualizado_em" }, 
                                new string[] { "1", "'Descricao 01 EF 5º ano'", "'(EF05A01)'", "'fifth'", ID_JUREMA_COMPONENTE_CURRICULAR_ARTE.ToString(), "'2024-01-01'", "'2024-01-03'" });
            await InserirNaBase("objetivo_aprendizagem", new string[] { "id", "descricao", "codigo", "ano_turma", "componente_curricular_id", "criado_em", "atualizado_em" },
                                new string[] { "2", "'Descricao 02 EF 5º ano'", "'(EF05A02)'", "'fifth'", ID_JUREMA_COMPONENTE_CURRICULAR_ARTE.ToString(), "'2024-01-01'", "'2024-01-03'" });
            await InserirNaBase("objetivo_aprendizagem", new string[] { "id", "descricao", "codigo", "ano_turma", "componente_curricular_id", "criado_em", "atualizado_em" },
                                new string[] { "3", "'Descricao 02 EJA 3º ano'", "'(EFEJAECA02)'", "'twelfth'", ID_JUREMA_COMPONENTE_CURRICULAR_ARTE.ToString(), "'2024-01-01'", "'2024-01-03'" });
            await InserirNaBase("objetivo_aprendizagem", new string[] { "id", "descricao", "codigo", "ano_turma", "componente_curricular_id", "criado_em", "atualizado_em" },
                                new string[] { "4", "'Descricao 03 EJA 3º ano'", "'(EFEJAECA03)'", "'twelfth'", ID_JUREMA_COMPONENTE_CURRICULAR_ARTE.ToString(), "'2024-01-01'", "'2024-01-03'" });
        }
    }
}