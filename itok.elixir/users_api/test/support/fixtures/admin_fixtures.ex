defmodule UsersApi.AdminFixtures do
  @moduledoc """
  This module defines test helpers for creating
  entities via the `UsersApi.Admin` context.
  """

  @doc """
  Generate a user.
  """
  def user_fixture(attrs \\ %{}) do
    {:ok, user} =
      attrs
      |> Enum.into(%{

      })
      |> UsersApi.Admin.create_user()

    user
  end

  @doc """
  Generate a unique user email.
  """
  def unique_user_email, do: "some email#{System.unique_integer([:positive])}"
end
