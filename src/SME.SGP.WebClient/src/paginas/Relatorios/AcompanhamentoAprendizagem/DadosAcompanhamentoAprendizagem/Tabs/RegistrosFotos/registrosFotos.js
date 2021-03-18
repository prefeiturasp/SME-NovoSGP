import PropTypes from 'prop-types';
import React from 'react';
import FotosCrianca from './FotosCrianca/fotosCrianca';
import ObservacoesAdicionais from './ObservacoesAdicionais/observacoesAdicionais';
import RegistrosIndividuais from './RegistrosIndividuais/registrosIndividuais';

const RegistrosFotos = props => {
  const { semestreSelecionado } = props;

  return (
    <>
      <RegistrosIndividuais semestreSelecionado={semestreSelecionado} />
      <ObservacoesAdicionais />
      <FotosCrianca semestreSelecionado={semestreSelecionado} />
    </>
  );
};

RegistrosFotos.propTypes = {
  semestreSelecionado: PropTypes.string,
};

RegistrosFotos.defaultProps = {
  semestreSelecionado: '',
};

export default RegistrosFotos;
