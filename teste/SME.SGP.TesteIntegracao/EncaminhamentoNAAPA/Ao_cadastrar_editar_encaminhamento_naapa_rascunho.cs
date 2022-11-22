using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_cadastrar_editar_encaminhamento_naapa_rascunho: EncaminhamentoNAAPATesteBase
    {
        
        public Ao_cadastrar_editar_encaminhamento_naapa_rascunho(CollectionFixture collectionFixture) : base(collectionFixture)
        { }
        
        [Fact]
        public async Task Ao_cadastrar_encaminhamento_rascunho()
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
                                QuestaoId = 2,
                                Resposta = "1",
                                TipoQuestao = TipoQuestao.Combo,
                                
                            }
                        }
                    }
                }
            };

            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeFalse();
            (retorno.Auditoria.CriadoEm.Year == dataAtual.Year).ShouldBeTrue();
            
            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault().Situacao.Equals(SituacaoNAAPA.Rascunho).ShouldBeTrue();
            
            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.ShouldNotBeNull();
            encaminhamentoNAAPASecao.FirstOrDefault().SecaoEncaminhamentoNAAPAId.ShouldBe(1);
            
            var questaoEncaminhamentoNAAPA = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questaoEncaminhamentoNAAPA.ShouldNotBeNull();
            questaoEncaminhamentoNAAPA.Count.ShouldBe(2);
            questaoEncaminhamentoNAAPA.Any(a=> a.QuestaoId == 1).ShouldBeTrue();
            questaoEncaminhamentoNAAPA.Any(a=> a.QuestaoId == 2).ShouldBeTrue();
            
            var respostaEncaminhamentoNAAPA = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostaEncaminhamentoNAAPA.ShouldNotBeNull();
            respostaEncaminhamentoNAAPA.Count.ShouldBe(2);
            respostaEncaminhamentoNAAPA.Any(a=> a.RespostaId == 1).ShouldBeTrue();
            respostaEncaminhamentoNAAPA.Any(a=> a.Texto.Equals(dataQueixa.ToString("dd/MM/yyyy"))).ShouldBeTrue();
        }
        
        [Fact]
        public async Task Ao_editar_encaminhamento_rascunho()
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
            
            await CriarEncaminhamentoNAAPA();
            
            await CriarEncaminhamentoNAAPASecao();
            
            await CriarQuestoesEncaminhamentoNAAPA();

            await CriarRespostasEncaminhamentoNAAPA(dataQueixa);

            dataQueixa.AddDays(4);
            
            var encaminhamentosNaapaDto = new EncaminhamentoNAAPADto()
            {
                Id = 1,
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
                                RespostaEncaminhamentoId = 1,
                                QuestaoId = 1,
                                Resposta = dataQueixa.ToString("dd/MM/yyyy"),
                                TipoQuestao = TipoQuestao.Data,
                                
                            },
                            new ()
                            {
                                RespostaEncaminhamentoId = 2,
                                QuestaoId = 2,
                                Resposta = "2",
                                TipoQuestao = TipoQuestao.Combo,
                                
                            }
                        }
                    }
                }
            };

            var encaminhamento = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            
            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeTrue();
            
            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault().Situacao.Equals(SituacaoNAAPA.Rascunho).ShouldBeTrue();
            
            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.ShouldNotBeNull();
            encaminhamentoNAAPASecao.FirstOrDefault().SecaoEncaminhamentoNAAPAId.ShouldBe(1);
            
            var questaoEncaminhamentoNAAPA = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questaoEncaminhamentoNAAPA.ShouldNotBeNull();
            questaoEncaminhamentoNAAPA.Count.ShouldBe(2);
            questaoEncaminhamentoNAAPA.Any(a=> a.QuestaoId == 1).ShouldBeTrue();
            questaoEncaminhamentoNAAPA.Any(a=> a.QuestaoId == 2).ShouldBeTrue();
            
            var respostaEncaminhamentoNAAPA = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostaEncaminhamentoNAAPA.ShouldNotBeNull();
            respostaEncaminhamentoNAAPA.Count.ShouldBe(2);
            respostaEncaminhamentoNAAPA.Any(a=> a.RespostaId == 2).ShouldBeTrue();
            respostaEncaminhamentoNAAPA.Any(a=> a.Texto.Equals(dataQueixa.ToString("dd/MM/yyyy"))).ShouldBeTrue();
        }
        
        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPASecao()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}

