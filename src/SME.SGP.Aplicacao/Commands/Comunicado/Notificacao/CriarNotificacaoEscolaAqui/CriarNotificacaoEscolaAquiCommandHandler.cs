﻿using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarNotificacaoEscolaAquiCommandHandler : IRequestHandler<CriarNotificacaoEscolaAquiCommand, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public CriarNotificacaoEscolaAquiCommandHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CriarNotificacaoEscolaAquiCommand request, CancellationToken cancellationToken)
        {
            var comunicadoServico = new ComunicadoInserirAeDto();

            MapearParaEntidadeServico(comunicadoServico, request.Comunicado);
            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");
            
            var parametros = JsonConvert.SerializeObject(comunicadoServico);

            var resposta = await httpClient.PostAsync("v1/notificacao", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                return true;
            else
                throw new Exception($"Não foi possivel criar a notificação para o comunucado de id : {request.Comunicado.Id}", new Exception($"Erro ao enviar a notificação para o App Aluno: {resposta.Content.ReadAsStringAsync().Result}"));

        }
        private void MapearParaEntidadeServico(ComunicadoInserirAeDto comunicadoServico, Comunicado comunicado)
        {
            comunicadoServico.Id = comunicado.Id;
            comunicadoServico.AlteradoEm = comunicado.AlteradoEm;
            comunicadoServico.AlteradoPor = comunicado.AlteradoPor;
            comunicadoServico.AlteradoRF = comunicado.AlteradoRF;
            comunicadoServico.DataEnvio = comunicado.DataEnvio;
            comunicadoServico.DataExpiracao = comunicado.DataExpiracao;
            comunicadoServico.Mensagem = comunicado.Descricao;
            comunicadoServico.Titulo = comunicado.Titulo;
            comunicadoServico.CriadoEm = comunicado.CriadoEm;
            comunicadoServico.CriadoPor = comunicado.CriadoPor;
            comunicadoServico.CriadoRF = comunicado.CriadoRF;
            comunicadoServico.Alunos = comunicado?.Alunos?.Select(x => x.AlunoCodigo);
            comunicadoServico.AnoLetivo = comunicado.AnoLetivo;
            comunicadoServico.CodigoDre = comunicado.CodigoDre;
            comunicadoServico.CodigoUe = comunicado.CodigoUe;
            comunicadoServico.Turmas = comunicado.Turmas.Select(x => x.CodigoTurma);
            comunicadoServico.TipoComunicado = comunicado.TipoComunicado;
            comunicadoServico.Semestre = comunicado.Semestre;
            comunicadoServico.SeriesResumidas = comunicado.SeriesResumidas;
            comunicadoServico.Modalidades = string.Join(",", comunicado?.Modalidades?.Select(x => x).ToArray());
            if (comunicado.TiposEscolas != null)
                comunicadoServico.TiposEscolas = string.Join(",", comunicado.TiposEscolas.Select(x => x).ToArray());
        }
    }
}
