import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import BtnExpandirFrequenciaAluno from './btnExpandirFrequenciaAluno';
import AusenciasAluno from './ausenciasAluno';
import Ordenacao from '~/componentes-sgp/Ordenacao/ordenacao';
import { setExpandirLinhaFrequenciaAluno } from '~/redux/modulos/acompanhamentoFrequencia/actions';

import {
  TabelaColunasFixas,
  Marcadores,
  MarcadorAulas,
} from './listaAlunos.css';
import ModalAnotacoesAcompanhamentoFrequencia from './modalAnotacoesAcompanhamentoFrequencia';

import ServicoAcompanhamentoFrequencia from '~/servicos/Paginas/DiarioClasse/ServicoAcompanhamentoFrequencia';
import { erros } from '~/servicos';

const ListaAlunos = props => {
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { bimestreSelecionado, componenteCurricularId } = props;
  const dispatch = useDispatch();

  const [dadosBimestre, setDadosBimestre] = useState([]);

  useEffect(() => {
    const obterAlunos = async () => {
      const retorno = await ServicoAcompanhamentoFrequencia.obterAcompanhamentoFrequenciaPorBimestre(
        turmaSelecionada?.id,
        componenteCurricularId,
        bimestreSelecionado
      ).catch(e => erros(e));

      if (retorno?.data) {
        setDadosBimestre(retorno?.data);
      }
    };

    obterAlunos();
    // setDadosBimestres(dados);
    // setDadosBimestre(dados.find(a => a.bimestreId === bimestreSelecionado));
  }, []);

  const onChangeOrdenacao = alunosOrdenados => {
    dispatch(setExpandirLinhaFrequenciaAluno([]));
    setDadosBimestre({ ...dadosBimestre, frequenciaAlunos: alunosOrdenados });
  };

  return (
    <>
      <ModalAnotacoesAcompanhamentoFrequencia />
      <TabelaColunasFixas>
        <div className="row">
          <div className="col-md-6 col-sm-12">
            <Ordenacao
              className="mb-2"
              conteudoParaOrdenar={dadosBimestre?.frequenciaAlunos}
              ordenarColunaNumero="numeroChamada"
              ordenarColunaTexto="nome"
              retornoOrdenado={retorno => {
                onChangeOrdenacao(retorno);
              }}
            />
          </div>

          <Marcadores className="col-md-6 col-sm-12 d-flex justify-content-end">
            <MarcadorAulas className="ml-2">
              <span>Aulas previstas </span>
              <span className="numero">
                {dadosBimestre && dadosBimestre.totalAulasPrevistas
                  ? dadosBimestre.totalAulasPrevistas
                  : 0}
              </span>
            </MarcadorAulas>
            <MarcadorAulas className="ml-2">
              <span>Aulas dadas </span>
              <span className="numero">
                {dadosBimestre && dadosBimestre.totalAulasDadas
                  ? dadosBimestre.totalAulasDadas
                  : 0}
              </span>
            </MarcadorAulas>
          </Marcadores>
        </div>
        <div className="wrapper">
          <div className="header-fixo">
            <table className="table">
              <thead className="tabela-dois-thead">
                <tr>
                  <th className="col-linha-quatro" colSpan="2">
                    Nome
                  </th>
                  <th className="col-linha-dois">Ausências no Bimestre</th>
                  <th className="col-linha-dois">Compensações de ausência</th>
                  <th className="col-linha-dois">Frequência</th>
                </tr>
              </thead>
              <tbody className="tabela-um-tbody">
                {dadosBimestre?.frequenciaAlunos?.map((data, index) => {
                  return (
                    <>
                      <tr id={index}>
                        <td className="col-valor-linha-tres">
                          <strong>{data.numeroChamada}</strong>
                        </td>
                        <td className="col-valor-linha-quatro">{data.nome}</td>
                        <td className="col-valor-linha-dois">
                          {data.ausencias}
                        </td>
                        <td className="col-valor-linha-dois">
                          {data.compensacoes}
                        </td>
                        <td className="col-valor-linha-dois">
                          {data.frequencia}%
                          {data.ausencias > 0 ? (
                            <BtnExpandirFrequenciaAluno
                              indexLinha={index}
                              codigoAluno={data.alunoRf}
                            />
                          ) : (
                            <></>
                          )}
                        </td>
                      </tr>
                      <AusenciasAluno
                        indexLinha={index}
                        dados={data.ausencias}
                        turmaId={turmaSelecionada?.id}
                        componenteCurricularId={componenteCurricularId}
                        codigoAluno={data.alunoRf}
                      />
                    </>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
      </TabelaColunasFixas>
    </>
  );
};

ListaAlunos.propTypes = {
  componenteCurricularId: PropTypes.string,
  bimestreSelecionado: PropTypes.number,
};

ListaAlunos.defaultProps = {
  componenteCurricularId: PropTypes.string,
  bimestreSelecionado: PropTypes.number,
};

export default ListaAlunos;
