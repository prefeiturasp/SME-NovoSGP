import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Ordenacao } from '~/componentes-sgp';
import { setAlunosAcompanhamentoAprendizagem } from '~/redux/modulos/acompanhamentoAprendizagem/actions';

const BotaoOrdenarListaAlunos = () => {
  const alunosAcompanhamentoAprendizagem = useSelector(
    store => store.acompanhamentoAprendizagem.alunosAcompanhamentoAprendizagem
  );

  const dispatch = useDispatch();

  return (
    <>
      {alunosAcompanhamentoAprendizagem?.length ? (
        <Ordenacao
          conteudoParaOrdenar={alunosAcompanhamentoAprendizagem}
          ordenarColunaNumero="numeroChamada"
          ordenarColunaTexto="nome"
          retornoOrdenado={retorno =>
            dispatch(setAlunosAcompanhamentoAprendizagem(retorno))
          }
        />
      ) : (
        ''
      )}
    </>
  );
};

export default BotaoOrdenarListaAlunos;
