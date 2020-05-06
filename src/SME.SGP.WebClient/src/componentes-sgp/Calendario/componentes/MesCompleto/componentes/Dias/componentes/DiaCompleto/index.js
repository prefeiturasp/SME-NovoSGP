import React, { useMemo, useCallback } from 'react';
import shortid from 'shortid';
import t from 'prop-types';

// Estilos
import { DiaCompletoWrapper, LinhaEvento } from './styles';

// Componentes
import { Loader } from '~/componentes';

// Componentes internos
import AlertaDentroPeriodo from './componentes/AlertaPeriodoEncerrado';
import LabelAulaEvento from './componentes/LabelAulaEvento';
import BotoesAuxiliares from './componentes/BotoesAuxiliares';
import SemEventos from './componentes/SemEventos';
import BotaoAvaliacoes from './componentes/BotaoAvaliacoes';
import BotaoFrequencia from './componentes/BotaoFrequencia';

// Utils
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

// Serviços
import history from '~/servicos/history';

// DTOs
import RotasDTO from '~/dtos/rotasDto';

function DiaCompleto({
  dia,
  eventos,
  carregandoDia,
  diasPermitidos,
  permissaoTela,
  tipoCalendarioId,
}) {
  const deveExibir = useMemo(
    () => dia && !!diasPermitidos.find(x => x.toString() === dia.toString()),
    [dia, diasPermitidos]
  );

  const dadosDia = useMemo(() => {
    return eventos.length > 0 && dia instanceof Date
      ? eventos.filter(diaAtual => diaAtual.dia === dia.getDate())[0]
      : null;
  }, [dia, eventos]);

  const onClickNovaAulaHandler = useCallback(
    diaSelecionado => {
      history.push(
        `${RotasDTO.CADASTRO_DE_AULA}/novo/${tipoCalendarioId}/${diaSelecionado}`
      );
    },
    [tipoCalendarioId]
  );

  return (
    <DiaCompletoWrapper className={`${deveExibir && `visivel`}`}>
      {deveExibir && (
        <Loader loading={carregandoDia} tip="Carregando...">
          <AlertaDentroPeriodo
            exibir={dadosDia.dados && !!dadosDia.dados.mensagemPeriodoEncerrado}
          />
          {dadosDia && dadosDia.dados && dadosDia.dados.eventosAulas.length > 0
            ? dadosDia.dados.eventosAulas.map(eventoAula => (
                <LinhaEvento
                  key={shortid.generate()}
                  className={`${!eventoAula.ehAula && `evento`}`}
                >
                  <div className="labelEventoAula">
                    <LabelAulaEvento dadosEvento={eventoAula} />
                  </div>
                  <div className="tituloEventoAula">{eventoAula.titulo}</div>
                  <div className="botoesEventoAula">
                    {eventoAula &&
                      eventoAula.ehAula &&
                      !eventoAula.mostrarBotaoFrequencia && (
                        <BotaoFrequencia onClickFrequencia={() => null} />
                      )}
                    {eventoAula &&
                      eventoAula.atividadesAvaliativas.length > 0 && (
                        <BotaoAvaliacoes
                          atividadesAvaliativas={
                            eventoAula.atividadesAvaliativas
                          }
                          permissaoTela={permissaoTela}
                        />
                      )}
                  </div>
                </LinhaEvento>
              ))
            : !carregandoDia && <SemEventos />}
          <BotoesAuxiliares
            temAula={
              dadosDia.dados &&
              dadosDia.dados.eventosAulas.length > 0 &&
              dadosDia.dados.eventosAulas.filter(evento => evento.ehAula)
                .length > 0
            }
            podeCadastrarAvaliacao={
              dadosDia.dados &&
              typeof dadosDia.dados.eventosAulas === 'array' &&
              dadosDia.dados.eventosAulas.filter(
                evento =>
                  evento.ehAula && evento.dadosAula.podeCadastrarAvaliacao
              ).length > 0
            }
            onClickNovaAula={() =>
              onClickNovaAulaHandler(window.moment(dia).format('YYYY-MM-DD'))
            }
            onClickNovaAvaliacao={() => {}}
            permissaoTela={permissaoTela}
            dentroPeriodo={
              dadosDia.dados &&
              valorNuloOuVazio(dadosDia.dados.mensagemPeriodoEncerrado)
            }
            desabilitado={carregandoDia}
          />
        </Loader>
      )}
    </DiaCompletoWrapper>
  );
}

DiaCompleto.propTypes = {
  dia: t.oneOfType([t.any]),
  eventos: t.oneOfType([t.any]),
  diasPermitidos: t.oneOfType([t.any]),
  carregandoDia: t.bool,
  permissaoTela: t.oneOfType([t.any]),
  tipoCalendarioId: t.oneOfType([t.any]),
};

DiaCompleto.defaultProps = {
  dia: {},
  eventos: {},
  diasPermitidos: [],
  carregandoDia: false,
  permissaoTela: {},
  tipoCalendarioId: null,
};

export default DiaCompleto;
