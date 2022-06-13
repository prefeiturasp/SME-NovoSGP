using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_Enviar_para_analise_encaminhamento_AEE : TesteBase
    {
        private const string SISTEMA = "Sistema";
        
        private const string ALUNO_CODIGO_11 = "11";
        
        private const long TURMA_ID = 1;
        
        private const string NOME_ALUNO_11 = "Nome do aluno";

        private const string DRE_NOME_10 = "Nome da DRE";
        private const string DRE_CODIGO_10 = "10";
        private const string DRE_ABREVIACAO_10 = "DRE-AB";

        private const string UE_NOME_100 = "Nome da UE";
        private const string UE_CODIGO_100 = "100";
        private const string UE_ABREVIACAO_100 = "DRE-AB";

        private const string TURMA_NOME_7P = "7P";
        private const string TURMA_CODIGO_7P = "EI-7P";
        private const string TURMA_ANO_1 = "1";

        private const long ENCAMINHAMENTO_ID_1 = 1;

        private const string USUARIO_CODIGO_RF_1 = "1";
        private const string USUARIO_NOME_1 = "Nome do usuário 1";

        private const string PARAMETRO_AEE_GERAR_PENDENCIA_NOME = "GerarPendenciasEncaminhamentoAEE";
        private const string PARAMETRO_AEE_GERAR_PENDENCIA_DESCRICAO = "Controle de geração de pendências para os processos do Encaminhamento AEE";
        private const string PARAMETRO_AEE_GERAR_PENDENCIA_NUMERO = "5";
        
        private const int ANO = 2022;

        private const string USUARIO_LOGADO_RF_1111111 = "1111111";
        private const string USUARIO_LOGADO_NOME_1111111 = "USUARIO LOGADO 1111111";

        public Ao_Enviar_para_analise_encaminhamento_AEE(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //[Fact]
        //public async Task Deve_sem_paee_paai()
        //{
        //    var mediator = ServiceProvider.GetService<IMediator>();

        //    await InserirNaBase(new Dre()
        //    {
        //        Nome = DRE_NOME_10,
        //        CodigoDre = DRE_CODIGO_10,
        //        Abreviacao = DRE_ABREVIACAO_10
        //    });

        //    await InserirNaBase(new Ue()
        //    {
        //        Nome = UE_NOME_100,
        //        DreId = 1,
        //        TipoEscola = TipoEscola.EMEF,
        //        CodigoUe = UE_CODIGO_100
        //    });

        //    await InserirNaBase(new Turma()
        //    {
        //        Nome = TURMA_NOME_7P,
        //        CodigoTurma = TURMA_CODIGO_7P,
        //        Ano = TURMA_ANO_1,
        //        AnoLetivo = 2022,
        //        TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
        //        ModalidadeCodigo = Modalidade.Fundamental,
        //        UeId = 1
        //    });

        //    await InserirNaBase(new Usuario()
        //    {
        //        CodigoRf = USUARIO_CODIGO_RF_1,
        //        Login = USUARIO_NOME_1,
        //        Nome = USUARIO_NOME_1,
        //        PerfilAtual = Guid.Parse(PerfilUsuario.CP.ObterNome()),
        //        CriadoPor = "",
        //        CriadoRF = "",
        //        CriadoEm = new DateTime(2022, 01, 01),
        //    });

        //    await InserirNaBase(new Usuario()
        //    {
        //        CodigoRf = USUARIO_LOGADO_RF_1111111,
        //        Login = USUARIO_LOGADO_RF_1111111,
        //        Nome = USUARIO_LOGADO_NOME_1111111,
        //        PerfilAtual = Guid.Parse(PerfilUsuario.CP.ObterNome()),
        //        CriadoPor = "",
        //        CriadoRF = "",
        //        CriadoEm = new DateTime(2022, 01, 01),
        //    });

        //    await InserirNaBase( new EncaminhamentoAEE() 
        //    { 
        //        TurmaId = TURMA_ID,
        //        AlunoCodigo = ALUNO_CODIGO_11,
        //        Excluido = false,
        //        CriadoEm = DateTime.Now,
        //        CriadoPor = SISTEMA,
        //        CriadoRF = SISTEMA,
        //        Situacao = Dominio.Enumerados.SituacaoAEE.Encaminhado,
        //        AlunoNome = NOME_ALUNO_11                
        //    });

        //    await InserirNaBase(new Pendencia()
        //    {
        //        Tipo = TipoPendencia.AEE,
        //        Descricao = $"O encaminhamento do estudante {NOME_ALUNO_11} ({ALUNO_CODIGO_11}) da turma {TURMA_CODIGO_7P} da {UE_NOME_100} ({DRE_NOME_10}) está disponível para análise. <br/><a href='https://hom-novosgp.sme.prefeitura.sp.gov.br/aee/encaminhamento/editar/0'>Clique aqui para acessar o encaminhamento.</a> <br/><br/>Esta pendência será resolvida automaticamente quando o parecer do AEE for registrado no sistema",
        //        Titulo = $"Encaminhamento AEE para análise - {NOME_ALUNO_11} ({ALUNO_CODIGO_11}) - {UE_NOME_100} ({DRE_NOME_10})",
        //        CriadoPor = "",
        //        CriadoRF = "",
        //        CriadoEm = new DateTime(2022, 06, 13)
        //    });

        //    await InserirNaBase(new PendenciaEncaminhamentoAEE()
        //    {
        //        EncaminhamentoAEEId = 1,
        //        PendenciaId = 1,
        //        CriadoEm = DateTime.Now,
        //        CriadoPor = SISTEMA,
        //        CriadoRF = SISTEMA
        //    });

        //    await InserirNaBase(new PendenciaPerfil()
        //    {
        //        PendenciaId = 1,
        //        PerfilCodigo = PerfilUsuario.CP,
        //        CriadoEm = DateTime.Now,
        //        CriadoPor = SISTEMA,
        //        CriadoRF = SISTEMA
        //    });

        //    await InserirNaBase(new PendenciaPerfilUsuario()
        //    {
        //        PendenciaPerfilId = 1,
        //        UsuarioId = 1,
        //        PerfilCodigo = PerfilUsuario.CP,
        //        CriadoEm = DateTime.Now,
        //        CriadoPor = SISTEMA,
        //        CriadoRF = SISTEMA
        //    });

        //    await InserirNaBase(new ParametrosSistema
        //    {
        //        Nome = PARAMETRO_AEE_GERAR_PENDENCIA_NOME,
        //        Tipo = TipoParametroSistema.GerarPendenciasEncaminhamentoAEE,
        //        Descricao = PARAMETRO_AEE_GERAR_PENDENCIA_DESCRICAO,
        //        Valor = PARAMETRO_AEE_GERAR_PENDENCIA_NUMERO,
        //        Ano = ANO,
        //        CriadoEm = DateTime.Now,
        //        CriadoPor = SISTEMA,
        //        CriadoRF = SISTEMA,
        //        Ativo = true
        //    });

        //    CriarClaimFundamental();

        //    var encaminhamentoAeeId = ObterTodos<EncaminhamentoAEE>();

        //    var retorno = await mediator.Send(new EnviarParaAnaliseEncaminhamentoAEECommand(encaminhamentoAeeId.FirstOrDefault().Id));

        //    retorno.ShouldBeTrue();

        //    var encaminhamentoAtualizado = ObterTodos<EncaminhamentoAEE>();

        //    encaminhamentoAtualizado.Any().ShouldBeTrue();

        //    encaminhamentoAtualizado.Any(a => a.Situacao == Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel).ShouldBeTrue();

        //    var pendenciaPerfilCEFAI = ObterTodos<PendenciaPerfil>();

        //    pendenciaPerfilCEFAI.Any().ShouldBeTrue();

        //    pendenciaPerfilCEFAI.Any(a => a.PerfilCodigo == PerfilUsuario.CEFAI).ShouldBeTrue();
        //}

        //private void CriarClaimFundamental()
        //{
        //    var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
        //    var variaveis = new Dictionary<string, object>
        //    {
        //        { "NomeUsuario", USUARIO_LOGADO_NOME_1111111 },
        //        { "UsuarioLogado", USUARIO_LOGADO_RF_1111111 },
        //        { "RF", USUARIO_LOGADO_RF_1111111 },
        //        { "login", USUARIO_LOGADO_RF_1111111 },
        //        {
        //            "Claims", new List<InternalClaim> 
        //            {
        //                new InternalClaim { Value = USUARIO_LOGADO_RF_1111111, Type = "rf" },
        //                new InternalClaim { Value = "44E1E074-37D6-E911-ABD6-F81654FE895D", Type = "perfil" }
        //            }
        //        }
        //    };
        //    contextoAplicacao.AdicionarVariaveis(variaveis);
        //}

    }
}