import React from 'react';
import PropTypes from 'prop-types';
import CardCollapse from '~/componentes/cardCollapse';
import CollapseAtribuicaoResponsavelDados from './collapseAtribuicaoResponsavelDados';

const CollapseAtribuicaoResponsavel = props => {
  const {
    validarAntesProximoPasso,
    changeLocalizadorResponsavel,
    clickCancelar,
    codigoTurma,
    url,
  } = props;

  return (
    <CardCollapse
      key="responsavel-collapse-key"
      titulo="Atribuição de responsável"
      indice="responsavel-collapse-indice"
      alt="responsavel-alt"
      show
    >
      <CollapseAtribuicaoResponsavelDados
        validarAntesProximoPasso={validarAntesProximoPasso}
        changeLocalizadorResponsavel={changeLocalizadorResponsavel}
        clickCancelar={clickCancelar}
        codigoTurma={codigoTurma}
        url={url}
      />
    </CardCollapse>
  );
};

CollapseAtribuicaoResponsavel.propTypes = {
  validarAntesProximoPasso: PropTypes.func,
  changeLocalizadorResponsavel: PropTypes.func,
  clickCancelar: PropTypes.func,
  codigoTurma: PropTypes.string,
  url: PropTypes.string,
};

CollapseAtribuicaoResponsavel.defaultProps = {
  validarAntesProximoPasso: () => {},
  changeLocalizadorResponsavel: () => {},
  clickCancelar: () => {},
  codigoTurma: '',
  url: '',
};

export default CollapseAtribuicaoResponsavel;
