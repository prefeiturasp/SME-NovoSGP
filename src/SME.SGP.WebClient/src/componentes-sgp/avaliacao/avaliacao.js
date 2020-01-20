import { Tooltip, Icon } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import shortid from 'shortid';
import notasConceitos from '~/dtos/notasConceitos';
import { setModoEdicaoGeral } from '~/redux/modulos/notasConceitos/actions';

import Ordenacao from '../Ordenacao/ordenacao';
import {
  Lista,
  CaixaMarcadores,
  IconePlusMarcadores,
  CabecalhoNotaConceitoFinal,
  LinhaNotaConceitoFinal,
} from './avaliacao.css';
import CampoConceito from './campoConceito';
import CampoNota from './campoNota';
import { LabelSemDados } from '~/componentes';

const Avaliacao = props => {
  const dispatch = useDispatch();

  const { dados, notaTipo, onChangeOrdenacao, desabilitarCampos } = props;

  const [expandirLinha, setExpandirLinha] = useState([]);

  const onChangeNotaConceito = (nota, valorNovo) => {
    if (!desabilitarCampos && nota.podeEditar) {
      nota.notaConceito = valorNovo;
      nota.modoEdicao = true;
      dados.modoEdicao = true;
      dispatch(setModoEdicaoGeral(true));
    }
  };

  const descricaoAlunoAusente = 'Aluno ausente na data da avaliação';

  const montarCabecalhoAvaliacoes = () => {
    return dados.avaliacoes && dados.avaliacoes.length > 0
      ? dados.avaliacoes.map(avaliacao => {
          const descricaoSemHtml = avaliacao.descricao.replace(
            /<[^>]*>?/gm,
            ''
          );
          return (
            <th key={shortid.generate()} className="width-150">
              <div className="texto-header-avaliacao">{avaliacao.nome}</div>
              <div className="texto-header-avaliacao">
                <Tooltip title={descricaoSemHtml}>{descricaoSemHtml}</Tooltip>
              </div>
              <div className="texto-header-avaliacao">
                {window.moment(avaliacao.data).format('DD/MM/YYYY')}
              </div>
            </th>
          );
        })
      : '';
  };

  const montarCabecalhoInterdisciplinar = () => {
    return dados.avaliacoes && dados.avaliacoes.length > 0
      ? dados.avaliacoes.map(avaliacao => {
          return avaliacao.ehInterdisciplinar ? (
            <th key={shortid.generate()} className="width-150">
              <Tooltip
                title={montarToolTipDisciplinas(avaliacao.disciplinas)}
                placement="bottom"
                overlayStyle={{ fontSize: '12px' }}
              >
                <CaixaMarcadores>Interdisciplinar</CaixaMarcadores>
              </Tooltip>
            </th>
          ) : (
            <th key={shortid.generate()} className="width-150"></th>
          );
        })
      : '';
  };

  const montarToolTipDisciplinas = disciplinas => {
    let nomes = '';
    disciplinas.forEach(nomeDisciplina => {
      nomes += nomes.length > 0 ? `, ${nomeDisciplina}` : nomeDisciplina;
    });
    return nomes;
  };

  const montarCampoNotaConceito = nota => {
    return Number(notasConceitos.Notas) === Number(notaTipo) ? (
      <CampoNota
        nota={nota}
        onChangeNotaConceito={valorNovo =>
          onChangeNotaConceito(nota, valorNovo)
        }
        desabilitarCampo={desabilitarCampos}
      />
    ) : (
      <CampoConceito
        nota={nota}
        onChangeNotaConceito={valorNovo =>
          onChangeNotaConceito(nota, valorNovo)
        }
        desabilitarCampo={desabilitarCampos}
      />
    );
  };

  const onClickExpandir = index => {
    expandirLinha[index] = !expandirLinha[index];
    setExpandirLinha([...expandirLinha]);
  };

  return (
    <>
      {dados ? (
        <Lista className="mt-4 table-responsive">
          {dados.avaliacoes && dados.avaliacoes.length ? (
            <table className="table mb-0 ">
              <thead className="tabela-avaliacao-thead">
                <div className="scroll-tabela-avaliacao-thead">
                  <tr className="coluna-ordenacao-tr">
                    <th colSpan="2" className="width-460 coluna-ordenacao-th">
                      <Ordenacao
                        className="botao-ordenacao-avaliacao"
                        conteudoParaOrdenar={dados.alunos}
                        ordenarColunaNumero="numeroChamada"
                        ordenarColunaTexto="nome"
                        retornoOrdenado={retorno => {
                          setExpandirLinha([]);
                          dados.alunos = retorno;
                          onChangeOrdenacao(dados);
                        }}
                      />
                    </th>
                    {montarCabecalhoAvaliacoes()}
                    <CabecalhoNotaConceitoFinal
                      className="width-150"
                      rowspan="2"
                    >
                      Nota final
                    </CabecalhoNotaConceitoFinal>
                  </tr>
                  <tr>
                    <th colSpan="2" className="width-460 " />
                    {montarCabecalhoInterdisciplinar()}
                  </tr>
                </div>
              </thead>
            </table>
          ) : (
            <LabelSemDados
              text="Bimestre selecionado não possui atividade avaliativa cadastrada"
              center
            />
          )}
          <table className="table mb-0">
            <tbody className="tabela-avaliacao-tbody">
              <div className="scroll-tabela-avaliacao-tbody">
                {dados.alunos.map((aluno, i) => {
                  return (
                    <>
                      <tr key={shortid.generate()}>
                        <td className="width-60 text-center font-weight-bold">
                          {aluno.numeroChamada}
                        </td>
                        <td className="width-400 text-left">
                          {aluno.nome}
                          {aluno.marcador ? (
                            <>
                              <CaixaMarcadores>
                                {aluno.marcador.nome}
                              </CaixaMarcadores>
                              <IconePlusMarcadores
                                onClick={() => onClickExpandir(i)}
                                className={
                                  expandirLinha[i]
                                    ? 'fas fa-minus fa-minus-linha-expandida '
                                    : 'fas fa-plus-circle'
                                }
                              />
                            </>
                          ) : (
                            ''
                          )}
                        </td>
                        {aluno.notasAvaliacoes.length
                          ? aluno.notasAvaliacoes.map(nota => {
                              return (
                                <td
                                  key={shortid.generate()}
                                  className={`width-150 ${
                                    nota.podeEditar ? '' : 'desabilitar-nota'
                                  }`}
                                  //style={{ padding: '3px' }}
                                >
                                  {montarCampoNotaConceito(nota)}
                                  {nota.ausente ? (
                                    <Tooltip title={descricaoAlunoAusente}>
                                      <i className="fas fa-user-times icon-aluno-ausente" />
                                    </Tooltip>
                                  ) : (
                                    ''
                                  )}
                                </td>
                              );
                            })
                          : ''}
                        <LinhaNotaConceitoFinal className="width-150">
                          {montarCampoNotaConceito({
                            atividadeAvaliativaId: null,
                            ausente: null,
                            notaConceito: '',
                            podeEditar: true,
                          })}
                        </LinhaNotaConceitoFinal>
                      </tr>
                      {expandirLinha[i] ? (
                        <>
                          <tr className="linha-expandida">
                            <td colSpan="1" className="text-center">
                              <Icon type="double-right" />
                            </td>
                            <td colSpan={dados.avaliacoes.length + 2}>
                              {aluno.marcador.descricao}
                            </td>
                          </tr>
                        </>
                      ) : (
                        ''
                      )}
                    </>
                  );
                })}
              </div>
            </tbody>
          </table>
        </Lista>
      ) : (
        ''
      )}
    </>
  );
};

Avaliacao.propTypes = {
  notaTipo: PropTypes.number,
  onChangeOrdenacao: () => {},
};

Avaliacao.defaultProps = {
  notaTipo: 0,
  onChangeOrdenacao: () => {},
};

export default Avaliacao;
