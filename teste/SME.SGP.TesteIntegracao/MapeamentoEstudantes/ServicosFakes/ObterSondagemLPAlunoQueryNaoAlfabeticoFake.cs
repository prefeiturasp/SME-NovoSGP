using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Sondagem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes
{
    public class ObterSondagemLPAlunoQueryNaoAlfabeticoFake : IRequestHandler<ObterSondagemLPAlunoQuery, SondagemLPAlunoDto>
    {
        private const string CODIGO_ALUNO_HIPOTESE_ESCRITA_NAO_ALFABETICA = "5";
        public ObterSondagemLPAlunoQueryNaoAlfabeticoFake()
        {}

        public async Task<SondagemLPAlunoDto> Handle(ObterSondagemLPAlunoQuery request, CancellationToken cancellationToken)
        {
            if (request.AlunoCodigo.Equals(CODIGO_ALUNO_HIPOTESE_ESCRITA_NAO_ALFABETICA))
                return new SondagemLPAlunoDto()
                {
                    Escrita1Bim = "PS",
                    Escrita2Bim = "PS",
                    Escrita3Bim = "SA",
                    Escrita4Bim = "SA",
                };
            return new SondagemLPAlunoDto()
                {
                    Escrita1Bim = "A",
                    Escrita2Bim = "A",
                    Escrita3Bim = "A",
                    Escrita4Bim = "A",
                };
        }
    }

}
