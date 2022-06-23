using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
{
    public abstract class TesteAvaliacao : TesteBaseComuns
    {
        protected const string COMPONENTE_INVALIDO = "0";
        private const string NOME_ATIVIDADE_AVALIATIVA = "Nome atividade avaliativa";

        protected TesteAvaliacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBasicos(CriacaoDeDadosDto dadosBasicosDto)
        {
            await CriarTipoCalendario(dadosBasicosDto.TipoCalendario);
            await CriarItensComuns(dadosBasicosDto.CriarPeriodo, dadosBasicosDto.DataInicio, dadosBasicosDto.DataFim, dadosBasicosDto.Bimestre, dadosBasicosDto.TipoCalendarioId);
            CriarClaimUsuario(dadosBasicosDto.Perfil);
            await CriarUsuarios();
            await CriarTurma(dadosBasicosDto.ModalidadeTurma);
            await CriaTipoAvaliacao(dadosBasicosDto.TipoAvaliacao);
        }

        protected async Task CrieAula(string componente, DateTime dataAula,int quantidade = 1, bool aulaCJ = false)
        {
            await InserirNaBase(new Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = componente,
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
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

        protected async Task ExecuteTesteResgistrarAvaliacaoPorPerfil(AtividadeAvaliativaDto dto)
        {
            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var retorno = await comando.Inserir(dto);

            retorno.ShouldNotBeNull();

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();
            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);

            var atividadeAvaliativasDiciplina = ObterTodos<AtividadeAvaliativaDisciplina>();

            atividadeAvaliativasDiciplina.ShouldNotBeEmpty();
            atividadeAvaliativasDiciplina.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        protected async Task ExecuteTesteResgistrarAvaliacaoPorPerfilRegente(AtividadeAvaliativaDto dto)
        {
            await ExecuteTesteResgistrarAvaliacaoPorPerfil(dto);

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativaRegencia>();

            atividadeAvaliativas.ShouldNotBeEmpty();
            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        protected AtividadeAvaliativaDto ObterAtividadeAvaliativaDto(string componente, CategoriaAtividadeAvaliativa categoria, DateTime dataAvaliacao, TipoAvaliacaoCodigo tipoAvaliacao)
        {
            return new AtividadeAvaliativaDto()
            {
                UeId = UE_CODIGO_1,
                DreId = DRE_CODIGO_1,
                TurmaId = TURMA_CODIGO_1,
                DisciplinasId = new string[] { componente },
                Descricao = "",
                Nome = NOME_ATIVIDADE_AVALIATIVA,
                CategoriaId = categoria,    
                DataAvaliacao = dataAvaliacao,
                TipoAvaliacaoId = (long)tipoAvaliacao
            };
        }

        protected AtividadeAvaliativaDto ObterAtividadeAvaliativaRegenciaDto(string componente, CategoriaAtividadeAvaliativa categoria, DateTime dataAvaliacao, TipoAvaliacaoCodigo tipoAvaliacao, string[] disciplinaRegencia)
        {
            var atividadeAvaliativa = ObterAtividadeAvaliativaDto(componente, categoria, dataAvaliacao, tipoAvaliacao);

            atividadeAvaliativa.DisciplinaContidaRegenciaId = disciplinaRegencia;
            atividadeAvaliativa.EhRegencia = true;

            return atividadeAvaliativa;
        }

        protected FiltroAtividadeAvaliativaDto ObterFiltro(string componente, DateTime dataAvaliacao)
        {
            return new FiltroAtividadeAvaliativaDto()
            {
                UeID = UE_CODIGO_1,
                TurmaId = DRE_CODIGO_1,
                DreId = TURMA_CODIGO_1,
                TipoAvaliacaoId = TIPO_CALENDARIO_ID,
                Nome = NOME_ATIVIDADE_AVALIATIVA,
                DataAvaliacao = dataAvaliacao,
                DisciplinasId = !string.IsNullOrEmpty(componente) ? new string[] { componente } : Array.Empty<string>()
            };
        }

        protected class CriacaoDeDadosDto
        {
            public CriacaoDeDadosDto()
            {
                this.CriarPeriodo = true;
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
        }

        
    }
}
