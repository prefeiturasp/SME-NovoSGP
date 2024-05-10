using MediatR;

namespace SME.SGP.Aplicacao.Queries
{
    public class ValidaSeExisteTurmaPorCodigoQuery : IRequest<bool>
    {
        public ValidaSeExisteTurmaPorCodigoQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; set; }
    }
}
