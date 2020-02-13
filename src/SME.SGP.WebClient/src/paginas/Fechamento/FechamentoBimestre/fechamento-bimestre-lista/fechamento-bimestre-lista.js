import { Tooltip } from 'antd';
import React, { useState } from 'react';
import shortid from 'shortid';
import Ordenacao from '~/componentes-sgp/Ordenacao/ordenacao';
import FechamentoRegencia from '../fechamanto-regencia/fechamento-regencia';
import BotaoExpandir from './botao-expandir';
import {
  Info,
  MarcadorAulas,
  Marcadores,
  Situacao,
  MarcadorSituacao,
  TabelaFechamento
} from './fechamento-bimestre-lista.css';
import Button from '~/componentes/button';
import { Colors } from '~/componentes';

const FechamentoBimestreLista = props => {
  const { dados, ehRegencia } = props;
  const [dadosLista, setDadosLista] = useState(
    dados ? dados.alunos : undefined
  );
  const situacoes = {
    NAO_EXECUTADO: 'Não executado',
    PROCESSADO_COM_SUCESSO: 'Processado com sucesso',
    PROCESSADO_COM_PENDENCIA: 'Processado com pendências',
  };
  const [situacao, setSituacao] = useState(situacoes.PROCESSADO_COM_PENDENCIA);

  const reprocessar = () => { }

  return (
    <TabelaFechamento>
      <div className="row pb-4">
        <div className="col-md-6 col-sm-12 d-flex justify-content-start mt-3">
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
          {situacao === situacoes.PROCESSADO_COM_PENDENCIA ? (
            <Button
              id={shortid.generate()}
              label="Reprocessar"
              icon="sync-alt"
              color={Colors.Azul}
              onClick={reprocessar}
              border
              className="ml-2"
            />) : <></>
          }
        </div>
        <div className="col-md-6 m-0 p-0">
          <Situacao className="col-md-12 col-sm-12 mt-2">
            <span>Situação do fechamento: </span>
            <MarcadorSituacao className="ml-2 pl-2 pr-2">{situacao}</MarcadorSituacao>
          </Situacao>
          <Marcadores className="col-md-12 col-sm-12">
            <MarcadorAulas>
              <span>Aulas previstas </span>
              <span className="numero">
                {dados && dados.totalAulasPrevistas
                  ? dados.totalAulasPrevistas
                  : 0}
              </span>
            </MarcadorAulas>
            <MarcadorAulas className="ml-2">
              <span className="ml-2">Aulas dadas </span>
              <span className="numero mr-2">
                {dados && dados.totalAulasDadas ? dados.totalAulasDadas : 0}
              </span>
            </MarcadorAulas>
          </Marcadores>
        </div>
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
    </TabelaFechamento >
  );
};

export default FechamentoBimestreLista;
