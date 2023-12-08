using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterDadosTurmaEolPorCodigoQueryHandlerFake : IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>
    {
        public async Task<DadosTurmaEolDto> Handle(ObterDadosTurmaEolPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new DadosTurmaEolDto
            {
                Ano = '\u0000',
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Codigo = 2366531,
                CodigoModalidade = 0,
                DataFim = null,
                DataInicioTurma = DateTime.Now,
                DuracaoTurno = 7,
                Ehistorico = false,
                EnsinoEspecial = false,
                EtapaEJA = 0,
                Extinta = false,
                Modalidade = null,
                NomeTurma = "2A",
                Semestre = 0,
                SerieEnsino = null,
                TipoTurma = TipoTurma.Regular,
                TipoTurno = 5,
            });
        }
    }
}
