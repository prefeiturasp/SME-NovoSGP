using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public  class ObterProfessoresTitularesPorTurmaIdQueryHandler: IRequestHandler<ObterProfessoresTitularesPorTurmaIdQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IHttpClientFactory httpClientFactory;

        public ObterProfessoresTitularesPorTurmaIdQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta, IHttpClientFactory httpClientFactory)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            var turma = await repositorioTurmaConsulta.ObterPorId(request.TurmaId);
            
            if (turma.EhNulo())
                throw new NegocioException(MensagensNegocioFrequencia.TURMA_NAO_ENCONTRADA_POR_CODIGO);

            StringBuilder url = new StringBuilder();

            url.Append(ServicosEolConstants.URL_PROFESSORES_TITULARES  + $"?codigosTurmas={turma.CodigoTurma}");

            //Ao passar o RF do professor, o endpoint retorna todas as disciplinas que o professor não é titular para evitar
            //que o professor se atribua como CJ da própria da turma que ele é titular da disciplina
            if (!string.IsNullOrEmpty(request.ProfessorRf))
                url.Append($"?codigoRf={request.ProfessorRf}");

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var resposta = await httpClient.GetAsync(url.ToString());

            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ProfessorTitularDisciplinaEol>>(json);
        }
    }
}
