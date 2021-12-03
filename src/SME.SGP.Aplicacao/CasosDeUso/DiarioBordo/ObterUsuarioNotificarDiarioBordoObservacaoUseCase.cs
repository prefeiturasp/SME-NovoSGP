using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioNotificarDiarioBordoObservacaoUseCase : AbstractUseCase, IObterUsuarioNotificarDiarioBordoObservacaoUseCase
    {
        public ObterUsuarioNotificarDiarioBordoObservacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> Executar(ObterUsuarioNotificarDiarioBordoObservacaoDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(dto.TurmaId));
            if (turma is null)
                throw new NegocioException("A turma informada não foi encontrada.");

            var diarioBordo = await mediator.Send(new ObterDiarioDeBordoPorIdQuery(208));//dto.DiarioBordoId););

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            //if (!diarioBordo.Auditoria.CriadoRF.Equals(usuarioLogado.CodigoRf))
                var x =  await mediator.Send(new ObterUsuarioNotificarDiarioBordoObservacaoQuery(turma, ObterProfessorTitular(diarioBordo), dto.ObservacaoId));
            return x;
            //else
              //  return default;
        }

        private List<ProfessorTitularDisciplinaEol> ObterProfessorTitular(DiarioBordoDetalhesDto diarioBordo)
        {
            return new List<ProfessorTitularDisciplinaEol> { new ProfessorTitularDisciplinaEol { ProfessorRf = diarioBordo.Auditoria.CriadoRF, ProfessorNome = diarioBordo.Auditoria.CriadoPor } };
        }
    }
}