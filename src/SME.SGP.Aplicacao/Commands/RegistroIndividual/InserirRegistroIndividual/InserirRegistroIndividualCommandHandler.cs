using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroIndividualCommandHandler : IRequestHandler<InserirRegistroIndividualCommand, RegistroIndividual>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public InserirRegistroIndividualCommandHandler(IMediator mediator,
                                                       IRepositorioRegistroIndividual repositorioRegistroIndividual)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioRegistroIndividual));
        }

        public async Task<RegistroIndividual> Handle(InserirRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));

            var componenteCurricular = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { request.ComponenteCurricularId }));

            if (componenteCurricular == null || !componenteCurricular.Any())
                throw new NegocioException(MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_NAO_ENCONTRADO);
            
            var registroExistente = await repositorioRegistroIndividual.ObterPorAlunoData(request.TurmaId, request.ComponenteCurricularId, request.AlunoCodigo, request.DataRegistro);

            if (registroExistente != null)
                throw new NegocioException(MensagemNegocioRegistroIndividual.JA_EXISTE_REGISTRO_PARA_ALUNO_DA_TURMA_NESTA_DATA);
            
            await MoverArquivos(request);
            
            var registroIndividual = MapearParaEntidade(request);
            
            await repositorioRegistroIndividual.SalvarAsync(registroIndividual);

            return registroIndividual;
        }
        private async Task MoverArquivos(InserirRegistroIndividualCommand novo)
        {
            if (!string.IsNullOrEmpty(novo.Registro))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.RegistroIndividual, string.Empty, novo.Registro));
                novo.Registro = moverArquivo;
            }
        }
        private RegistroIndividual MapearParaEntidade(InserirRegistroIndividualCommand request)
            => new RegistroIndividual()
            {
                AlunoCodigo = request.AlunoCodigo,
                ComponenteCurricularId = request.ComponenteCurricularId,
                DataRegistro = request.DataRegistro,
                Registro = request.Registro,
                TurmaId = request.TurmaId
            };
    }
}
