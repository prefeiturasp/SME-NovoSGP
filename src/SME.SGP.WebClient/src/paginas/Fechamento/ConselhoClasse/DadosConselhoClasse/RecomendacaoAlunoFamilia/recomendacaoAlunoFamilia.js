import PropTypes from 'prop-types';
import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';

const RecomendacaoAlunoFamilia = props => {
  const {
    onChangeRecomentacaoAluno,
    onChangeRecomentacaoFamilia,
    codigoTurma,
    codigoAluno,
    numeroBimestre,
  } = props;

  const [exibirCardRecomendacao, setExibirCardRecomendacao] = useState(false);
  const [modoEdicao, setModoEdicao] = useState(false);

  const [recomentacaoAluno, setRecomentacaoAluno] = useState('teste 1');
  const [recomentacaoFamilia, setRecomentacaoFamilia] = useState('teste 2');

  const onClickExpandirRecomendacao = async () => {
    // TODO
    // if (!modoEdicao && !exibirCardRecomendacao) {
    //   const retorno = await ServicoConselhoClasse.obterRecomendacoesAluno(
    //     codigoTurma,
    //     codigoAluno,
    //     numeroBimestre
    //   ).catch(e => erros(e));

    //   if (retorno && retorno.data) {
    //     setRecomentacaoAluno(retorno.data.recomendacaoAluno);
    //     setRecomentacaoFamilia(retorno.data.recomentacaoFamilia);
    //   }
    // }

    setExibirCardRecomendacao(!exibirCardRecomendacao);
  };

  const onChangeAluno = textoAluno => {
    setModoEdicao(true);
    onChangeRecomentacaoAluno(textoAluno);
  };

  const onChangeFamilia = textoFamilia => {
    setModoEdicao(true);
    onChangeRecomentacaoFamilia(textoFamilia);
  };

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
      <CardCollapse
        key="recomentacao-aluno-familia-collapse"
        onClick={onClickExpandirRecomendacao}
        titulo="Recomendações ao aluno e a família"
        indice="recomentacao-aluno-familia-collapse"
        show={exibirCardRecomendacao}
        alt="recomentacao-aluno-familia"
      >
        <div className="row">
          <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6">
            <Editor
              label="Recomendações ao aluno"
              id="recomentacao-aluno"
              inicial={recomentacaoAluno}
              onChange={onChangeAluno}
            />
          </div>
          <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6">
            <Editor
              label="Recomendações a família "
              id="recomentacao-familia"
              inicial={recomentacaoFamilia}
              onChange={onChangeFamilia}
            />
          </div>
        </div>
      </CardCollapse>
    </div>
  );
};

RecomendacaoAlunoFamilia.propTypes = {
  onChangeRecomentacaoAluno: PropTypes.func,
  onChangeRecomentacaoFamilia: PropTypes.func,
  codigoTurma: PropTypes.string,
  codigoAluno: PropTypes.string,
  numeroBimestre: PropTypes.string,
};

RecomendacaoAlunoFamilia.defaultProps = {
  onChangeRecomentacaoAluno: () => {},
  onChangeRecomentacaoFamilia: () => {},
  codigoTurma: '',
  codigoAluno: '',
  numeroBimestre: '0',
};

export default RecomendacaoAlunoFamilia;
