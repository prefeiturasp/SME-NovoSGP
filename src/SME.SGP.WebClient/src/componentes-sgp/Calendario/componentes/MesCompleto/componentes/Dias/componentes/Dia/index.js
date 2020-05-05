import React, { useEffect } from 'react';
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
      onClick={onClick}
    >
      <div className="numeroDia">
        <div>
          {eventos && eventos.temAvaliacao && (
            <Tooltip title="Atividade avaliativa">
              <IconeAtividadeAvaliativa className="fas fa-sticky-note" />
            </Tooltip>
          )}
        </div>
        <div>{dia.getDate() < 10 ? `0${dia.getDate()}` : dia.getDate()}</div>
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
