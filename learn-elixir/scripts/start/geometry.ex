defmodule Geometry do
	def rectangle_area(a, b) do
		a * b
	end
	def square_area(a) do
		rectangle_area(a, a)
	end
end

defmodule Calculator do
	def sum(a) do
		sum(a,0)
	end

	def sum(a,b) do
		a+b
	end
end