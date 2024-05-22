using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Commands;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.Frequencia.NotificacaoFrequenciaMensalAlunoInsuficiente.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia.ConsolidacaoReflexoFrequenciaBuscaAtiva
{
    public class Ao_consolidar_produtividade_frequencia : FrequenciaTesteBase
    {
        public Ao_consolidar_produtividade_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandFakeConsolidacaoProdutividadeFreq), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Ao consolidar produtividade frequencia")]
        public async Task Ao_consolidar_produtividade_frequencia_bimestre()
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarItensComuns(true, 
                             new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1),
                             new DateTime(DateTimeExtension.HorarioBrasilia().Year, 12, 31),
                             3,
                             TIPO_CALENDARIO_1);
            await CriarTurma(Modalidade.Medio, ANO_2, TURMA_CODIGO_1);
            await CriarTurma(Modalidade.Fundamental, ANO_5, TURMA_CODIGO_2);

            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            await CriaAulaFrequencia(new(dataReferencia.Year, 5, 01), 5, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TURMA_CODIGO_1);
            await CriaAulaFrequencia(new(dataReferencia.Year, 5, 10), 10, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TURMA_CODIGO_1);
            await CriaAulaFrequencia(new(dataReferencia.Year, 7, 01), 10, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TURMA_CODIGO_2);
            await CriaAulaFrequencia(new(dataReferencia.Year, 7, 10), 15, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TURMA_CODIGO_2);

            var useCase = ServiceProvider.GetService<IConsolidarInformacoesProdutividadeFrequenciaUseCase>();
            var retorno = await Consolidar(useCase, dataReferencia);
            retorno.ShouldBeTrue();

            var consolidacoes = ObterTodos<ConsolidacaoProdutividadeFrequencia>().OrderBy(c => c.Id);
            consolidacoes.Count().ShouldBe(4);
            consolidacoes.All(cc => cc.NomeProfessor.Equals(USUARIO_PROFESSOR_NOME_2222222)
                                    && cc.RfProfessor.Equals(USUARIO_PROFESSOR_CODIGO_RF_2222222)
                                    && cc.DescricaoUe.Equals("EMEF Nome da UE")
                                    && cc.DescricaoDre.Equals("DRE 1")
                                    && cc.Bimestre.Equals(3)
                                    && cc.CodigoDre.Equals(DRE_CODIGO_1)
                                    && cc.CodigoUe.Equals(UE_CODIGO_1)).ShouldBeTrue();

            var consolidacoesTurma1 = consolidacoes.Where(cc => cc.CodigoTurma.Equals(TURMA_CODIGO_1));
            consolidacoesTurma1.Count().ShouldBe(2);
            consolidacoesTurma1.FirstOrDefault().DescricaoTurma.ShouldBe("EM-Turma Nome 1");

            var consolidacoesPortugues = consolidacoesTurma1.Where(cc => cc.CodigoComponenteCurricular.Equals(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()));
            consolidacoesPortugues.Any().ShouldBeTrue();
            consolidacoesPortugues.FirstOrDefault().DataAula.ShouldBe(new(dataReferencia.Year, 5, 01));
            consolidacoesPortugues.FirstOrDefault().DataRegistroFrequencia.ShouldBe(new(dataReferencia.Year, 5, 6));
            consolidacoesPortugues.FirstOrDefault().DiferenciaDiasDataAulaRegistroFrequencia.ShouldBe(5);

            var consolidacoesArtes = consolidacoesTurma1.Where(cc => cc.CodigoComponenteCurricular.Equals(COMPONENTE_CURRICULAR_ARTES_ID_139.ToString()));
            consolidacoesArtes.Any().ShouldBeTrue();
            consolidacoesArtes.FirstOrDefault().DataAula.ShouldBe(new(dataReferencia.Year, 5, 10));
            consolidacoesArtes.FirstOrDefault().DataRegistroFrequencia.ShouldBe(new(dataReferencia.Year, 5, 20));
            consolidacoesArtes.FirstOrDefault().DiferenciaDiasDataAulaRegistroFrequencia.ShouldBe(10);

            var consolidacoesTurma2 = consolidacoes.Where(cc => cc.CodigoTurma.Equals(TURMA_CODIGO_2));
            consolidacoesTurma2.Count().ShouldBe(2);
            consolidacoesTurma2.FirstOrDefault().DescricaoTurma.ShouldBe("EF-Turma Nome 1");

            consolidacoesPortugues = consolidacoesTurma2.Where(cc => cc.CodigoComponenteCurricular.Equals(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()));
            consolidacoesPortugues.Any().ShouldBeTrue();
            consolidacoesPortugues.FirstOrDefault().DataAula.ShouldBe(new(dataReferencia.Year, 7, 01));
            consolidacoesPortugues.FirstOrDefault().DataRegistroFrequencia.ShouldBe(new(dataReferencia.Year, 7, 11));
            consolidacoesPortugues.FirstOrDefault().DiferenciaDiasDataAulaRegistroFrequencia.ShouldBe(10);

            consolidacoesArtes = consolidacoesTurma2.Where(cc => cc.CodigoComponenteCurricular.Equals(COMPONENTE_CURRICULAR_ARTES_ID_139.ToString()));
            consolidacoesArtes.Any().ShouldBeTrue();
            consolidacoesArtes.FirstOrDefault().DataAula.ShouldBe(new(dataReferencia.Year, 7, 10));
            consolidacoesArtes.FirstOrDefault().DataRegistroFrequencia.ShouldBe(new(dataReferencia.Year, 7, 25));
            consolidacoesArtes.FirstOrDefault().DiferenciaDiasDataAulaRegistroFrequencia.ShouldBe(15);
        }

        private async Task<bool> Consolidar(IConsolidarInformacoesProdutividadeFrequenciaUseCase useCase, DateTime dataReferencia)
        {
            var jsonMensagem = JsonSerializer.Serialize(new FiltroIdAnoLetivoDto(0, dataReferencia));
            return await useCase.Executar(new MensagemRabbit(jsonMensagem));
        }

        private async Task CriaAulaFrequencia(DateTime data, int diasDiferencaRegistroFrequencia, string codigoComponente, string codigoTurma)
        {
            var id = await InserirNaBaseAsync(new Dominio.Aula
            {
                CriadoPor = USUARIO_PROFESSOR_NOME_2222222,
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                UeId = UE_CODIGO_1,
                DisciplinaId = codigoComponente,
                TurmaId = codigoTurma,
                ProfessorRf = "",
                TipoCalendarioId = TIPO_CALENDARIO_1,
                DataAula = data,
                Quantidade = 1
            });

            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = id,
                CriadoPor = USUARIO_PROFESSOR_NOME_2222222,
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                CriadoEm = data.AddDays(diasDiferencaRegistroFrequencia),
            });

            
        }
    }
}
