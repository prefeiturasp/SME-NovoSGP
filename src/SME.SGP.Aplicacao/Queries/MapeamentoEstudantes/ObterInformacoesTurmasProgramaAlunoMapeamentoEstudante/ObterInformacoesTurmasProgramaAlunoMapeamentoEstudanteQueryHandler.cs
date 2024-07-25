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
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Aplicacao
{
    public class ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryHandler : IRequestHandler<ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery, InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        private const long SRM = 1030;
        private const long PAEE_COLABORATIVO = 1310;

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
                retorno.ComponentesPAP.AddRange(componentesTurmasAluno.Where(cc => new List<long> { ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_MATEMATICA,
                                                                                                    ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_CIENCIAS,
                                                                                                    ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_GEOGRAFIA,
                                                                                                    ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_HISTORIA,
                                                                                                    ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_PORTUGUES,
                                                                                                    ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_ALFABETIZACAO,
                                                                                                    ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_COLABORATIVO_ALFABETIZACAO  }
                                                                                    .Contains(cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto() { Id = cc.codigoComponenteCurricular, 
                                                                                                                                Descricao =  cc.nomeComponenteCurricular})
                                                                      .DistinctBy(cc => cc.Id));
                retorno.ComponentesPAP.AddRange(componentesTurmasAluno.Where(cc => new List<long> { ComponentesCurricularesConstants.CODIGO_PAP_RECUPERACAO_APRENDIZAGENS }
                                                                                    .Contains(cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto()
                                                                      {
                                                                          Id = cc.codigoComponenteCurricular,
                                                                          Descricao = "Contraturno"
                                                                      })
                                                                      .DistinctBy(cc => cc.Id));
                retorno.ComponentesPAP.AddRange(componentesTurmasAluno.Where(cc => new List<long> { ComponentesCurricularesConstants.CODIGO_PAP_PROJETO_COLABORATIVO }
                                                                                    .Contains(cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto()
                                                                      {
                                                                          Id = cc.codigoComponenteCurricular,
                                                                          Descricao = "Colaborativo"
                                                                      })
                                                                      .DistinctBy(cc => cc.Id));

                //SRM/CEFAI 
                retorno.ComponentesSRMCEFAI.AddRange(componentesTurmasAluno.Where(cc => new List<long> { SRM, PAEE_COLABORATIVO }
                                                                                    .Contains(cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto()
                                                                      {
                                                                          Id = cc.codigoComponenteCurricular,
                                                                          Descricao = cc.nomeComponenteCurricular
                                                                      })
                                                                      .DistinctBy(cc => cc.Id));

                //Fortalecimento de Aprendizagens 
                retorno.ComponentesFortalecimentoAprendizagens.AddRange(componentesTurmasAluno.Where(cc => new List<long> { ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_AUTORAL_PORTUGUES,
                                                                                                    ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_AUTORAL_MATEMATICA,
                                                                                                    ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_AUTORAL_CIENCIAS_NATURAIS,
                                                                                                    ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_AUTORAL_CIENCIAS_HUMANAS,
                                                                                                    ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_AUTORAL_GEOGRAFIA,
                                                                                                    ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_AUTORAL_HISTORIA
                                                                                                     }
                                                                                    .Contains(cc.codigoComponenteCurricular))
                                                                      .Select(cc => new ComponenteCurricularSimplificadoDto()
                                                                      {
                                                                          Id = cc.codigoComponenteCurricular,
                                                                          Descricao = cc.nomeComponenteCurricular
                                                                      })
                                                                      .DistinctBy(cc => cc.Id));

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
                                                                      .DistinctBy(cc => cc.Id));
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

            return JsonConvert.SerializeObject(lista.Select(cc => new { index = cc.Id.ToString(), value = cc.Descricao }));
        }
    }
}
