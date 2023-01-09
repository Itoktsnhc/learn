defmodule PlugEx do
  use Application
  require Logger
  def start(_type,_args) do
    port = Application.get_env(:plug_ex, :cowboy_port, 5000)
    children = [
      Plug.Cowboy.child_spec(
        scheme: :http,
        plug: PlugEx.Router,
        options: [port: port]
      )
    ]
    IO.puts "Server running on port #{port}"
    Supervisor.start_link(children, strategy: :one_for_one, name: PlugEx.Supervisor)
  end
end
