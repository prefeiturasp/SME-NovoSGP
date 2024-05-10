using MediatR;

namespace SME.SGP.Aplicacao
{

    public class ConsolidarConselhoClasseCommand : IRequest<bool>
    {
        public int DreId { get; set; }

        public ConsolidarConselhoClasseCommand(int dreId)
        {
            DreId = dreId;
        }
    }
}