using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAula.Base;
using SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
namespace SME.SGP.TesteIntegracao.Aula.PlanoAula
{
    public class Ao_carregar_objetivos_plano_aula : PlanoAulaTesteBase
    {
        public Ao_carregar_objetivos_plano_aula(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }


        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        }

        [Fact]
        public async Task Deve_obter_sem_repeticoes()
        {
            await DadosPlanoAula();

            var mediator = ServiceProvider.GetService<IMediator>();

            var objetivos = await mediator.Send(new ObterObjetivosPlanoDisciplinaQuery
                (1, 1, 1, 1, false));


            objetivos.ShouldNotBeNull();
            objetivos.Any(ob => ob.ObjetivosAprendizagem.Where(ob => ob.Id == 1).Count() == 1).ShouldBe(true);
        }

        private async Task DadosPlanoAula()
        {
            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                AnoLetivo = DateTime.Now.Year,
                Nome = "Calendário teste",
                Periodo = Periodo.Semestral,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                Semestre = null
            });
            await InserirNaBase(new PeriodoEscolar
            {
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = new DateTime(DateTime.Now.Year, 1,1),
                PeriodoFim = new DateTime(DateTime.Now.Year, 1, 31),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });

            await InserirNaBase(new PeriodoEscolar
            {
                TipoCalendarioId = 1,
                Bimestre = 2,
                PeriodoInicio = new DateTime(DateTime.Now.Year, 1, 1),
                PeriodoFim = new DateTime(DateTime.Now.Year, 1, 31),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });

            await InserirNaBase(new Dre()
            {
                Id = DRE_ID_1,
                Nome = "Dre Teste",
                CodigoDre = CODIGO_1,
                Abreviacao = "DT"
            });

            await InserirNaBase(new Ue()
            {
                Id = UE_ID_1,
                Nome = "Ue Teste",
                DreId = 1,
                TipoEscola = TipoEscola.EMEF,
                CodigoUe = CODIGO_1
            });
            await InserirNaBase(new Dominio.Turma()
            {
                Id = TURMA_ID_1,
                Nome = "1A",
                CodigoTurma = "1234",
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await InserirNaBase("componente_curricular_grupo_matriz", new string[] { "id", "nome" }, new string[] { "1", "'gm1'" });
            await InserirNaBase("componente_curricular_area_conhecimento", new string[] { "id", "nome" }, new string[] { "1", "'ac1'" });
            await InserirNaBase("componente_curricular", new[] { "id", "grupo_matriz_id", "area_conhecimento_id" }, new[] { "1", "1", "1" });


            await InserirNaBase(new PlanejamentoAnual()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PlanejamentoAnualPeriodoEscolar
            {
                Id = 1,
                PeriodoEscolarId = 1,
                PlanejamentoAnualId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PlanejamentoAnualComponente()
            {
                Id = 1,
                Descricao = "Planejamento Teste",
                PlanejamentoAnualPeriodoEscolarId = 1,
                ComponenteCurricularId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PlanejamentoAnualPeriodoEscolar
            {
                Id = 2,
                PeriodoEscolarId = 1,
                PlanejamentoAnualId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase("objetivo_aprendizagem", new string [] { "id", "descricao", "codigo", "ano_turma", "componente_curricular_id", "criado_em", "atualizado_em" }, new string [] { "1", "'Objetivo'", "1", "1", "1", "'2024-01-01'", "'2024-01-03'" });

            await InserirNaBase(new PlanejamentoAnualObjetivoAprendizagem
            {
                Id = 1,
                PlanejamentoAnualComponenteId = 1,
                ObjetivoAprendizagemId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

        }
    }
}
