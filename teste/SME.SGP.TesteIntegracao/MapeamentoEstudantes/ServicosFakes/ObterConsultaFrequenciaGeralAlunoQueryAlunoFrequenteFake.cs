using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes
{
    public class ObterConsultaFrequenciaGeralAlunoQueryAlunoFrequenteFake : IRequestHandler<ObterConsultaFrequenciaGeralAlunoQuery, string>
    {
        public ObterConsultaFrequenciaGeralAlunoQueryAlunoFrequenteFake()
        {}

        public Task<string> Handle(ObterConsultaFrequenciaGeralAlunoQuery request, CancellationToken cancellationToken)
        => Task.Run(() => "89%");
        
    }
}
