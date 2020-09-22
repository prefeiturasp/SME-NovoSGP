import React from 'react';
import t from 'prop-types';

// Ant
import { Tooltip } from 'antd';

// Estilos
import {
  DiaWrapper,
  TipoEventosLista,
  TipoEvento,
  IconeAtividadeAvaliativa,
} from './styles';

// Componentes
import { Base } from '~/componentes';
import { IconeDiaComPendencia } from '../../styles';

function Dia({
  dia,
  eventos,
  selecionado,
  numeroDia,
  mesSelecionado,
  onClick,
}) {
  return (
    <DiaWrapper
      numeroDia={numeroDia}
      mesAtual={Number(mesSelecionado.numeroMes) === Number(dia.getMonth() + 1)}
      selecionado={selecionado}
      className="col"
      onClick={() => onClick()}
    >
      <div className="numeroDia">
        <div>
          {eventos?.possuiPendencia && (
            <Tooltip title="Dia com pendência">
              <IconeDiaComPendencia className="fas fa-exclamation-triangle" />
            </Tooltip>
          )}
        </div>
        <div style={{ flexDirection: 'row', display: 'flex' }}>
          <div>{dia.getDate() < 10 ? `0${dia.getDate()}` : dia.getDate()}</div>
          <div className="ml-2">
            {eventos?.temAvaliacao && (
              <Tooltip title="Atividade avaliativa">
                <IconeAtividadeAvaliativa className="fas fa-sticky-note" />
              </Tooltip>
            )}
          </div>
        </div>
      </div>
      {eventos && (
        <TipoEventosLista className="position-absolute">
          {eventos.temAula && <TipoEvento cor={Base.Roxo}>Aula</TipoEvento>}
          {eventos.temAulaCJ && <TipoEvento cor={Base.Laranja}>CJ</TipoEvento>}
          {eventos.temEvento && (
            <TipoEvento cor={Base.RoxoEventoCalendario}>Evento</TipoEvento>
          )}
        </TipoEventosLista>
      )}
    </DiaWrapper>
  );
}

Dia.propTypes = {
  dia: t.oneOfType([t.any]),
  eventos: t.oneOfType([t.any]),
  selecionado: t.bool,
  numeroDia: t.number,
  mesSelecionado: t.oneOfType([t.any]),
  onClick: t.func,
};

Dia.defaultProps = {
  dia: {},
  eventos: {},
  selecionado: false,
  numeroDia: null,
  mesSelecionado: {},
  onClick: () => {},
};

export default Dia;
