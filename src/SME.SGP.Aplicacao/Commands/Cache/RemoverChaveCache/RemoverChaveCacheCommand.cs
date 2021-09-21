using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RemoverChaveCacheCommand : IRequest
    {
        public RemoverChaveCacheCommand(string chave)
        {
            Chave = chave;
        }

        public string Chave { get; set; }
    }
}
