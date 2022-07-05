using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Nota
{
    public abstract class NotaBase : TesteBaseComuns
    {
        protected const long AULA_ID_1 = 1;

        protected const string TIPO_FREQUENCIA_COMPARECEU = "C";
        protected const string TIPO_FREQUENCIA_FALTOU = "F";
        protected const string TIPO_FREQUENCIA_REMOTO = "R";

        protected const string CODIGO_ALUNO_99999 = "99999";

        private const string REABERTURA_GERAL = "Reabrir Geral";

        private readonly DateTime DATA_01_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);
        private readonly DateTime DATA_31_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 31);

        protected NotaBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionarioCoreSSOPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
        }
        protected async Task<NotasConceitosRetornoDto> ExecutarNotasConceito(ListaNotasConceitosDto consultaListaNotasConceitosDto, NotaConceitoListaDto notaConceitoLista)
        {
            var comandosNotasConceitos = ServiceProvider.GetService<IComandosNotasConceitos>();

            var obterNotasParaAvaliacoesUseCase = ServiceProvider.GetService<IObterNotasParaAvaliacoesUseCase>();

            await comandosNotasConceitos.Salvar(notaConceitoLista);

            return await obterNotasParaAvaliacoesUseCase.Executar(consultaListaNotasConceitosDto);
        }

        protected async Task CriarDadosBase(ValoresDadosBasicosDto dto)
        {
            await CriarTipoCalendario(dto.TipoCalendario);

            await CriarItensComuns(dto.CriarPeriodo, dto.DataInicio, dto.DataFim, dto.Bimestre, dto.TipoCalendarioId);

            CriarClaimUsuario(dto.Perfil);

            await CriarUsuarios();
        }

        protected async Task CriarDadosBasicos(ValoresDadosBasicosDto dto)
        {
            await CriarDadosBase(dto);

            await CriarTurma(dto.Modalidade);

            await CriarAula(dto.ComponenteCurricular, dto.DataAula, RecorrenciaAula.AulaUnica, dto.QuantidadeAula);

            if (dto.CriarPeriodoEscolarEAbertura)
                await CriarPeriodoEscolarEAbertura();

            await CriarParametrosNotas();
        }

        private async Task CriarParametrosNotas()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new ParametrosSistema() 
            { 
                Nome = DATA_INICIO_SGP,
                Descricao = DATA_INICIO_SGP,
                Tipo = TipoParametroSistema.DataInicioSGP, 
                Valor = dataAtual.Year.ToString(), 
                Ano = dataAtual.Year, 
                Ativo = true, 
                CriadoEm = dataAtual, 
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = DATA_INICIO_SGP,
                Descricao = DATA_INICIO_SGP,
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Valor = NUMERO_50,
                Ano = dataAtual.Year,
                Ativo = true,
                CriadoEm = dataAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = DATA_INICIO_SGP,
                Descricao = DATA_INICIO_SGP,
                Tipo = TipoParametroSistema.MediaBimestre,
                Valor = NUMERO_5,
                Ano = dataAtual.Year,
                Ativo = true,
                CriadoEm = dataAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });
        }

        protected async Task CriarAbrangenciaCj()
        {
            await InserirNaBase(new Abrangencia
            {
                UsuarioId = 1,
                DreId = 1,
                UeId = 1,
                TurmaId = 1,
                Historico = false,
                Perfil = new Guid(ObterPerfilCJ())
            });
        }

        protected async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_1, DATA_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_2, DATA_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_3, DATA_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_4, DATA_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        private ComponenteCurricularDto ObterComponenteCurricular(long componenteCurricularId)
        {
            if (componenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_PORTUGUES_NOME
                };
            else if (componenteCurricularId == COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_DESCONHECIDO_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_NOME
                };

            return null;
        }

        protected async Task CriarAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            await InserirNaBase(ObterAula(componenteCurricularCodigo, dataAula, recorrencia, quantidadeAula, rf));
        }

        private Aula ObterAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            return new Aula
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
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_1, DATA_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_2, DATA_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_3, DATA_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_4, DATA_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
        protected async Task CriarMotivosAusencias(string descricao)
        {
            await InserirNaBase(new MotivoAusencia() { Descricao = descricao });
        }

        protected async Task CriarPeriodoReabertura(long tipoCalendarioId)
        {
            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = DATA_01_01,
                Fim = DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        } 

        protected async Task CriarPredefinicaoAluno(string codigoAluno, TipoFrequencia tipoFrequencia, long componenteCurricularId, long turmaId)
        {
            await InserirNaBase(new FrequenciaPreDefinida()
            {
                CodigoAluno = codigoAluno,
                TipoFrequencia = tipoFrequencia,
                ComponenteCurricularId = componenteCurricularId,
                TurmaId = turmaId
            });
        }
        protected class ValoresDadosBasicosDto
        {
            public ValoresDadosBasicosDto()
            {
                CriarPeriodo = true;
                TipoCalendarioId = 1;
                CriarPeriodoEscolarEAbertura = true;
            }

            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }
            public int Bimestre { get; set; }
            public DateTime DataAula { get; set; }
            public string ComponenteCurricular { get; set; }
            public int QuantidadeAula { get; set; }
            public bool CriarPeriodo { get; set; }
            public long TipoCalendarioId { get; set; }
            public bool CriarPeriodoEscolarEAbertura { get; set; }
        }
    }
}