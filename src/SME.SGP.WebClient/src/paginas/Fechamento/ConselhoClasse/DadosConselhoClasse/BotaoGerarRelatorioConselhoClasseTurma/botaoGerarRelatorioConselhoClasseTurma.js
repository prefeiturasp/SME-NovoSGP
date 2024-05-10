import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Colors } from '~/componentes';
import Button from '~/componentes/button';
import { setExibirModalImpressaoConselhoClasse } from '~/redux/modulos/conselhoClasse/actions';
import { erro } from '~/servicos/alertas';

const BotaoGerarRelatorioConselhoClasseTurma = () => {
  const dispatch = useDispatch();

  const dadosBimestresConselhoClasse = useSelector(
    store => store.conselhoClasse.dadosBimestresConselhoClasse
  );

  const onClickImprimir = () => {
    if (dadosBimestresConselhoClasse && dadosBimestresConselhoClasse.length) {
      dispatch(setExibirModalImpressaoConselhoClasse(true));
    } else {
      erro(
        'Não foi encontrado nenhum registro de conselho de classe para a turma selecionada.'
      );
    }
  };

  return (
    <Button
      className="btn-imprimir"
      icon="print"
      color={Colors.Azul}
      border
      onClick={onClickImprimir}
      id="btn-imprimir-relatorio-pendencias"
    />
  );
};

export default BotaoGerarRelatorioConselhoClasseTurma;
