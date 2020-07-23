import PropTypes from 'prop-types';
import React, { useEffect } from 'react';
import { LinhaObservacao } from './observacoesChat.css';
import LinhaObservacaoProprietario from './linhaObservacaoProprietario';

const ObservacoesChatMontarDados = props => {
  const { dados } = props;

  useEffect(() => {
    console.log(dados);
  }, [dados]);

  const montaLinhaObservacao = () => {
    return (
      <div className="row">
        <LinhaObservacao className="col-md-8">
          Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
          eiusmod tempor incididunt ut labore et dolore reprehenderit in
          voluptate velit esse cillum dolore Lorem ipsum dolor sit amet,
          consectetur adipiscing elit, sed do eiusmod tempor incididunt ut
          labore et dolore reprehenderit in voluptate velit esse cillum dolore
        </LinhaObservacao>
        <div className="col-md-4" />
      </div>
    );
  };

  const montarValores = obs => {
    if (obs && obs.proprietario) {
      return <LinhaObservacaoProprietario observacao={obs} />;
    }
    return montaLinhaObservacao(obs);
  };

  return (
    <div className="col-md-12 mb-2 mt-2">
      {dados && dados.length
        ? dados.map(obs => {
            return <> {montarValores(obs)} </>;
          })
        : ''}
    </div>
  );
};

ObservacoesChatMontarDados.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.array]),
};

ObservacoesChatMontarDados.defaultProps = {
  dados: [],
};

export default ObservacoesChatMontarDados;
