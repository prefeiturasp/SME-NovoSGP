﻿using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace SME.SGP.Aplicacao
{
    public class ObterMatriculasAlunoNaUEQueryHandler : IRequestHandler<ObterMatriculasAlunoNaUEQuery, IEnumerable<AlunoPorUeDto>>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ObterMatriculasAlunoNaUEQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async System.Threading.Tasks.Task<IEnumerable<AlunoPorUeDto>> Handle(ObterMatriculasAlunoNaUEQuery request, CancellationToken cancellationToken)
        {
            var alunos = Enumerable.Empty<AlunoPorUeDto>();
            var httpClient = _httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_ESCOLAS_ALUNO_MATRICULAS, request.UeCodigo, request.AlunoCodigo));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoPorUeDto>>(json);
            }

            return alunos;
        }
    }
}
