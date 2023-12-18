using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using System.Collections.Generic;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.RelatorioDinamico
{
    public class Ao_obter_questoes_relatorio_dinamico_naapa_por_modalidades : EncaminhamentoNAAPATesteBase
    {
        public Ao_obter_questoes_relatorio_dinamico_naapa_por_modalidades(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact(DisplayName = "Encaminhamento NAAPA - Obter questões para relatório dinâmico para uma modalidade")]
        public async Task Ao_obter_questoes_de_uma_modalidade()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = ANO_8,
                DreId = DRE_ID_1,
                CodigoUe = UE_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };
            await CriarDadosBase(filtroNAAPA);

            var useCase = ServiceProvider.GetService<IObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesUseCase>();
            var retorno = await useCase.Executar(new int[] { (int)ModalidadeTipoCalendario.FundamentalMedio });
            retorno.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Obter questões para relatório dinâmico para todas as modalidades")]
        public async Task Ao_obter_questoes_de_todas_as_modalidade()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = ANO_8,
                DreId = DRE_ID_1,
                CodigoUe = UE_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };
            await CriarDadosBase(filtroNAAPA);

            var useCase = ServiceProvider.GetService<IObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesUseCase>();
            var retorno = await useCase.Executar(null);
            retorno.ShouldNotBeNull();
        }
    }
}
