import React from 'react';

import { useSelector } from 'react-redux';
import CaixaTextoExpandivel from '../CaixaTextoExpandivel/caixaTextoExpandivel';

const MontarEditor = () => {
  const dadosPlanejamentos = useSelector(
    store => store.devolutivas.dadosPlanejamentos
  );

  const planejamentoExpandido = useSelector(
    store => store.devolutivas.planejamentoExpandido
  );

  const planejamentoSelecionado = useSelector(
    store => store.devolutivas.planejamentoSelecionado
  );

  return (
    <>
      {planejamentoExpandido ? (
        <CaixaTextoExpandivel item={planejamentoSelecionado} />
      ) : (
        dadosPlanejamentos.items.map((item, index) => (
          <CaixaTextoExpandivel key={String(index)} item={item} />
        ))
      )}
    </>
  );
};

export default MontarEditor;
