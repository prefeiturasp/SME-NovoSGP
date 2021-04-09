using MediatR;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalDreCommand : IRequest<bool>
    {
        public TrataSincronizacaoInstitucionalDreCommand(long dreCodigo)
        {
            DreCodigo = dreCodigo;
        }

        public long DreCodigo { get; set; }
    }
}
