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

namespace SME.SGP.TesteIntegracao.PlanoDeAula.Base
{
    public abstract class PlanoAulaTesteBase : TesteBaseComuns
    {
        private const int QUANTIDADE_3 = 3;
        protected const long AULA_ID_1 = 1;
        protected const int NUMERO_AULAS_1 = 1;
        protected const int NUMERO_AULAS_2 = 2;
        protected const int NUMERO_AULAS_3 = 3;

        protected const int QTDE_1 = 1;
        protected const int QTDE_2 = 2;
        protected const int QTDE_3 = 3;

        protected readonly DateTime DATA_01_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 01);
        protected readonly DateTime DATA_02_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 02);

        protected PlanoAulaTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionarioCoreSSOPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
        }

        #region Execução dos UseCases

        protected async Task<PlanoAulaDto> ExecutarSalvarPlanoDeAulaUseCase(PlanoAulaDto planoAulaDto)
        {
            var useCase = ServiceProvider.GetService<ISalvarPlanoAulaUseCase>();

            return await useCase.Executar(planoAulaDto);
        }

        protected async Task<PlanoAulaRetornoDto> ExecutarObterPlanoDeAulaUseCase(long aulaId, long turmaId, long? componenteCurricularId)
        {
            var useCase = ServiceProvider.GetService<IObterPlanoAulaUseCase>();

            return await useCase.Executar(new FiltroObterPlanoAulaDto(aulaId, turmaId, componenteCurricularId));
        }

        protected async Task<IEnumerable<PlanoAulaRetornoDto>> ExecutarObterPlanoDeAulaPorTurmaComponentePeriodoUseCase(string turmaCodigo, string componenteCurricularCodigo, string componenteCurricularId, [FromQuery] DateTime aulaInicio, [FromQuery] DateTime aulaFim)
        {
            var useCase = ServiceProvider.GetService<IObterPlanoAulasPorTurmaEComponentePeriodoUseCase>();

            return await useCase.Executar(new FiltroObterPlanoAulaPeriodoDto(turmaCodigo, componenteCurricularCodigo, componenteCurricularId, aulaInicio, aulaFim));
        }

        protected async Task<bool> ExecutarMigrarPlanoDeAulaUseCase(MigrarPlanoAulaDto migrarPlanoAulaDto)
        {
            var useCase = ServiceProvider.GetService<IMigrarPlanoAulaUseCase>();

            return await useCase.Executar(migrarPlanoAulaDto);
        }

        protected IEnumerable<PlanoAulaExistenteRetornoDto> ExecutarValidarPlanoDeAulaExistenteUseCase(FiltroPlanoAulaExistenteDto filtroPlanoAulaExistenteDto)
        {
            var consulta = ServiceProvider.GetService<IConsultasPlanoAula>();

            return consulta.ValidarPlanoAulaExistente(filtroPlanoAulaExistenteDto);
        }

        #endregion Execução dos UseCases

        protected async Task CriarDadosBasicos(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, DateTime dataInicio, DateTime dataFim, int bimestre, DateTime dataAula, string componenteCurricular, bool criarPeriodo = true, long tipoCalendarioId = 1, bool criarPeriodoEscolarEAbertura = true, int quantidadeAula = QUANTIDADE_3)
        {
            await CriarDadosBase(perfil, modalidade, tipoCalendario, dataInicio, dataFim, bimestre, tipoCalendarioId, criarPeriodo);
            await CriarTurma(modalidade);
            await CriarAula(componenteCurricular, dataAula, RecorrenciaAula.AulaUnica, quantidadeAula);
            if (criarPeriodoEscolarEAbertura)
                await CriarPeriodoEscolarEAbertura();
        }

        protected async Task CriarDadosBase(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1, bool criarPeriodo = true)
        {
            await CriarTipoCalendario(tipoCalendario);
            await CriarItensComuns(criarPeriodo, dataInicio, dataFim, bimestre, tipoCalendarioId);
            CriarClaimUsuario(perfil);
            await CriarUsuarios();
        }

        protected async Task CriarAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula = QUANTIDADE_3, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            await InserirNaBase(ObterAula(componenteCurricularCodigo, dataAula, recorrencia, quantidadeAula, rf));
        }

        protected async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
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