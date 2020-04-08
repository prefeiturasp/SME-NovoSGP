import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';

import CampoConceitoFinal from './campoConceitoFinal';
import CampoNotaFinal from './campoNotaFinal';
import ColunaNotaFinalRegencia from './colunaNotaFinalRegencia';
import { Info } from './fechamentoFinal.css';
import LinhaConceitoFinal from './linhaConceitoFinal';

const LinhaAluno = ({
  aluno,
  ehRegencia,
  ehNota,
  listaConceitos,
  disciplinaSelecionada,
  onChange,
  eventoData,
  notaMedia,
  frequenciaMedia,
  indexAluno,
  desabilitarCampo,
  ehSintese,
}) => {

  const obterValorConceito = valor => {
    if (listaConceitos && listaConceitos.length) {
      const conceito = listaConceitos.find(item => item.id == valor);
      return conceito ? conceito.valor : '';
    }
    return '';
  };

  const montaLinhaNotasConceitos = () => {
    if (ehNota) {
      return aluno.notasConceitoBimestre.map(c => (
        <div className="input-notas">{c.notaConceito}</div>
      ));
    }
    return aluno.notasConceitoBimestre
      .filter(n => n.disciplinaCodigo == disciplinaSelecionada)
      .map(c => (
        <div className="input-notas">{obterValorConceito(c.notaConceito)}</div>
      ));
  };

  const montaNotaFinal = (aluno, indexNotaConceito) => {
    if (aluno && aluno.notasConceitoFinal && aluno.notasConceitoFinal.length) {
      if (ehRegencia) {
        return aluno.notasConceitoFinal[indexNotaConceito];
      }
      return aluno.notasConceitoFinal[0];
    }
    return '';
  };

  const onChangeNotaConceitoFinal = (notaBimestre, valorNovo) => {
    notaBimestre.notaConceito = valorNovo;
    onChange(aluno, valorNovo, notaBimestre.disciplinaCodigo);
  };

  const montarCampoNotaConceitoFinal = (aluno, label, indexNotaConceito) => {
    if (ehNota) {
      return (
        <CampoNotaFinal
          montaNotaFinal={() => montaNotaFinal(aluno, indexNotaConceito)}
          onChangeNotaConceitoFinal={(nota, valor) =>
            onChangeNotaConceitoFinal(nota, valor)
          }
          desabilitarCampo={desabilitarCampo}
          podeEditar={aluno.podeEditar}
          eventoData={eventoData}
          mediaAprovacaoBimestre={notaMedia}
          label={label}
        />
      );
    }
    if (!ehNota) {
      return (
        <CampoConceitoFinal
          montaNotaConceitoFinal={() =>
            montaNotaFinal(aluno, indexNotaConceito)
          }
          onChangeNotaConceitoFinal={(nota, valor) =>
            onChangeNotaConceitoFinal(nota, valor)
          }
          desabilitarCampo={desabilitarCampo}
          podeEditar={aluno.podeEditar}
          listaTiposConceitos={listaConceitos}
          label={label}
        />
      );
    }
    return '';
  };

  return (
    <>
      <tr>
        <td className="col-numero-chamada">
          {aluno.informacao ? (
            <>
              <div className="linha-numero-chamada">{aluno.numeroChamada}</div>
              <Tooltip title={aluno.informacao} placement="top">
                <Info className="fas fa-circle" />
              </Tooltip>
            </>
          ) : (
            <div style={{ display: 'inline' }}>{aluno.numeroChamada}</div>
          )}
        </td>
        <td className="col-nome-aluno"> {aluno.nome}</td>
        {ehSintese ? (
          <td className="col-nota-conceito">{aluno.sintese}</td>
        ) : (
          <td className="col-nota-conceito">{montaLinhaNotasConceitos()}</td>
        )}
        <td>{aluno.totalFaltas}</td>
        <td>{aluno.totalAusenciasCompensadas}</td>
        {ehSintese ? (
          ''
        ) : (
          <td className="col-conceito-final">
            {ehRegencia ? (
              <ColunaNotaFinalRegencia indexLinha={indexAluno} />
            ) : (
              montarCampoNotaConceitoFinal(aluno)
            )}
          </td>
        )}

        <td>
          <span
            className={`${
              frequenciaMedia && aluno.frequencia < frequenciaMedia
                ? 'indicativo-alerta'
                : ''
            } `}
          >
            {aluno.frequencia}%
          </span>
        </td>
      </tr>
      <LinhaConceitoFinal
        indexLinha={indexAluno}
        aluno={aluno}
        montarCampoNotaConceitoFinal={(label, indexNotaConceito) =>
          montarCampoNotaConceitoFinal(aluno, label, indexNotaConceito)
        }
      />
    </>
  );
};

LinhaAluno.propTypes = {
  onChange: PropTypes.func,
  desabilitarCampo: PropTypes.bool,
  ehSintese: PropTypes.bool,
};

LinhaAluno.defaultProps = {
  onChange: () => {},
  desabilitarCampo: false,
  ehSintese: false,
};

export default LinhaAluno;
