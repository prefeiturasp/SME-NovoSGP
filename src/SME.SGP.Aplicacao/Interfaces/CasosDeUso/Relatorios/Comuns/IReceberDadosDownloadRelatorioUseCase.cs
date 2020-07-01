using System;

namespace SME.SGP.Aplicacao
{
    public interface IReceberDadosDownloadRelatorioUseCase : IUseCase<Guid, (byte[], string, string)>
    {
    }
}
