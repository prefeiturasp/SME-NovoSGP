import React, { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
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

const ListaAlunos = props => {
  const { bimestreSelecionado } = props;
  const dispatch = useDispatch();

  const [dadosBimestres, setDadosBimestres] = useState([]);
  const [dadosBimestre, setDadosBimestre] = useState([]);

  const dados = [
    {
      bimestreId: 1,
      totalAulasPrevistas: 10,
      totalAulasDadas: 5,
      alunos: [
        {
          numeroChamada: 1,
          nome: 'Aluno com um nome grande',
          compensacoes: 1,
          frequencia: '90%',
          ausencias: [
            {
              data: '12/10/2020',
              motivo: 'Atestado médico do estudante',
            },
          ],
        },
        {
          numeroChamada: 2,
          nome: 'Aluno com um nome grande 2 ',
          compensacoes: 1,
          frequencia: '90%',
          ausencias: [
            {
              data: '12/10/2020',
              motivo: 'Atestado médico do estudante',
              anotacao:
                '<strong>Atestado médico do estudante Atestado médico do estudante Atestado médico do estudante Atestado médico do estudante</strong> Amaral dos Santos',
            },
          ],
        },
      ],
    },
  ];

  useEffect(() => {
    setDadosBimestres(dados);
    setDadosBimestre(dados.find(a => a.bimestreId === bimestreSelecionado));
  }, []);

  const onChangeOrdenacao = alunosOrdenados => {
    dispatch(setExpandirLinhaFrequenciaAluno([]));
    setDadosBimestres({
      bimestreId: bimestreSelecionado,
      alunos: alunosOrdenados,
    });
    // const bimestre = dados.findIndex(
    //   obj => obj.bimestreId === bimestreSelecionado
    // );
    // dados[bimestre].alunos = alunosOrdenados;
    // setDadosBimestres(dados);
  };

  // onChangeOrdenacao={onChangeOrdenacao}

  return (
    <>
      <TabelaColunasFixas>
        <div className="row">
          <div className="col-md-6 col-sm-12">
            <Ordenacao
              className="mb-2"
              conteudoParaOrdenar={dadosBimestre?.alunos}
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
                {dadosBimestre?.alunos?.map((data, index) => {
                  return (
                    <>
                      <tr id={index}>
                        <td className="col-valor-linha-tres">
                          <strong>{data.numeroChamada}</strong>
                        </td>
                        <td className="col-valor-linha-quatro">{data.nome}</td>
                        <td className="col-valor-linha-dois">
                          {data.ausencias.length}
                        </td>
                        <td className="col-valor-linha-dois">
                          {data.compensacoes}
                        </td>
                        <td className="col-valor-linha-dois">
                          {data.frequencia}
                          <BtnExpandirFrequenciaAluno indexLinha={index} />
                        </td>
                      </tr>
                      <AusenciasAluno
                        indexLinha={index}
                        dados={data.ausencias}
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
  bimestreSelecionado: PropTypes.number,
};

ListaAlunos.defaultProps = {
  bimestreSelecionado: PropTypes.number,
};

export default ListaAlunos;
