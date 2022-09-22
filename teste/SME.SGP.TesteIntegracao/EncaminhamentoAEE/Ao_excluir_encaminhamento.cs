using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_excluir_encaminhamento : EncaminhamentoAEETesteBase
    {
        public Ao_excluir_encaminhamento(CollectionFixture collectionFixture) : base(
            collectionFixture)
        {
        }

        [Fact(DisplayName = "Encaminhamento AEE - Gestor da Ue deve excluir encaminhamento em situação rascunho")]
        public async Task Gestor_Ue_deve_excluir_em_situacao_rascunho()
        {
            var filtroAee = ObterFiltroAeeDto();

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await CriarEncaminhamentoSecaoPerguntasRespostas();

            await CriarPendenciasEncaminhamento();

            var obterServicoExcluirEncaminhamentoAee = ObterServicoExcluirEncaminhamentoAee();

            var retorno = await obterServicoExcluirEncaminhamentoAee.Executar(1);
            retorno.ShouldBeTrue();

            var encaminhamentoExcluido = ObterTodos<Dominio.EncaminhamentoAEE>();
            (encaminhamentoExcluido.FirstOrDefault().Excluido).ShouldBeTrue();
            
            var encaminhamentoAeeSecaoExcluido = ObterTodos<Dominio.EncaminhamentoAEESecao>();
            (encaminhamentoAeeSecaoExcluido.FirstOrDefault().Excluido).ShouldBeTrue();
            
            var questaoEncaminhamentoAeeExcluido = ObterTodos<Dominio.QuestaoEncaminhamentoAEE>();
            questaoEncaminhamentoAeeExcluido.FirstOrDefault().Respostas.Any(a => !a.Excluido).ShouldBeFalse();

            var respostaEncaminhamentoAeeExcluido = ObterTodos<Dominio.RespostaEncaminhamentoAEE>();
            (respostaEncaminhamentoAeeExcluido.FirstOrDefault().Excluido).ShouldBeTrue();

            var pendenciaEncaminhamentoAee = ObterTodos<PendenciaEncaminhamentoAEE>();
            pendenciaEncaminhamentoAee.Any().ShouldBeFalse();

            var pendenciaPerfilUsuario = ObterTodos<PendenciaPerfilUsuario>();
            pendenciaPerfilUsuario.Any().ShouldBeFalse();

            var pendenciaPerfil = ObterTodos<PendenciaPerfil>();
            pendenciaPerfil.Any().ShouldBeFalse();

            var pendencia = ObterTodos<Pendencia>();
            (pendencia.FirstOrDefault().Excluido).ShouldBeTrue();
        }

        [Fact(DisplayName = "Encaminhamento AEE - Gestor da Ue deve excluir encaminhamento em situação encaminhado")]
        public async Task Gestor_Ue_deve_excluir_em_situacao_encaminhado()
        {
            var filtroAee = ObterFiltroAeeDto();

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Encaminhado,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await CriarEncaminhamentoSecaoPerguntasRespostas();

            await CriarPendenciasEncaminhamento();

            var obterServicoExcluirEncaminhamentoAee = ObterServicoExcluirEncaminhamentoAee();

            var retorno = await obterServicoExcluirEncaminhamentoAee.Executar(1);
            retorno.ShouldBeTrue();

            var encaminhamentoExcluido = ObterTodos<Dominio.EncaminhamentoAEE>();
            (encaminhamentoExcluido.FirstOrDefault().Excluido).ShouldBeTrue();
            
            var encaminhamentoAeeSecaoExcluido = ObterTodos<Dominio.EncaminhamentoAEESecao>();
            (encaminhamentoAeeSecaoExcluido.FirstOrDefault().Excluido).ShouldBeTrue();
            
            var questaoEncaminhamentoAeeExcluido = ObterTodos<Dominio.QuestaoEncaminhamentoAEE>();
            questaoEncaminhamentoAeeExcluido.FirstOrDefault().Respostas.Any(a => !a.Excluido).ShouldBeFalse();

            var respostaEncaminhamentoAeeExcluido = ObterTodos<Dominio.RespostaEncaminhamentoAEE>();
            (respostaEncaminhamentoAeeExcluido.FirstOrDefault().Excluido).ShouldBeTrue();

            var pendenciaEncaminhamentoAee = ObterTodos<PendenciaEncaminhamentoAEE>();
            pendenciaEncaminhamentoAee.Any().ShouldBeFalse();

            var pendenciaPerfilUsuario = ObterTodos<PendenciaPerfilUsuario>();
            pendenciaPerfilUsuario.Any().ShouldBeFalse();

            var pendenciaPerfil = ObterTodos<PendenciaPerfil>();
            pendenciaPerfil.Any().ShouldBeFalse();

            var pendencia = ObterTodos<Pendencia>();
            (pendencia.FirstOrDefault().Excluido).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Gestor da Ue não deve excluir encaminhamento em situação diferente de encaminhado e rascunho")]
        public async Task Gestor_Ue_nao_deve_excluir_em_situacao_diferente_encaminhado_rascunho()
        {
            var filtroAee = ObterFiltroAeeDto();

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Indeferido,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await CriarEncaminhamentoSecaoPerguntasRespostas();

            await CriarPendenciasEncaminhamento();

            var obterServicoExcluirEncaminhamentoAee = ObterServicoExcluirEncaminhamentoAee();

            await Assert.ThrowsAsync<NegocioException>(() => obterServicoExcluirEncaminhamentoAee.Executar(1));
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Professor não criador do encaminhamento não deve excluir encaminhamento em situação diferente de encaminhado ou rascunho")]
        public async Task Professor_nao_criador_nao_deve_excluir_em_situacao_diferente_encaminhado_rascunho()
        {
            var filtroAee = ObterFiltroAeeDto();
            filtroAee.Perfil = ObterPerfilProfessor();

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Indeferido,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = "Professor não criador 2", CriadoRF = "2"
            });

            await CriarEncaminhamentoSecaoPerguntasRespostas();

            await CriarPendenciasEncaminhamento();

            var obterServicoExcluirEncaminhamentoAee = ObterServicoExcluirEncaminhamentoAee();

            await Assert.ThrowsAsync<NegocioException>(() => obterServicoExcluirEncaminhamentoAee.Executar(1));
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Professor não criador do encaminhamento não deve excluir encaminhamento em situação encaminhado")]
        public async Task Professor_nao_criador_nao_deve_excluir_em_situacao_encaminhado()
        {
            var filtroAee = ObterFiltroAeeDto();
            filtroAee.Perfil = ObterPerfilProfessor();

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Encaminhado,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = "Professor não Criador", CriadoRF = "2"
            });

            await CriarEncaminhamentoSecaoPerguntasRespostas();

            await CriarPendenciasEncaminhamento();

            var obterServicoExcluirEncaminhamentoAee = ObterServicoExcluirEncaminhamentoAee();

            await Assert.ThrowsAsync<NegocioException>(() => obterServicoExcluirEncaminhamentoAee.Executar(1));
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Professor não criador do encaminhamento não deve excluir encaminhamento em situação rascunho")]
        public async Task Professor_nao_criador_nao_deve_excluir_em_situacao_rascunho()
        {
            var filtroAee = ObterFiltroAeeDto();
            filtroAee.Perfil = ObterPerfilProfessor();

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = "Professor não Criador", CriadoRF = "2"
            });

            await CriarEncaminhamentoSecaoPerguntasRespostas();

            await CriarPendenciasEncaminhamento();

            var obterServicoExcluirEncaminhamentoAee = ObterServicoExcluirEncaminhamentoAee();

            await Assert.ThrowsAsync<NegocioException>(() => obterServicoExcluirEncaminhamentoAee.Executar(1));
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Professor criador do encaminhamento não deve excluir encaminhamento em situação diferente de encaminhado ou rascunho")]
        public async Task Professor_criador_nao_deve_excluir_em_situacao_diferente_encaminhado_rascunho()
        {
            var filtroAee = ObterFiltroAeeDto();
            filtroAee.Perfil = ObterPerfilProfessor();

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Indeferido,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = "Professor criador 1", CriadoRF = USUARIO_PROFESSOR_LOGIN_2222222
            });

            await CriarEncaminhamentoSecaoPerguntasRespostas();

            await CriarPendenciasEncaminhamento();

            var obterServicoExcluirEncaminhamentoAee = ObterServicoExcluirEncaminhamentoAee();

            await Assert.ThrowsAsync<NegocioException>(() => obterServicoExcluirEncaminhamentoAee.Executar(1));
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Professor criador do encaminhamento deve excluir encaminhamento em situação encaminhado")]
        public async Task Professor_criador_deve_excluir_em_situacao_encaminhado()
        {
            var filtroAee = ObterFiltroAeeDto();
            filtroAee.Perfil = ObterPerfilProfessor();

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Encaminhado,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = "Professor Criador", CriadoRF = USUARIO_PROFESSOR_LOGIN_2222222
            });

            await CriarEncaminhamentoSecaoPerguntasRespostas();

            await CriarPendenciasEncaminhamento();

            var obterServicoExcluirEncaminhamentoAee = ObterServicoExcluirEncaminhamentoAee();

            var retorno = await obterServicoExcluirEncaminhamentoAee.Executar(1);
            retorno.ShouldBeTrue();

            var encaminhamentoExcluido = ObterTodos<Dominio.EncaminhamentoAEE>();
            (encaminhamentoExcluido.FirstOrDefault().Excluido).ShouldBeTrue();
            
            var encaminhamentoAeeSecaoExcluido = ObterTodos<Dominio.EncaminhamentoAEESecao>();
            (encaminhamentoAeeSecaoExcluido.FirstOrDefault().Excluido).ShouldBeTrue();
            
            var questaoEncaminhamentoAeeExcluido = ObterTodos<Dominio.QuestaoEncaminhamentoAEE>();
            questaoEncaminhamentoAeeExcluido.FirstOrDefault().Respostas.Any(a => !a.Excluido).ShouldBeFalse();

            var respostaEncaminhamentoAeeExcluido = ObterTodos<Dominio.RespostaEncaminhamentoAEE>();
            (respostaEncaminhamentoAeeExcluido.FirstOrDefault().Excluido).ShouldBeTrue();

            var pendenciaEncaminhamentoAee = ObterTodos<PendenciaEncaminhamentoAEE>();
            pendenciaEncaminhamentoAee.Any().ShouldBeFalse();

            var pendenciaPerfilUsuario = ObterTodos<PendenciaPerfilUsuario>();
            pendenciaPerfilUsuario.Any().ShouldBeFalse();

            var pendenciaPerfil = ObterTodos<PendenciaPerfil>();
            pendenciaPerfil.Any().ShouldBeFalse();

            var pendencia = ObterTodos<Pendencia>();
            (pendencia.FirstOrDefault().Excluido).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Professor criador do encaminhamento deve excluir encaminhamento em situação rascunho")]
        public async Task Professor_criador_deve_excluir_em_situacao_rascunho()
        {
            var filtroAee = ObterFiltroAeeDto();
            filtroAee.Perfil = ObterPerfilProfessor();

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = "Professor Criador", CriadoRF = USUARIO_PROFESSOR_LOGIN_2222222
            });

            await CriarEncaminhamentoSecaoPerguntasRespostas();

            await CriarPendenciasEncaminhamento();

            var obterServicoExcluirEncaminhamentoAee = ObterServicoExcluirEncaminhamentoAee();

            var retorno = await obterServicoExcluirEncaminhamentoAee.Executar(1);
            retorno.ShouldBeTrue();

            var encaminhamentoExcluido = ObterTodos<Dominio.EncaminhamentoAEE>();
            (encaminhamentoExcluido.FirstOrDefault().Excluido).ShouldBeTrue();
            
            var encaminhamentoAeeSecaoExcluido = ObterTodos<Dominio.EncaminhamentoAEESecao>();
            (encaminhamentoAeeSecaoExcluido.FirstOrDefault().Excluido).ShouldBeTrue();
            
            var questaoEncaminhamentoAeeExcluido = ObterTodos<Dominio.QuestaoEncaminhamentoAEE>();
            questaoEncaminhamentoAeeExcluido.FirstOrDefault().Respostas.Any(a => !a.Excluido).ShouldBeFalse();

            var respostaEncaminhamentoAeeExcluido = ObterTodos<Dominio.RespostaEncaminhamentoAEE>();
            (respostaEncaminhamentoAeeExcluido.FirstOrDefault().Excluido).ShouldBeTrue();

            var pendenciaEncaminhamentoAee = ObterTodos<PendenciaEncaminhamentoAEE>();
            pendenciaEncaminhamentoAee.Any().ShouldBeFalse();

            var pendenciaPerfilUsuario = ObterTodos<PendenciaPerfilUsuario>();
            pendenciaPerfilUsuario.Any().ShouldBeFalse();

            var pendenciaPerfil = ObterTodos<PendenciaPerfil>();
            pendenciaPerfil.Any().ShouldBeFalse();

            var pendencia = ObterTodos<Pendencia>();
            (pendencia.FirstOrDefault().Excluido).ShouldBeTrue();
        }

        private FiltroAEEDto ObterFiltroAeeDto()
        {
            return new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
        }
        
        private async Task CriarPendenciasEncaminhamento()
        {
            await InserirNaBase(new Pendencia()
            {
                Titulo = "CP editando pendência",
                Descricao =
                    "Com o CP editar um encaminhamento que está na situação 'Aguardando validação da Coordenação' e clicar em 'Devolver', preenchendo o campo de motivo",
                Tipo = TipoPendencia.AEE,
                Situacao = SituacaoPendencia.Pendente,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PendenciaEncaminhamentoAEE()
            {
                EncaminhamentoAEEId = 1,
                PendenciaId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PendenciaPerfil()
            {
                PerfilCodigo = PerfilUsuario.CP,
                PendenciaId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PendenciaPerfilUsuario()
            {
                PendenciaPerfilId = 1,
                UsuarioId = 1,
                PerfilCodigo = PerfilUsuario.CP,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoSecaoPerguntasRespostas()
        {
            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 1,
                Nome = "Informações escolares",
                Ordem = 1,
                Etapa = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoAEESecao()
            {
                EncaminhamentoAEEId = 1,
                SecaoEncaminhamentoAEEId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = "Resposta",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}