using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public class Ao_validar_sugestao_de_topicos : RegistroIndividualTesteBase
    {
        private readonly Dictionary<int, string> sugestaoDeTopicosDic;
        public Ao_validar_sugestao_de_topicos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            sugestaoDeTopicosDic = ObterDicionarioDeSugestao();
        }

        [Fact(DisplayName = "Registro Individual - validar sugestão de tópicos por mês")]
        public async Task Ao_validar_sugestao_de_topicos_por_mes()
        {
            var dto = new FiltroRegistroIndividualDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil
            };

            await CriarDadosBasicos(dto);
            await CriarSugestaoDeTopicos(sugestaoDeTopicosDic);

            var useCase = ObterServicoSugestaoRegistroIndividualUseCase();

            foreach (var chave in sugestaoDeTopicosDic.Keys)
            {
                var sugestao = await useCase.Executar(chave);
                sugestao.ShouldNotBeNull();
                sugestao.Descricao.ShouldBe(sugestaoDeTopicosDic[chave]);
            }
        }
    }
}
