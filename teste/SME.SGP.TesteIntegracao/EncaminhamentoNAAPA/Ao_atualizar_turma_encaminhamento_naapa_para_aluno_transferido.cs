using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_atualizar_turma_encaminhamento_naapa_para_aluno_transferido : EncaminhamentoNAAPATesteBase
    {
        public Ao_atualizar_turma_encaminhamento_naapa_para_aluno_transferido(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryHandlerFake_Transferido), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Alterar turma do encaminhamento NAAPA para aluno transferido")]
        public async Task Ao_atualizar_turma_encaminhamento_naapa_aluno_transferido()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "2",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            await CriarEncaminhamentoNAAPA();
            await CriarTurma(filtroNAAPA.Modalidade);

            var useCase = ObterServicoAtualizarTurmaDoEncaminhamentoNAAPA();

            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(ObterEncaminhamentoDto()));

            await useCase.Executar(mensagem);

            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>().FirstOrDefault();

            encaminhamentoNAAPA.TurmaId.ShouldBe(TURMA_ID_3);
        }

        private AtendimentoNAAPADto ObterEncaminhamentoDto()
        {
            return new AtendimentoNAAPADto()
            {
                Id = 1,
                AlunoCodigo = ALUNO_CODIGO_1,
                TurmaId = TURMA_ID_1
            };
        }

        private async Task CriarEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
