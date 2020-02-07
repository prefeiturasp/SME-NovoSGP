import { Tooltip } from 'antd';
import React, { useState } from 'react';
import Ordenacao from '~/componentes-sgp/Ordenacao/ordenacao';
import {
  Info,
  MarcadorAulas,
  Marcadores,
  TabelaFechamento,
  MaisMenos,
} from './fechamento-bimestre-lista.css';
import BotaoExpandir from './botao-expandir';
import FechamentoRegencia from '../fechamanto-regencia/fechamento-regencia';

const FechamentoBimestreLista = props => {
  const { dados } = props;
  const [dadosLista, setDadosLista] = useState(dados.lista);

  return (
    <TabelaFechamento>
      <div className="row pb-4">
        <div className="col-md-6 col-sm-12 d-flex justify-content-start">
          <Ordenacao
            className="botao-ordenacao-avaliacao"
            conteudoParaOrdenar={dadosLista}
            ordenarColunaNumero="contador"
            ordenarColunaTexto="nome"
            retornoOrdenado={retorno => {
              setDadosLista(retorno);
            }}
          />
        </div>
        <Marcadores className="col-md-6 col-sm-12 d-flex justify-content-end">
          <MarcadorAulas>
            <span>Aulas previstas </span>
            <span className="numero">{dados.totalAulasPrevistas}</span>
          </MarcadorAulas>
          <MarcadorAulas className="ml-2">
            <span>Aulas dadas </span>
            <span className="numero">{dados.totalAulasDadas}</span>
          </MarcadorAulas>
        </Marcadores>
      </div>
      <div className="col-md-12 p-0 container-table">
        <table className="table mb-0" id="table-fechamento-bimestre">
          <thead className="tabela-fechamento-thead" key="thead-fechamento">
            <tr>
              <th
                className="text-center fundo-cinza"
                style={{ minWidth: '250px' }}
                colSpan={2}
              >
                Nome
              </th>
              <th className="text-center fundo-cinza">Nota/Conceito</th>
              <th className="text-center fundo-cinza">Faltas no Bimestre</th>
              <th className="text-center fundo-cinza">Ausências Compensadas</th>
              <th className="text-center fundo-cinza">Frequência</th>
            </tr>
          </thead>
          <tbody>
            {dadosLista.map((item, index) => {
              return (
                <>
                  <tr>
                    <td
                      className={`text-center ${
                        !item.ativo ? 'fundo-cinza' : ''
                      }`}
                    >
                      {item.contador}
                      {item.informacao ? (
                        <Tooltip title={item.informacao} placement="top">
                          <Info className="fas fa-circle" />
                        </Tooltip>
                      ) : (
                        ''
                      )}
                    </td>
                    <td className={`${!item.ativo ? 'fundo-cinza' : ''}`}>
                      {item.nome}
                    </td>
                    <td
                      className={`text-center ${
                        !item.ativo ? 'fundo-cinza' : ''
                      }`}
                    >
                      {item.regencia && item.regencia.length > 0 ? (
                        <BotaoExpandir index={index} />
                      ) : (
                        item.notaConceito
                      )}
                    </td>
                    <td
                      className={`text-center ${
                        !item.ativo ? 'fundo-cinza' : ''
                      }`}
                    >
                      {item.faltasBimestre}
                    </td>
                    <td
                      className={`text-center ${
                        !item.ativo ? 'fundo-cinza' : ''
                      }`}
                    >
                      {item.ausenciasCompensadas}
                    </td>
                    <td
                      className={`text-center ${
                        !item.ativo ? 'fundo-cinza' : ''
                      }`}
                    >
                      {item.frequencia ? item.frequencia + '%' : ''}
                    </td>
                  </tr>
                  {item.regencia && item.regencia.length > 0 ? (
                    <FechamentoRegencia
                      dados={item.regencia}
                      idRegencia={`fechamento-regencia-${index}`}
                    />
                  ) : null}
                </>
              );
            })}
          </tbody>
        </table>
      </div>
    </TabelaFechamento>
  );
};

export default FechamentoBimestreLista;
