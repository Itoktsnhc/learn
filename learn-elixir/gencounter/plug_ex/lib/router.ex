defmodule PlugEx.Router do
  use Plug.Router
  plug :match
  plug :dispatch
  plug Plug.Static, at: "/home", from: :server

  get "/hello" do
    send_resp(conn, 200, "Hello World!")
  end
  get "/about/:user_name" do
    send_resp(conn, 200, "Hello #{user_name}")
  end
  get "/home" do
    conn = put_resp_content_type(conn, "text/html")
    send_file(conn, 200, "lib/index.html")
  end

  match(_, do: send_resp(conn, 404, "Oops!"))
end
