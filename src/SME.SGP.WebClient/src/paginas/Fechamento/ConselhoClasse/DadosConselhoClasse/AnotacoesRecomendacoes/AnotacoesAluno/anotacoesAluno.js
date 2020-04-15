import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import AnotacoesAlunoLista from './anotacaoAlunoLista';

const AnotacoesAluno = () => {
  const anotacoesAluno = useSelector(
    store => store.conselhoClasse.anotacoesAluno
  );

  const [exibirCardAnotacaoAluno, setExibirCardAnotacaoAluno] = useState(false);

  const onClickAnotacaoAluno = () =>
    setExibirCardAnotacaoAluno(!exibirCardAnotacaoAluno);

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
      <CardCollapse
        key="anotacao-aluno-collapse"
        onClick={onClickAnotacaoAluno}
        titulo="Anotações do aluno"
        indice="anotacao-aluno-collapse"
        show={exibirCardAnotacaoAluno}
        alt="card-collapse-anotacao-aluno"
      >
        {exibirCardAnotacaoAluno ? (
          <AnotacoesAlunoLista anotacoes={anotacoesAluno} />
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

export default AnotacoesAluno;
