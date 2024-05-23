docker_compose(["./docker-compose.elk.yml","./docker-compose.logs.yml","./docker-compose.infra.yml","./docker-compose.monitor.yml"])
local_resource('check-kibana',serve_cmd='ping 127.0.0.1 -n 10000', labels=["saude"], readiness_probe=probe(initial_delay_secs = 30 , timeout_secs = 1 , period_secs = 10, tcp_socket = tcp_socket_action(5601, host = 'localhost')))
local_resource('check-elastic',serve_cmd='ping 127.0.0.1 -n 10000', labels=["saude"], readiness_probe=probe(initial_delay_secs = 30 , timeout_secs = 1 , period_secs = 10, tcp_socket = tcp_socket_action(9200, host = 'localhost')))
local_resource('check-rabbitmq',serve_cmd='ping 127.0.0.1 -n 10000', labels=["saude"], readiness_probe=probe(initial_delay_secs = 30 , timeout_secs = 1 , period_secs = 10, tcp_socket = tcp_socket_action(5672, host = 'localhost')))
local_resource('check-postgres',serve_cmd='ping 127.0.0.1 -n 10000', labels=["saude"], readiness_probe=probe(initial_delay_secs = 30 ,  timeout_secs = 1 , period_secs = 10, tcp_socket = tcp_socket_action(5432, host = 'localhost')))
dc_resource('setup-elastic', resource_deps=['elastic','check-elastic'], labels="elk")
dc_resource('elastic', labels="elk")
dc_resource('kibana', resource_deps=['elastic','check-elastic'], labels="elk")
dc_resource('apm', trigger_mode = TRIGGER_MODE_MANUAL, resource_deps=['elastic','check-elastic','kibana','check-kibana'], labels="apm")
dc_resource('rabbitmq', labels="logs")
dc_resource('setup-rabbitmq',trigger_mode = TRIGGER_MODE_MANUAL, resource_deps=['rabbitmq','check-rabbitmq'], labels="logs")
dc_resource('logstash',trigger_mode = TRIGGER_MODE_MANUAL, resource_deps=['rabbitmq','check-rabbitmq','elastic','check-elastic'], labels="logs")
dc_resource('flyway',trigger_mode = TRIGGER_MODE_MANUAL, resource_deps=['postgres','check-postgres'], labels="banco")
dc_resource('postgres',trigger_mode = TRIGGER_MODE_MANUAL, labels="banco")
dc_resource('redis',trigger_mode = TRIGGER_MODE_MANUAL, labels="cache")
dc_resource('minio',trigger_mode = TRIGGER_MODE_MANUAL, labels="minio")
dc_resource('prometheus',trigger_mode = TRIGGER_MODE_MANUAL, labels="monitor")
dc_resource('grafana',trigger_mode = TRIGGER_MODE_MANUAL, labels="monitor")



config.define_string_list("to-run", args=True)
cfg = config.parse()
groups = {
  'logs': ['setup-elastic', 'elastic', 'kibana', 'setup-rabbitmq','rabbitmq', 'logstash'],
  'elk': ['setup-elastic', 'elastic', 'kibana', 'logstash'],
  'apm': ['setup-rabbitmq', 'elastic', 'kibana', 'apm'],
  'rabbitmq': ['setup-rabbitmq', 'rabbitmq'],
  'postgres' : ['postgres','flyway'],
  'redis' : ['redis'],
  'minio' : ['minio'],
  'monitor' : ['prometheus','grafana'],
}
resources = []
for arg in cfg.get('to-run', []):
  if arg in groups:
    resources += groups[arg]
  else:
    # also support specifying individual services instead of groups, e.g. `tilt up a b d`
    resources.append(arg)
config.set_enabled_resources(resources)
