import PropTypes from 'prop-types';
import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';

const AnotacoesPedagogicas = props => {
  const {
    onChangeAnotacaoPedagogicas,
    codigoTurma,
    codigoAluno,
    numeroBimestre,
  } = props;

  const [exibirCardAnotacao, setExibirCardAnotacao] = useState(false);
  
  const [anotacaoPedagogica, setAnotacaoPedagogica] = useState('teste 1');

  const onClickExpandirAnotacao = async () => {
    // TODO
    // if (!modoEdicao && !exibirCardAnotacao) {
    //   const retorno = await ServicoConselhoClasse.obterAnotacaoPedagogica(
    //     codigoTurma,
    //     codigoAluno,
    //     numeroBimestre
    //   ).catch(e => erros(e));

    //   if (retorno && retorno.data) {
    //     setAnotacaoPedagogica(retorno.data);
    //   }
    // }

    setExibirCardAnotacao(!exibirCardAnotacao);
  };

  const onChange = texto => {
    onChangeAnotacaoPedagogicas(texto);
  };

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
      <CardCollapse
        key="anotacoes-pedagogicas-collapse"
        onClick={onClickExpandirAnotacao}
        titulo="Anotações pedagógicas"
        indice="anotacoes-pedagogicas-collapse"
        show={exibirCardAnotacao}
        alt="anotacoes-pedagogicas"
      >
        <Editor
          id="anotacoes-pedagogicas-editor"
          inicial={anotacaoPedagogica}
          onChange={onChange}
        />
      </CardCollapse>
    </div>
  );
};

AnotacoesPedagogicas.propTypes = {
  onChangeAnotacaoPedagogicas: PropTypes.func,
  codigoTurma: PropTypes.string,
  codigoAluno: PropTypes.string,
  numeroBimestre: PropTypes.string,
};

AnotacoesPedagogicas.defaultProps = {
  onChangeAnotacaoPedagogicas: () => {},
  codigoTurma: '',
  codigoAluno: '',
  numeroBimestre: '0',
};

export default AnotacoesPedagogicas;
