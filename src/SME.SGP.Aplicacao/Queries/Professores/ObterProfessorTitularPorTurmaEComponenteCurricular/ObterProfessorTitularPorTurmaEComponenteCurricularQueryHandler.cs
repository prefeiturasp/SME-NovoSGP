using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandler : IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<ProfessorTitularDisciplinaEol> Handle(ObterProfessorTitularPorTurmaEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            var dadosProfessor = new ProfessorTitularDisciplinaEol();

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"professores/titular/turmas/{request.TurmaCodigo}/componentes-curriculares/{request.ComponenteCurricularCodigo}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                dadosProfessor = JsonConvert.DeserializeObject<ProfessorTitularDisciplinaEol>(json);
            }
            return dadosProfessor;
        }
    }
}
