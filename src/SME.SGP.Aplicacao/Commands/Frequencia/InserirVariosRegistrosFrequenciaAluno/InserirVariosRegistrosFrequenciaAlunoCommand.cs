using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirVariosRegistrosFrequenciaAlunoCommand : IRequest<bool>
    {
        public InserirVariosRegistrosFrequenciaAlunoCommand(List<RegistroFrequenciaAluno> frequenciasPersistir)
        {
            FrequenciasPersistir = frequenciasPersistir;
        }

        public List<RegistroFrequenciaAluno> FrequenciasPersistir { get; }
    }
}
