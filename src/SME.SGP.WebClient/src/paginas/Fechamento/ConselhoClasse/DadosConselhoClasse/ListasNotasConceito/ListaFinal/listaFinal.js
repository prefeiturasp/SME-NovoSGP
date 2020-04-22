import React from 'react';
import shortid from 'shortid';
import {
  BarraLateralBordo,
  BarraLateralVerde,
  CampoDesabilitado,
  Lista,
} from '../listasNotasConceitos.css';
import { Dados } from '../mock';
import notasConceitos from '~/dtos/notasConceitos';
import CampoNota from '../CamposNotaConceito/campoNota';
import CampoConceito from '../CamposNotaConceito/campoConceito';

const ListaFinal = () => {
  const dadosLista = Dados;

  const montaCampoPosConselho = (notaPosConselho, tipoNota) => {
    switch (Number(tipoNota)) {
      case Number(notasConceitos.Notas):
        return <CampoNota notaPosConselho={notaPosConselho} />;
      case Number(notasConceitos.Conceitos):
        return <CampoConceito notaPosConselho={notaPosConselho} />;
      default:
        return '';
    }
  };

  return (
    <Lista className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
      <div className="table-responsive">
        <table className="table mt-2">
          <thead className="tabela-conselho-thead">
            <tr>
              <th
                colSpan="2"
                className="coluna-disciplina sombra-direita"
                style={{ width: '250px', paddingLeft: '27px' }}
              >
                Componete
              </th>
              <th>
                {dadosLista.tipoNota === notasConceitos.Notas
                  ? 'Nota'
                  : 'Conceito'}
              </th>
              <th>Total de faltas</th>
              <th>Ausências Compensadas</th>
              <th>% Total de freq.</th>
              <th>
                {dadosLista.tipoNota === notasConceitos.Notas
                  ? 'Nota final'
                  : 'Conceito final'}
              </th>
            </tr>
          </thead>
          <tbody className="tabela-conselho-tbody">
            {dadosLista.componenteRegencia.componentesCurriculares.map(item => {
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
                      <span>{item.notaConceito[0]}</span>
                    </CampoDesabilitado>
                  </td>
                  <td>{dadosLista.componenteRegencia.faltas}</td>
                  <td>{dadosLista.componenteRegencia.ausenciasCompensadas}</td>
                  <td>{dadosLista.componenteRegencia.frequencia}%</td>
                  <td>{montaCampoPosConselho(item.notaPosConselho)} </td>
                </tr>
              );
            })}
            {dadosLista.componentesCurriculares.map(item => {
              return (
                <tr key={shortid.generate()}>
                  <BarraLateralBordo />
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
                      <span>{item.notaConceito[0]}</span>
                    </CampoDesabilitado>
                  </td>
                  <td>{item.faltas}</td>
                  <td>{item.ausenciasCompensadas}</td>
                  <td>{item.frequencia}%</td>
                  <td>{montaCampoPosConselho(item.notaPosConselho)} </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </Lista>
  );
};

export default ListaFinal;
