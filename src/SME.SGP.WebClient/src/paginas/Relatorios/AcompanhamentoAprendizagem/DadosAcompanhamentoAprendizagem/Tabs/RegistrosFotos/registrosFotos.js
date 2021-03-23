import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import AuditoriaAcompanhamentoAprendizagem from '../../AuditoriaAcompanhamento/auditoriaAcompanhamento';
import FotosCrianca from './FotosCrianca/fotosCrianca';
import ObservacoesAdicionais from './ObservacoesAdicionais/observacoesAdicionais';
import RegistrosIndividuais from './RegistrosIndividuais/registrosIndividuais';

const RegistrosFotos = props => {
  const { semestreSelecionado } = props;

  const dadosAcompanhamentoAprendizagem = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAcompanhamentoAprendizagem
  );

  return dadosAcompanhamentoAprendizagem ? (
    <>
      <RegistrosIndividuais semestreSelecionado={semestreSelecionado} />
      <ObservacoesAdicionais />
      <FotosCrianca semestreSelecionado={semestreSelecionado} />
      <AuditoriaAcompanhamentoAprendizagem />
    </>
  ) : (
    ''
  );
};

RegistrosFotos.propTypes = {
  semestreSelecionado: PropTypes.string,
};

RegistrosFotos.defaultProps = {
  semestreSelecionado: '',
};

export default RegistrosFotos;
