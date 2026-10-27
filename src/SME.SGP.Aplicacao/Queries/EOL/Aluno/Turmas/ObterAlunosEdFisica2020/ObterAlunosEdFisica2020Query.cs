using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosEdFisica2020Query : IRequest<IEnumerable<FechamentoAlunoComponenteDTO>>
    {
        private static ObterAlunosEdFisica2020Query _instance;
        public static ObterAlunosEdFisica2020Query Instance => _instance ??= new();
    }
}
