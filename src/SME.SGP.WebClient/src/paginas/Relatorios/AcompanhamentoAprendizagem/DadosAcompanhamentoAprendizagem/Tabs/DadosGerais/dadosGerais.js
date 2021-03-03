import React from 'react';
import FrequenciaCardCollapse from './Frequencia/frequenciaCardCollapse';
import OcorrenciasCardCollapse from './Ocorrencias/ocorrenciasCardCollapse';
import RegistroComunicacaoEscolaAquiCardCollapse from './RegistroComunicacaoEscolaAqui/registroComunicacaoEscolaAquiCardCollapse';

const DadosGerais = () => {
  return (
    <>
      <FrequenciaCardCollapse />
      <OcorrenciasCardCollapse />
      <RegistroComunicacaoEscolaAquiCardCollapse />
    </>
  );
};

export default DadosGerais;
