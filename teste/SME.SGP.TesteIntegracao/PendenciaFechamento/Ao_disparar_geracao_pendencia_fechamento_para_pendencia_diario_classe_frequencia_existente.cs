using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PendenciaFechamento.Base;
using SME.SGP.TesteIntegracao.PendenciaFechamento.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaFechamento
{
    public class Ao_disparar_geracao_pendencia_fechamento_para_pendencia_diario_classe_frequencia_existente : PendenciaFechamentoBase
    {
        public Ao_disparar_geracao_pendencia_fechamento_para_pendencia_diario_classe_frequencia_existente(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaGeracaoPendenciasFechamentoCommand, bool>), typeof(IncluirFilaGeracaoPendenciasFechamentoCommandHandlerFake), ServiceLifetime.Scoped));
        }

            [Fact]
        public async Task Disparar_geracao_pendencia_fechamento_quando_pendencia_aula_frequencia_existente_em_fechamento()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138
            };
            
            await CriarDadosBasicos(dto);
            await CriaPendenciaPorTipo(TipoPendencia.Frequencia);
            await CriarPendenciaAula(AULA_ID, PENDENCIA_ID_1);

            var useCase = ObterUseCaseGerarPendenciaAulaFrequenciaFechamento();
            var command = new DreUeDto(1, 1, UE_CODIGO_1);
            await useCase.Executar(new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(command) });

            var pendeciasSemFrequencia = ObterTodos<Pendencia>().FindAll(p => p.Tipo == TipoPendencia.AulasSemFrequenciaNaDataDoFechamento);
            pendeciasSemFrequencia.ShouldNotBeNull();
            var pendeciasFechamento = ObterTodos<Dominio.PendenciaFechamento>().Find(pf => pf.PendenciaId == pendeciasSemFrequencia.FirstOrDefault().Id);
            pendeciasFechamento.ShouldNotBeNull();
            var pendenciasAula = ObterTodos<Dominio.PendenciaAula>().Select(pendenciaAula => pendenciaAula.PendenciaId);
            pendenciasAula.Count().ShouldBe(1);
            var pendencias = ObterTodos<Dominio.Pendencia>().Where(pendencia => pendenciasAula.Contains(pendencia.Id));
            pendencias.Count(pendencia => pendencia.Excluido && pendencia.Tipo == TipoPendencia.Frequencia).ShouldBe(1);
            pendencias.Any(pendencia => pendencia.Excluido).ShouldBeTrue();
        }

    }
}
