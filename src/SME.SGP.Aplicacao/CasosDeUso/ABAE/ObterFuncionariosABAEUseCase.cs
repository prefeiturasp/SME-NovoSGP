using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosABAEUseCase : AbstractUseCase, IObterFuncionariosABAEUseCase
    {

        public ObterFuncionariosABAEUseCase(IMediator mediator) : base(mediator)
        {}

        public Task<IEnumerable<NomeCpfABAEDto>> Executar(FiltroFuncionarioDto filtro)
        => mediator.Send(new ObterCadastroAcessoABAEPorDreQuery(filtro.CodigoRF,
                                                                filtro.CodigoDRE,
                                                                filtro.CodigoUE,
                                                                filtro.NomeServidor));
    }
}