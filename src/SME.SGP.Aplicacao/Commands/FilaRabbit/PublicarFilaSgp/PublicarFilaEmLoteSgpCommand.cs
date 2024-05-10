using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaEmLoteSgpCommand : IRequest<bool>
    {
        public PublicarFilaEmLoteSgpCommand(IEnumerable<PublicarFilaSgpCommand> commands)
        {
            Commands = commands;
        }
        public IEnumerable<PublicarFilaSgpCommand> Commands { get; set; }
    }
}
