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
using SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes;

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
        private const long USUARIO_CP_3333333_ID_3 = 3;
        private const long USUARIO_CP_999999_ID_4 = 4;
        private const long USUARIO_DIRETOR_999998_ID_5 = 5;
        private const long USUARIO_AD_999997_ID_6 = 6;
        
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
            await CriarTipoCalendario(filtroPlanoAula.TipoCalendario, semestre:SEMESTRE_1);
            
            await CriarItensComuns(filtroPlanoAula.CriarPeriodoEscolarBimestre, filtroPlanoAula.DataInicio, filtroPlanoAula.DataFim, filtroPlanoAula.Bimestre, filtroPlanoAula.TipoCalendarioId);
            
            CriarClaimUsuario(filtroPlanoAula.Perfil);
            
            await CriarUsuarios();
            
            await CriarTurma(filtroPlanoAula.Modalidade);
            
            await CriarAula(filtroPlanoAula.ComponenteCurricularCodigo, filtroPlanoAula.DataAula, RecorrenciaAula.AulaUnica, filtroPlanoAula.QuantidadeAula);

            if (filtroPlanoAula.CriarPeriodoEscolarTodosBimestres)
                await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);
            
            if (filtroPlanoAula.CriarPeriodoReabertura)
                await CriarPeriodoReabertura(TIPO_CALENDARIO_1);

            await CriarObjetivoAprendizagem(filtroPlanoAula.ComponenteCurricularCodigo);
            
            if(filtroPlanoAula.CriarPlanejamentoAnual)
                await CriarPlanejamentoAnual(filtroPlanoAula.ComponenteCurricularCodigo);
            
            await CriarAbrangencia(filtroPlanoAula.Perfil);
        }
        
        protected async Task CriarAbrangencia(string perfil)
        {
            await InserirNaBase(new Abrangencia()
            {
                DreId = DRE_ID_1,
                Historico = false,
                Perfil = new Guid(perfil),
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                UsuarioId = USUARIO_ID_1
            });
            
            await InserirNaBase(new Abrangencia()
            {
                DreId = DRE_ID_1,
                Historico = false,
                Perfil = Guid.Parse(PerfilUsuario.CP.Name()),
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                UsuarioId = USUARIO_CP_3333333_ID_3
            });
            
            await InserirNaBase(new Abrangencia()
            {
                DreId = DRE_ID_1,
                Historico = false,
                Perfil = Guid.Parse(PerfilUsuario.CP.Name()),
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                UsuarioId = USUARIO_CP_999999_ID_4
            });
            
            await InserirNaBase(new Abrangencia()
            {
                DreId = DRE_ID_1,
                Historico = false,
                Perfil = Guid.Parse(PerfilUsuario.DIRETOR.Name()),
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                UsuarioId = USUARIO_DIRETOR_999998_ID_5
            });
            
            await InserirNaBase(new Abrangencia()
            {
                DreId = DRE_ID_1,
                Historico = false,
                Perfil = Guid.Parse(PerfilUsuario.AD.Name()),
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                UsuarioId = USUARIO_AD_999997_ID_6
            });
        }

        private async Task CriarPlanejamentoAnual(string componenteCurricularCodigo)
        {
            await InserirNaBase(new PlanejamentoAnual()
            {
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = long.Parse(componenteCurricularCodigo),
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var periodosEscolares = ObterTodos<PeriodoEscolar>();

            foreach (var periodoEscolar in periodosEscolares)
            {
                await InserirNaBase(new PlanejamentoAnualPeriodoEscolar
                {
                    PeriodoEscolarId = periodoEscolar.Id,
                    PlanejamentoAnualId = 1,
                    CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });                
            }
        }
        
        protected async Task CriarPlanejamentoAnualTodosBimestres(string componenteCurricularCodigo)
        {
            await InserirNaBase(new PlanejamentoAnual()
            {
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = long.Parse(componenteCurricularCodigo),
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var periodosEscolares = ObterTodos<PeriodoEscolar>();

            foreach (var periodoEscolar in periodosEscolares)
            {
                await InserirNaBase(new PlanejamentoAnualPeriodoEscolar()
                {
                    PeriodoEscolarId = periodoEscolar.Id,
                    PlanejamentoAnualId = 1,
                    CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }
        
        protected async Task CriarPeriodoEscolarCustomizadoQuartoBimestre(bool periodoEscolarValido = false)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await CriarPeriodoEscolar(dataReferencia.AddDays(-285), dataReferencia.AddDays(-210), BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-200), dataReferencia.AddDays(-125), BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-115), dataReferencia.AddDays(-40), BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-20), periodoEscolarValido ? dataReferencia : dataReferencia.AddDays(-5), BIMESTRE_4, TIPO_CALENDARIO_1);
        }
        
        protected async Task CriarPeriodoAberturaCustomizadoQuartoBimestre(bool periodoEscolarValido = true)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new PeriodoFechamento()
                { CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                PeriodoFechamentoId = 1, 
                InicioDoFechamento = dataReferencia.AddDays(-209),
                FinalDoFechamento =  dataReferencia.AddDays(-205)
            });
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = 1, 
                InicioDoFechamento = dataReferencia.AddDays(-120),
                FinalDoFechamento =  dataReferencia.AddDays(-116)
            });
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_3,
                PeriodoFechamentoId = 1, 
                InicioDoFechamento = dataReferencia.AddDays(-38),
                FinalDoFechamento =  dataReferencia.AddDays(-34)
            });  
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_4,
                PeriodoFechamentoId = 1, 
                InicioDoFechamento = periodoEscolarValido ? dataReferencia : dataReferencia.AddDays(-5),
                FinalDoFechamento =  periodoEscolarValido ? dataReferencia.AddDays(4) : dataReferencia.AddDays(-2)
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
    }
}