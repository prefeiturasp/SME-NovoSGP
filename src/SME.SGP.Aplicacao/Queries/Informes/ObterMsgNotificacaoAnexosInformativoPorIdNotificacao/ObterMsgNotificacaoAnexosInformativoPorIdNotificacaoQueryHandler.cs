using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQueryHandler : IRequestHandler<ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQuery, string>
    {
        private readonly IRepositorioInformativo repositorio;
        private readonly IMediator mediator;
        private const string styleCss = @"<style>
                                            .download-list {
                                              list-style-image: url(""data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='16' height='16' viewBox='0 0 16 16' fill='none'%3E%3Cpath d='M6.75 0H9.25C9.66562 0 10 0.334375 10 0.75V6H12.7406C13.2969 6 13.575 6.67188 13.1812 7.06563L8.42813 11.8219C8.19375 12.0562 7.80937 12.0562 7.575 11.8219L2.81562 7.06563C2.42188 6.67188 2.7 6 3.25625 6H6V0.75C6 0.334375 6.33437 0 6.75 0ZM16 11.75V15.25C16 15.6656 15.6656 16 15.25 16H0.75C0.334375 16 0 15.6656 0 15.25V11.75C0 11.3344 0.334375 11 0.75 11H5.33437L6.86562 12.5312C7.49375 13.1594 8.50625 13.1594 9.13437 12.5312L10.6656 11H15.25C15.6656 11 16 11.3344 16 11.75ZM12.125 14.5C12.125 14.1562 11.8438 13.875 11.5 13.875C11.1562 13.875 10.875 14.1562 10.875 14.5C10.875 14.8438 11.1562 15.125 11.5 15.125C11.8438 15.125 12.125 14.8438 12.125 14.5ZM14.125 14.5C14.125 14.1562 13.8438 13.875 13.5 13.875C13.1562 13.875 12.875 14.1562 12.875 14.5C12.875 14.8438 13.1562 15.125 13.5 15.125C13.8438 15.125 14.125 14.8438 14.125 14.5Z' fill='%23064F79'/%3E%3C/svg%3E"");
                                            }

                                            .download-list li {
                                              margin-left: 1.5em; 
                                            }

                                            .titulo-anexos-container {
                                                    display: flex;
                                                    justify-content: space-between;
                                                    align-items: center;
                                                }
                                            .titulo-anexos-left {
                                                    font-weight: bold;
                                                    text-align: left;
                                                }
                                            .titulo-anexos-right {
                                                    text-align: right;
                                                }
                                          </style>";
        private const string urlDownloadAnexo = "{0}/v1/armazenamento/informes/{1}";
        private const string urlDownloadTodosAnexos = "{0}/v1/armazenamento/informes/{1}/anexos/compactados";
        private string htmlCabecalhoAnexos = @$"<div class=""titulo-anexos-container"">
                                                        <div class=""titulo-anexos-left"">Anexo(s):</div>
                                                        <div class=""titulo-anexos-right"">
                                                            <ul class=""download-list"">
                                                                <li><a href=""{urlDownloadTodosAnexos}"" download>Baixar todos os anexos</a></li>
                                                            </ul>
                                                        </div>
                                                    </div>";
            /*@$"<p style=""display: flex; justify-content: space-between; align-items: center;""><strong>Anexo(s):</strong> 
                                                        <a href="""">Baixar todos os anexos</a></p>";*/

        public ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQueryHandler(IRepositorioInformativo repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Handle(ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQuery request, CancellationToken cancellationToken)
        {
            var informativoId = await mediator.Send(new ObterIdInformativoPorNotificacaoIdQuery(request.NotificacaoId));
            var anexos = await mediator.Send(new ObterAnexosPorInformativoIdQuery(informativoId));
            if (anexos.NaoPossuiRegistros())
                return string.Empty;

            var htmlAnexos = @$"{styleCss}
                                {string.Format(htmlCabecalhoAnexos, "/api", informativoId)}
                                <ul class=""download-list"">";
            foreach (var anexo in anexos)
                htmlAnexos += @$"<li><a href=""{string.Format(urlDownloadAnexo, "/api", anexo.Codigo)}"" download>{anexo.Nome}</a></li>";

            htmlAnexos += "</ul>";
            return htmlAnexos;
        }
    }
}
