import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import LinhaObservacaoProprietario from './linhaObservacaoProprietario';
import { LinhaObservacao } from './observacoesChat.css';
import Auditoria from '~/componentes/auditoria';

const ObservacoesChatMontarDados = props => {
  const { onClickSalvarEdicao, onClickExcluir } = props;

  const dadosObservacoes = useSelector(
    store => store.observacoesChat.dadosObservacoes
  );

  const auditoria = observacao => {
    return (
      <Auditoria
        alteradoEm={observacao.auditoria.alteradoEm}
        alteradoPor={observacao.auditoria.alteradoPor}
        alteradoRf={observacao.auditoria.alteradoRF}
        criadoEm={observacao.auditoria.criadoEm}
        criadoPor={observacao.auditoria.criadoPor}
        criadoRf={observacao.auditoria.criadoRF}
      />
    );
  };

  const montaLinhaObservacao = obs => {
    return (
      <div className="row">
        <LinhaObservacao className="col-md-8 mb-5">
          <div>{obs.texto}</div>
          <div className="row">
            {obs.auditoria ? <>{auditoria(obs)}</> : ''}
          </div>
        </LinhaObservacao>
        <div className="col-md-4" />
      </div>
    );
  };

  const montarValores = (obs, index) => {
    if (obs && obs.proprietario) {
      return (
        <>
          <LinhaObservacaoProprietario
            observacao={obs}
            onClickSalvarEdicao={onClickSalvarEdicao}
            onClickExcluir={onClickExcluir}
            index={index}
          >
            <div className="row">
              {obs.auditoria ? <>{auditoria(obs)}</> : ''}
            </div>
          </LinhaObservacaoProprietario>
        </>
      );
    }
    return montaLinhaObservacao(obs);
  };

  return (
    <div className="col-md-12 mb-2 mt-2">
      {dadosObservacoes && dadosObservacoes.length
        ? dadosObservacoes.map((obs, index) => {
            return <> {montarValores(obs, index)} </>;
          })
        : ''}
    </div>
  );
};

ObservacoesChatMontarDados.propTypes = {
  onClickSalvarEdicao: PropTypes.func,
  onClickExcluir: PropTypes.func,
};

ObservacoesChatMontarDados.defaultProps = {
  onClickSalvarEdicao: () => {},
  onClickExcluir: () => {},
};

export default ObservacoesChatMontarDados;
