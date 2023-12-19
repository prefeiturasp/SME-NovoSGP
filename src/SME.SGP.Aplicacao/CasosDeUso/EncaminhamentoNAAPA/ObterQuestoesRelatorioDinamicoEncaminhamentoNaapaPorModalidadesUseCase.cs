using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Questionario;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesRelatorioDinamicoEncaminhamentoNaapaPorModalidadesUseCase : IObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesUseCase
    {
        private readonly IMediator mediator;
        private const string OPCAO_TODOS = "-99";

        public ObterQuestoesRelatorioDinamicoEncaminhamentoNaapaPorModalidadesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<SecaoQuestoesDTO>> Executar(int[] modalidadesId)
        {
            if (modalidadesId.PossuiRegistros(x => x.ToString().Equals(OPCAO_TODOS)))
                modalidadesId = new int[] {};
            return
                await mediator
                .Send(new ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery(modalidadesId));
        }
    }
}
