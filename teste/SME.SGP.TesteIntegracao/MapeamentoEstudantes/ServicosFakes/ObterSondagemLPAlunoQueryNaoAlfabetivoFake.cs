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
    public class ObterSondagemLPAlunoQueryNaoAlfabetivoFake : IRequestHandler<ObterSondagemLPAlunoQuery, SondagemLPAlunoDto>
    {
        public ObterSondagemLPAlunoQueryNaoAlfabetivoFake()
        {}

        public Task<SondagemLPAlunoDto> Handle(ObterSondagemLPAlunoQuery request, CancellationToken cancellationToken)
        => Task.Run(() => new SondagemLPAlunoDto()
        {
            Escrita1Bim = "Não alfabético",
            Escrita2Bim = "Não alfabético",
            Escrita3Bim = "Não alfabético",
            Escrita4Bim = "Não alfabético",
        });
    }

}
