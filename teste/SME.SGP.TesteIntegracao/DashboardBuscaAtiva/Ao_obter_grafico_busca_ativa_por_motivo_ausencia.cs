using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.DashboardBuscaAtiva
{
    public class Ao_obter_grafico_busca_ativa_por_motivo_ausencia : RegistroAcaoBuscaAtivaTesteBase
    {
        public Ao_obter_grafico_busca_ativa_por_motivo_ausencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_obter_qdade_busca_ativa_por_motivo_ausencia()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
            await CriarDadosBase(filtro);

            await InserirRegistrosAcao(new int[] { 1, 2, 3 });
            await InserirRegistrosAcao(new int[] { 1, 2, 3 });
            await InserirRegistrosAcao(new int[] { 4, 5 });
            await InserirRegistrosAcao(new int[] { 6, 7 });
            await InserirRegistrosAcao(new int[] { 6, 7 });
            await InserirRegistrosAcao(new int[] { 6, 7 });

            var useCase = ServiceProvider.GetService<IObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCase>();
            var dto = new FiltroGraficoBuscaAtivaDto() { AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, 
                                                         Modalidade = Modalidade.Fundamental };
            var retorno = await useCase.Executar(dto);
            Validar(retorno);
            dto = new FiltroGraficoBuscaAtivaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Fundamental,
                DreId = 1, UeId = 1
            };
            retorno = await useCase.Executar(dto);
            Validar(retorno);
        }

        private void Validar(GraficoBuscaAtivaDto retorno)
        {
            retorno.ShouldNotBeNull();
            retorno.Graficos.Count.ShouldBe(7);
            var nomesOpcaoResposta = ConstantesQuestionarioBuscaAtiva.ObterOpcoesRespostas_JUSTIFICATIVA_MOTIVO_FALTA().Where(opcao => opcao.id.Equals(1) ||
                                                                                                      opcao.id.Equals(2) ||
                                                                                                      opcao.id.Equals(3)).Select(opcao => opcao.descricao);
            retorno.Graficos.Where(gf => nomesOpcaoResposta.Any(nm => nm.Equals(gf.Descricao))).All(gf => gf.Quantidade.Equals(2)).ShouldBeTrue();

            nomesOpcaoResposta = ConstantesQuestionarioBuscaAtiva.ObterOpcoesRespostas_JUSTIFICATIVA_MOTIVO_FALTA().Where(opcao => opcao.id.Equals(4) ||
                                                                                                      opcao.id.Equals(5)).Select(opcao => opcao.descricao);
            retorno.Graficos.Where(gf => nomesOpcaoResposta.Any(nm => nm.Equals(gf.Descricao))).All(gf => gf.Quantidade.Equals(1)).ShouldBeTrue();

            nomesOpcaoResposta = ConstantesQuestionarioBuscaAtiva.ObterOpcoesRespostas_JUSTIFICATIVA_MOTIVO_FALTA().Where(opcao => opcao.id.Equals(6) ||
                                                                                                      opcao.id.Equals(7)).Select(opcao => opcao.descricao);
            retorno.Graficos.Where(gf => nomesOpcaoResposta.Any(nm => nm.Equals(gf.Descricao))).All(gf => gf.Quantidade.Equals(3)).ShouldBeTrue();
        }

        private async Task InserirRegistrosAcao(int[] codigosOpcoesResposta)
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
                SecaoRegistroAcaoBuscaAtivaId = ConstantesQuestionarioBuscaAtiva.SECAO_REGISTRO_ACAO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Concluido = false
            });

            await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = idBaseRegistroAcao,
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            foreach (var codigoOpcao in codigosOpcoesResposta)
            {
                var nomeOpcaoResposta = ConstantesQuestionarioBuscaAtiva.ObterOpcoesRespostas_JUSTIFICATIVA_MOTIVO_FALTA().Where(opcao => opcao.id.Equals(codigoOpcao)).FirstOrDefault().descricao;
                var opcaoResposta = opcaoRespostaBase.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA
                                                                 && q.Nome.Equals(nomeOpcaoResposta)).FirstOrDefault();
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
}
