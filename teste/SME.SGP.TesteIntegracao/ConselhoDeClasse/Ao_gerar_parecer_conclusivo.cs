using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.Base;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_gerar_parecer_conclusivo : ConselhoClasseTesteBase
    {
        public Ao_gerar_parecer_conclusivo(CollectionFixture collectionFixture) : base(collectionFixture)
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
        public async Task Ao_gerar_parecer_conclusivo_aluno()
        {
            var filtroConselhoClasse = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                InserirConselhoClassePadrao = true,
                InserirFechamentoAlunoPadrao = true,
            };

            await CriarDadosBase(filtroConselhoClasse);

            var conselhoClasseFechamentoAluno = new ConselhoClasseFechamentoAlunoDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = 1,
                FechamentoTurmaId = 1
            };
            
            var gerarParecerConclusivoUseCase = RetornarGerarParecerConclusivoUseCase();

            var retorno = await gerarParecerConclusivoUseCase.Executar(conselhoClasseFechamentoAluno);

            retorno.ShouldNotBeNull();
        }
    }
}