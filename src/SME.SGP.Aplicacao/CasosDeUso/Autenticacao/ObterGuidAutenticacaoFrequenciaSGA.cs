using MediatR;
using SME.SGP.Aplicacao.Commands.Autenticacao.DeslogarSuporteUsuario;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGuidAutenticacaoFrequenciaSGA : AbstractUseCase, IObterGuidAutenticacaoFrequenciaSGA
    {
        private readonly IServicoAutenticacao servicoAutenticacao;

        public ObterGuidAutenticacaoFrequenciaSGA(IMediator mediator, IServicoAutenticacao servicoAutenticacao) : base(mediator)
        {
            this.servicoAutenticacao = servicoAutenticacao ?? throw new ArgumentNullException(nameof(servicoAutenticacao));
        }

        public async Task<Guid> Executar(SolicitacaoGuidAutenticacaoFrequenciaSGADto input)
        {
            var guid = Guid.NewGuid();

            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEolSemSenha(input.Rf);
            if (!retornoAutenticacaoEol.Item1.Autenticado)
                throw new NegocioException("", 401);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(input.TurmaCodigo));
            if (turma == null)
                throw new NegocioException($"Turma não encontrada para o código: {input.TurmaCodigo}");


            var autenticacaoFrequenciaSGADto = new AutenticacaoFrequenciaSGADto { Rf = input.Rf, 
                                                                                  ComponenteCurricularCodigo = input.ComponenteCurricularCodigo, 
                                                                                  Turma = turma, 
                                                                                  usuarioAutenticado = retornoAutenticacaoEol };
            await mediator.Send(new SalvarCachePorValorObjetoCommand(string.Format(NomeChaveCache.CHAVE_AUTENTICACAO_FREQUENCIA_SGA, guid), autenticacaoFrequenciaSGADto, 1));
            return guid;
        }
    }
}
