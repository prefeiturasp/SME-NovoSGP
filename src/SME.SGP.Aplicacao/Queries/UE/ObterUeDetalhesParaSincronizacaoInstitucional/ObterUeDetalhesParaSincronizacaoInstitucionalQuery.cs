using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterUeDetalhesParaSincronizacaoInstitucionalQuery : IRequest<UeDetalhesParaSincronizacaoInstituicionalDto>
    {
        public ObterUeDetalhesParaSincronizacaoInstitucionalQuery(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }

        public string UeCodigo { get; set; }
    }
}
