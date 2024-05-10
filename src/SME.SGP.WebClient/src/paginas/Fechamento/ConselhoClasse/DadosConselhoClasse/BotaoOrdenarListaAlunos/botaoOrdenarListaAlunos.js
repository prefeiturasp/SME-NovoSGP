import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Ordenacao } from '~/componentes-sgp';

import { setAlunosConselhoClasse } from '~/redux/modulos/conselhoClasse/actions';

const BotaoOrdenarListaAlunos = () => {
  const alunosConselhoClasse = useSelector(
    store => store.conselhoClasse.alunosConselhoClasse
  );

  const dispatch = useDispatch();

  return (
    <Ordenacao
      conteudoParaOrdenar={alunosConselhoClasse}
      ordenarColunaNumero="numeroChamada"
      ordenarColunaTexto="nome"
      retornoOrdenado={retorno => dispatch(setAlunosConselhoClasse(retorno))}
    />
  );
};

export default BotaoOrdenarListaAlunos;
