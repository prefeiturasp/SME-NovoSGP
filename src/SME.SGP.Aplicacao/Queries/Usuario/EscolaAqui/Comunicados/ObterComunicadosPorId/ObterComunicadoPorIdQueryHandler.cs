using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadoPorIdQueryHandler : IRequestHandler<ObterComunicadoPorIdQuery, ComunicadoCompletoDto>
    {
        private const string TODAS = "todas";
        private readonly IRepositorioComunicado _repositorioComunicado;        
        private readonly IRepositorioComunicadoTurma _repositorioComunicadoTurma;
        private readonly IRepositorioComunicadoAluno _repositorioComunicadoAluno;
        private readonly IConsultasAbrangencia _consultasAbrangencia;
        private readonly IRepositorioComunicadoModalidade repositorioComunicadoModalidade;

        public ObterComunicadoPorIdQueryHandler(
              IRepositorioComunicado repositorioComunicado
            , IRepositorioComunicadoTurma repositorioComunicadoTurma
            , IRepositorioComunicadoAluno repositorioComunicadoAluno
            , IConsultasAbrangencia consultasAbrangencia
            , IRepositorioComunicadoModalidade repositorioComunicadoModalidade)
        {
            this._repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this._repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
            this._repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
            this._consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioComunicadoModalidade = repositorioComunicadoModalidade ?? throw new ArgumentNullException(nameof(repositorioComunicadoModalidade));
        }

        public async Task<ComunicadoCompletoDto> Handle(ObterComunicadoPorIdQuery request, CancellationToken cancellationToken)
        {
            var comunicado = await _repositorioComunicado.ObterPorIdAsync(request.Id);

            if (comunicado.Excluido)
                throw new NegocioException("Não é possivel acessar um registro excluido");

            comunicado.Alunos = (await _repositorioComunicadoAluno.ObterPorComunicado(comunicado.Id)).ToList();

            comunicado.Turmas = (await _repositorioComunicadoTurma.ObterPorComunicado(comunicado.Id)).ToList();

            comunicado.Modalidades = (await repositorioComunicadoModalidade.ObterModalidadesPorComunicadoId(comunicado.Id)).ToArray();

            comunicado.Modalidades = comunicado.Modalidades.Length == Enum.GetValues(typeof(Modalidade)).Length ? new int[] { -99 } : comunicado.Modalidades;

            var dto = (ComunicadoCompletoDto)comunicado;            

            return dto;
        }
    }
}
