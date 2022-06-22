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
        protected TesteAvaliacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBasicos(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, DateTime dataInicio, DateTime dataFim, int bimestre, bool criarPeriodo = true, long tipoCalendarioId = 1)
        {
            await CriarTipoCalendario(tipoCalendario);
            await CriarItensComuns(criarPeriodo, dataInicio, dataFim, bimestre, tipoCalendarioId);
            CriarClaimUsuario(perfil);
            await CriarUsuarios();
            await CriarTurma(modalidade);
            await CriaTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral);
        }

        protected AtividadeAvaliativaDto ObtenhaDto(string componente, CategoriaAtividadeAvaliativa categoria, DateTime dataAvaliacao)
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
    }
}
