using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PendenciaFechamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaFechamento
{
    public class Ao_obter_aulas_em_fechamento_na_geracao_pendencia_diario_classe_frequencia : PendenciaFechamentoBase
    {
        public Ao_obter_aulas_em_fechamento_na_geracao_pendencia_diario_classe_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ignorar_aulas_turma_em_fechamento_na_geracao_pendencia_diario_classe()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138
            };
            
            await CriarDadosBasicos(dto);
            var mediator = ServiceProvider.GetService<IMediator>();

            var aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.Frequencia,
                                                                                 "registro_frequencia",
                                                                                 new long[] { (int)Modalidade.EducacaoInfantil, (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio },
                                                                                 1, 1));
            aulas.ShouldNotBeEmpty();
            aulas.Count().ShouldBe(1);

            var aulasSemTurmaFechamento = await mediator.Send(new ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery(aulas));
            aulasSemTurmaFechamento.ShouldBeEmpty();
        }

        [Fact]    
        public async Task Nao_ignorar_aulas_turma_sem_fechamento_geracao_pendencia_diario_classe()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                IgnorarCricaoFechamento = true
            };

            await CriarDadosBasicos(dto);
            var mediator = ServiceProvider.GetService<IMediator>();

            var aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.Frequencia,
                                                                                 "registro_frequencia",
                                                                                 new long[] { (int)Modalidade.EducacaoInfantil, (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio },
                                                                                 1, 1));
            aulas.ShouldNotBeEmpty();
            aulas.Count().ShouldBe(1);

            var aulasSemTurmaFechamento = await mediator.Send(new ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery(aulas));
            aulasSemTurmaFechamento.ShouldNotBeEmpty();
            aulasSemTurmaFechamento.Count().ShouldBe(1);
        }

    }
}
