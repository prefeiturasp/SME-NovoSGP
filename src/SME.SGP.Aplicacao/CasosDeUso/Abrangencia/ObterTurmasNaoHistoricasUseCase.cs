using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasNaoHistoricasUseCase : AbstractUseCase, IObterTurmasNaoHistoricasUseCase
    {
        public ObterTurmasNaoHistoricasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<TurmaNaoHistoricaDto>> Executar()
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var perfil = await mediator.Send(new ObterPerfilAtualQuery());
            var possuiAbrangenciaUE = usuario.EhAbrangenciaUEouProfessor();

            if (possuiAbrangenciaUE)
            {
                var obterDadosUe = await mediator.Send(new ObterUePorLoginPerfilProfessorOuAbrangenciaUeQuery(usuario.Login, perfil));
                int anoLetivo = DateTime.Now.Year;
                return await mediator.Send(new ObterTurmasPorAnoAtualECodigoUeQuery(obterDadosUe.Codigo, anoLetivo));
            }
            else
                return null;
        }
    }
}
