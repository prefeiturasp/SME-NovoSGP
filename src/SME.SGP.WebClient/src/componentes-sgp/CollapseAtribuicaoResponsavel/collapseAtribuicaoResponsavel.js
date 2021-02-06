import React from 'react';
import PropTypes from 'prop-types';
import CardCollapse from '~/componentes/cardCollapse';
import CollapseAtribuicaoResponsavelDados from './collapseAtribuicaoResponsavelDados';

const CollapseAtribuicaoResponsavel = props => {
  const { changeLocalizadorResponsavel, clickCancelar, codigoTurma } = props;

  return (
    <CardCollapse
      key="responsavel-collapse-key"
      titulo="Atribuição de responsável"
      indice="responsavel-collapse-indice"
      alt="responsavel-alt"
      show
    >
      <CollapseAtribuicaoResponsavelDados
        changeLocalizadorResponsavel={changeLocalizadorResponsavel}
        clickCancelar={clickCancelar}
        codigoTurma={codigoTurma}
      />
    </CardCollapse>
  );
};

CollapseAtribuicaoResponsavel.propTypes = {
  changeLocalizadorResponsavel: PropTypes.func,
  clickCancelar: PropTypes.func,
  codigoTurma: PropTypes.string,
};

CollapseAtribuicaoResponsavel.defaultProps = {
  changeLocalizadorResponsavel: () => {},
  clickCancelar: () => {},
  codigoTurma: '',
};

export default CollapseAtribuicaoResponsavel;
