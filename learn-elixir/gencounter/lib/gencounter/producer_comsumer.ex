defmodule Gencounter.ProducerComsumer do
  use GenStage
  require Integer

  def start_link(_any) do
    GenStage.start_link(__MODULE__, :state, name: __MODULE__)
  end

  def init(state) do
    {:producer_consumer, state, subscribe_to: [Gencounter.Producer]}
  end

  def handle_events(events, _from, state) do
    numbers =
      events
      |> Enum.filter(&Integer.is_even/1)

    {:noreply, numbers, state}
  end
end
