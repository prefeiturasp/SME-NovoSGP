import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Ordenacao } from '~/componentes-sgp';

import { setAlunosRelatorioSemestral } from '~/redux/modulos/relatorioSemestral/actions';

const BotaoOrdenarListaAlunos = () => {
  const alunosRelatorioSemestral = useSelector(
    store => store.relatorioSemestral.alunosRelatorioSemestral
  );

  const dispatch = useDispatch();

  return (
    <Ordenacao
      conteudoParaOrdenar={alunosRelatorioSemestral}
      ordenarColunaNumero="numeroChamada"
      ordenarColunaTexto="nome"
      retornoOrdenado={retorno =>
        dispatch(setAlunosRelatorioSemestral(retorno))
      }
    />
  );
};

export default BotaoOrdenarListaAlunos;
