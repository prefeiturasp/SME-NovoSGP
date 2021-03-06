import PropTypes from 'prop-types';
import React from 'react';
import FrequenciaCardCollapse from './Frequencia/frequenciaCardCollapse';
import OcorrenciasCardCollapse from './Ocorrencias/ocorrenciasCardCollapse';
import RegistroComunicacaoEscolaAquiCardCollapse from './RegistroComunicacaoEscolaAqui/registroComunicacaoEscolaAquiCardCollapse';

const DadosGerais = props => {
  const { semestreSelecionado } = props;

  return (
    <>
      <FrequenciaCardCollapse semestreSelecionado={semestreSelecionado} />
      <OcorrenciasCardCollapse />
      <RegistroComunicacaoEscolaAquiCardCollapse />
    </>
  );
};

DadosGerais.propTypes = {
  semestreSelecionado: PropTypes.string,
};

DadosGerais.defaultProps = {
  semestreSelecionado: '',
};

export default DadosGerais;
