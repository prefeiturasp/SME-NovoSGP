using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public class Ao_validar_sugestao_de_topicos : RegistroIndividualTesteBase
    {
        private const string NOME_TABELA_SUGESTAO = "registro_individual_sugestao (mes, descricao)";
        private Dictionary<int, string> sugestaoDeTopicosDic;
        public Ao_validar_sugestao_de_topicos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            CarregarDicionarioDeSugestao();
        }

        [Fact(DisplayName = "Registro Individual - validar sugestão de tópicos por mês")]
        public async Task Ao_validar_sugestao_de_topicos_por_mes()
        {
            var dto = new FiltroRegistroIndividualDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBasicos(dto);
            await CriaSugestaoDeTopicos();

            var useCase = ObterServicoSugestaoRegistroIndividualUseCase();

            foreach (var chave in sugestaoDeTopicosDic.Keys)
            {
                var sugestao = await useCase.Executar(chave);
                sugestao.ShouldNotBeNull();
                sugestao.Descricao.ShouldBe(sugestaoDeTopicosDic[chave]);
            }
        }

        private void CarregarDicionarioDeSugestao()
        {
            sugestaoDeTopicosDic = new Dictionary<int, string>();
            sugestaoDeTopicosDic.Add(2, "Momento de adaptação e acolhimento. Como foi ou está sendo este processo para a criança e a família?");
            sugestaoDeTopicosDic.Add(3, "Como a criança brinca e interage no parque, área externa e outros espaços da unidade?");
            sugestaoDeTopicosDic.Add(4, "Como as crianças se relacionam consigo mesmas e com o grupo?");
            sugestaoDeTopicosDic.Add(5, "Como a criança responde às intervenções do professor(a)?");
            sugestaoDeTopicosDic.Add(6, "Quais os maiores interesses da criança? Como está a relação da família com a escola?");
            sugestaoDeTopicosDic.Add(7, "Evidências de oferta e evidências de aprendizagem.");
            sugestaoDeTopicosDic.Add(8, "Evidências de oferta e evidências de aprendizagem.");
            sugestaoDeTopicosDic.Add(9, "Evidências de oferta e evidências de aprendizagem.");
            sugestaoDeTopicosDic.Add(10, "Evidências de oferta e evidências de aprendizagem.");
            sugestaoDeTopicosDic.Add(11, "Evidências de oferta e evidências de aprendizagem.");
            sugestaoDeTopicosDic.Add(12, "Evidências de oferta e evidências de aprendizagem.");
        }

        private async Task CriaSugestaoDeTopicos()
        {
            foreach(var chave in sugestaoDeTopicosDic.Keys)
            {
                await InserirNaBase(NOME_TABELA_SUGESTAO, chave.ToString(), "'" + sugestaoDeTopicosDic[chave] + "'");
            }
            
        }
    }
}
