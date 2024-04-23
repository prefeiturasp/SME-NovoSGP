using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryHandler : IRequestHandler<ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery, InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private const long RECUPERACAO_PARALELA_AUTORAL_PORTUGUES = 1663;
        private const long RECUPERACAO_PARALELA_AUTORAL_MATEMATICA = 1664;
        private const long RECUPERACAO_PARALELA_AUTORAL_CIENCIAS_NATURAIS = 1665;
        private const long RECUPERACAO_PARALELA_AUTORAL_CIENCIAS_HUMANAS = 1666;
        private const long RECUPERACAO_PARALELA_AUTORAL_GEOGRAFIA = 1764;
        private const long RECUPERACAO_PARALELA_AUTORAL_HISTORIA = 1765;

        private const long PAP_RECUPERACAO_DE_APRENDIZAGENS = 1322;
        private const long PAP_PROJETO_COLABORATIVO = 1770;

        private const long SRM = 1030;
        private const long PAEE_COLABORATIVO = 1030;

        private const long ACOMPANHAMENTO_PEDAGOGICO_MATEMATICA = 1255;
        private const long ACOMPANHAMENTO_PEDAGOGICO_PORTUGUES = 1204;
        private const long ACOMPANHAMENTO_LEITURA = 1304;
        private const long REFORCO_ACOMPANHAMENTO_ALFABETIZACAO = 1302;
        private const long REFORCO_ACOMPANHAMENTO_CIENCIAS = 1295;

        public ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto> Handle(ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery request, CancellationToken cancellationToken)
        {
            var retorno = new InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto();
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_PROGRAMA_ALUNO, request.CodigoAluno, request.AnoLetivo));
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var componentesTurmasAluno = JsonConvert.DeserializeObject<IEnumerable<ComponenteTurmaAlunoDto>>(json);
                //PAP
                retorno.ComponentesPAP.AddRange(componentesTurmasAluno.Where(cc => new List<long> { RECUPERACAO_PARALELA_AUTORAL_PORTUGUES,
                                                                                                    RECUPERACAO_PARALELA_AUTORAL_MATEMATICA,
                                                                                                    RECUPERACAO_PARALELA_AUTORAL_CIENCIAS_NATURAIS,
                                                                                                    RECUPERACAO_PARALELA_AUTORAL_CIENCIAS_HUMANAS,
                                                                                                    RECUPERACAO_PARALELA_AUTORAL_GEOGRAFIA,
                                                                                                    RECUPERACAO_PARALELA_AUTORAL_HISTORIA}
                                                                                    .Contains(cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto() { Id = cc.codigoComponenteCurricular, 
                                                                                                                                Descricao =  cc.nomeComponenteCurricular})
                                                                      .Distinct());
                retorno.ComponentesPAP.AddRange(componentesTurmasAluno.Where(cc => new List<long> { PAP_RECUPERACAO_DE_APRENDIZAGENS }
                                                                                    .Contains(cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto()
                                                                      {
                                                                          Id = cc.codigoComponenteCurricular,
                                                                          Descricao = "Contraturno"
                                                                      })
                                                                      .Distinct());
                retorno.ComponentesPAP.AddRange(componentesTurmasAluno.Where(cc => new List<long> { PAP_PROJETO_COLABORATIVO }
                                                                                    .Contains(cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto()
                                                                      {
                                                                          Id = cc.codigoComponenteCurricular,
                                                                          Descricao = "Colaborativo"
                                                                      })
                                                                      .Distinct());

                //SRM/CEFAI 
                retorno.ComponentesSRMCEFAI.AddRange(componentesTurmasAluno.Where(cc => new List<long> { SRM, PAEE_COLABORATIVO }
                                                                                    .Contains(cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto()
                                                                      {
                                                                          Id = cc.codigoComponenteCurricular,
                                                                          Descricao = cc.nomeComponenteCurricular
                                                                      })
                                                                      .Distinct());

                //Fortalecimento de Aprendizagens 
                retorno.ComponentesFortalecimentoAprendizagens.AddRange(componentesTurmasAluno.Where(cc => new List<long> { ACOMPANHAMENTO_PEDAGOGICO_MATEMATICA,
                                                                                                                            ACOMPANHAMENTO_PEDAGOGICO_PORTUGUES,
                                                                                                                            ACOMPANHAMENTO_LEITURA,
                                                                                                                            REFORCO_ACOMPANHAMENTO_ALFABETIZACAO,
                                                                                                                            REFORCO_ACOMPANHAMENTO_CIENCIAS }
                                                                                    .Contains(cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto()
                                                                      {
                                                                          Id = cc.codigoComponenteCurricular,
                                                                          Descricao = cc.nomeComponenteCurricular
                                                                      })
                                                                      .Distinct());

                //Mais Educação
                retorno.ComponentesMaisEducacao.AddRange(componentesTurmasAluno.Where(cc =>
                                                                                     !retorno.ComponentesPAP
                                                                                             .Any(c => c.Id == cc.codigoComponenteCurricular) &&
                                                                                     !retorno.ComponentesFortalecimentoAprendizagens
                                                                                             .Any(c => c.Id == cc.codigoComponenteCurricular) &&
                                                                                     !retorno.ComponentesSRMCEFAI
                                                                                             .Any(c => c.Id == cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto()
                                                                      {
                                                                          Id = cc.codigoComponenteCurricular,
                                                                          Descricao = cc.nomeComponenteCurricular
                                                                      })
                                                                      .Distinct());
            }

            return retorno;
        }
    }

    public record ComponenteTurmaAlunoDto(string codigoAluno, long codigoTurma, long codigoComponenteCurricular, string nomeComponenteCurricular);

    public static class ListExtension
    {
        public static string SerializarJsonTipoQuestaoComboMultiplaEscolhaDinamico(this List<ComponenteCurricularSimplificadoDto> source)
        {
            var lista = source.PossuiRegistros()
                             ? source
                             : new List<ComponenteCurricularSimplificadoDto>() { new() { Id = 0, Descricao = "Não"} };

            return JsonConvert.SerializeObject(lista.Select(cc => new { index = cc.Id, value = cc.Descricao }));
        }
    }
}
