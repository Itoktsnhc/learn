defmodule PlugEx do
  use Application
  require Logger
  def start(_type,_args) do
    children = [
      Plug.Cowboy.child_spec(
        scheme: :http,
        plug: PlugEx.Router,
        options: [port: 8000]
      )
    ]
    Logger.info "Starting PlugEx"
    Supervisor.start_link(children, strategy: :one_for_one, name: PlugEx.Supervisor)
  end
end
