import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import modalidadeDto from '~/dtos/modalidade';
import notasConceitos from '~/dtos/notasConceitos';
import CampoConceito from '../CamposNotaConceito/campoConceito';
import CampoNota from '../CamposNotaConceito/campoNota';
import { BarraLateralLista, Lista } from '../listasNotasConceitos.css';
import LinhaJustificativa from '../../Justificativa/LinhaJustificativa/LinhaJustificativa';

const ListaFinal = props => {
  const {
    dadosLista,
    tipoNota,
    listaTiposConceitos,
    mediaAprovacao,
    alunoDesabilitado,
    corBarraLateral,
    corRegenciaBarraLateral,
  } = props;

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const { modalidade } = turmaSelecionada;

  const descricaoTipoNota =
    tipoNota === notasConceitos.Notas ? 'Nota' : 'Conceito';

  const descricaoGrupoMatriz =
    dadosLista && dadosLista.grupoMatriz
      ? dadosLista.grupoMatriz
      : 'Componente';

  const alturaLinhaMesclada =
    dadosLista &&
    dadosLista.componenteRegencia &&
    dadosLista.componenteRegencia.componentesCurriculares &&
    dadosLista.componenteRegencia.componentesCurriculares.length
      ? dadosLista.componenteRegencia.componentesCurriculares.length * 2
      : 0;

  const montaCampoPosConselho = (
    id,
    notaPosConselho,
    idCampo,
    codigoComponenteCurricular
  ) => {
    switch (Number(tipoNota)) {
      case Number(notasConceitos.Notas):
        return (
          <CampoNota
            id={id}
            notaPosConselho={notaPosConselho}
            idCampo={idCampo}
            codigoComponenteCurricular={String(codigoComponenteCurricular)}
            mediaAprovacao={mediaAprovacao}
            alunoDesabilitado={alunoDesabilitado}
          />
        );
      case Number(notasConceitos.Conceitos):
        return (
          <CampoConceito
            notaPosConselho={notaPosConselho}
            listaTiposConceitos={listaTiposConceitos}
            id={id}
            idCampo={idCampo}
            codigoComponenteCurricular={codigoComponenteCurricular}
            alunoDesabilitado={alunoDesabilitado}
          />
        );
      default:
        return '';
    }
  };

  const obterValorNotaConceito = valor => {
    const ehNota = Number(notasConceitos.Notas) === tipoNota;
    if (valor && !ehNota && listaTiposConceitos && listaTiposConceitos.length) {
      const conceito = listaTiposConceitos.find(item => item.id == valor);
      return conceito ? conceito.valor : '';
    }
    return valor;
  };

  const montarValoresNotasConceitos = notasFechamentos => {
    const ehEja = modalidade === modalidadeDto.EJA;

    const primeiroBimestre = notasFechamentos.find(item => item.bimestre === 1);
    const segundoBimestre = notasFechamentos.find(item => item.bimestre === 2);
    const terceiroBimestre = notasFechamentos.find(item => item.bimestre === 3);
    const quartoBimestre = notasFechamentos.find(item => item.bimestre === 4);

    return (
      <>
        <div className="input-notas-conceitos-final float-left">
          {obterValorNotaConceito(
            primeiroBimestre ? primeiroBimestre.notaConceito : ''
          )}
        </div>
        <div className="input-notas-conceitos-final float-left">
          {obterValorNotaConceito(
            segundoBimestre ? segundoBimestre.notaConceito : ''
          )}
        </div>
        {!ehEja ? (
          <>
            <div className="input-notas-conceitos-final float-left">
              {obterValorNotaConceito(
                terceiroBimestre ? terceiroBimestre.notaConceito : ''
              )}
            </div>
            <div className="input-notas-conceitos-final float-left">
              {obterValorNotaConceito(
                quartoBimestre ? quartoBimestre.notaConceito : ''
              )}
            </div>
          </>
        ) : (
          ''
        )}
      </>
    );
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
                {descricaoGrupoMatriz}
              </th>
              <th>{descricaoTipoNota}</th>
              <th>{`${descricaoTipoNota} final`}</th>
              <th style={{ width: '100px' }}>Total de faltas</th>
              <th style={{ width: '100px' }}>Ausências Compensadas</th>
              <th>% Total de freq.</th>
            </tr>
          </thead>
          <tbody className="tabela-conselho-tbody">
            {dadosLista &&
              dadosLista.componentesCurriculares &&
              dadosLista.componentesCurriculares.map((item, index) => {
                return (
                  <React.Fragment key={shortid.generate()}>
                    <tr>
                      <BarraLateralLista cor={corBarraLateral} />
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
                      <td className="col-nota-conceito">
                        {montarValoresNotasConceitos(item.notasFechamentos)}
                      </td>
                      <td>
                        {montaCampoPosConselho(
                          item.notaPosConselho.id,
                          item.notaPosConselho.nota,
                          `${descricaoGrupoMatriz} ${index} componente`,
                          item.codigoComponenteCurricular
                        )}
                      </td>
                      <td>{item.faltas}</td>
                      <td>{item.ausenciasCompensadas}</td>
                      <td>{item.frequencia}%</td>
                    </tr>
                    <LinhaJustificativa
                      idCampo={`${descricaoGrupoMatriz} ${index} componente`}
                      ehRegencia={false}
                      alunoDesabilitado={alunoDesabilitado}
                    />
                  </React.Fragment>
                );
              })}
            {dadosLista &&
              dadosLista.componenteRegencia &&
              dadosLista.componenteRegencia.componentesCurriculares &&
              dadosLista.componenteRegencia.componentesCurriculares.map(
                (item, index) => {
                  return (
                    <>
                      <tr key={shortid.generate()}>
                        <BarraLateralLista cor={corRegenciaBarraLateral} />
                        <td
                          className="coluna-disciplina sombra-direita"
                          style={{ textAlign: 'left', paddingLeft: '20px' }}
                        >
                          {item.nome}
                        </td>
                        <td>
                          {montarValoresNotasConceitos(item.notasFechamentos)}
                        </td>
                        <td>
                          {montaCampoPosConselho(
                            item.notaPosConselho.id,
                            item.notaPosConselho.nota,
                            `${descricaoGrupoMatriz} ${index} regencia`,
                            item.codigoComponenteCurricular
                          )}
                        </td>
                        {index === 0 ? (
                          <td rowSpan={alturaLinhaMesclada}>
                            {dadosLista.componenteRegencia.faltas}
                          </td>
                        ) : null}
                        {index === 0 ? (
                          <td rowSpan={alturaLinhaMesclada}>
                            {dadosLista.componenteRegencia.ausenciasCompensadas}
                          </td>
                        ) : null}
                        {index === 0 ? (
                          <td rowSpan={alturaLinhaMesclada}>
                            {dadosLista.componenteRegencia.frequencia}%
                          </td>
                        ) : null}
                      </tr>
                      <LinhaJustificativa
                        idCampo={`${descricaoGrupoMatriz} ${index} regencia`}
                        ehRegencia
                      />
                    </>
                  );
                }
              )}
          </tbody>
        </table>
      </div>
    </Lista>
  );
};

ListaFinal.propTypes = {
  dadosLista: PropTypes.oneOfType([PropTypes.object]),
  tipoNota: PropTypes.oneOfType([PropTypes.any]),
  listaTiposConceitos: PropTypes.oneOfType([PropTypes.array]),
  mediaAprovacao: PropTypes.number,
  alunoDesabilitado: PropTypes.bool,
  corBarraLateral: PropTypes.string,
  corRegenciaBarraLateral: PropTypes.string,
};

ListaFinal.defaultProps = {
  dadosLista: {},
  tipoNota: 0,
  listaTiposConceitos: [],
  mediaAprovacao: 5,
  alunoDesabilitado: false,
  corBarraLateral: '',
  corRegenciaBarraLateral: '',
};

export default ListaFinal;
