using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Fechamento.NotaFechamentoBimestre.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Fechamento.Notificacoes
{
    public class Ao_notificar_fechamento_em_andamento : TesteBaseComuns
    {
        public Ao_notificar_fechamento_em_andamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpNotificacaoCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery, IEnumerable<ComponenteCurricularDto>>), typeof(ObterComponentesCurricularesEOLPorTurmaECodigoUeQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Fechamento - Não deve notificar fechamento em andamento")]
        public async Task Ao_nao_deve_notificar_fechamento_em_andamento()
        {
            var useCase = ServiceProvider.GetService<INotificacaoAndamentoFechamentoUseCase>();

            await useCase.Executar(new Infra.MensagemRabbit());

            var wf = ObterTodos<WorkflowAprovacao>();
            wf.ShouldNotBeNull();
            wf.Count.ShouldBe(0);
        }

        [Fact(DisplayName = "Fechamento - Deve notificar fechamento em andamento")]
        public async Task Ao_notificar_fechamento()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilCP());
            await CriarUsuarios();

            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio, false);
            await CriarTurma(Modalidade.Fundamental, ANO_1, false);
            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);
            await CriarPeriodoFechamento();
            await CriarFechamento();

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "GerarNotificacaoPendenciaFechamento",
                Descricao = "GerarNotificacaoPendenciaFechamento",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Tipo = TipoParametroSistema.GerarNotificacaoPendenciaFechamento,
                Valor = "",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "DiasNotificacaoAndamentoFechamento",
                Descricao = "DiasNotificacaoAndamentoFechamento",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Tipo = TipoParametroSistema.DiasNotificacaoAndamentoFechamento,
                Valor = "5",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var useCase = ServiceProvider.GetService<INotificacaoAndamentoFechamentoUseCase>();

            await useCase.Executar(new Infra.MensagemRabbit());

            var wf = ObterTodos<WorkflowAprovacao>();
            wf.ShouldNotBeNull();
            wf.Count.ShouldBe(1);
        }

        private async Task CriarPeriodoFechamento()
        {
            var periodoFechamento = new PeriodoFechamento
            {
                Id = 1,
                Migrado = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };

            var periodoEscolar = ObterTodos<PeriodoEscolar>().FirstOrDefault(p => p.Bimestre == BIMESTRE_4); 

            periodoFechamento.AdicionarFechamentoBimestre(new PeriodoFechamentoBimestre(1, periodoEscolar, periodoEscolar.PeriodoFim, DateTime.Now.Date.AddDays(5)));

            await InserirNaBase(periodoFechamento);

            await InserirNaBase(periodoFechamento.FechamentosBimestre.FirstOrDefault());
        }

        private async Task CriarFechamento()
        {
            await InserirNaBase(new FechamentoTurma()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                FechamentoTurmaId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

        }

    }
}
