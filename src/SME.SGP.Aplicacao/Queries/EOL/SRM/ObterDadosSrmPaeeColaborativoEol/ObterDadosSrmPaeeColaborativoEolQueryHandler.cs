using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosSrmPaeeColaborativoEolQueryHandler : IRequestHandler<ObterDadosSrmPaeeColaborativoEolQuery, IEnumerable<SrmPaeeColaborativoSgpDto>>
    {
        public ObterDadosSrmPaeeColaborativoEolQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory;
            this.mediator = mediator;
        }

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        public async Task<IEnumerable<SrmPaeeColaborativoSgpDto>> Handle(ObterDadosSrmPaeeColaborativoEolQuery request, CancellationToken cancellationToken)
        {
            var dados = new List<SrmPaeeColaborativoSgpDto>();
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var url = string.Format(ServicosEolConstants.URL_ALUNOS_SRM_PAEE_ALUNO, request.CodigoAluno);

            var resposta = await httpClient.GetAsync(url);
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var dtoEol = JsonConvert.DeserializeObject<IEnumerable<DadosSrmPaeeColaborativoEolDto>>(json).ToList();


                if (dtoEol.Any())
                    await MontarDados(dtoEol, dados);
                return dados;
            }
            else
            {
                var erro = $"Não foi possível obter os dados do SRM/PAEE no EOL - HttpCode {(int)resposta.StatusCode} - erro: {JsonConvert.SerializeObject(resposta.RequestMessage)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Turma, string.Empty));

                throw new NegocioException(erro);
            }
        }
        private async Task MontarDados(List<DadosSrmPaeeColaborativoEolDto> dadosSrmPaeeColaborativoEolDtos, List<SrmPaeeColaborativoSgpDto> srmPaeeColaborativoSgpDtos)
        {
            var idsTurmas = dadosSrmPaeeColaborativoEolDtos.Select(x => x.CodigoTurma.ToString()).ToArray();
            var turmas = (await mediator.Send(new ObterTurmasPorCodigosQuery(idsTurmas))).ToList();

            var idsUes = dadosSrmPaeeColaborativoEolDtos.Select(x => x.CodigoEscola).ToArray();
            var ues = (await mediator.Send(new ObterUesComDrePorCodigoUesQuery(idsUes))).ToList();

            foreach (var dadoEol in dadosSrmPaeeColaborativoEolDtos)
            {
                var dados = new SrmPaeeColaborativoSgpDto
                {
                    TurmaTurno = turmas.FirstOrDefault(x => x.CodigoTurma == dadoEol.CodigoTurma.ToString())?.NomeFiltro + " - " + dadoEol.Turno,
                    ComponenteCurricular = dadoEol.Componente,
                    DreUe = ues.FirstOrDefault(x => x.CodigoUe == dadoEol.CodigoEscola)!.Dre.Abreviacao + " - " + ues.FirstOrDefault(x => x.CodigoUe == dadoEol.CodigoEscola)?.TipoEscola.ShortName() + " "
                            + ues.FirstOrDefault(x => x.CodigoUe == dadoEol.CodigoEscola)?.Nome
                };

                srmPaeeColaborativoSgpDtos.Add(dados);
            }
        }

    }
}