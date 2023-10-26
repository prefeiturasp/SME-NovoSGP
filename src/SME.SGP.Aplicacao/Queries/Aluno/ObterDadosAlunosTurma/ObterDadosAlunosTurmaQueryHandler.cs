using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosAlunosTurmaQueryHandler : IRequestHandler<ObterDadosAlunosTurmaQuery, IEnumerable<AlunoDadosBasicosDto>>
    {
        private readonly IMediator mediator;

        public ObterDadosAlunosTurmaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> Handle(ObterDadosAlunosTurmaQuery request, CancellationToken cancellationToken)
        {
            var dadosAlunos = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(request.TurmaCodigo, DateTime.Today));

            if (dadosAlunos.EhNulo() || !dadosAlunos.Any())
                throw new NegocioException($"Não foram localizados dados dos alunos para turma {request.TurmaCodigo} no EOL para o ano letivo {request.AnoLetivo}");

            var dadosAlunosDto = new List<AlunoDadosBasicosDto>();

            foreach (var dadoAluno in dadosAlunos)
            {
                var dadosBasicos = (AlunoDadosBasicosDto)dadoAluno;
                dadosBasicos.DataMatricula = dadoAluno.DataMatricula;

                if ((TipoResponsavel)Convert.ToInt32(dadoAluno.TipoResponsavel) == TipoResponsavel.ProprioEstudante &&
                    !string.IsNullOrEmpty(dadoAluno.NomeSocialAluno) && dadoAluno.Maioridade)
                {
                    dadosBasicos.NomeResponsavel = dadoAluno.NomeSocialAluno;
                }

                dadosBasicos.TipoResponsavel = ObterTipoResponsavel(dadoAluno.TipoResponsavel);
                // se informado periodo escolar carrega marcadores no periodo
                if (request.PeriodoEscolar.NaoEhNulo())
                    dadosBasicos.Marcador = await mediator.Send(new ObterMarcadorAlunoQuery(dadoAluno, request.PeriodoEscolar.PeriodoInicio, request.EhInfantil));

                dadosBasicos.EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(dadoAluno.CodigoAluno, request.AnoLetivo));

                dadosAlunosDto.Add(dadosBasicos);
            }

            return dadosAlunosDto;
        }

        private string ObterTipoResponsavel(string tipoResponsavel)
        {
            return !string.IsNullOrEmpty(tipoResponsavel) ?
                 ((TipoResponsavel)Enum.Parse(typeof(TipoResponsavel), tipoResponsavel)).Name() :
                 TipoResponsavel.Filicacao1.Name();
        }
    }
}
