using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IDownloadTodosAnexosInformativoUseCase : IUseCase<long, (byte[], string, string)>
    {

    }
}
