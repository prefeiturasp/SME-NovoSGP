using System;
using System.Collections.Generic;
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
    public class Ao_devolver_encaminhamento_cp : EncaminhamentoAEETesteBase
    {
        public Ao_devolver_encaminhamento_cp(CollectionFixture collectionFixture) : base(
            collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(
                typeof(IRequestHandler<ExecutaNotificacaoDevolucaoEncaminhamentoAEECommand, bool>),
                typeof(ExecutaNotificacaoDevolucaoEncaminhamentoAEECommandHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_devolver_encaminhamento_em_situacao_aguardando_validacao_coordenacao_cp()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Encaminhado,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
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

            await InserirNaBase(new RespostaEncaminhamentoAEE() {
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
            
            await InserirNaBase(new Pendencia()
            {
                Titulo = "CP editando pendência",
                Descricao = "Com o CP editar um encaminhamento que está na situação 'Aguardando validação da Coordenação' e clicar em 'Devolver', preenchendo o campo de motivo",
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

            var obterServicoDevolverEncaminhamentoAee = ObterServicoDevolverEncaminhamentoAee();

            var filtroEncaminhamentoAeeDto = new DevolucaoEncaminhamentoAEEDto()
            {
                EncaminhamentoAEEId = 1,
                Motivo = "Devolvendo encaminhamento pelo CP"
            };

            var retorno = await obterServicoDevolverEncaminhamentoAee.Executar(filtroEncaminhamentoAeeDto);
            retorno.ShouldBeTrue();
            
            var encaminhamentoDevolvido = ObterTodos<Dominio.EncaminhamentoAEE>();
            (encaminhamentoDevolvido.FirstOrDefault().Situacao == SituacaoAEE.Devolvido).ShouldBeTrue();

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.Any().ShouldBeTrue();

            var pendenciaEncaminhamentoAee = ObterTodos<PendenciaEncaminhamentoAEE>();
            pendenciaEncaminhamentoAee.Any().ShouldBeFalse();

            var pendenciaPerfilUsuario = ObterTodos<PendenciaPerfilUsuario>();
            pendenciaPerfilUsuario.Any().ShouldBeFalse();
            
            var pendenciaPerfil = ObterTodos<PendenciaPerfil>();
            pendenciaPerfil.Any().ShouldBeFalse();
            
            var pendencia = ObterTodos<Pendencia>();
            (pendencia.FirstOrDefault().Excluido).ShouldBeTrue();
        }
        
         [Fact]
        public async Task Ao_devolver_encaminhamento_em_situacao_diferente_de_aguardando_validacao_coordenacao_cp_deve_gerar_excecao()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Analise,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
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

            await InserirNaBase(new RespostaEncaminhamentoAEE() {
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

            var obterServicoDevolverEncaminhamentoAee = ObterServicoDevolverEncaminhamentoAee();

            var filtroEncaminhamentoAeeDto = new DevolucaoEncaminhamentoAEEDto()
            {
                EncaminhamentoAEEId = 1,
                Motivo = "Devolvendo encaminhamento pelo CP"
            };

            await Assert.ThrowsAsync<NegocioException>(() => obterServicoDevolverEncaminhamentoAee.Executar(filtroEncaminhamentoAeeDto));
        }
    }
}