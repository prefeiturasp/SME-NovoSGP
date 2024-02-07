using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.DashboardBuscaAtiva
{
    public class Ao_obter_grafico_busca_ativa_por_procedimento_trabalho : RegistroAcaoBuscaAtivaTesteBase
    {
        public Ao_obter_grafico_busca_ativa_por_procedimento_trabalho(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_obter_qdade_busca_ativa_por_procedimento_trabalho_ligacao_telefonica()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
            await CriarDadosBase(filtro);

            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.LigacaoTelefonica, true);
            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.VisitaDomiciliar, true);
            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.VisitaDomiciliar, true);
            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.VisitaDomiciliar, true);
            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.LigacaoTelefonica, false);
            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.LigacaoTelefonica, false);

            var useCase = ServiceProvider.GetService<IObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDreUseCase>();
            var dto = new FiltroGraficoProcedimentoTrabalhoBuscaAtivaDto() { AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, 
                                                                             Modalidade = Modalidade.Fundamental,
                                                                             TipoProcedimentoTrabalho = EnumProcedimentoTrabalhoBuscaAtiva.LigacaoTelefonica  };
            var retorno = await useCase.Executar(dto);
            ValidarProcedimentoLigacaoTelefonica(retorno, true);
            dto = new FiltroGraficoProcedimentoTrabalhoBuscaAtivaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Fundamental,
                DreId = 1, UeId = 1,
                TipoProcedimentoTrabalho = EnumProcedimentoTrabalhoBuscaAtiva.LigacaoTelefonica
            };
            retorno = await useCase.Executar(dto);
            ValidarProcedimentoLigacaoTelefonica(retorno);
        }

        [Fact]
        public async Task Ao_obter_qdade_busca_ativa_por_procedimento_trabalho_visita_domiciliar()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
            await CriarDadosBase(filtro);

            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.LigacaoTelefonica, true);
            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.VisitaDomiciliar, true);
            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.VisitaDomiciliar, true);
            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.VisitaDomiciliar, true);
            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.LigacaoTelefonica, false);
            await InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva.LigacaoTelefonica, false);

            var useCase = ServiceProvider.GetService<IObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDreUseCase>();
            var dto = new FiltroGraficoProcedimentoTrabalhoBuscaAtivaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Fundamental,
                TipoProcedimentoTrabalho = EnumProcedimentoTrabalhoBuscaAtiva.VisitaDomiciliar
            };
            var retorno = await useCase.Executar(dto);
            ValidarProcedimentoVisitaDomiciliar(retorno, true);
            dto = new FiltroGraficoProcedimentoTrabalhoBuscaAtivaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Fundamental,
                DreId = 1,
                UeId = 1,
                TipoProcedimentoTrabalho = EnumProcedimentoTrabalhoBuscaAtiva.VisitaDomiciliar
            };
            retorno = await useCase.Executar(dto);
            ValidarProcedimentoVisitaDomiciliar(retorno);
        }

        private void ValidarProcedimentoVisitaDomiciliar(GraficoBuscaAtivaDto retorno, bool filtroTodosDre = false)
        {
            retorno.ShouldNotBeNull();
            retorno.Graficos.Count.ShouldBe(1);
            retorno.Graficos.Where(gf => gf.Descricao.Equals("Com sucesso")).FirstOrDefault()?.Quantidade.ShouldBe(3);
            (retorno.Graficos.Where(gf => gf.Descricao.Equals("Sem sucesso")).FirstOrDefault()?.Quantidade ?? 0).ShouldBe(0);
            retorno.Graficos.All(gf => gf.Grupo.Equals(DRE_NOME_1)).ShouldBe(filtroTodosDre);
        }

        private void ValidarProcedimentoLigacaoTelefonica(GraficoBuscaAtivaDto retorno, bool filtroTodosDre = false)
        {
            retorno.ShouldNotBeNull();
            retorno.Graficos.Count.ShouldBe(2);
            retorno.Graficos.Where(gf => gf.Descricao.Equals("Com sucesso")).FirstOrDefault()?.Quantidade.ShouldBe(1);
            retorno.Graficos.Where(gf => gf.Descricao.Equals("Sem sucesso")).FirstOrDefault()?.Quantidade.ShouldBe(2);
            retorno.Graficos.All(gf => gf.Grupo.Equals(DRE_NOME_1)).ShouldBe(filtroTodosDre);
        }

        private async Task InserirRegistrosAcao(EnumProcedimentoTrabalhoBuscaAtiva tipoProcedimentoTrabalho, bool realizouContatoResponsavel = false)
        {
            var opcaoRespostaBase = ObterTodos<OpcaoResposta>();
            var registroAcaoBase = ObterTodos<SME.SGP.Dominio.RegistroAcaoBuscaAtiva>();
            var idBaseRegistroAcao = registroAcaoBase.Count() + 1;

            await InserirNaBase(new SME.SGP.Dominio.RegistroAcaoBuscaAtiva()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNome = ALUNO_NOME_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaId = TURMA_ID_1,
            });

            await InserirNaBase(new Dominio.RegistroAcaoBuscaAtivaSecao()
            {
                RegistroAcaoBuscaAtivaId = idBaseRegistroAcao,
                SecaoRegistroAcaoBuscaAtivaId = SECAO_REGISTRO_ACAO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Concluido = false
            });

            await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = idBaseRegistroAcao,
                QuestaoId = (realizouContatoResponsavel ? QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO : QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var opcaoResposta = opcaoRespostaBase.Where(q => q.QuestaoId == (realizouContatoResponsavel ? QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO 
                                                                                                        : QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP)
                                                             && q.Ordem.Equals((int)tipoProcedimentoTrabalho)).FirstOrDefault();
            await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
                {
                    QuestaoRegistroAcaoBuscaAtivaId = idBaseRegistroAcao,
                    RespostaId = opcaoResposta.Id,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
        }

    }
}
