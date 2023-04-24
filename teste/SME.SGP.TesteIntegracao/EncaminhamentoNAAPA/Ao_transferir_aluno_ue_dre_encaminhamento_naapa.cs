﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_transferir_aluno_ue_dre_encaminhamento_naapa : EncaminhamentoNAAPATesteBase
    {
        public Ao_transferir_aluno_ue_dre_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryHandlerFakeNAAPA_Desistente_ReclassificadoSaida), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosDreOuUePorPerfisQuery, IEnumerable<FuncionarioUnidadeDto>>), typeof(ObterFuncionariosDreOuUePorPerfisQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Ao receber transferencia de aluno entre Dres/Ues, deve notificar CP NAAPA, psicopedagogo, psicólogo e assist. social")]
        public async Task Deve_notificar_CP_NAAPA()
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

            var useCase = ObterServicoNotificacaoTransfAlunoDreUeDoEncaminhamentoNAAPA();

            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(ObterEncaminhamentoDto(ALUNO_CODIGO_1, ALUNO_NOME_1)));

            var gerouNotificacoes = (await useCase.Executar(mensagem));
            gerouNotificacoes.ShouldBe(true);

            var notificacoes = ObterTodos<Dominio.Notificacao>();
            notificacoes.ShouldNotBeNull().ShouldNotBeEmpty();
            notificacoes.Count.ShouldBe(4);
        }


        private EncaminhamentoNAAPADto ObterEncaminhamentoDto(string alunoCodigo, string nomeAluno)
        {
            return new EncaminhamentoNAAPADto()
            {
                Id = 1,
                AlunoCodigo = alunoCodigo,
                AlunoNome = nomeAluno,
                TurmaId = TURMA_ID_1,
                SituacaoMatriculaAluno = SituacaoMatriculaAluno.Ativo
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
                CriadoRF = SISTEMA_CODIGO_RF,
                SituacaoMatriculaAluno = SituacaoMatriculaAluno.Ativo
            });
        }
    }
}
