using MediatR;
using SME.SGP.Aplicacao.Commands.Autenticacao.DeslogarSuporteUsuario;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGuidAutenticacaoFrequenciaSGA : AbstractUseCase, IObterGuidAutenticacaoFrequenciaSGA
    {
        public ObterGuidAutenticacaoFrequenciaSGA(IMediator mediator) : base(mediator)
        {
        }

        public async Task<Guid> Executar(SolicitacaoGuidAutenticacaoFrequenciaSGADto input)
        {
            var guid = Guid.NewGuid();

            var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(input.Rf));
            if (usuario == null)
                throw new NegocioException($"Usuário não encontrado para o RF: {input.Rf}");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(input.TurmaCodigo));
            if (turma == null)
                throw new NegocioException($"Turma não encontrada para o código: {input.TurmaCodigo}");

            var autenticacaoFrequenciaSGADto = new AutenticacaoFrequenciaSGADto { Rf = input.Rf, ComponenteCurricularCodigo = input.ComponenteCurricularCodigo, Turma = turma };
            await mediator.Send(new SalvarCachePorValorObjetoCommand(string.Format(NomeChaveCache.CHAVE_AUTENTICACAO_FREQUENCIA_SGA, guid), autenticacaoFrequenciaSGADto, 1));
            return guid;
        }
    }
}
