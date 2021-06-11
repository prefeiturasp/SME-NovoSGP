import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import NomeEstudanteLista from '~/componentes-sgp/NomeEstudanteLista/nomeEstudanteLista';

import { setExpandirLinha } from '~/redux/modulos/notasConceitos/actions';
import {
  acharItem,
  converterAcaoTecla,
  esperarMiliSegundos,
  moverCursor,
  tratarString,
} from '~/utils';

import CampoConceitoFinal from './campoConceitoFinal';
import CampoNotaFinal from './campoNotaFinal';
import ColunaNotaFinalRegencia from './colunaNotaFinalRegencia';
import { Info } from './fechamentoFinal.css';
import LinhaConceitoFinal from './linhaConceitoFinal';

const LinhaAluno = ({
  dados,
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
  registraFrequencia,
}) => {
  const expandirLinha = useSelector(
    store => store.notasConceitos.expandirLinha
  );
  const dispatch = useDispatch();

  const obterValorConceito = valor => {
    if (listaConceitos && listaConceitos.length) {
      const conceito = listaConceitos.find(item => item.id == valor);
      return conceito ? conceito.valor : '';
    }
    return '';
  };

  const montaLinhaNotasConceitos = () => {
    if (ehNota && ehRegencia) {
      return aluno.notasConceitoBimestre
        .filter(n => n.disciplinaCodigo == disciplinaSelecionada)
        .map(c => <div className="input-notas">{c.notaConceito}</div>);
    }

    if (ehNota && !ehRegencia) {
      return aluno.notasConceitoBimestre.map(c => (
        <div className="input-notas">{c.notaConceito}</div>
      ));
    }

    if (!ehNota && !ehRegencia) {
      return aluno.notasConceitoBimestre.map(c => (
        <div className="input-notas">{obterValorConceito(c.notaConceito)}</div>
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

  const clicarSetas = (
    e,
    alunoEscolhido,
    label = '',
    index = 0,
    regencia = false
  ) => {
    const direcao = converterAcaoTecla(e.keyCode);
    const disciplina = label.toLowerCase();

    if (direcao && regencia) {
      let novaLinha = [];
      const novoIndex = index + direcao;

      if (expandirLinha[novoIndex]) {
        expandirLinha[novoIndex] = false;
        novaLinha = expandirLinha;
      } else {
        novaLinha[novoIndex] = true;
      }
      dispatch(setExpandirLinha([...novaLinha]));
    }

    const alunoEscolhidoMover =
      direcao && acharItem(dados, alunoEscolhido, direcao, 'codigo');
    if (alunoEscolhidoMover.length) {
      const disciplinaTratada = tratarString(disciplina);
      const item = regencia ? disciplinaTratada : 'aluno';
      const itemEscolhido = `${item}${alunoEscolhidoMover[0].codigo}`;
      moverCursor(itemEscolhido, 0, regencia);
    }
  };

  const montarCampoNotaConceitoFinal = (
    alunoEscolhido,
    label,
    indexNotaConceito
  ) => {
    if (ehNota) {
      return (
        <CampoNotaFinal
          esconderSetas
          name={`aluno${alunoEscolhido.codigo}`}
          clicarSetas={e =>
            clicarSetas(e, alunoEscolhido, label, indexAluno, ehRegencia)
          }
          step={0}
          montaNotaFinal={() =>
            montaNotaFinal(alunoEscolhido, indexNotaConceito)
          }
          onChangeNotaConceitoFinal={(nota, valor) =>
            onChangeNotaConceitoFinal(nota, valor)
          }
          desabilitarCampo={desabilitarCampo}
          podeEditar={alunoEscolhido.podeEditar}
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
        <td className="col-nome-aluno">
          <NomeEstudanteLista
            nome={aluno?.nome}
            exibirSinalizacao={aluno.ehAtendidoAEE}
          />
        </td>
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
        {registraFrequencia ? <td>{aluno.frequencia}%</td> : ''}
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
