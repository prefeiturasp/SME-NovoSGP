import React from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Ordenacao } from '~/componentes-sgp';

import { setAlunosRegistroIndividual } from '~/redux/modulos/registroIndividual/actions';

const BotaoOrdenarListaAlunos = () => {
  const alunosRegistroIndividual = useSelector(
    store => store.registroIndividual.alunosRegistroIndividual
  );

  const dispatch = useDispatch();

  return (
    <Ordenacao
      conteudoParaOrdenar={alunosRegistroIndividual}
      ordenarColunaNumero="numeroChamada"
      ordenarColunaTexto="nome"
      retornoOrdenado={retorno =>
        dispatch(setAlunosRegistroIndividual(retorno))
      }
    />
  );
};

export default BotaoOrdenarListaAlunos;
