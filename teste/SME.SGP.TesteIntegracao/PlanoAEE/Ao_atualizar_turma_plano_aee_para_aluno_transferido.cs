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
    public class Ao_atualizar_turma_plano_aee_para_aluno_transferido : PlanoAEETesteBase
    {
        public Ao_atualizar_turma_plano_aee_para_aluno_transferido(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryHandlerFake_Transferido), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano AEE - Alterar turma do Plano AEE para aluno transferido")]
        public async Task Ao_atualizar_turma_plano_aee_aluno_transferido()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCoordenadorCefai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TurmasMesmaUe = true
            });
            await CriarPlanoAEE();

            var useCase = ObterServicoAtualizarTurmaDoPlanoAEE();

            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(ObterPlanoDto()));

            await useCase.Executar(mensagem);

            var planoAEE = ObterTodos<Dominio.PlanoAEE>().FirstOrDefault();

            planoAEE.TurmaId.ShouldBe(TURMA_ID_2);
        }

        private PlanoAEETurmaDto ObterPlanoDto()
        {
            return new PlanoAEETurmaDto()
            {
                Id = 1,
                AlunoCodigo = ALUNO_CODIGO_1,
                TurmaId = TURMA_ID_1
            };
        }

        private async Task CriarPlanoAEE()
        {
            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
