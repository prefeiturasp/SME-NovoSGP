using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAula.Base
{
    public abstract class PlanoAulaTesteBase : TesteBaseComuns
    {
        private const int QUANTIDADE_3 = 3;
        protected const long AULA_ID_1 = 1;
        private const string OBJETIVO_APRENDIZAGEM_DESCRICAO_1 = "'OBJETIVO APRENDIZAGEM 1'";
        private const string OBJETIVO_APRENDIZAGEM_CODIGO_1 = "'CDGAPRE1'";
        private const string OBJETIVO_APRENDIZAGEM_DESCRICAO_2 = "'OBJETIVO APRENDIZAGEM 2'";
        private const string OBJETIVO_APRENDIZAGEM_CODIGO_2 = "'CDGAPRE2'";
        private const string OBJETIVO_APRENDIZAGEM_DESCRICAO_3 = "'OBJETIVO APRENDIZAGEM 3'";
        private const string OBJETIVO_APRENDIZAGEM_CODIGO_3 = "'CDGAPRE3'";
        private const string OBJETIVO_APRENDIZAGEM_DESCRICAO_4 = "'OBJETIVO APRENDIZAGEM 4'";
        private const string OBJETIVO_APRENDIZAGEM_CODIGO_4 = "'CDGAPRE4'";
        private const string OBJETIVO_APRENDIZAGEM_DESCRICAO_5 = "'OBJETIVO APRENDIZAGEM 5'";
        private const string OBJETIVO_APRENDIZAGEM_CODIGO_5 = "'CDGAPRE5'";
        private const string OBJETIVO_APRENDIZAGEM_TABELA = "objetivo_aprendizagem";
        
        protected PlanoAulaTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionarioCoreSSOPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected ISalvarPlanoAulaUseCase ObterServicoISalvarPlanoAulaUseCase()
        {
            return ServiceProvider.GetService<ISalvarPlanoAulaUseCase>();
        }

        protected ISalvarPlanoAulaUseCase ObterServicoSalvarPlanoAulaUseCase()
        {
            return ServiceProvider.GetService<ISalvarPlanoAulaUseCase>();
        }
        protected IObterPlanoAulaUseCase ObterServicoObterPlanoAulaUseCase()
        {
            return ServiceProvider.GetService<IObterPlanoAulaUseCase>();
        }

        protected IObterPlanoAulasPorTurmaEComponentePeriodoUseCase ObterServicoObterPlanoAulasPorTurmaEComponentePeriodoUseCase()
        {
            return ServiceProvider.GetService<IObterPlanoAulasPorTurmaEComponentePeriodoUseCase>();
        }

        protected IMigrarPlanoAulaUseCase ObterServicoMigrarPlanoAulaUseCase()
        {
            return ServiceProvider.GetService<IMigrarPlanoAulaUseCase>();
        }

        protected IConsultasPlanoAula ObterServicoIConsultasPlanoAula()
        {
            return ServiceProvider.GetService<IConsultasPlanoAula>();
        }

        protected async Task CriarDadosBasicos(FiltroPlanoAula filtroPlanoAula)
        {
            await CriarTipoCalendario(filtroPlanoAula.TipoCalendario);
            
            await CriarItensComuns(filtroPlanoAula.CriarPeriodoEscolarBimestre, filtroPlanoAula.DataInicio, filtroPlanoAula.DataFim, filtroPlanoAula.Bimestre, filtroPlanoAula.TipoCalendarioId);
            
            CriarClaimUsuario(filtroPlanoAula.Perfil);
            
            await CriarUsuarios();
            
            await CriarTurma(filtroPlanoAula.Modalidade);
            
            await CriarAula(filtroPlanoAula.ComponenteCurricularCodigo, filtroPlanoAula.DataAula, RecorrenciaAula.AulaUnica, filtroPlanoAula.QuantidadeAula);
            
            if (filtroPlanoAula.CriarPeriodoEscolarTodosBimestres)
                await CriarPeriodoEscolarTodosBimestres();
            
            if (filtroPlanoAula.CriarPeriodoAbertura)
                await CriarPeriodoReabertura(TIPO_CALENDARIO_1);

            await CriarObjetivoAprendizagem(filtroPlanoAula.ComponenteCurricularCodigo);
            
            if(filtroPlanoAula.CriarPlanejamentoAnual)
                await CriarPlanejamentoAnual(filtroPlanoAula.ComponenteCurricularCodigo);
        }

        private async Task CriarPlanejamentoAnual(string componenteCurricularCodigo)
        {
            await InserirNaBase(new PlanejamentoAnual()
            {
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = long.Parse(componenteCurricularCodigo),
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new PlanejamentoAnualPeriodoEscolar()
            {
                PeriodoEscolarId = 2,
                PlanejamentoAnualId = 1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarObjetivoAprendizagem(string componenteCurricularCodigo)
        {
            await InserirNaBase(OBJETIVO_APRENDIZAGEM_TABELA, CODIGO_1,OBJETIVO_APRENDIZAGEM_DESCRICAO_1,OBJETIVO_APRENDIZAGEM_CODIGO_1,"'first'",componenteCurricularCodigo,FALSE,"'2022-09-12'","'2022-09-12'");
            await InserirNaBase(OBJETIVO_APRENDIZAGEM_TABELA, CODIGO_2,OBJETIVO_APRENDIZAGEM_DESCRICAO_2,OBJETIVO_APRENDIZAGEM_CODIGO_2,"'first'",componenteCurricularCodigo,FALSE,"'2022-09-12'","'2022-09-12'");
            await InserirNaBase(OBJETIVO_APRENDIZAGEM_TABELA, CODIGO_3,OBJETIVO_APRENDIZAGEM_DESCRICAO_3,OBJETIVO_APRENDIZAGEM_CODIGO_3,"'first'",componenteCurricularCodigo,FALSE,"'2022-09-12'","'2022-09-12'");
            await InserirNaBase(OBJETIVO_APRENDIZAGEM_TABELA, CODIGO_4,OBJETIVO_APRENDIZAGEM_DESCRICAO_4,OBJETIVO_APRENDIZAGEM_CODIGO_4,"'first'",componenteCurricularCodigo,FALSE,"'2022-09-12'","'2022-09-12'");
            await InserirNaBase(OBJETIVO_APRENDIZAGEM_TABELA, CODIGO_5,OBJETIVO_APRENDIZAGEM_DESCRICAO_5,OBJETIVO_APRENDIZAGEM_CODIGO_5,"'first'",componenteCurricularCodigo,FALSE,"'2022-09-12'","'2022-09-12'");
        }

        protected async Task CriarAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula = QUANTIDADE_3, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            await InserirNaBase(ObterAula(componenteCurricularCodigo, dataAula, recorrencia, quantidadeAula, rf));
        }

        protected async Task CriarPeriodoEscolarTodosBimestres()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);
        }

        private Dominio.Aula ObterAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            return new Dominio.Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = componenteCurricularCodigo,
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = rf,
                Quantidade = quantidadeAula,
                DataAula = dataAula,
                RecorrenciaAula = recorrencia,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            };
        }

        protected async Task CriarPeriodoEscolarEAberturaPadrao()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}