using MediatR;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.TesteIntegracao.RegistroIndividual.ServicosFakes
{
    public class MoverArquivosTemporariosCommandHandlerFake : IRequestHandler<MoverArquivosTemporariosCommand, string>
    {
        public async Task<string> Handle(MoverArquivosTemporariosCommand request, CancellationToken cancellationToken)
        {
            return request.TextoEditorNovo;
        }
    }
}
