using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Avaliacao
{
    public abstract class TesteAvaliacao : TesteBaseComuns
    {
        protected const string COMPONENTE_INVALIDO = "0";

        protected TesteAvaliacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBasicos(CriacaoDeDadosDto dto)
        {
            await CriarTipoCalendario(dto.TipoCalendario);
            await CriarItensComuns(dto.CriarPeriodo, dto.DataInicio, dto.DataFim, dto.Bimestre, dto.TipoCalendarioId);
            CriarClaimUsuario(dto.Perfil);
            await CriarUsuarios();
            await CriarTurma(dto.ModalidadeTurma);
            await CriaTipoAvaliacao(dto.TipoAvaliacao);
        }

        protected async Task CrieAula(string componente, DateTime dataAula,int quantidade = 1, bool aulaCJ = false)
        {
            await InserirNaBase(new Aula
            {
                UeId = "1",
                DisciplinaId = componente,
                TurmaId = "1",
                TipoCalendarioId = quantidade,
                ProfessorRf = "2222222",
                Quantidade = 1,
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

        protected AtividadeAvaliativaDto ObterAtividadeAvaliativaDto(string componente, CategoriaAtividadeAvaliativa categoria, DateTime dataAvaliacao)
        {
            return new AtividadeAvaliativaDto()
            {
                UeId = "1",
                DreId = "1",
                TurmaId = "1",
                DisciplinasId = new string[] { componente },
                Descricao = "",
                Nome = "Prova",
                CategoriaId = categoria,
                DataAvaliacao = dataAvaliacao,
                TipoAvaliacaoId = 1
            };
        }

        protected FiltroAtividadeAvaliativaDto ObterFiltro(string componente, DateTime dataAvaliacao)
        {
            return new FiltroAtividadeAvaliativaDto()
            {
                UeID = "1",
                TurmaId = "1",
                DreId = "1",
                TipoAvaliacaoId = 1,
                Nome = "teste",
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
