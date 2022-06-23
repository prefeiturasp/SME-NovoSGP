using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
{
    public abstract class TesteAvaliacao : TesteBaseComuns
    {
        protected const string COMPONENTE_INVALIDO = "0";

        private const string REABERTURA_GERAL = "Reabrir Geral";

        private readonly DateTime DATA_01_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);

        private readonly DateTime DATA_31_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 31);

        protected TesteAvaliacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBasicos(CriacaoDeDadosDto dto)
        {
            await CriarTipoCalendario(dto.TipoCalendario);
            await CriarItensComuns(dto.CriarPeriodo, dto.DataInicio, dto.DataFim, dto.Bimestre, dto.TipoCalendarioId, dto.CriarComponente);
            CriarClaimUsuario(dto.Perfil);
            await CriarUsuarios();
            await CriarTurma(dto.ModalidadeTurma);
            await CriaTipoAvaliacao(dto.TipoAvaliacao);
        }

        protected async Task CrieAula(string componente, DateTime dataAula,int quantidade = 1, bool aulaCJ = false)
        {
            await InserirNaBase(new Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = componente,
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                Quantidade = quantidade,
                DataAula = dataAula,
                RecorrenciaAula = 0,
                TipoAula = TipoAula.Normal,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false,
                Excluido = false,
                AulaCJ = aulaCJ
            });
        }

        protected AtividadeAvaliativaDto ObterAtividadeAvaliativaDto(string componente, CategoriaAtividadeAvaliativa categoria, DateTime dataAvaliacao, TipoAvaliacaoCodigo tipoAvaliacao)
        {
            return new AtividadeAvaliativaDto()
            {
                UeId = UE_CODIGO_1,
                DreId = DRE_CODIGO_1,
                TurmaId = TURMA_CODIGO_1,
                DisciplinasId = new string[] { componente },
                Descricao = string.Empty,
                Nome = PROVA,
                CategoriaId = categoria,
                DataAvaliacao = dataAvaliacao,
                TipoAvaliacaoId = (long)tipoAvaliacao
            };
        }

        protected FiltroAtividadeAvaliativaDto ObterFiltro(string componente, DateTime dataAvaliacao)
        {
            return new FiltroAtividadeAvaliativaDto()
            {
                UeID = UE_CODIGO_1,
                TurmaId = TURMA_CODIGO_1,
                DreId = DRE_CODIGO_1,
                TipoAvaliacaoId = 1,
                Nome = TESTE,
                DataAvaliacao = dataAvaliacao,
                DisciplinasId = !string.IsNullOrEmpty(componente) ? new string[] { componente } : Array.Empty<string>()
            };
        }

        protected class CriacaoDeDadosDto
        {
            public CriacaoDeDadosDto()
            {
                this.CriarPeriodo = true;
                this.CriarComponente = true;
            }

            public string Perfil { get; set; }
            public Modalidade ModalidadeTurma { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }
            public int Bimestre { get; set; }
            public long TipoCalendarioId { get; set; }
            public TipoAvaliacaoCodigo TipoAvaliacao { get; set; }
            public bool CriarPeriodo { get; set; }

            public bool CriarComponente { get; set; }
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
    }
}
