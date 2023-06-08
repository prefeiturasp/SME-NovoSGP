using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_atualizar_turmas_planos_aee_para_aluno_transferido : PlanoAEETesteBase
    {
        public Ao_atualizar_turmas_planos_aee_para_aluno_transferido(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryHandlerFake_Transferido), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandPlanoAEEUseCase), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano AEE - Alterar turmas dos Planos AEE para aluno transferido")]
        public async Task Ao_atualizar_turma_plano_aee_aluno_transferido()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCoordenadorCefai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });
            await CriarPlanoAEE(ALUNO_CODIGO_1, SituacaoPlanoAEE.ParecerCP);
            await CriarPlanoAEE(ALUNO_CODIGO_1, SituacaoPlanoAEE.Validado);
            var useCase = ObterServicoAtualizarInformacoesDoPlanoAEE();
            var mensagem = new MensagemRabbit("{}");

            await useCase.Executar(mensagem);

            var planoAEE = ObterTodos<Dominio.PlanoAEE>();
            planoAEE.All(plano => plano.TurmaId == TURMA_ID_2).ShouldBe(true, $"Todos os planos devem sofrer alteração da turma para Turma Id [{TURMA_ID_2}]");
        }

        [Fact(DisplayName = "Plano AEE - Não deve alterar turmas dos Planos AEE com situações Encerrado/Encerrado Automaticamente")]
        public async Task Nao_atualizar_turmas_planos_aee_situacao_encerrada()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCoordenadorCefai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });
            await CriarPlanoAEE(ALUNO_CODIGO_1, SituacaoPlanoAEE.EncerradoAutomaticamente);
            await CriarPlanoAEE(ALUNO_CODIGO_1, SituacaoPlanoAEE.Encerrado);
            var useCase = ObterServicoAtualizarInformacoesDoPlanoAEE();
            var mensagem = new MensagemRabbit("{}");

            await useCase.Executar(mensagem);

            var planoAEE = ObterTodos<Dominio.PlanoAEE>();
            planoAEE.All(plano => plano.TurmaId == TURMA_ID_1).ShouldBe(true, $"Todos os planos devem se manter com Turma Id [{TURMA_ID_1}] pois suas situações são Encerrado/Encerrado Automaticamente");
        }


        private async Task CriarPlanoAEE(string alunoCodigo = ALUNO_CODIGO_1, SituacaoPlanoAEE situacao = SituacaoPlanoAEE.ParecerCP)
        {
            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = alunoCodigo,
                Situacao = situacao,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
