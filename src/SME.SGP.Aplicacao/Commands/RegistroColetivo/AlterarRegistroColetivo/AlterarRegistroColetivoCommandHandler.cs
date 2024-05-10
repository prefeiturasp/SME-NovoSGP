using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRegistroColetivoCommandHandler : IRequestHandler<AlterarRegistroColetivoCommand, ResultadoRegistroColetivoDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRegistroColetivo repositorio;
        private readonly IUnitOfWork unitOfWork;

        public AlterarRegistroColetivoCommandHandler(IMediator mediator,
                                                     IRepositorioRegistroColetivo repositorio,
                                                     IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ResultadoRegistroColetivoDto> Handle(AlterarRegistroColetivoCommand request, CancellationToken cancellationToken)
        {
            var registroColetivo = await repositorio.ObterPorIdAsync(request.RegistroColetivo.Id.GetValueOrDefault());

            if (registroColetivo.EhNulo())
                throw new NegocioException(MensagemNegocioComuns.REGISTRO_COLETIVO_NAO_ENCONTRADA);

            CarregaAlteracoes(registroColetivo, request.RegistroColetivo);

            unitOfWork.IniciarTransacao();
            try
            {
                await repositorio.SalvarAsync(registroColetivo);

                await AlterarUes(registroColetivo.Id, request.RegistroColetivo.UeIds);

                await AlterarAnexos(registroColetivo.Id, request.RegistroColetivo.Anexos ?? new List<AnexoDto>());

                unitOfWork.PersistirTransacao();

                return new ResultadoRegistroColetivoDto()
                {
                    Id = registroColetivo.Id,
                    Auditoria = new AuditoriaDto()
                    {
                        CriadoPor = registroColetivo.CriadoPor,
                        CriadoRF = registroColetivo.CriadoRF,
                        AlteradoEm = registroColetivo.AlteradoEm,
                        AlteradoPor = registroColetivo.AlteradoPor,
                        AlteradoRF = registroColetivo.AlteradoRF
                    }
                };
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
            }

            return null;
        }

        private async Task AlterarUes(long registroColetivoId, IEnumerable<long> uesAlterados)
        {
            await mediator.Send(new RemoverRegistroColetivoUeCommand(registroColetivoId));

            await mediator.Send(new InserirRegistroColetivoUeCommand(registroColetivoId, uesAlterados));
        }

        private async Task AlterarAnexos(long registroColetivoId, IEnumerable<AnexoDto> anexosAlterados)
        {
            var anexosCadastrados = await mediator.Send(new ObterAnexosRegistroColetivoQuery(registroColetivoId));

            await mediator.Send(new RemoverRegistroColetivoAnexoCommand(registroColetivoId));

            await RemoverArquivos(anexosCadastrados, anexosAlterados);

            if (anexosAlterados.Any())
                await mediator.Send(new InserirRegistroColetivoAnexoCommand(registroColetivoId, anexosAlterados));
        }

        private async Task RemoverArquivos(IEnumerable<AnexoDto> anexosCadastrados, IEnumerable<AnexoDto> anexosAlterados)
        {
            var arquivosIdsAlterados = anexosAlterados.Where(anexo => anexo.ArquivoId.NaoEhNulo()).Select(anexo => anexo.ArquivoId.GetValueOrDefault()).ToList();
            var arquivosIdsCadastro = anexosCadastrados.Select(anexo => anexo.ArquivoId.GetValueOrDefault()).ToList();
            var arquivosIdsRemovidos = arquivosIdsCadastro.Except(arquivosIdsAlterados);

            foreach(var arquivoId in arquivosIdsRemovidos)
                await mediator.Send(new ExcluirArquivoPorIdCommand(arquivoId));
        }
        
        private void CarregaAlteracoes(RegistroColetivo registro, RegistroColetivoDto registroDto)
        {
            registro.TipoReuniaoId = registroDto.TipoReuniaoId;
            registro.DataRegistro = registroDto.DataRegistro;
            registro.QuantidadeParticipantes = registroDto.QuantidadeParticipantes;
            registro.QuantidadeCuidadores = registroDto.QuantidadeCuidadores;
            registro.QuantidadeEducadores = registroDto.QuantidadeEducadores;
            registro.QuantidadeEducandos = registroDto.QuantidadeEducandos;
            registro.Descricao = registroDto.Descricao;
            registro.Observacao = registroDto.Observacao;
        }
    }
}
