using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_cadastrar_encaminhamento_naapa_rascunho: EncaminhamentoNAAPATesteBase
    {
        
        public Ao_cadastrar_encaminhamento_naapa_rascunho(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        [Fact]
        public async Task Ao_editar_encaminhamento_rascunho_professor()
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
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var registrarEncaminhamentoNaapaUseCase = ObterServicoRegistrarEncaminhamento();

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);
            
            var encaminhamentosNaapaDto = new EncaminhamentoNAAPADto()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 1,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = 1,
                                Resposta = dataQueixa.ToString("dd/MM/yyyy"),
                                TipoQuestao = TipoQuestao.Data,
                                
                            },
                            new ()
                            {
                                QuestaoId = 1,
                                Resposta = "1",
                                TipoQuestao = TipoQuestao.Combo,
                                
                            }
                        }
                    }
                }
            };

            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull();
            retorno.ShouldNotBeNull();
            // retorno.Count().ShouldBe(1);
            // retorno.Items.FirstOrDefault().Situacao.Equals(((int)SituacaoNAAPA.Rascunho).ToString()).ShouldBeTrue();
        }
    }
}

