import React, { useEffect } from 'react';
import {
  Lista,
  CampoDesabilitado,
  BarraLateralVerde,
  BarraLateralBordo,
} from './listaNotasConselho.css';
import { Dados } from './mock';
import shortid from 'shortid';
import notasConceitos from '~/dtos/notasConceitos';
import CampoNotaPosConselho from '~/componentes-sgp/conselho-classe/campoNotaPosConselho';
import CampoConceitoPosConselho from '~/componentes-sgp/conselho-classe/CampoConceitoPosConselho';
import LinhaJustificativa from '../Justificativa/LinhaJustificativa/LinhaJustificativa';
import { setNotasJustificativas } from '~/redux/modulos/conselhoClasse/actions';
import { useDispatch, useSelector } from 'react-redux';
import { TipoComponente } from '../Justificativa/LinhaJustificativa/TipoComponente';

const ListaNotasConselho = props => {
  const dispatch = useDispatch();
  const { bimestreSelecionado } = props;
  const dadosLista = Dados;
  const alturaLinhaMesclada = dadosLista.componentesRegencia
    ? dadosLista.componentesRegencia.notas.length * 2
    : 0;

  useEffect(() => {
    dispatch(
      setNotasJustificativas({
        componentes: dadosLista.componentes.notas,
        componentesRegencia: dadosLista.componentesRegencia.notas,
      })
    );
  }, [bimestreSelecionado]);

  const montarCampoNotaConceito = (nota, tipoNota, index) => {
    switch (Number(tipoNota)) {
      case Number(notasConceitos.Notas):
        return (
          <CampoNotaPosConselho
            nota={nota}
            desabilitarCampo={false}
            listaTiposConceitos={dadosLista.listaTiposConceitos}
            ehRegencia={false}
            index={index}
          />
        );
      case Number(notasConceitos.Conceitos):
        return (
          <CampoConceitoPosConselho
            nota={nota}
            desabilitarCampo={false}
            ehRegencia={false}
            index={index}
          />
        );
      default:
        return '';
    }
  };

  return (
    <Lista className="pl-2 pr-2">
      <div className="table-responsive pl-2 pr-2">
        <table className="table mt-4">
          <thead className="tabela-conselho-thead">
            <tr>
              <th
                colSpan="2"
                className="coluna-disciplina sombra-direita"
                style={{ width: '250px', paddingLeft: '27px' }}
              >
                Componete
              </th>
              <th>Nota/Conceito</th>
              <th>Nota pós-conselho</th>
              <th>Aulas</th>
              <th>Faltas</th>
              <th style={{ width: '100px' }}>Ausências Compensadas</th>
              <th>%Freq.</th>
            </tr>
          </thead>
          <tbody className="tabela-conselho-tbody">
            {dadosLista.componentes.notas.map((item, index) => {
              return (
                <tr key={shortid.generate()}>
                  <BarraLateralVerde />
                  <td
                    className="coluna-disciplina sombra-direita"
                    style={{
                      width: '250px',
                      textAlign: 'left',
                      paddingLeft: '20px',
                    }}
                  >
                    {item.nome}
                  </td>
                  <td>
                    <CampoDesabilitado>
                      <span>{item.notaConceito}</span>
                    </CampoDesabilitado>
                  </td>
                  <td>
                    {montarCampoNotaConceito(
                      item.notaPosConslelho,
                      dadosLista.componentes.tipoNota,
                      index
                    )}
                  </td>
                  <td>{item.quantidadeAulas}</td>
                  <td>{item.faltas}</td>
                  <td>{item.ausenciasCompensadas}</td>
                  <td>{item.frequencia}%</td>
                </tr>
              );
            })}
            {dadosLista.componentesRegencia.notas.map((item, index) => {
              return (
                <>
                  <tr key={shortid.generate()}>
                    <BarraLateralBordo />
                    <td
                      className="coluna-disciplina sombra-direita"
                      style={{ textAlign: 'left', paddingLeft: '20px' }}
                    >
                      {item.nome}
                    </td>
                    <td>
                      <CampoDesabilitado>
                        <span>{item.notaConceito}</span>
                      </CampoDesabilitado>
                    </td>
                    <td>
                      {montarCampoNotaConceito(
                        item.notaPosConslelho,
                        dadosLista.componentesRegencia.tipoNota,
                        index
                      )}
                    </td>
                    {index === 0 ? (
                      <td rowSpan={alturaLinhaMesclada}>
                        {item.quantidadeAulas}
                      </td>
                    ) : null}
                    {index === 0 ? (
                      <td rowSpan={alturaLinhaMesclada}>{item.faltas}</td>
                    ) : null}
                    {index === 0 ? (
                      <td rowSpan={alturaLinhaMesclada}>
                        {item.ausenciasCompensadas}
                      </td>
                    ) : null}
                    {index === 0 ? (
                      <td rowSpan={alturaLinhaMesclada}>{item.frequencia}%</td>
                    ) : null}
                  </tr>
                  <LinhaJustificativa
                    index={index}
                    tipoComponente={TipoComponente.ComponenteRegencia}
                  />
                </>
              );
            })}
          </tbody>
        </table>
      </div>
    </Lista>
  );
};

export default ListaNotasConselho;
