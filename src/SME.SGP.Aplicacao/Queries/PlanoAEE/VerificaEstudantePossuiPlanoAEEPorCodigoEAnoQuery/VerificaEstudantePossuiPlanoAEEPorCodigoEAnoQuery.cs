using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery : IRequest<bool>
    {
        public VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(string codigoEstudante, int anoLetivo)
        {
            CodigoEstudante = codigoEstudante;
            AnoLetivo = anoLetivo;
        }

        public string CodigoEstudante { get; }
        public int AnoLetivo { get; }
    }
}
