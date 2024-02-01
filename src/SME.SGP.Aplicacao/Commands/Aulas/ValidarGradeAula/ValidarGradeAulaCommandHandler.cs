using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidarGradeAulaCommandHandler: IRequestHandler<ValidarGradeAulaCommand, (bool resultado, string mensagem)>
    {
        private readonly IMediator mediator;

        public ValidarGradeAulaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<(bool resultado, string mensagem)> Handle(ValidarGradeAulaCommand request, CancellationToken cancellationToken)
        {
            if (request.EhRegencia)
                return await ValidarGradeAulaRegencia(request);
            
            var gradeAulas = await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(request.TurmaCodigo, request.ComponenteCurricularesCodigo, request.Data, request.UsuarioRf, request.EhRegencia, request.EhGestor));
            return ValidarGradeAula(gradeAulas, request.Quantidade);
        }

        private (bool resultado, string mensagem) ValidarGradeAula(GradeComponenteTurmaAulasDto gradeAulas, int quantidadeAulas)
        {
            if (gradeAulas.NaoEhNulo())
            {
                if (gradeAulas.QuantidadeAulasRestante < quantidadeAulas)
                    return (false, "Quantidade de aulas superior ao limíte de aulas da grade.");
                if (!gradeAulas.PodeEditar && (quantidadeAulas != gradeAulas.QuantidadeAulasRestante))
                    return (false, "Quantidade de aulas não pode ser diferente do valor da grade curricular.");
            }

            return (true, string.Empty);
        }

        private async Task<(bool resultado, string mensagem)> ValidarGradeAulaRegencia(ValidarGradeAulaCommand request)
        {
            if (request.AulasExistentes.PossuiRegistros(c => c.TipoAula != TipoAula.Reposicao) 
                && await UsuarioLogadoNaoEhProfCJ())
                return (false, ObterMsgQdadeAulasGradeRegencia(request.TurmaModalidade));
            return (true, string.Empty);
        }

        private async Task<bool> UsuarioLogadoEhProfCJ()
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            return usuarioLogado.EhProfessorCj() || usuarioLogado.EhProfessorCjInfantil();
        }

        private async Task<bool> UsuarioLogadoNaoEhProfCJ() => !(await UsuarioLogadoEhProfCJ());

        private string ObterMsgQdadeAulasGradeRegencia(Modalidade modalidade)
        {
            if (modalidade == Modalidade.EJA)
                return "Para regência de EJA só é permitido a criação de 5 aulas por dia.";
            return "Para regência de classe só é permitido a criação de 1 (uma) aula por dia.";
        }
    }
}
