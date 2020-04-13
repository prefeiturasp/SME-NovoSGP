import React from 'react';
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

const ListaNotasConselho = props => {
  const dadosLista = Dados;

  const montarCampoNotaConceito = (nota, tipoNota) => {
    switch (Number(tipoNota)) {
      case Number(notasConceitos.Notas):
        return (
          <CampoNotaPosConselho
            nota={nota}
            onChangeNotaConceito={() => {}}
            desabilitarCampo={false}
          />
        );
      case Number(notasConceitos.Conceitos):
        return (
          <CampoConceitoPosConselho
            nota={nota}
            onChangeNotaConceito={() => {}}
            desabilitarCampo={false}
            listaTiposConceitos={dadosLista.listaTiposConceitos}
          />
        );
      default:
        return '';
    }
  };

  return (
    <Lista>
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
                      dadosLista.componentes.tipoNota
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
                      dadosLista.componentesRegencia.tipoNota
                    )}
                  </td>
                  <td>{item.quantidadeAulas}</td>
                  <td>{item.faltas}</td>
                  <td>{item.ausenciasCompensadas}</td>
                  <td>{item.frequencia}%</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </Lista>
  );
};

export default ListaNotasConselho;
