import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import Auditoria from '~/componentes/auditoria';
import LinhaObservacaoProprietario from './linhaObservacaoProprietario';
import { LinhaObservacao } from './observacoesUsuario.css';

const ObservacoesUsuarioMontarDados = props => {
  const {
    onClickSalvarEdicao,
    onClickExcluir,
    inserindoNovaObservacao,
  } = props;

  const dadosObservacoes = useSelector(
    store => store.observacoesUsuario.dadosObservacoes
  );

  const auditoria = observacao => {
    return (
      <div className="row mt-1">
        <Auditoria
          alteradoEm={observacao.auditoria.alteradoEm}
          alteradoPor={observacao.auditoria.alteradoPor}
          alteradoRf={observacao.auditoria.alteradoRF}
          criadoEm={observacao.auditoria.criadoEm}
          criadoPor={observacao.auditoria.criadoPor}
          criadoRf={observacao.auditoria.criadoRF}
          ignorarMarginTop
        />
      </div>
    );
  };

  const montaLinhaObservacao = obs => {
    return (
      <div className="mb-5" key={shortid.generate()}>
        <LinhaObservacao className="col-md-12">
          <div>{obs.observacao}</div>
        </LinhaObservacao>
        {obs.auditoria ? <>{auditoria(obs)}</> : ''}
      </div>
    );
  };

  const montarValores = (obs, index) => {
    if (obs && obs.proprietario) {
      return (
        <div className="mb-5" key={shortid.generate()}>
          <LinhaObservacaoProprietario
            dados={obs}
            onClickSalvarEdicao={onClickSalvarEdicao}
            onClickExcluir={onClickExcluir}
            index={index}
            inserindoNovaObservacao={inserindoNovaObservacao}
          >
            {obs.auditoria ? auditoria(obs) : ''}
          </LinhaObservacaoProprietario>
        </div>
      );
    }
    return montaLinhaObservacao(obs);
  };

  return (
    <div className="col-md-12 mb-2 mt-2">
      {dadosObservacoes && dadosObservacoes.length
        ? dadosObservacoes.map((obs, index) => {
            return montarValores(obs, index);
          })
        : ''}
    </div>
  );
};

ObservacoesUsuarioMontarDados.propTypes = {
  onClickSalvarEdicao: PropTypes.func,
  onClickExcluir: PropTypes.func,
  inserindoNovaObservacao: PropTypes.bool,
};

ObservacoesUsuarioMontarDados.defaultProps = {
  onClickSalvarEdicao: () => {},
  onClickExcluir: () => {},
  inserindoNovaObservacao: false,
};

export default ObservacoesUsuarioMontarDados;
