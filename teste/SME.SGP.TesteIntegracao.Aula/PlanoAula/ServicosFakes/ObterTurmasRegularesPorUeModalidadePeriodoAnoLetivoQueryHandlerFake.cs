using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes
{
    public class ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQueryHandlerFake  : IRequestHandler<ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQuery,IEnumerable<AbrangenciaTurmaRetorno>>
    {
        public Task<IEnumerable<AbrangenciaTurmaRetorno>> Handle(ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<AbrangenciaTurmaRetorno>()
            {
                new AbrangenciaTurmaRetorno
                {
                    NomeFiltro = "Turma Nome 1",
                    Ano = "2",
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    Codigo = "1",
                    CodigoModalidade = 5,
                    Nome = "Turma Nome 1",
                    Semestre = 1,
                    EnsinoEspecial = false,
                    Id = 1,
                    TipoTurma = 1
                }
            };

            return Task.FromResult<IEnumerable<AbrangenciaTurmaRetorno>>(retorno);
        }
    }
}