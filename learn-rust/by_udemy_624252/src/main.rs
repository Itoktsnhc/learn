#![allow(unused)]

mod sh;

use std::mem;
use std::f64::consts::PI;
use crate::sh::stack_and_heap;
use std::io::stdin;
use crate::State::Locked;
use std::collections::HashMap;
use std::fmt::{Debug, Formatter};

fn main() {
    //trait_params();
    //traits();
    //hashmaps();
    //vectors();
    //structures();
    //combination_lock();
    //match_statement();
    //if_statement();
    //stack_and_heap();
    //scope_and_shadowing();
}

trait Shape {
    fn area(&self) -> f64;
}

#[derive(Debug)]
struct Circle {
    radius: f64,
}

#[derive(Debug)]
struct Square {
    side: f64,
}

impl Shape for Square {
    fn area(&self) -> f64 {
        return self.side * self.side;
    }
}

impl Shape for Circle {
    fn area(&self) -> f64 {
        PI * self.radius * self.radius
    }
}


fn print_info<T: Shape + Debug>(shape: T) {
    println!("the area of shape {:?} is {}", shape, shape.area())
}

fn trait_params() {
    let cir = Circle { radius: 4.0 };
    print_info(cir);
    let square = Square { side: 4.0 };
    print_info(square);
}

trait Animal {
    fn create(name: &'static str) -> Self;
    fn name(&self) -> &'static str;
    fn talk(&self) {
        println!("{} cannot talk", self.name())
    }
}

struct Human {
    name: &'static str,
}

struct Cat {
    name: &'static str,
}

impl Animal for Cat {
    fn create(name: &'static str) -> Cat {
        Cat { name }
    }

    fn name(&self) -> &'static str {
        self.name
    }
    fn talk(&self) {
        println!("{} just meo meo", self.name);
    }
}

impl Animal for Human {
    fn create(name: &'static str) -> Human {
        Human { name }
    }

    fn name(&self) -> &'static str {
        self.name
    }
    fn talk(&self) {
        println!("{} can talk !", self.name())
    }
}

trait Summable<T> {
    fn sum(&self) -> T;
}

impl Summable<i32> for Vec<i32> {
    fn sum(&self) -> i32 {
        self.iter().sum()
    }
}

fn traits() {
    let h = Human::create("123");
    h.talk();
    let c = Cat::create("糊糊");
    c.talk();
    let vec = vec![1, 2, 3];

    println!("sum = {}", vec.sum());
}

fn hashmaps() {
    let mut shapes = HashMap::new();
    shapes.insert(String::from("triangle"), 3);
    shapes.insert(String::from("square"), 4);

    for (key, val) in shapes {
        println!("{} => {}", key, val);
    }
}

fn vectors() {
    let mut a = Vec::new();
    a.push(1);
    a.push(2);
    a.push(3);
    while let Some(x) = a.pop() {
        println!("{}", x);
    }
}

fn structures() {
    let start = Point { x: 3.0, y: 4.0 };
    let end = Point { x: 5.0, y: 10.0 };
    let line = Line { start, end };
}

struct Line {
    start: Point,
    end: Point,
}

struct Point {
    x: f64,
    y: f64,
}

enum State {
    Locked,
    Failed,
    Unlocked,
}

fn combination_lock() {
    println!("Please input your code:");
    let code = String::from("1234");
    let mut state = State::Locked;
    let mut entry = String::new();
    loop {
        match state {
            State::Locked => {
                let mut input = String::new();
                match stdin().read_line(&mut input) {
                    Ok(_) => {
                        entry.push_str(&input.trim_end())
                    }
                    Err(_) => continue
                }
                if entry == code {
                    state = State::Unlocked;
                    continue;
                }
                if !code.starts_with(&entry) {
                    state = State::Failed;
                }
            }
            State::Failed => {
                println!("FAILED");
                entry.clear();
                state = Locked;
                continue;
            }
            State::Unlocked => {
                println!("UNLOCKED");
                return;
            }
        }
    }
}

fn match_statement() {
    let country_code = 1000;
    let country = match country_code {
        44 => "UK",
        46 => "Sweden",
        7 => "Russia",
        1..=1000 => "unknown",
        _ => "invalid"
    };
    println!("the country with code {}  is {}", country_code, country);
}

fn if_statement() {
    let tmp = 35;
    if tmp > 30
    {
        println!("So Hot");
    }

    let day = if tmp > 20 { "sunny" } else { "cloudy" };
}


fn scope_and_shadowing() {
    let a = 123;
    {
        let b = 456;
        println!("inside b is  {}", b);
        let a = 678;
        println!("inside a is  {}", a);
    }
    println!("outside a is  {}", a);
}

fn operators() {
    let mut a = 2 + 3 * 4;
    println!("{}", a);
    a += 1;
    let a_cubed = i32::pow(a, 3);
    println!("cube of a is {}", a_cubed);
    let b = 2.5;
    let b_cubed = f64::powi(b, 3);
    let b_to_pi = f64::powf(b, PI);
    println!("cube of b is {}; and pi of b is {}", b_cubed, b_to_pi);
    let c = 1 | 2;
    println!("c is {}", c);
    println!("2>>1 is {}", 2 >> 1);
}

fn core_types() {
    let a: u8 = 10;
    println!("a = {}", a);
    let mut b: i8 = 19;
    println!("b = {} before", b);
    b = 34;
    println!("b = {} after", b);
    let c: i64 = 123456789;
    println!("c= {}, takes up to {} bytes", c, mem::size_of_val(&c));
    let z: isize = 123;
    let size_of_z = mem::size_of_val(&z);
    println!("z= {}, takes up to {} bytes on {}-bit OS", z, mem::size_of_val(&z), size_of_z * 8);

    let x: char = 'c';
    println!("d = {}, takes up to {} bytes", x, mem::size_of_val(&x));
    let e = 2.5;
    println!("e = {}, takes up to {} bytes", e, mem::size_of_val(&e));

    let g: bool = true;
    println!("g = {}, takes up to {} bytes", g, mem::size_of_val(&g));
}
