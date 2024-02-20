using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nest;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Contexto;

namespace SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.Base
{
    public class ConsultaCriancaEstudantesAusentesTestFixture : BaseFixture
    {
        public int ANO_LETIVO_ANO_ATUAL = DateTimeExtension.HorarioBrasilia().Year;
        public int ANO_LETIVO_ANO_ANTERIOR = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year;
        public DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        public DateTime DATA_08_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public ConsultaCriancaEstudantesAusentesTestFixture()
        {
            Task.Run(() => CriarDadosBasicos()).Wait();
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ServicosFake.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaFreqGeralQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>), typeof(ObterDisciplinasPorCodigoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasPorCodigosQuery, IEnumerable<Dominio.Turma>>), typeof(ObterTurmasPorCodigosQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoFrequenciaGlobalQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ServicosFake.ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected async Task CriarDadosBasicos()
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarItensComuns(true, DATA_02_05, DATA_08_07, Base.Constantes.BIMESTRE_2, Base.Constantes.TIPO_CALENDARIO_1);
            CriarClaimUsuario(ObterPerfilCoordenadorNAAPA());
            await CriarUsuarios();
            await CriarTurma(Modalidade.Medio);
        }

        protected async Task CriarTurma(Modalidade modalidade, bool turmaHistorica = false, bool turmasMesmaUe = false)
        {
            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = Base.Constantes.TURMA_ANO_2,
                CodigoTurma = Base.Constantes.TURMA_CODIGO_1,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = Base.Constantes.SEMESTRE_1,
                Nome = Base.Constantes.TURMA_NOME_1,
                TipoTurma = TipoTurma.Regular
            });
        }

        protected async Task CriarUsuarios()
        {
            await InserirNaBase(new Usuario
            {
                Login = Base.Constantes.USUARIO_PROFESSOR_LOGIN_2222222,
                CodigoRf = Base.Constantes.USUARIO_PROFESSOR_CODIGO_RF_2222222,
                Nome = Base.Constantes.USUARIO_PROFESSOR_NOME_2222222,
                CriadoPor = Base.Constantes.SISTEMA_NOME,
                CriadoRF = Base.Constantes.SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Usuario
            {
                Login = Base.Constantes.USUARIO_PROFESSOR_LOGIN_1111111,
                CodigoRf = Base.Constantes.USUARIO_PROFESSOR_CODIGO_RF_1111111,
                Nome = Base.Constantes.USUARIO_PROFESSOR_NOME_1111111,
                CriadoPor = Base.Constantes.SISTEMA_NOME,
                CriadoRF = Base.Constantes.SISTEMA_CODIGO_RF
            });
        }

        protected string ObterPerfilCoordenadorNAAPA()
        {
            return Guid.Parse(PerfilUsuario.COORDENADOR_NAAPA.Name()).ToString();
        }

        protected void CriarClaimUsuario(string perfil)
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();

            contextoAplicacao.AdicionarVariaveis(ObterVariaveisPorPerfil(perfil));
        }

        private Dictionary<string, object> ObterVariaveisPorPerfil(string perfil)
        {
            var rfLoginPerfil = ObterRfLoginPerfil(perfil);

            return new Dictionary<string, object>
            {
                { Base.Constantes.USUARIO_CHAVE, rfLoginPerfil },
                { Base.Constantes.USUARIO_LOGADO_CHAVE, rfLoginPerfil },
                { Base.Constantes.USUARIO_RF_CHAVE, rfLoginPerfil },
                { Base.Constantes.USUARIO_LOGIN_CHAVE, rfLoginPerfil },
                { Base.Constantes.NUMERO_PAGINA, "0" },
                { Base.Constantes.NUMERO_REGISTROS, "10" },
                { Base.Constantes.ADMINISTRADOR, rfLoginPerfil },
                { Base.Constantes.NOME_ADMINISTRADOR, rfLoginPerfil },
                {
                    Base.Constantes.USUARIO_CLAIMS_CHAVE,
                    new List<InternalClaim> {
                        new InternalClaim { Value = rfLoginPerfil, Type = Base.Constantes.USUARIO_CLAIM_TIPO_RF },
                        new InternalClaim { Value = perfil, Type = Base.Constantes.USUARIO_CLAIM_TIPO_PERFIL }
                    }
                }
            };
        }

        private string ObterRfLoginPerfil(string perfil)
        {
            return Base.Constantes.USUARIO_PROFESSOR_LOGIN_2222222;
        }

        protected async Task CriarTipoCalendario(ModalidadeTipoCalendario tipoCalendario, bool considerarAnoAnterior = false, int semestre = Base.Constantes.SEMESTRE_1)
        {
            await InserirNaBase(new TipoCalendario
            {
                AnoLetivo = considerarAnoAnterior ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Nome = considerarAnoAnterior ? Base.Constantes.NOME_TIPO_CALENDARIO_ANO_ANTERIOR : Base.Constantes.NOME_TIPO_CALENDARIO_ANO_ATUAL,
                Periodo = Periodo.Semestral,
                Modalidade = tipoCalendario,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = Base.Constantes.SISTEMA_NOME,
                CriadoRF = Base.Constantes.SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                Semestre = tipoCalendario.EhEjaOuCelp() ? semestre : null
            });
        }

        protected async Task CriarItensComuns(bool criarPeriodo, DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1)
        {
            await CriarDreUePerfil();
            if (criarPeriodo) await CriarPeriodoEscolar(dataInicio, dataFim, bimestre, tipoCalendarioId);
           // await CriarComponenteCurricular();
        }

        protected async Task CriarPeriodoEscolar(DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1, bool considerarAnoAnterior = false)
        {
            await InserirNaBase(new PeriodoEscolar
            {
                TipoCalendarioId = tipoCalendarioId,
                Bimestre = bimestre,
                PeriodoInicio = considerarAnoAnterior ? dataInicio.AddYears(-1) : dataInicio,
                PeriodoFim = considerarAnoAnterior ? dataFim.AddYears(-1) : dataFim,
                CriadoPor = Base.Constantes.SISTEMA_NOME,
                CriadoRF = Base.Constantes.SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarComponenteCurricular()
        {
            await InserirNaBase(Base.Constantes.COMPONENTE_CURRICULAR_GRUPO_MATRIZ, Base.Constantes.CODIGO_1, Base.Constantes.GRUPO_MATRIZ_1);
            await InserirNaBase(Base.Constantes.COMPONENTE_CURRICULAR_GRUPO_MATRIZ, Base.Constantes.CODIGO_2, Base.Constantes.GRUPO_MATRIZ_2);
            await InserirNaBase(Base.Constantes.COMPONENTE_CURRICULAR_GRUPO_MATRIZ, Base.Constantes.CODIGO_3, Base.Constantes.GRUPO_MATRIZ_3);
            await InserirNaBase(Base.Constantes.COMPONENTE_CURRICULAR_GRUPO_MATRIZ, Base.Constantes.CODIGO_4, Base.Constantes.GRUPO_MATRIZ_4);
            await InserirNaBase(Base.Constantes.COMPONENTE_CURRICULAR_GRUPO_MATRIZ, Base.Constantes.CODIGO_8, Base.Constantes.GRUPO_MATRIZ_8);


            await InserirNaBase(Base.Constantes.COMPONENTE_CURRICULAR, Base.Constantes.COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), Base.Constantes.NULO, Base.Constantes.CODIGO_1, Base.Constantes.CODIGO_1, Base.Constantes.COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME, Base.Constantes.FALSE, Base.Constantes.FALSE, Base.Constantes.FALSE, Base.Constantes.TRUE, Base.Constantes.TRUE, Base.Constantes.TRUE, Base.Constantes.COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME, Base.Constantes.NULO);
            await InserirNaBase(Base.Constantes.COMPONENTE_CURRICULAR, Base.Constantes.COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), Base.Constantes.NULO, Base.Constantes.CODIGO_1, Base.Constantes.CODIGO_1, Base.Constantes.COMPONENTE_CURRICULAR_ARTES_NOME, Base.Constantes.FALSE, Base.Constantes.FALSE, Base.Constantes.TRUE,  Base.Constantes.FALSE, Base.Constantes.TRUE, Base.Constantes.TRUE, Base.Constantes.TRUE, Base.Constantes.COMPONENTE_CURRICULAR_ARTES_NOME, Base.Constantes.NULO);
            await InserirNaBase(Base.Constantes.COMPONENTE_CURRICULAR, Base.Constantes.COMPONENTE_MATEMATICA_ID_2.ToString(), Base.Constantes.NULO, Base.Constantes.CODIGO_1, Base.Constantes.CODIGO_2, Base.Constantes.COMPONENTE_CURRICULAR_MATEMATICA_NOME, Base.Constantes.FALSE, Base.Constantes.FALSE, Base.Constantes.FALSE, Base.Constantes.TRUE, Base.Constantes.TRUE, Base.Constantes.TRUE, Base.Constantes.COMPONENTE_CURRICULAR_MATEMATICA_NOME, Base.Constantes.NULO);
            await InserirNaBase(Base.Constantes.COMPONENTE_CURRICULAR, Base.Constantes.COMPONENTE_GEOGRAFIA_ID_8.ToString(), Base.Constantes.NULO, Base.Constantes.CODIGO_1, Base.Constantes.CODIGO_1, Base.Constantes.COMPONENTE_GEOGRAFIA_NOME, Base.Constantes.FALSE, Base.Constantes.FALSE, Base.Constantes.FALSE, Base.Constantes.TRUE, Base.Constantes.TRUE, Base.Constantes.TRUE, Base.Constantes.COMPONENTE_GEOGRAFIA_NOME, Base.Constantes.NULO);

        }

        public Task<long> CriarAula(DateTime dataAula, RecorrenciaAula recorrenciaAula, TipoAula tipoAula, string professorRf, string turmaCodigo, string ueCodigo, string disciplinaCodigo, long tipoCalendarioId, bool aulaCJ = false)
        {
            return SalvarNaBase(new Dominio.Aula()
            {
                UeId = ueCodigo,
                DisciplinaId = disciplinaCodigo,
                TurmaId = turmaCodigo,
                TipoCalendarioId = tipoCalendarioId,
                ProfessorRf = professorRf,
                Quantidade = 1,
                DataAula = dataAula.Date,
                RecorrenciaAula = recorrenciaAula,
                TipoAula = tipoAula,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = Base.Constantes.SISTEMA_NOME,
                CriadoRF = Base.Constantes.SISTEMA_CODIGO_RF,
                Excluido = false,
                AulaCJ = aulaCJ
            });
        }

        protected async Task CriarDreUePerfil()
        {
            await InserirNaBase(new Dre
            {
                CodigoDre = Base.Constantes.DRE_CODIGO_1,
                Abreviacao = Base.Constantes.DRE_NOME_1,
                Nome = Base.Constantes.DRE_NOME_1
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = Base.Constantes.UE_CODIGO_1,
                DreId = 1,
                Nome = Base.Constantes.UE_NOME_1,
                TipoEscola = TipoEscola.EMEF
            });

            await InserirNaBase("tipo_escola", new string[] { "cod_tipo_escola_eol", "descricao", "criado_em", "criado_por", "criado_rf" }, new string[] { "1", "'EMEF'", "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'", "'" + Base.Constantes.SISTEMA_NOME + "'", "'" + Base.Constantes.SISTEMA_CODIGO_RF + "'" });
        }
    }

    [CollectionDefinition(nameof(ConsultaCriancaEstudantesAusentesTestFixture))]
    public class ConsultaCriancaEstudantesAusentesCollection
    : ICollectionFixture<ConsultaCriancaEstudantesAusentesTestFixture>
    { }
}
