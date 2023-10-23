using MediatR;
using SME.SGP.Aplicacao;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PendenciaFechamento.ServicosFakes
{
    public class ObterProfessoresTitularesDasTurmasQueryHandlerFake : IRequestHandler<ObterProfessoresTitularesDasTurmasQuery, IEnumerable<string>>
    {
        private const string USUARIO_PROFESSOR_CODIGO_RF_2222222 = "2222222";
        public async Task<IEnumerable<string>> Handle(ObterProfessoresTitularesDasTurmasQuery request, CancellationToken cancellationToken)
        {
            return new List<string>()
            {
                USUARIO_PROFESSOR_CODIGO_RF_2222222
            };
        }
    }
}
