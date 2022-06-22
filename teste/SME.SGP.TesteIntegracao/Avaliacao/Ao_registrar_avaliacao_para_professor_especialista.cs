using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Avaliacao
{
    public class Ao_registrar_avaliacao_para_professor_especialista : TesteAvaliacao
    {
        private DateTime dataInicio = new DateTime(2022, 05, 02);
        private DateTime dataFim = new DateTime(2022, 07, 08);

        public Ao_registrar_avaliacao_para_professor_especialista(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Registrar_avaliacao_para_professor_especialista()
        {
            await CriarItensComuns(true, dataInicio, dataFim, BIMESTRE_2);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = new AtividadeAvaliativaDto()
            {
                UeId = "1",
                DreId = "1",
                TurmaId = "1",
                DisciplinasId = new string[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString() },
                Descricao = "",
                Nome = "Prova",
                CategoriaId = CategoriaAtividadeAvaliativa.Normal,
                DataAvaliacao = new DateTime(),
                TipoAvaliacaoId = 1
            };

            await comando.Inserir(dto);

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();
            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);
        }
    }
}
