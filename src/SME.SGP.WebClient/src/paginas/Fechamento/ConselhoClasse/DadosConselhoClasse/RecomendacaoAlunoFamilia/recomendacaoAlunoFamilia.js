import PropTypes from 'prop-types';
import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';

const RecomendacaoAlunoFamilia = props => {
  const {
    onChangeRecomendacaoAluno,
    onChangeRecomendacaoFamilia,
    codigoTurma,
    codigoAluno,
    numeroBimestre,
  } = props;

  const [exibirCardRecomendacao, setExibirCardRecomendacao] = useState(false);

  const [recomendacaoAluno, setRecomendacaoAluno] = useState('teste 1');
  const [recomendacaoFamilia, setRecomendacaoFamilia] = useState('teste 2');

  const onClickExpandirRecomendacao = async () => {
    // TODO
    // if (!modoEdicao && !exibirCardRecomendacao) {
    //   const retorno = await ServicoConselhoClasse.obterRecomendacoesAluno(
    //     codigoTurma,
    //     codigoAluno,
    //     numeroBimestre
    //   ).catch(e => erros(e));

    //   if (retorno && retorno.data) {
    //     setRecomendacaoAluno(retorno.data.recomendacaoAluno);
    //     setRecomendacaoFamilia(retorno.data.recomendacaoFamilia);
    //   }
    // }

    setExibirCardRecomendacao(!exibirCardRecomendacao);
  };

  const onChangeAluno = textoAluno => {
    onChangeRecomendacaoAluno(textoAluno);
  };

  const onChangeFamilia = textoFamilia => {
    onChangeRecomendacaoFamilia(textoFamilia);
  };

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
      <CardCollapse
        key="recomendacao-aluno-familia-collapse"
        onClick={onClickExpandirRecomendacao}
        titulo="Recomendações ao aluno e a família"
        indice="recomendacao-aluno-familia-collapse"
        show={exibirCardRecomendacao}
        alt="recomendacao-aluno-familia"
      >
        <div className="row">
          <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6">
            <Editor
              label="Recomendações ao aluno"
              id="recomendacao-aluno"
              inicial={recomendacaoAluno}
              onChange={onChangeAluno}
            />
          </div>
          <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6">
            <Editor
              label="Recomendações a família "
              id="recomendacao-familia"
              inicial={recomendacaoFamilia}
              onChange={onChangeFamilia}
            />
          </div>
        </div>
      </CardCollapse>
    </div>
  );
};

RecomendacaoAlunoFamilia.propTypes = {
  onChangeRecomendacaoAluno: PropTypes.func,
  onChangeRecomendacaoFamilia: PropTypes.func,
  codigoTurma: PropTypes.string,
  codigoAluno: PropTypes.string,
  numeroBimestre: PropTypes.string,
};

RecomendacaoAlunoFamilia.defaultProps = {
  onChangeRecomendacaoAluno: () => {},
  onChangeRecomendacaoFamilia: () => {},
  codigoTurma: '',
  codigoAluno: '',
  numeroBimestre: '0',
};

export default RecomendacaoAlunoFamilia;
