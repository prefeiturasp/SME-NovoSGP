using MediatR;
using SME.SGP.Dominio;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivoPastaDestinoCommandHendler : IRequestHandler<MoverArquivoPastaDestinoCommand, bool>
    {
        private readonly IMediator mediator;
        public MoverArquivoPastaDestinoCommandHendler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(MoverArquivoPastaDestinoCommand request, CancellationToken cancellationToken)
        {
            var expressao = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
            var regex = new Regex(expressao);
            var matches = regex.Matches(request.TextoEditor).Cast<Match>().Select(c => c.Value).ToList();
            foreach (var item in matches)
            {
               await mediator.Send(new MoverArquivoCommand(item, request.TipoArquivo));
            }
            return true;

        }
    }
}
