using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarIdepPainelEducacionalUseCase : AbstractUseCase, IConsolidarIdepPainelEducacionalUseCase
    {
        private readonly IRepositorioIdepPainelEducacionalConsulta repositorioIdep;

        public ConsolidarIdepPainelEducacionalUseCase(IMediator mediator, IRepositorioIdepPainelEducacionalConsulta repositorioIdep) : base(mediator)
        {
            this.repositorioIdep = repositorioIdep;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var registrosIdep = repositorioIdep.ObterTodosIdep();

            //Precisa usar a função Inserir() do RepositorioIdepPainelEducacionalConsolidacao
            //Porem precisa ver o padrao a ser seguido, se vai usar mediator ou  direto o repositorio


            //await repositorioIdep.Inserir(registrosIdep);

            throw new NotImplementedException();
        }
    }
}
