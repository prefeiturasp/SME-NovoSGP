using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    public class Ao_atualizar_turma_encaminhamento_naapa_para_aluno_ano_posterior : AtendimentoNAAPATesteBase
    {
        public Ao_atualizar_turma_encaminhamento_naapa_para_aluno_ano_posterior(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryHandlerFake_AnoPosterior), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Não deve atualizar encaminhamento NAAPA para aluno com situação ativa para ano posterior")]
        public async Task Nao_deve_atualizar_encaminhamento_naapa_para_aluno_situacao_turma_com_ano_posterior()
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
                Prioridade = NORMAL,
                CriarTurmaPadrao = false
            };

            await CriarDadosBase(filtroNAAPA);
            await CriarTurma(filtroNAAPA.Modalidade);
            await CriarEncaminhamentoNAAPA(ALUNO_CODIGO_1);

            var useCase = ObterServicoAtualizarTurmaDoEncaminhamentoNAAPA();

            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(ObterEncaminhamentoDto(ALUNO_CODIGO_1)));

            await useCase.Executar(mensagem);

            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>().FirstOrDefault();

            encaminhamentoNAAPA.TurmaId.ShouldBe(TURMA_ID_1);
        }


        //[Fact(DisplayName = "Encaminhamento NAAPA - Deve atualizar encaminhamento NAAPA para aluno com situação ativa para ano atual")]
        //public async Task Deve_atualizar_encaminhamento_naapa_para_aluno_situacao_turma_ano_atual()
        //{
        //    var filtroNAAPA = new FiltroNAAPADto()
        //    {
        //        Perfil = ObterPerfilCP(),
        //        TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
        //        Modalidade = Modalidade.Fundamental,
        //        AnoTurma = "2",
        //        DreId = 1,
        //        CodigoUe = "1",
        //        TurmaId = TURMA_ID_2,
        //        Situacao = (int)SituacaoNAAPA.Rascunho,
        //        Prioridade = NORMAL,
        //        CriarTurmaPadrao = false
        //    };

        //    await CriarDadosBase(filtroNAAPA);
        //    await CriarTurma(filtroNAAPA.Modalidade);
        //    await CriarEncaminhamentoNAAPA(ALUNO_CODIGO_3);

        //    var useCase = ObterServicoAtualizarTurmaDoEncaminhamentoNAAPA();

        //    var mensagem = new MensagemRabbit(JsonSerializer.Serialize(ObterEncaminhamentoDto(ALUNO_CODIGO_3)));

        //    await useCase.Executar(mensagem);

        //    var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>().FirstOrDefault();

        //    encaminhamentoNAAPA.TurmaId.ShouldBe(TURMA_ID_2);
        //}

        private AtendimentoNAAPADto ObterEncaminhamentoDto(string alunoCodigo)
        {
            return new AtendimentoNAAPADto()
            {
                Id = 1,
                AlunoCodigo = alunoCodigo,
                TurmaId = TURMA_ID_1
            };
        }

        private async Task CriarEncaminhamentoNAAPA(string alunoCodigo)
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = alunoCodigo,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
