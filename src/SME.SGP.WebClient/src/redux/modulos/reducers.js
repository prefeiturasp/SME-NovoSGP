import { combineReducers } from 'redux';

import navegacao from './navegacao/reducers';
import alertas from './alertas/reducers';
import usuario from './usuario/reducers';
import notificacoes from './notificacoes/reducers';
import perfil from './perfil/reducers';
import calendarioEscolar from './calendarioEscolar/reducers';
import calendarioProfessor from './calendarioProfessor/reducers';
import bimestres from './planoAnual/reducers';
import filtro from './filtro/reducers';
import planoAula from './planoAula/reducers';

export default combineReducers({
  navegacao,
  alertas,
  usuario,
  perfil,
  calendarioEscolar,
  notificacoes,
  bimestres,
  filtro,
  planoAula,
  calendarioProfessor,
});
