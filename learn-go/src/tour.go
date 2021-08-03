package main

import (
	"fmt"
	"math"
	"math/cmplx"
	"runtime"
	"time"
)

func main1() {
	fmt.Println(math.Pi)
	fmt.Println(add(1, 5))
	fmt.Println(addV2(1, 5))
	fmt.Println(swap("1", "3"))
	fmt.Println(split(17))
	mainVar()
	basicTypes()
	CheckEnv()
	SliceAndArray()

}

func main2() {
	var first = intSeq()
	var second = intSeq()
	fmt.Println(first())
	fmt.Println(second())
	fmt.Println(first())
	fmt.Println(second())
	fmt.Println(first())
	fmt.Println(second())
}

func main() {
	done := make(chan bool, 1)
	go worker(done)
	<-done
}

func ping(pings chan<- string, msg string) {
	pings <- msg
}

func worker(done chan bool) {
	fmt.Print("working...")
	time.Sleep(time.Second)
	fmt.Println("done")
	done <- true
}

func f(from string) {
	for i := 0; i < 3; i++ {
		fmt.Println(from, ":", i)
	}
}

func intSeq() func() int {
	i := 0
	return func() int {
		i++
		return i
	}
}

func add(x int, y int) int {
	return x + y
}

//参数列表中如果多个同类型参数，只需要标明最后一个
func addV2(x, y int) int {
	return x + y
}

//多值返回
func swap(x, y string) (string, string) {
	return y, x
}

//命名返回值
func split(sum int) (x, y int) {
	x = sum * 4 / 9
	y = sum - x
	return
}

//简短版本变量定义与赋值
func mainVar() {
	var i, j int = 1, 2
	k := 3
	c, python, java := true, false, "no!"

	fmt.Println(i, j, k, c, python, java)
}

//基础类型
var (
	ToBe   bool       = false
	MaxInt uint64     = 1<<64 - 1
	z      complex128 = cmplx.Sqrt(-5 + 12i)
)

func basicTypes() {
	fmt.Printf("Type: %T Value: %v\n", ToBe, ToBe)
	fmt.Printf("Type: %T Value: %v\n", MaxInt, MaxInt)
	fmt.Printf("Type: %T Value: %v\n", z, z)
}

func Sqrt(x float64) float64 {
	z := 1.0
	for i := 0; i < 10; i++ {
		z -= (z*z - x) / (2 * z)
		fmt.Println(z)
	}
	return z
}

func CheckEnv() {
	fmt.Print("Go runs on ")
	switch os := runtime.GOOS; os {
	case "darwin":
		fmt.Println("OS X.")
	case "linux":
		fmt.Println("Linux.")
	default:
		// freebsd, openbsd,
		// plan9, windows...
		fmt.Printf("%s.\n", os)
	}
}

func SliceAndArray() {
	names := [4]string{
		"John",
		"Paul",
		"George",
		"Ringo",
	}
	fmt.Println(names)
	a := names[0:2]
	b := names[1:3]
	fmt.Println(a, b)

	b[0] = "XXX"
	fmt.Println(a, b)
	fmt.Println(names)
}
