import React from 'react';
import PropTypes from 'prop-types';
import CardCollapse from '~/componentes/cardCollapse';
import CollapseLocalizarEstudanteDados from './collapseLocalizarEstudanteDados';

const CollapseLocalizarEstudante = props => {
  const {
    changeDre,
    changeUe,
    changeTurma,
    changeLocalizadorEstudante,
    clickCancelar,
    validarSePermiteProximoPasso,
  } = props;

  return (
    <CardCollapse
      key="localizar-estudante-collapse-key"
      titulo="Localizar estudante"
      indice="localizar-estudante-collapse-indice"
      alt="localizar-estudante-alt"
      show
    >
      <CollapseLocalizarEstudanteDados
        changeDre={changeDre}
        changeUe={changeUe}
        changeTurma={changeTurma}
        changeLocalizadorEstudante={changeLocalizadorEstudante}
        clickCancelar={clickCancelar}
        validarSePermiteProximoPasso={validarSePermiteProximoPasso}
      />
    </CardCollapse>
  );
};

CollapseLocalizarEstudante.propTypes = {
  changeDre: PropTypes.func,
  changeUe: PropTypes.func,
  changeTurma: PropTypes.func,
  changeLocalizadorEstudante: PropTypes.func,
  clickCancelar: PropTypes.func,
  validarSePermiteProximoPasso: PropTypes.func,
};

CollapseLocalizarEstudante.defaultProps = {
  changeDre: () => {},
  changeUe: () => {},
  changeTurma: () => {},
  changeLocalizadorEstudante: () => {},
  clickCancelar: () => {},
  validarSePermiteProximoPasso: null,
};

export default CollapseLocalizarEstudante;
