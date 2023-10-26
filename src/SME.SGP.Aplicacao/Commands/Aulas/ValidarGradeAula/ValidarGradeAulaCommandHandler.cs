﻿using MediatR;
using SME.SGP.Dominio;
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
            {
                var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
                if (request.AulasExistentes.NaoEhNulo() && request.AulasExistentes.Any(c => c.TipoAula != TipoAula.Reposicao) && !(usuarioLogado.EhProfessorCj() || usuarioLogado.EhProfessorCjInfantil()))
                {
                    if (request.TurmaModalidade == Modalidade.EJA)
                        return (false, "Para regência de EJA só é permitido a criação de 5 aulas por dia.");
                    else return (false, "Para regência de classe só é permitido a criação de 1 (uma) aula por dia.");
                }
            }
            else
            {
                var gradeAulas = await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(request.TurmaCodigo, request.ComponenteCurricularesCodigo, request.Data, request.UsuarioRf, request.EhRegencia, request.EhGestor));
                var quantidadeAulasRestantes = gradeAulas.EhNulo() ? int.MaxValue : gradeAulas.QuantidadeAulasRestante;

                if (gradeAulas.NaoEhNulo())
                {
                    if (quantidadeAulasRestantes < request.Quantidade)
                        return (false, "Quantidade de aulas superior ao limíte de aulas da grade.");
                    if (!gradeAulas.PodeEditar && (request.Quantidade != gradeAulas.QuantidadeAulasRestante))
                        return (false, "Quantidade de aulas não pode ser diferente do valor da grade curricular.");
                }
            }

            return (true, string.Empty);
        }
    }
}
