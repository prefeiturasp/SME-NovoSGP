using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarRelatorioFaltasFrequenciaUseCase : IGerarRelatorioFaltasFrequenciaUseCase
    {
        private readonly IMediator mediator;

        public GerarRelatorioFaltasFrequenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(FiltroRelatorioFaltasFrequenciaDto filtro)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuario == null)
                throw new NegocioException("Não foi possível localizar o usuário.");

            filtro.NomeUsuario = usuario.Nome;
            filtro.CodigoRf = usuario.CodigoRf;


            if (filtro.Bimestres.Any(c => c == -99))
            {
                filtro.Bimestres = new List<int>();
                switch (filtro.Modalidade)
                {
                    case Modalidade.Fundamental:
                    case Modalidade.Medio:
                        filtro.Bimestres.AddRange(new int[] { 0, 1, 2, 3, 4, -99 });
                        break;
                    case Modalidade.EJA:
                        filtro.Bimestres.AddRange(new int[] { 0, 1, 2, -99 });
                        break;
                }
            }

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.FaltasFrequencia, filtro, usuario, filtro.TipoFormatoRelatorio));
        }
    }
}
