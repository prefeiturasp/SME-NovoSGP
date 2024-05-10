using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SolicitarExclusaoComunicadosEscolaAquiCommand : IRequest<string>
    {
        public long[] Ids { get; set; }


        public SolicitarExclusaoComunicadosEscolaAquiCommand(long[] ids)
        {
            Ids = ids;
        }
    }
}
