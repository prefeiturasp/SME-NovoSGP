import React from 'react';
import PropTypes from 'prop-types';
import CardCollapse from '~/componentes/cardCollapse';
import CollapseAtribuicaoResponsavelDados from './collapseAtribuicaoResponsavelDados';

const CollapseAtribuicaoResponsavel = props => {
  const {
    validarAntesAtribuirResponsavel,
    changeLocalizadorResponsavel,
    clickCancelar,
    clickRemoverResponsavel,
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
        validarAntesAtribuirResponsavel={validarAntesAtribuirResponsavel}
        changeLocalizadorResponsavel={changeLocalizadorResponsavel}
        clickCancelar={clickCancelar}
        codigoTurma={codigoTurma}
        url={url}
        clickRemoverResponsavel={clickRemoverResponsavel}
      />
    </CardCollapse>
  );
};

CollapseAtribuicaoResponsavel.propTypes = {
  validarAntesAtribuirResponsavel: PropTypes.func,
  changeLocalizadorResponsavel: PropTypes.func,
  clickCancelar: PropTypes.func,
  clickRemoverResponsavel: PropTypes.func,
  codigoTurma: PropTypes.string,
  url: PropTypes.string,
};

CollapseAtribuicaoResponsavel.defaultProps = {
  validarAntesAtribuirResponsavel: () => {},
  changeLocalizadorResponsavel: () => {},
  clickCancelar: () => {},
  clickRemoverResponsavel: () => {},
  codigoTurma: '',
  url: '',
};

export default CollapseAtribuicaoResponsavel;
