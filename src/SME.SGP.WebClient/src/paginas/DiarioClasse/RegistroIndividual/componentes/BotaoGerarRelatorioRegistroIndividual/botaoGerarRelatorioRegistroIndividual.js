import React from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Colors } from '~/componentes';
import { BotaoCustomizado } from '~/paginas/AEE/RegistroItinerancia/registroItinerancia.css';

import { setExibirModalImpressaoRegistroIndividual } from '~/redux/modulos/registroIndividual/actions';

const BotaoGerarRelatorioRegistroIndividual = () => {
  const dispatch = useDispatch();

  const alunos = useSelector(
    store => store.registroIndividual.alunosRegistroIndividual
  );

  const onClickImprimir = () => {
    if (alunos?.length) {
      dispatch(setExibirModalImpressaoRegistroIndividual(true));
    }
  };

  return (
    <BotaoCustomizado
      className="btn-imprimir"
      icon="print"
      color={Colors.Azul}
      border
      onClick={onClickImprimir}
      id="btn-imprimir-relatorio-registro-individual"
      width="48px"
    />
  );
};

export default BotaoGerarRelatorioRegistroIndividual;
