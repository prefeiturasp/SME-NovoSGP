using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_pesquisar_aluno_plano_aee : PlanoAEETesteBase
    {
        public Ao_pesquisar_aluno_plano_aee(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificarExistenciaPlanoAEEPorEstudanteQuery, PlanoAEEResumoDto>), typeof(VerificarExistenciaPlanoAEEPorEstudanteQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorCodigoEolNomeQuery, IEnumerable<AlunoSimplesDto>>), typeof(ObterAlunosPorCodigoEolNomeQueryHandlerFake), ServiceLifetime.Scoped));
        }


        [Fact(DisplayName = "Plano AEE - Selecionar aluno que já possua plano validado (deve apresentar mensagem de erro)")]
        public async Task Selecionar_aluno_com_plano_validado()
        {
            var filtro = new FiltroBuscaEstudanteDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                CodigoUe = "1",
                CodigoTurma = 1,
                Codigo = int.Parse(ALUNO_CODIGO_1),
                Nome = ALUNO_CODIGO_1
            };
            var obterAlunosServico = ObterAlunosPorCodigoEolNomeUseCase();
            var aluno = await obterAlunosServico.Executar(filtro);
            
            aluno.ShouldNotBeNull();
            aluno.Items.Count().ShouldBeGreaterThanOrEqualTo(1);

            var verificarExistenciaPlanoAee = ObterServicoVerificarExistenciaPlanoAEEPorEstudanteUseCase();
            var existePlanoEstudante = new FiltroEstudantePlanoAEEDto(aluno.Items.FirstOrDefault().Codigo, "1234");
            var ex = await Assert.ThrowsAsync<NegocioException>(() => verificarExistenciaPlanoAee.Executar(existePlanoEstudante));
            ex.Message.ShouldNotBeNullOrEmpty();
        }

        [Fact(DisplayName = "Plano AEE - Pesquisar por nome e código EOL em uma turma com usuário de professor")]
        public async Task Pesquisar_por_nome_codigo_eol_em_uma_turma_com__professor()
        {
            var filtro = new FiltroBuscaEstudanteDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                CodigoUe = "1",
                CodigoTurma = 1,
                Nome = ALUNO_CODIGO_1
            };
            var obterAlunosServico = ObterAlunosPorCodigoEolNomeUseCase();
            var aluno = await obterAlunosServico.Executar(filtro);
            
            aluno.ShouldNotBeNull();
            aluno.Items.Where(x => x.Nome.Contains(filtro.Nome)).Count().ShouldBe(1);
        }

        [Fact(DisplayName = "Plano AEE - Pesquisar por nome e código EOL com usuário CP (pesquisar sem informar a turma")]
        public async Task Pesquisar_por_nome_codigo_EOL_com_usuário_CP_sem_informar_turma()
        {
            var filtro = new FiltroBuscaEstudanteDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                CodigoUe = "1",
                Nome = ALUNO_CODIGO_1
            };
            var obterAlunosServico = ObterAlunosPorCodigoEolNomeUseCase();
            var aluno = await obterAlunosServico.Executar(filtro);
            
            aluno.ShouldNotBeNull();
            aluno.Items.Where(x => x.Nome.Contains(filtro.Nome)).Count().ShouldBe(1);
        }
    }
}