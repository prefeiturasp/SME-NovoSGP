import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import Label from '~/componentes/label';
import modalidade from '~/dtos/modalidade';
import { erros } from '~/servicos/alertas';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { Input } from 'antd';
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import situacaoPendenciaDto from '~/dtos/situacaoPendenciaDto';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import Auditoria from '~/componentes/auditoria';
import {
  PendenteForm,
  AprovadoForm,
  ResolvidoForm,
} from './labelSituacaoFechamento.css';

const { TextArea } = Input;

const PendenciasFechamentoForm = ({ match }) => {
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const [carregandoDisciplinas, setCarregandoDisciplinas] = useState(false);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [listaBimestres, setListaBimestres] = useState([]);
  const [auditoria, setAuditoria] = useState([]);
  const [exibirAuditoria, setExibirAuditoria] = useState(false);

  const [idPendenciaFechamento, setIdPendenciaFechamento] = useState(0);
  const [codigoComponenteCurricular, setCodigoComponenteCurricular] = useState(
    undefined
  );
  const [bimestre, setBimestre] = useState('');
  const [situacaoPendencia, setSituacaoPendencia] = useState();
  const [descricao, setdescricao] = useState('');
  const [detalhamento, setDetalhamento] = useState('');

  useEffect(() => {
    const obterDisciplinas = async () => {
      setCarregandoDisciplinas(true);
      const disciplinas = await ServicoDisciplina.obterDisciplinasPorTurma(
        turmaSelecionada.turma
      ).catch(e => erros(e));

      if (disciplinas && disciplinas.data && disciplinas.data.length) {
        setListaDisciplinas(disciplinas.data);
      } else {
        setListaDisciplinas([]);
      }
      setCarregandoDisciplinas(false);
    };

    if (turmaSelecionada.turma) {
      obterDisciplinas(turmaSelecionada.turma);
    }

    let listaBi = [];
    if (turmaSelecionada.modalidade == modalidade.EJA) {
      listaBi = [
        { valor: 1, descricao: 'Primeiro bimestre' },
        { valor: 2, descricao: 'Segundo bimestre' },
      ];
    } else {
      listaBi = [
        { valor: 1, descricao: 'Primeiro bimestre' },
        { valor: 2, descricao: 'Segundo bimestre' },
        { valor: 3, descricao: 'Terceiro bimestre' },
        { valor: 4, descricao: 'Quarto bimestre' },
      ];
    }
    setListaBimestres(listaBi);
  }, [turmaSelecionada.turma, turmaSelecionada.modalidade]);

  const mock = () => {
    setSituacaoPendencia({
      id: 1,
      descricao: 'Pendente',
    });
    setCodigoComponenteCurricular('1105');
    setBimestre('2');
    setdescricao(
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore '
    );
    setDetalhamento(
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore '
    );
    setExibirAuditoria(true);
    setAuditoria({
      criadoPor: 'TESTE',
      criadoRf: '999999',
      criadoEm: '01/01/2020',
      alteradoPor: 'TESTE 2',
      alteradoRf: '000000',
      alteradoEm: '01/01/2020',
    });
    debugger;
  };

  useEffect(() => {
    const consultaPorId = async () => {
      if (match && match.params && match.params.id) {
        setBreadcrumbManual(
          match.url,
          'Análise de pendências',
          RotasDto.PENDENCIAS_FECHAMENTO
        );
        setIdPendenciaFechamento(match.params.id);

        // TODO Remover
        mock();

        // const retorno = await api
        //   .get(`v1/fechamento/pendencias-fechamento/${match.params.id}`)
        //   .catch(e => erros(e));

        // if (retorno && retorno.data) {
        //   // SETAR DADOS!
        //   setAuditoria({
        //     criadoPor: cadastrado.data.criadoPor,
        //     criadoRf: cadastrado.data.criadoRf,
        //     criadoEm: cadastrado.data.criadoEm,
        //     alteradoPor: cadastrado.data.alteradoPor,
        //     alteradoRf: cadastrado.data.alteradoRf,
        //     alteradoEm: cadastrado.data.alteradoEm,
        //   });
        //   setExibirAuditoria(true);
        // }
      }
    };

    consultaPorId();
  }, []);

  const onClickVoltar = () => history.push('/fechamento/pendencias-fechamento');

  const onClickAprovar = () => {
    // TODO Chamar endpoint
    alert('Aprovar');
  };

  const montarLabelSituacaoPendencia = () => {
    if (situacaoPendencia && situacaoPendencia.id) {
      switch (situacaoPendencia.id) {
        case situacaoPendenciaDto.Aprovada:
          return (
            <AprovadoForm>
              <span>{situacaoPendencia.descricao}</span>
            </AprovadoForm>
          );
        case situacaoPendenciaDto.Pendente:
          return (
            <PendenteForm>
              <span>{situacaoPendencia.descricao}</span>
            </PendenteForm>
          );
        case situacaoPendenciaDto.Resolvida:
          return (
            <ResolvidoForm>
              <span>{situacaoPendencia.descricao}</span>
            </ResolvidoForm>
          );
        default:
          return '';
      }
    }
    return '';
  };

  return (
    <>
      {usuario && turmaSelecionada.turma ? (
        ''
      ) : (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'pendencias-selecione-turma',
            mensagem: 'Você precisa escolher uma turma.',
            estiloTitulo: { fontSize: '18px' },
          }}
          className="mb-2"
        />
      )}
      <Cabecalho pagina="Análise de Pendências" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <Button
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVoltar}
              />
              <Button
                label="Aprovar"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={onClickAprovar}
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-6 col-lg-4 col-xl-3 mb-2">
              <SelectComponent
                label="Bimestre"
                id="bimestre"
                valueOption="valor"
                valueText="descricao"
                lista={listaBimestres}
                valueSelect={bimestre}
                disabled
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-4 col-xl-3 mb-2">
              <Loader loading={carregandoDisciplinas} tip="">
                <SelectComponent
                  label="Componente curricular"
                  id="disciplina"
                  lista={listaDisciplinas}
                  valueOption="codigoComponenteCurricular"
                  valueText="nome"
                  valueSelect={codigoComponenteCurricular}
                  disabled
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-4 col-xl-3 mb-2">
              {montarLabelSituacaoPendencia()}
            </div>
            <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
              <Label text="Descrição" />
              <TextArea
                id="descricao"
                autoSize={{ minRows: 3, maxRows: 5 }}
                value={descricao}
                disabled
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
              <Label text="Detalhamento" />
              <TextArea
                id="detalhamento"
                autoSize={{ minRows: 3, maxRows: 5 }}
                value={detalhamento}
                disabled
              />
            </div>
          </div>
        </div>
        {exibirAuditoria ? (
          <Auditoria
            criadoEm={auditoria.criadoEm}
            criadoPor={auditoria.criadoPor}
            alteradoPor={auditoria.alteradoPor}
            alteradoEm={auditoria.alteradoEm}
          />
        ) : (
          ''
        )}
      </Card>
    </>
  );
};

export default PendenciasFechamentoForm;
