import { Tooltip } from 'antd';
import React, { useState } from 'react';
import Ordenacao from '~/componentes-sgp/Ordenacao/ordenacao';
import FechamentoRegencia from '../fechamanto-regencia/fechamento-regencia';
import BotaoExpandir from './botao-expandir';
import {
  Info,
  MarcadorAulas,
  Marcadores,
  TabelaFechamento,
} from './fechamento-bimestre-lista.css';

const FechamentoBimestreLista = props => {
  const { dados, ehRegencia } = props;
  const [dadosLista, setDadosLista] = useState(
    dados ? dados.alunos : undefined
  );

  return (
    <TabelaFechamento>
      <div className="row pb-4">
        <div className="col-md-6 col-sm-12 d-flex justify-content-start">
          <Ordenacao
            className="botao-ordenacao-avaliacao"
            conteudoParaOrdenar={dadosLista}
            ordenarColunaNumero="numeroChamada"
            ordenarColunaTexto="nome"
            retornoOrdenado={retorno => {
              setDadosLista(retorno);
            }}
            desabilitado={dadosLista ? dadosLista.length <= 0 : true}
          />
        </div>
        <Marcadores className="col-md-6 col-sm-12 d-flex justify-content-end">
          <MarcadorAulas>
            <span>Aulas previstas </span>
            <span className="numero">
              {dados && dados.totalAulasPrevistas
                ? dados.totalAulasPrevistas
                : 0}
            </span>
          </MarcadorAulas>
          <MarcadorAulas className="ml-2">
            <span>Aulas dadas </span>
            <span className="numero">
              {dados && dados.totalAulasDadas ? dados.totalAulasDadas : 0}
            </span>
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
            {dadosLista && dadosLista.length > 0 ? (
              dadosLista.map((item, index) => {
                const idLinhaRegencia = `fechamento-regencia-${index}`;
                return (
                  <>
                    <tr>
                      <td
                        className={`text-center ${
                          !item.ativo ? 'fundo-cinza' : ''
                        }`}
                      >
                        {item.numeroChamada}
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
                        {ehRegencia && item.notas ? (
                          <BotaoExpandir
                            index={index}
                            idLinhaRegencia={idLinhaRegencia}
                          />
                        ) : item.notas && item.notas.length > 0 ? (
                          item.notas[0].notaConceito
                        ) : null}
                      </td>
                      <td
                        className={`text-center ${
                          !item.ativo ? 'fundo-cinza' : ''
                        }`}
                      >
                        {item.quantidadeFaltas}
                      </td>
                      <td
                        className={`text-center ${
                          !item.ativo ? 'fundo-cinza' : ''
                        }`}
                      >
                        {item.quantidadeCompensacoes}
                      </td>
                      <td
                        className={`text-center ${
                          !item.ativo ? 'fundo-cinza' : ''
                        }`}
                      >
                        {item.percentualFrequencia
                          ? item.percentualFrequencia + '%'
                          : ''}
                      </td>
                    </tr>
                    {ehRegencia ? (
                      <FechamentoRegencia
                        dados={item.notas}
                        idRegencia={`fechamento-regencia-${index}`}
                      />
                    ) : null}
                  </>
                );
              })
            ) : (
              <tr>
                <td colSpan="6" className="text-center">
                  Sem dados
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </TabelaFechamento>
  );
};

export default FechamentoBimestreLista;
