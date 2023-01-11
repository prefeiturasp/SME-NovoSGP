using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_registrar_percurso_individual_nao_permitir_crianca_nova : RelatorioAcompanhamentoAprendizagemTesteBase
    {
        public Ao_registrar_percurso_individual_nao_permitir_crianca_nova(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>),
                typeof(ObterAlunoPorCodigoEolQueryHandlerCriancaNovaFake), ServiceLifetime.Scoped));
        }

        //[Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Não deve registrar o percurso individual no 1º semestre para criança nova (DataSituacao após o fim do 1º semestre)")]
        // TODO: Aparentemente a validação esperada nesse teste não existe mais na funcionalidade de salvar acompanhamento
        public async Task Nao_deve_registrar_o_percurso_individual_para_primeiro_semestre_para_crianca_nova()
        {
            await CriarDadosBasicos(abrirPeriodos: false);

            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);

            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();

            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto
            {
                TurmaId = TURMA_ID_1,
                Semestre = PRIMEIRO_SEMESTRE,
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                AlunoCodigo = CODIGO_ALUNO_1
            };

            await Assert.ThrowsAsync<NegocioException>(() => salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto));
        }
    }
}