using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Frequencia.CompensacaoDeAusencia.ServicosFake;
using SME.SGP.TesteIntegracao.Notificacoes.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_ausencia_notificacao : Ao_lancar_compensacao_ausencia_bimestre_base
    {
        public Ao_lancar_compensacao_ausencia_notificacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpNotificacaoCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Compensação de Ausência - Deve lançar compensações ausência enviando notificação")]
        public async Task Ao_lancar_compensacao_ausencia_enviando_notificacao()
        {
            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                                    BIMESTRE_1,
                                    Modalidade.Fundamental,
                                    ModalidadeTipoCalendario.FundamentalMedio,
                                    COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                                    ANO_7,
                                    QUANTIDADE_AULAS_22,
                                    true,
                                    false,
                                    true,
                                    true);

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "GerarNotificacaoCadastroDeCompensacaoDeAusencia",
                Descricao = "GerarNotificacaoCadastroDeCompensacaoDeAusencia",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Tipo = TipoParametroSistema.GerarNotificacaoCadastroDeCompensacaoDeAusencia,
                Valor = "",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var casoDeUso = ServiceProvider.GetService<ISalvarCompensacaoAusenciaUseCase>();
            casoDeUso.ShouldNotBeNull();

            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(compensacaoDeAusencia);

            await casoDeUso.Executar(0, compensacaoAusenciaDosAlunos);

            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();
            listaDeCompensacaoAusencia.Count.ShouldBe(1);
            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            notificacoes.Count.ShouldBe(2);
        }

        [Fact(DisplayName = "Compensação de Ausência - Deve lançar compensações ausência sem enviando notificação")]
        public async Task Ao_lancar_compensacao_ausencia_sem_notificacao()
        {
            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                        BIMESTRE_1,
                        Modalidade.Fundamental,
                        ModalidadeTipoCalendario.FundamentalMedio,
                        COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                        ANO_7,
                        QUANTIDADE_AULAS_22,
                        true,
                        false,
                        true,
                        true);

            var casoDeUso = ServiceProvider.GetService<ISalvarCompensacaoAusenciaUseCase>();
            casoDeUso.ShouldNotBeNull();

            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(compensacaoDeAusencia);

            await casoDeUso.Executar(0, compensacaoAusenciaDosAlunos);

            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();
            listaDeCompensacaoAusencia.Count.ShouldBe(1);
            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            notificacoes.Count.ShouldBe(0);
        }
    }
}
