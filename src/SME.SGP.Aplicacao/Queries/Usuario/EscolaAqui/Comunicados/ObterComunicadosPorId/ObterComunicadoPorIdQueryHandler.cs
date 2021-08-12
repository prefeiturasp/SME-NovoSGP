using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadoPorIdQueryHandler : IRequestHandler<ObterComunicadoPorIdQuery, ComunicadoCompletoDto>
    {
        private const string TODAS = "todas";
        private readonly IRepositorioComunicado repositorioComunicado;
        private readonly IRepositorioComunicadoTurma repositorioComunicadoTurma;
        private readonly IRepositorioComunicadoAluno repositorioComunicadoAluno;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioComunicadoModalidade repositorioComunicadoModalidade;
        private readonly IMediator mediator;

        public ObterComunicadoPorIdQueryHandler(
              IRepositorioComunicado repositorioComunicado
            , IRepositorioComunicadoTurma repositorioComunicadoTurma
            , IRepositorioComunicadoAluno repositorioComunicadoAluno
            , IConsultasAbrangencia consultasAbrangencia
            , IRepositorioComunicadoModalidade repositorioComunicadoModalidade
            , IMediator mediator)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this.repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
            this.repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioComunicadoModalidade = repositorioComunicadoModalidade ?? throw new ArgumentNullException(nameof(repositorioComunicadoModalidade));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ComunicadoCompletoDto> Handle(ObterComunicadoPorIdQuery request, CancellationToken cancellationToken)
        {
            var comunicado = await repositorioComunicado.ObterPorIdAsync(request.Id);

            if (comunicado.Excluido)
                throw new NegocioException("Não é possivel acessar um registro excluido");

            comunicado.Alunos = (await repositorioComunicadoAluno.ObterPorComunicado(comunicado.Id)).ToList();

            if(comunicado.Alunos.Any())
            {
                var alunos = await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(comunicado.Alunos.Select(a => Convert.ToInt64(a.AlunoCodigo)).ToArray(), comunicado.AnoLetivo));
                List<ComunicadoAluno> comunicadosAlunos = new List<ComunicadoAluno>();
                foreach(var aluno in comunicado.Alunos)
                {
                    comunicadosAlunos.Add(new ComunicadoAluno()
                    {
                        ComunicadoId = aluno.ComunicadoId,
                        AlunoCodigo = aluno.AlunoCodigo,
                        AlunoNome = alunos.FirstOrDefault(a => a.CodigoAluno.ToString() == aluno.AlunoCodigo)?.NomeAluno
                    });
                }

                comunicado.Alunos = comunicadosAlunos;
            }
                

            comunicado.Turmas = (await repositorioComunicadoTurma.ObterPorComunicado(comunicado.Id)).ToList();

            comunicado.Modalidades = (await repositorioComunicadoModalidade.ObterModalidadesPorComunicadoId(comunicado.Id)).ToArray();

            comunicado.Modalidades = comunicado.Modalidades.Length == Enum.GetValues(typeof(Modalidade)).Length ? new int[] { -99 } : comunicado.Modalidades;

            var dto = (ComunicadoCompletoDto)comunicado;

            return dto;
        }
    }
}
