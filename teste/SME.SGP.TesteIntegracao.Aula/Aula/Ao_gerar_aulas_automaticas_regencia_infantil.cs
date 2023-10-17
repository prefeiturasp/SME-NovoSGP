using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dominio.Enumerados;
using System.Linq;
using SME.SGP.Aplicacao.Interfaces;
using Shouldly;
using Elastic.Apm.Api;

namespace SME.SGP.TesteIntegracao.Aula.Aula
{
    public class Ao_gerar_aulas_automaticas_regencia_infantil : AulaTeste
    {
        public Ao_gerar_aulas_automaticas_regencia_infantil(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.AddTransient<ObterTurmasInfantilNaoDeProgramaQueryHandler>();
        }

        [Fact]
        public async Task Deve_obter_somente_turma_regulares_e_nao_de_programa()
        {
            await InserirDados();
            var turmasInseridas = ObterTodos<Dominio.Turma>();
            var queryMediator = ServiceProvider.GetRequiredService<ObterTurmasInfantilNaoDeProgramaQueryHandler>();
            var retorno = await queryMediator.Handle(new ObterTurmasInfantilNaoDeProgramaQuery(DateTimeExtension.HorarioBrasilia().Year), new System.Threading.CancellationToken());

            Assert.NotNull(retorno);
            retorno.Any(r => r.TipoTurma == TipoTurma.Programa).ShouldBeFalse();
            retorno.Count().ShouldBe(2);
                
        }

        private async Task InserirDados()
        {
            await InserirNaBase(new Dre()
            {
                Id = 1
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1
            });
            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = TipoTurma.Regular,
                UeId = 1,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ano = "1"
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 2,
                CodigoTurma = "2",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = TipoTurma.Regular,
                UeId = 1,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ano = "1"
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 3,
                CodigoTurma = "3",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = TipoTurma.Programa,
                UeId = 1,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ano = "1"
            });
        }
    }
}
