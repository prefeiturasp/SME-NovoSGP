using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    public class Ao_reabrir_encaminhamento_naapa : AtendimentoNAAPATesteBase
    {
        public Ao_reabrir_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryHandlerFake_filtro), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Deve realizar a reabertura do encaminhamento NAAPA e situação retornar para Aguardando Atendimento")]
        public async Task Ao_reabrir_encaminhamento_naapa_sem_itinerancias()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Encerrado,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentoNAAPA();
            
            await CriarEncaminhamentoNAAPASecao();
            
            await CriarQuestoesEncaminhamentoNAAPA();

            await CriarRespostasEncaminhamentoNAAPA(new DateTime(DateTimeExtension.HorarioBrasilia().Date.Year, 11, 18));
            
            var reabrirEncaminhamentoNaapaUseCase = ObterServicoReaberturaEncaminhamento();

            var retorno = await reabrirEncaminhamentoNaapaUseCase.Executar(1);
            retorno.ShouldNotBeNull();
            retorno.Codigo.ShouldBe((int)SituacaoNAAPA.AguardandoAtendimento);

            var encaminhamento = ObterTodos<Dominio.EncaminhamentoNAAPA>().FirstOrDefault();
            encaminhamento.ShouldNotBeNull();
            encaminhamento.Situacao.ShouldBe(SituacaoNAAPA.AguardandoAtendimento);

            var historico = ObterTodos<Dominio.EncaminhamentoNAAPAHistoricoAlteracoes>().FirstOrDefault();
            historico.ShouldNotBeNull();
            historico.CamposAlterados.ShouldBe("Situação");
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Deve realizar a reabertura do encaminhamento NAAPA e situação retornar para Em Atendimento")]
        public async Task Ao_reabrir_encaminhamento_naapa_com_itinerancias()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Encerrado,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentoNAAPA();

            await CriarEncaminhamentoNAAPASecao(new long[] { ID_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE, ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA });

            await CriarQuestoesEncaminhamentoNAAPA();

            await CriarRespostasEncaminhamentoNAAPA(new DateTime(DateTimeExtension.HorarioBrasilia().Date.Year, 11, 18));

            var reabrirEncaminhamentoNaapaUseCase = ObterServicoReaberturaEncaminhamento();

            var retorno = await reabrirEncaminhamentoNaapaUseCase.Executar(1);
            retorno.ShouldNotBeNull();
            retorno.Codigo.ShouldBe((int)SituacaoNAAPA.EmAtendimento);

            var encaminhamento = ObterTodos<Dominio.EncaminhamentoNAAPA>().FirstOrDefault();
            encaminhamento.ShouldNotBeNull();
            encaminhamento.Situacao.ShouldBe(SituacaoNAAPA.EmAtendimento);

            var historico = ObterTodos<Dominio.EncaminhamentoNAAPAHistoricoAlteracoes>().FirstOrDefault();
            historico.ShouldNotBeNull();
            historico.CamposAlterados.ShouldBe("Situação");
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Não deve permitir o reabertura de um encaminhamento NAAPA que não existe")]
        public async Task Ao_reabrir_encaminhamento_naapa_inexistente()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Encerrado,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentoNAAPA();
            
            await CriarEncaminhamentoNAAPASecao();
            
            await CriarQuestoesEncaminhamentoNAAPA();

            await CriarRespostasEncaminhamentoNAAPA(new DateTime(DateTimeExtension.HorarioBrasilia().Date.Year, 11, 18));

            var reabrirEncaminhamentoNaapaUseCase = ObterServicoReaberturaEncaminhamento();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => reabrirEncaminhamentoNaapaUseCase.Executar(2));

            excecao.Message.ShouldBe(MensagemNegocioAtendimentoNAAPA.ATENDIMENTO_NAO_ENCONTRADO);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Não deve permitir o reabertura de um encaminhamento NAAPA com situação diferente de Encerrado")]
        public async Task Ao_reabrir_encaminhamento_naapa_nao_encerrado()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentoNAAPA(SituacaoNAAPA.AguardandoAtendimento);

            await CriarEncaminhamentoNAAPASecao();

            await CriarQuestoesEncaminhamentoNAAPA();

            await CriarRespostasEncaminhamentoNAAPA(new DateTime(DateTimeExtension.HorarioBrasilia().Date.Year, 11, 18));

            var reabrirEncaminhamentoNaapaUseCase = ObterServicoReaberturaEncaminhamento();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => reabrirEncaminhamentoNaapaUseCase.Executar(1));

            excecao.Message.ShouldBe(MensagemNegocioAtendimentoNAAPA.ATENDIMENTO_NAO_PODE_SER_REABERTO_NESTA_SITUACAO);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Não deve permitir o reabertura de um encaminhamento NAAPA de aluno com situação inativa")]
        public async Task Ao_reabrir_encaminhamento_naapa_aluno_inativo()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Encerrado,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentoNAAPA(SituacaoNAAPA.Encerrado, TURMA_ID_1, ALUNO_CODIGO_2);

            await CriarEncaminhamentoNAAPASecao();

            await CriarQuestoesEncaminhamentoNAAPA();

            await CriarRespostasEncaminhamentoNAAPA(new DateTime(DateTimeExtension.HorarioBrasilia().Date.Year, 11, 18));

            var reabrirEncaminhamentoNaapaUseCase = ObterServicoReaberturaEncaminhamento();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => reabrirEncaminhamentoNaapaUseCase.Executar(1));

            excecao.Message.ShouldBe(MensagemNegocioAtendimentoNAAPA.ATENDIMENTO_NAO_PODE_SER_REABERTO_NESTA_SITUACAO);
        }

        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPASecao(long[] secaoEncaminhamentoNAAPAIds = null)
        {
            if (secaoEncaminhamentoNAAPAIds.EhNulo())
                secaoEncaminhamentoNAAPAIds = new long[] { ID_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE };
            foreach (var secaoEncaminhamentoNAAPAId in secaoEncaminhamentoNAAPAIds)
            {
                await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
                {
                    EncaminhamentoNAAPAId = 1,
                    SecaoEncaminhamentoNAAPAId = secaoEncaminhamentoNAAPAId,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }

        private async Task CriarEncaminhamentoNAAPA(SituacaoNAAPA situacao = SituacaoNAAPA.Encerrado, long turmaId = TURMA_ID_1, string alunoCodigo = ALUNO_CODIGO_1)
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = turmaId,
                AlunoCodigo = alunoCodigo,
                Situacao = situacao,
                AlunoNome = $"Nome do aluno {ALUNO_CODIGO_1}",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
