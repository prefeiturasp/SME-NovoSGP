using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesAnoUseCase : IObterModalidadesAnoUseCase
    {
        private readonly IMediator mediator;

        public ObterModalidadesAnoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ModalidadesPorAnoDto>> Executar(List<string> anos)
        {
            var modalidades = await mediator.Send(new ObterModalidadesPorAnosQuery(anos));
            return ObterDescricoesPorModalidade(modalidades);
        }

        private static List<ModalidadesPorAnoDto> ObterDescricoesPorModalidade(IEnumerable<ModalidadesPorAnoDto> modalidades)
        {
            var resultado = new List<ModalidadesPorAnoDto>();
            foreach (var item in modalidades)
            {
                item.Modalidade = ObterModalidade(item.Modalidade);
                resultado.Add(item);
            }
            return resultado;
        }

        private static string ObterModalidade(string modalidade)
        {
            switch (modalidade)
            {
                case "1":
                    modalidade = Modalidade.InfantilPreEscola.ToString();
                    break;
                case "2":
                    modalidade = Modalidade.InfantilCEI.ToString();
                    break;
                case "3":
                    modalidade = Modalidade.EJA.ToString();
                    break;
                case "4":
                    modalidade = Modalidade.CIEJA.ToString();
                    break;
                case "5":
                    modalidade = Modalidade.Fundamental.ToString();
                    break;
                case "6":
                    modalidade = Modalidade.Medio.ToString();
                    break;
                case "7":
                    modalidade = Modalidade.CMCT.ToString();
                    break;
                case "8":
                    modalidade = Modalidade.MOVA.ToString();
                    break;
                case "9":
                    modalidade = Modalidade.ETEC.ToString();
                    break;
                default:
                    break;
            }
            return modalidade;
        }
    }
}
