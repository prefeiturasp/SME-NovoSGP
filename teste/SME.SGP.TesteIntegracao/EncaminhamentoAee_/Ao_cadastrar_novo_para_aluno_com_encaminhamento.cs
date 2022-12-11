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
    public class Ao_cadastrar_novo_para_aluno_com_encaminhamento : EncaminhamentoAEETesteBase
    {
        public Ao_cadastrar_novo_para_aluno_com_encaminhamento(CollectionFixture collectionFixture) : base(
            collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(
                typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTurmasAlunoPorFiltroQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Nao_deve_permitir_cadastrar_novo_com_situacao_deferido()
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
                Situacao = SituacaoAEE.Deferido,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var obterServicoPodeCadastrarEncaminhamentoAee = ObterServicoPodeCadastrarEncaminhamentoAee();

            var filtroEncaminhamentoAeeDto = new FiltroEncaminhamentoAeeDto()
            {
                EstudanteCodigo = ALUNO_CODIGO_1,
                UeCodigo = UE_CODIGO_1
            };

            await Assert.ThrowsAsync<NegocioException>(() =>
                obterServicoPodeCadastrarEncaminhamentoAee.Executar(filtroEncaminhamentoAeeDto));
        }
        
        [Fact] 
        public async Task Deve_permitir_cadastrar_novo_com_situacao_indeferido()
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
                Situacao = SituacaoAEE.Indeferido,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var obterServicoPodeCadastrarEncaminhamentoAee = ObterServicoPodeCadastrarEncaminhamentoAee();

            var filtroEncaminhamentoAeeDto = new FiltroEncaminhamentoAeeDto()
            {
                EstudanteCodigo = ALUNO_CODIGO_1,
                UeCodigo = UE_CODIGO_1
            };

            var retorno = await obterServicoPodeCadastrarEncaminhamentoAee.Executar(filtroEncaminhamentoAeeDto);
            retorno.ShouldBeTrue();
        }
        
        [Fact] 
        public async Task Deve_permitir_cadastrar_novo_com_situacao_encerrado_automaticamente()
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
                Situacao = SituacaoAEE.EncerradoAutomaticamente,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var obterServicoPodeCadastrarEncaminhamentoAee = ObterServicoPodeCadastrarEncaminhamentoAee();

            var filtroEncaminhamentoAeeDto = new FiltroEncaminhamentoAeeDto()
            {
                EstudanteCodigo = ALUNO_CODIGO_1,
                UeCodigo = UE_CODIGO_1
            };

            var retorno = await obterServicoPodeCadastrarEncaminhamentoAee.Executar(filtroEncaminhamentoAeeDto);
            retorno.ShouldBeTrue();
        }
    }
}