using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse;
using SME.SGP.TesteIntegracao.Fechamento;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse
{
    public class Ao_verificar_turmas_com_matriculas_validas_para_conselho_de_classe : ConselhoDeClasseTesteBase
    {
        public Ao_verificar_turmas_com_matriculas_validas_para_conselho_de_classe(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFakeSituacaoVinculoIndevido), ServiceLifetime.Scoped));          
        }

        [Fact]
        public async Task Ao_validar_situacao_parecer_conclusivo_sem_parecer()
        {
            await InserirNaBase(new Dre()
            {
                Id = 1,
                CodigoDre = "1",
                DataAtualizacao = DateTime.Now.Date
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DataAtualizacao = DateTime.Now.Date,
                DreId = 1
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                Nome = "1A",
                CodigoTurma = "1",
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });
            var mediator = ServiceProvider.GetService<IMediator>();

            var retorno = await mediator.Send(new ObterTurmasComMatriculaValidasParaValidarConselhoQuery("1", new string[] {"1","2"}, DateTimeExtension.HorarioBrasilia().AddMonths(-2), DateTimeExtension.HorarioBrasilia()));

            retorno.ShouldBeEmpty();
        }
    }
    
}
