import React from 'react';
import EstiloLinhaTempo from './linhaTempo.css';

const LinhaTempo = props => {
  const { listaDeStatus } = props;

  const obterEstiloPorStatus = status => {
    switch (status) {
      case 2:
        return 'active';
      case 3:
        return 'disapproved';
      default:
        return '';
    }
  };
  return (
    <EstiloLinhaTempo quantidadeItems={listaDeStatus.length}>
      <div className="w-100">
        <div className="row">
          <div className="col-12">
            <ul className="progressbar-titles">
              {listaDeStatus.map((status, key) => {
                return <li key={key}>{status.titulo}</li>;
              })}
            </ul>
            <ul className="progressbar">
              {listaDeStatus.map((item, key) => {
                return (
                  <li
                    key={key}
                    className={obterEstiloPorStatus(item.status)}
                    style={{ width: 100 / listaDeStatus.length + '%' }}
                  >
                    {item.timestamp}
                    <br />
                    {item.status && (
                      <span>
                        RF: {item.rf} - {item.nome}
                      </span>
                    )}
                  </li>
                );
              })}
            </ul>
          </div>
        </div>
      </div>
    </EstiloLinhaTempo>
  );
};
export default LinhaTempo;
