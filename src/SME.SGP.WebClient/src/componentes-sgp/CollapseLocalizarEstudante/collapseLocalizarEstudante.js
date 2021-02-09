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
    clickProximoPasso,
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
        clickProximoPasso={() => {
          document
            .getElementById(
              'expandir-retrair-localizar-estudante-collapse-indice'
            )
            .click();
          clickProximoPasso();
        }}
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
  clickProximoPasso: PropTypes.func,
};

CollapseLocalizarEstudante.defaultProps = {
  changeDre: () => {},
  changeUe: () => {},
  changeTurma: () => {},
  changeLocalizadorEstudante: () => {},
  clickCancelar: () => {},
  clickProximoPasso: () => {},
};

export default CollapseLocalizarEstudante;
