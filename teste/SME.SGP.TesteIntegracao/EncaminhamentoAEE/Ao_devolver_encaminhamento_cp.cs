using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_devolver_encaminhamento_cp: EncaminhamentoAEETesteBase
    {
        public Ao_devolver_encaminhamento_cp(CollectionFixture collectionFixture) : base(
            collectionFixture)
        {
        }

        // protected override void RegistrarFakes(IServiceCollection services)
        // {
        //     base.RegistrarFakes(services);
        //
        //     services.Replace(new ServiceDescriptor(
        //         typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>),
        //         typeof(ObterTurmasAlunoPorFiltroQueryHandlerFake), ServiceLifetime.Scoped));
        // }

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

            var obterServicoDevolverEncaminhamentoAee = ObterServicoDevolverEncaminhamentoAee();

            var filtroEncaminhamentoAeeDto = new DevolucaoEncaminhamentoAEEDto()
            {
                EncaminhamentoAEEId = 1,
                Motivo = "Devolvendo encaminhamento pelo CP"
            };

            await Assert.ThrowsAsync<NegocioException>(() =>
                obterServicoPodeCadastrarEncaminhamentoAee.Executar(filtroEncaminhamentoAeeDto));
        }