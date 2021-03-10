import React from 'react';
import { useSelector } from 'react-redux';
import ObjectCardEstudante from '~/componentes-sgp/ObjectCardEstudante/objectCardEstudante';

const ObjectCardEstudantePlanoAEE = () => {
  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  return dadosCollapseLocalizarEstudante?.codigoAluno ? (
    <ObjectCardEstudante
      codigoAluno={dadosCollapseLocalizarEstudante?.codigoAluno}
      anoLetivo={dadosCollapseLocalizarEstudante?.anoLetivo}
      exibirBotaoImprimir={false}
      exibirFrequencia={false}
    />
  ) : (
    ''
  );
};

export default ObjectCardEstudantePlanoAEE;
