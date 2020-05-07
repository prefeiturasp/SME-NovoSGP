import React, { useCallback } from 'react';
import t from 'prop-types';

// Estilos
import { DivMes, DivWrapperMes } from './styles';

function Mes({ tipoCalendarioId, onClickMes, mes }) {
  const handleOnClick = useCallback(() => {
    if (tipoCalendarioId) onClickMes(mes);
  }, [mes, onClickMes, tipoCalendarioId]);

  return (
    <DivWrapperMes className={`${mes.estaAberto ? 'aberto' : ''}`}>
      <DivMes
        onClick={handleOnClick}
        onKeyDown={handleOnClick}
        role="button"
        tabIndex={0}
        style={{
          cursor: tipoCalendarioId ? 'pointer' : 'not-allowed',
        }}
      >
        <div className="seta">
          <i className="iconeSeta fas fa-chevron-right" />
        </div>
        <div className="d-flex align-items-center w-100">
          <div className="w-100 pl-2 fonte-16">{mes.nome}</div>
          <div className="flex-shrink-1 d-flex align-items-center pr-3">
            <div className="fonte-14">
              <i className="far fa-calendar-alt" />
            </div>
          </div>
        </div>
      </DivMes>
    </DivWrapperMes>
  );
}

Mes.propTypes = {
  tipoCalendarioId: t.string,
  onClickMes: t.func,
  mes: t.oneOfType([t.any]),
};

Mes.defaultProps = {
  tipoCalendarioId: null,
  onClickMes: () => {},
  mes: {},
};

export default Mes;
