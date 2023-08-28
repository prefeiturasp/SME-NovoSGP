using MediatR;
using Minio.DataModel;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCartaIntencoesNotificacaoQueryHandler : IRequestHandler<ObterCartaIntencoesNotificacaoQuery, IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>>
    {
        private readonly IMediator mediator;

        public ObterCartaIntencoesNotificacaoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>> Handle(ObterCartaIntencoesNotificacaoQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            if (turma is null)
                throw new NegocioException("A turma informada não foi encontrada.");

            var ue = await mediator.Send(new ObterUePorIdQuery(turma.UeId));
            
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var professorTitular = await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(turma.CodigoTurma, request.ComponenteCurricular));

            var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesDasTurmasQuery(new List<string> { turma.CodigoTurma }));

            var funcionariosDiretor = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(ue.CodigoUe, (int)Cargo.Diretor));

            IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto> professoresNotificar = new List<UsuarioNotificarCartaIntencoesObservacaoDto>();

            if (funcionariosDiretor != null && funcionariosDiretor.Any(d => d.CodigoRF == usuarioLogado.CodigoRf) && professoresDaTurma.Any())
            {
                foreach (var professor in professoresDaTurma)
                {                    
                    var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(professor));
                    if (usuario == null)
                        throw new NegocioException("Usuário não encontrado no SGP");

                    var usuariosNotificar = await mediator.Send(new ObterUsuariosNotificarCartaIntencoesObservacaoQuery(ObterProfessorTitular(usuario.CodigoRf, usuario.Nome)));
                    professoresNotificar = professoresNotificar.Concat(usuariosNotificar.ToList());                        
                }

                return professoresNotificar;
            }


            if (professorTitular.ProfessorRf.Equals(usuarioLogado.CodigoRf))
            {                
                var segundoTitular = professoresDaTurma.FirstOrDefault(p => p != usuarioLogado.CodigoRf);
                var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(segundoTitular));
                if (usuario == null)
                    throw new NegocioException("Usuário não encontrado no SGP");

                return await mediator.Send(new ObterUsuariosNotificarCartaIntencoesObservacaoQuery(ObterProfessorTitular(segundoTitular, usuario.Nome)));

            }
            else if(professoresDaTurma.Any(p => p == usuarioLogado.CodigoRf))
            {
                return await mediator.Send(new ObterUsuariosNotificarCartaIntencoesObservacaoQuery(ObterProfessorTitular(professorTitular.ProfessorRf, professorTitular.ProfessorNome)));
            }
            else
                return default;

        }

        private List<ProfessorTitularDisciplinaEol> ObterProfessorTitular(string codigoRf, string nome)
        {
            return new List<ProfessorTitularDisciplinaEol> { new ProfessorTitularDisciplinaEol { ProfessorRf = codigoRf, ProfessorNome = nome } };
        }
    }
}
