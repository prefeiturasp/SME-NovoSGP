using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AvaliacaoAula
{
    public abstract class TesteAvaliacao : TesteBaseComuns
    {
        protected const string COMPONENTE_INVALIDO = "0";

        protected const string NOME_ATIVIDADE_AVALIATIVA = "Nome atividade avaliativa";

        protected const string NOME_ATIVIDADE_AVALIATIVA_2 = "Nome atividade avaliativa 2";

        protected TesteAvaliacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>), typeof(PodePersistirTurmaDisciplinaQueryHandlerFakeRetornaTrue), ServiceLifetime.Scoped));
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

        protected async Task CrieAula(string componente, DateTime dataAula, int quantidade = 1, bool aulaCJ = false)
        {
            await InserirNaBase(new Dominio.Aula
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

        protected async Task ExecuteTesteResgistrarAvaliacaoPorPerfil(AtividadeAvaliativaDto atividadeAvaliativa)
        {
            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            await Validar(comando, atividadeAvaliativa);

            var retorno = await comando.Inserir(atividadeAvaliativa);

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

        protected AtividadeAvaliativaDto ObterAtividadeAvaliativaDto(string componente, CategoriaAtividadeAvaliativa categoria, DateTime dataAvaliacao, TipoAvaliacaoCodigo tipoAvaliacao, string nome = NOME_ATIVIDADE_AVALIATIVA)
        {
            return new AtividadeAvaliativaDto()
            {
                UeId = UE_CODIGO_1,
                DreId = DRE_CODIGO_1,
                TurmaId = TURMA_CODIGO_1,
                DisciplinasId = new string[] { componente },
                Descricao = "",
                Nome = nome,
                CategoriaId = categoria,    
                DataAvaliacao = dataAvaliacao,
                TipoAvaliacaoId = (long)tipoAvaliacao
            };
        }

        protected AtividadeAvaliativaDto ObterAtividadeAvaliativaRegenciaDto(string componente, CategoriaAtividadeAvaliativa categoria, DateTime dataAvaliacao, TipoAvaliacaoCodigo tipoAvaliacao, string[] disciplinaRegencia, string nome = NOME_ATIVIDADE_AVALIATIVA)
        {
            var atividadeAvaliativa = ObterAtividadeAvaliativaDto(componente, categoria, dataAvaliacao, tipoAvaliacao, nome);

            atividadeAvaliativa.DisciplinaContidaRegenciaId = disciplinaRegencia;
            atividadeAvaliativa.EhRegencia = true;

            return atividadeAvaliativa;
        }

        protected static FiltroAtividadeAvaliativaDto ObterFiltroAtividadeAvaliativa(AtividadeAvaliativaDto atividadeAvaliativa, int avaliacaoId = 0)
        {
            return new FiltroAtividadeAvaliativaDto()
            {
                DataAvaliacao = atividadeAvaliativa.DataAvaliacao,
                DisciplinaContidaRegenciaId = atividadeAvaliativa.DisciplinaContidaRegenciaId,
                DisciplinasId = atividadeAvaliativa.DisciplinasId,
                DreId = atividadeAvaliativa.DreId,
                Nome = atividadeAvaliativa.Nome,
                TipoAvaliacaoId = int.Parse(atividadeAvaliativa.TipoAvaliacaoId.ToString()),
                TurmaId = atividadeAvaliativa.TurmaId,
                UeID = atividadeAvaliativa.UeId,
                Id = avaliacaoId
            };
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

        private static async Task Validar(IComandosAtividadeAvaliativa comando, Infra.AtividadeAvaliativaDto atividadeAvaliativa, int avaliacaoId = 0)
        {
            var filtroAtividadeAvaliativa = ObterFiltroAtividadeAvaliativa(atividadeAvaliativa, avaliacaoId);

            await comando.Validar(filtroAtividadeAvaliativa).ShouldNotThrowAsync();
        }
    }
}
