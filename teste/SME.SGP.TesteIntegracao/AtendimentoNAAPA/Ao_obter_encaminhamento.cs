using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ObterTurmaItinerarioEnsinoMedioQueryHandlerFake = SME.SGP.TesteIntegracao.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    //public class Ao_obter_encaminhamento: AtendimentoNAAPATesteBase
    //{
    //    public Ao_obter_encaminhamento(CollectionFixture collectionFixture) : base(collectionFixture)
    //    {}

    //    protected override void RegistrarFakes(IServiceCollection services)
    //    {
    //        base.RegistrarFakes(services);

    //        services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ServicosFake.ObterTurmasAlunoPorFiltroQueryHandlerFake), ServiceLifetime.Scoped));
    //        services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery, IEnumerable<AbrangenciaTurmaRetorno>>), typeof(ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandlerFake), ServiceLifetime.Scoped));
    //        services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
    //        services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
    //        services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
    //        services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
    //        services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterConsultaFrequenciaGeralAlunoQuery, string>), typeof(ObterConsultaFrequenciaGeralAlunoQueryHandlerFake), ServiceLifetime.Scoped));
            
    //    }

    //    [Fact(DisplayName = "Encaminhamento NAAPA - Obter encaminhamento NAAPA por Id")]
    //    public async Task Ao_obter_encaminhamento_por_id()
    //    {
    //        var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
    //        var filtroNAAPA = new FiltroNAAPADto()
    //        {
    //            Perfil = ObterPerfilCP(),
    //            TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
    //            Modalidade = Modalidade.Fundamental,
    //            AnoTurma = "8",
    //            DreId = 1,
    //            CodigoUe = "1",
    //            TurmaId = TURMA_ID_1,
    //            Situacao = (int)SituacaoNAAPA.Rascunho,
    //            Prioridade = NORMAL
    //        };

    //        await CriarDadosBase(filtroNAAPA);

    //        var encaminhamentoNAAPAId = 1;
    //        var encaminhamentoNAAPASecaoId = 1;
    //        var QuestaoEncaminhamentoId = 1;
            
    //        for (int i = 0; i < 10; i++)
    //        {
    //            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
    //            {
    //                TurmaId = TURMA_ID_1,
    //                AlunoCodigo = ALUNO_CODIGO_1,
    //                Situacao = SituacaoNAAPA.Rascunho,
    //                AlunoNome = "Nome do aluno 1",
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });
                
    //            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
    //            {
    //                EncaminhamentoNAAPAId = encaminhamentoNAAPAId,
    //                SecaoEncaminhamentoNAAPAId = 1,
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });
                
    //            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
    //            {
    //                EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoId,
    //                QuestaoId = 1,
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });
                
    //            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);
    //            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
    //            {
    //                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
    //                Texto = dataQueixa.ToString("dd/MM/yyyy"),
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });

    //            QuestaoEncaminhamentoId++;
                
    //            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
    //            {
    //                EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoId,
    //                QuestaoId = 2,
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });
                
    //            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
    //            {
    //                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
    //                RespostaId = 1,
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });
    //            encaminhamentoNAAPAId++;
    //            encaminhamentoNAAPASecaoId++;
    //        }
            
    //        var obterEncaminhamentosNAAPAUseCase = ObterServicoObterEncaminhamentoNAAPAPorId();

    //        var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(10);
    //        retorno.ShouldNotBeNull();
    //        retorno.Aluno.ShouldNotBeNull();

    //        var encaminhamentosNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
    //        encaminhamentosNAAPA.Count.ShouldBe(10);
            
    //        retorno.DreId.ShouldBe(1);
    //        retorno.DreCodigo.ShouldBe(DRE_CODIGO_1);
    //        retorno.DreNome.ShouldBe(DRE_NOME_1);
            
    //        retorno.UeId.ShouldBe(1);
    //        retorno.UeCodigo.ShouldBe(UE_CODIGO_1);
    //        retorno.UeNome.Contains(UE_NOME_1).ShouldBeTrue();
            
    //        retorno.TurmaId.ShouldBe(1);
    //        retorno.TurmaCodigo.ShouldBe(TURMA_CODIGO_1);
    //        retorno.TurmaNome.ShouldBe(TURMA_NOME_1);
            
    //        retorno.AnoLetivo.ShouldBe(dataAtual.Year);
    //        retorno.Situacao.ShouldBe((int)SituacaoNAAPA.Rascunho);
    //        retorno.DescricaoSituacao.ShouldBe(SituacaoNAAPA.Rascunho.Name());
    //        retorno.Modalidade.ShouldBe((int)Modalidade.Fundamental);
    //    }
        
    //    [Fact(DisplayName = "Encaminhamento NAAPA - Deve retornar as informações do encaminhamento naapa por Id")]
    //    public async Task Ao_obter_encaminhamento_por_id_com_encerramento()
    //    {
    //        var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
    //        var filtroNAAPA = new FiltroNAAPADto()
    //        {
    //            Perfil = ObterPerfilCP(),
    //            TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
    //            Modalidade = Modalidade.Fundamental,
    //            AnoTurma = "8",
    //            DreId = 1,
    //            CodigoUe = "1",
    //            TurmaId = TURMA_ID_1,
    //            Situacao = (int)SituacaoNAAPA.Rascunho,
    //            Prioridade = NORMAL
    //        };

    //        await CriarDadosBase(filtroNAAPA);

    //        var encaminhamentoNAAPAId = 1;
    //        var encaminhamentoNAAPASecaoId = 1;
    //        var QuestaoEncaminhamentoId = 1;
            
    //        for (int i = 0; i < 10; i++)
    //        {
    //            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
    //            {
    //                TurmaId = TURMA_ID_1,
    //                AlunoCodigo = ALUNO_CODIGO_1,
    //                Situacao = SituacaoNAAPA.Rascunho,
    //                MotivoEncerramento = MOTIVO_ENCERRAMENTO,
    //                AlunoNome = "Nome do aluno 1",
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });
                
    //            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
    //            {
    //                EncaminhamentoNAAPAId = encaminhamentoNAAPAId,
    //                SecaoEncaminhamentoNAAPAId = 1,
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });
                
    //            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
    //            {
    //                EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoId,
    //                QuestaoId = 1,
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });
                
    //            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);
    //            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
    //            {
    //                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
    //                Texto = dataQueixa.ToString("dd/MM/yyyy"),
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });

    //            QuestaoEncaminhamentoId++;
                
    //            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
    //            {
    //                EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoId,
    //                QuestaoId = 2,
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });
                
    //            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
    //            {
    //                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
    //                RespostaId = 1,
    //                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
    //            });
    //            encaminhamentoNAAPAId++;
    //            encaminhamentoNAAPASecaoId++;
    //        }
            
    //        var obterEncaminhamentosNAAPAUseCase = ObterServicoObterEncaminhamentoNAAPAPorId();

    //        var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(10);
    //        retorno.ShouldNotBeNull();
    //        retorno.Aluno.ShouldNotBeNull();

    //        var encaminhamentosNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
    //        encaminhamentosNAAPA.Count.ShouldBe(10);
            
    //        retorno.DreId.ShouldBe(1);
    //        retorno.DreCodigo.ShouldBe(DRE_CODIGO_1);
    //        retorno.DreNome.ShouldBe(DRE_NOME_1);
            
    //        retorno.UeId.ShouldBe(1);
    //        retorno.UeCodigo.ShouldBe(UE_CODIGO_1);
    //        retorno.UeNome.Contains(UE_NOME_1).ShouldBeTrue();
            
    //        retorno.TurmaId.ShouldBe(1);
    //        retorno.TurmaCodigo.ShouldBe(TURMA_CODIGO_1);
    //        retorno.TurmaNome.ShouldBe(TURMA_NOME_1);
            
    //        retorno.AnoLetivo.ShouldBe(dataAtual.Year);
    //        retorno.Situacao.ShouldBe((int)SituacaoNAAPA.Rascunho);
    //        retorno.DescricaoSituacao.ShouldBe(SituacaoNAAPA.Rascunho.Name());
    //        retorno.Modalidade.ShouldBe((int)Modalidade.Fundamental);
    //        retorno.MotivoEncerramento.ShouldBe(MOTIVO_ENCERRAMENTO);
    //    }
    //}
}