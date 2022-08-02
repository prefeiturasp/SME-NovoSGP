using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.Base;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_obter_conselho_classe_aluno_query : ConselhoClasseTesteBase
    {
        public Ao_obter_conselho_classe_aluno_query(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        // protected override void RegistrarFakes(IServiceCollection services)
        // {
        //     base.RegistrarFakes(services);
        //
        //     services.Replace(new ServiceDescriptor(
        //         typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>),
        //         typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake),
        //         ServiceLifetime.Scoped));
        // }

        [Fact]
        public async Task Ao_obter_conselho_classe_aluno_por_conselho_fechamento_aluno_codigo_query()
        {
            var filtroConselhoClasse = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                InserirConselhoClassePadrao = true,
            };

            await CriarDadosBase(filtroConselhoClasse);
            
            var servicoMediator = RetornarServicoMediator();

            var query = new ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQuery(1,1,CODIGO_ALUNO_1);

            var retornoQuery = await servicoMediator.Send(query);
        }
    }
}