using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes
{
    public class ObterAlunoEnderecoEolQueryAlunoMigranteFake : IRequestHandler<ObterAlunoEnderecoEolQuery, AlunoEnderecoRespostaDto>
    {
        public ObterAlunoEnderecoEolQueryAlunoMigranteFake()
        {}

        public Task<AlunoEnderecoRespostaDto> Handle(ObterAlunoEnderecoEolQuery request, CancellationToken cancellationToken)
        => Task.Run(() => new AlunoEnderecoRespostaDto() { EhImigrante = true });
    }
}
