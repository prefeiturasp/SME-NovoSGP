using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_atualizar_turma_plano_aee_para_aluno_ano_posterior : PlanoAEETesteBase
    {
        public Ao_atualizar_turma_plano_aee_para_aluno_ano_posterior(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryHandlerFake_AnoPosterior), ServiceLifetime.Scoped));
        }
        
        [Fact(DisplayName = "Plano AEE - Não deve atualizar Plano AEE para aluno com situação ativa para ano posterior")]
        public async Task Nao_deve_atualizar_plano_aee_para_aluno_situacao_turma_com_ano_posterior()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCoordenadorCefai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });
            await CriarPlanoAEE(ALUNO_CODIGO_1);

            var useCase = ObterServicoAtualizarTurmaDoPlanoAEE();

            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(ObterPlanoDto(ALUNO_CODIGO_1)));

            await useCase.Executar(mensagem);

            var encaminhamentoNAAPA = ObterTodos<Dominio.PlanoAEE>().FirstOrDefault();

            encaminhamentoNAAPA.TurmaId.ShouldBe(TURMA_ID_1);
        }


        [Fact(DisplayName = "Plano AEE - Deve atualizar Plano AEE para aluno com situação ativa para ano atual")]
        public async Task Deve_atualizar_plano_aee_para_aluno_situacao_turma_ano_atual()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCoordenadorCefai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });
            await CriarPlanoAEE(ALUNO_CODIGO_2);

            var useCase = ObterServicoAtualizarTurmaDoPlanoAEE();

            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(ObterPlanoDto(ALUNO_CODIGO_2)));

            await useCase.Executar(mensagem);

            var planoAEE = ObterTodos<Dominio.PlanoAEE>().FirstOrDefault();

            planoAEE.TurmaId.ShouldBe(TURMA_ID_2);
        }

        private PlanoAEETurmaDto ObterPlanoDto(string alunoCodigo)
        {
            return new PlanoAEETurmaDto()
            {
                Id = 1,
                AlunoCodigo = alunoCodigo,
                TurmaId = TURMA_ID_1
            };
        }

        private async Task CriarPlanoAEE(string alunoCodigo)
        {
            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = alunoCodigo,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
