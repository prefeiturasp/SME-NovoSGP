using MediatR;
using SME.SGP.Dto;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosAnoAtualQuery : IRequest<IEnumerable<ComunicadoTurmaAlunoDto>>
    {
        public ObterComunicadosAnoAtualQuery()
        {}

        private static ObterComunicadosAnoAtualQuery _instance;
        public static ObterComunicadosAnoAtualQuery Instance => _instance ??= new();

    }
}
