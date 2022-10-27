using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class RelatorioAcompanhamentoAprendizagemTesteBase : TesteBaseComuns
    {
        private const string PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_TURMA_NOME = "QuantidadeImagensPercursoTurma";
        private const string PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_TURMA_DESCRICAO = "Quantidade de Imagens Permitidas na Seção Percurso Coletivo da Turma";
        private const string PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_TURMA_VALOR = "2";
        
        private const string PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_INDIVIDUAL_CRIANCA_NOME = "QuantidadeImagensPercursoIndividualCrianca";
        private const string PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_INDIVIDUAL_CRIANCA_DESCRICAO = "Quantidade de Imagens permitiras no percurso individual da criança";
        private const string PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_INDIVIDUAL_CRIANCA_VALOR = "3";

        protected const string TEXTO_PADRAO_APANHADO_GERAL = "<html><body>teste</body><html/>";
        protected const string TEXTO_PADRAO_PERCURSO_INDIVIDUAL = "<html><body>Texto padrão do percurso individual</body><html/>";

        public RelatorioAcompanhamentoAprendizagemTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakeOutras), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(SME.SGP.TesteIntegracao.Nota.ServicosFakes.ObterAlunosPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected async Task CriarDadosBasicos(bool abrirPeriodos = true)
        {
            await CriarDreUePerfil();
            await CriarComponenteCurricular();
            if(abrirPeriodos)
              await CriarPeriodoEscolarTodosBimestres();
            await CriarTipoCalendario(ModalidadeTipoCalendario.Infantil);
            CriarClaimUsuario(ObterPerfilProfessorInfantil());
            await CriarUsuarios();
            await CriarTurma(Modalidade.EducacaoInfantil);
            await CriarParametrosSistema();
        }

        protected ISalvarAcompanhamentoTurmaUseCase ObterSalvarAcompanhamentoUseCase()
        {
            return ServiceProvider.GetService<ISalvarAcompanhamentoTurmaUseCase>();
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
                FinalDoFechamento = dataReferencia.AddDays(-205)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-120),
                FinalDoFechamento = dataReferencia.AddDays(-116)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_3,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-38),
                FinalDoFechamento = dataReferencia.AddDays(-34)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_4,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = periodoEscolarValido ? dataReferencia : dataReferencia.AddDays(-5),
                FinalDoFechamento = periodoEscolarValido ? dataReferencia.AddDays(4) : dataReferencia.AddDays(-2)
            });
        }

        private async Task CriarPeriodoEscolarTodosBimestres()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);
        }

        private async Task CriarParametrosSistema()
        {
            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_TURMA_NOME,
                Tipo = TipoParametroSistema.QuantidadeImagensPercursoTurma,
                Descricao = PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_TURMA_DESCRICAO,
                Valor = PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_TURMA_VALOR,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });
            
            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_INDIVIDUAL_CRIANCA_NOME,
                Tipo = TipoParametroSistema.QuantidadeImagensPercursoIndividualCrianca,
                Descricao = PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_INDIVIDUAL_CRIANCA_DESCRICAO,
                Valor = PARAMETRO_QUANTIDADE_IMAGENS_PERCURSO_INDIVIDUAL_CRIANCA_VALOR,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });
            
            await InserirNaBase(new ParametrosSistema
            {
                Nome = DATA_INICIO_SGP,
                Tipo = TipoParametroSistema.DataInicioSGP,
                Descricao = DATA_INICIO_SGP,
                Valor = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });            
        }  
        
        protected ISalvarAcompanhamentoAlunoUseCase ObterServicoSalvarAcompanhamentoAlunoUseCase()
        {
            return ServiceProvider.GetService<ISalvarAcompanhamentoAlunoUseCase>();
        }
        
        protected async Task CriarPeriodoEscolarCustomizadoQuartoBimestre(bool periodoEscolarValido = false)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await CriarPeriodoEscolar(dataReferencia.AddDays(-285), dataReferencia.AddDays(-210), BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-200), dataReferencia.AddDays(-125), BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-115), dataReferencia.AddDays(-40), BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-20), periodoEscolarValido ? dataReferencia : dataReferencia.AddDays(-5), BIMESTRE_4, TIPO_CALENDARIO_1);
        }
        
        protected async Task CriarPeriodoEscolarCustomizadoSegundoBimestre(bool periodoValido = false)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await CriarPeriodoEscolar(dataReferencia.AddDays(-115), dataReferencia.AddDays(-40), BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-35), periodoValido ? dataReferencia : dataReferencia.AddDays(-5), BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(10), dataReferencia.AddDays(85), BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(95), dataReferencia.AddDays(170), BIMESTRE_4, TIPO_CALENDARIO_1);
        }

        protected async Task CriarPeriodoAberturaCustomizadoQuartoBimestre(bool periodoValido = true)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new PeriodoFechamento()
            { CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-209),
                FinalDoFechamento = dataReferencia.AddDays(-205)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-120),
                FinalDoFechamento = dataReferencia.AddDays(-116)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-120),
                FinalDoFechamento = dataReferencia.AddDays(-116)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_3,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-38),
                FinalDoFechamento = dataReferencia.AddDays(-34)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_4,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = periodoValido ? dataReferencia : dataReferencia.AddDays(-5),
                FinalDoFechamento = periodoValido ? dataReferencia.AddDays(4) : dataReferencia.AddDays(-2)
            });
        }
    }
}