#![allow(unused)]

mod sh;

use std::mem;
use std::f64::consts::PI;
use crate::sh::stack_and_heap;
use std::io::{stdin, Take};
use crate::State::Locked;
use std::collections::HashMap;
use std::fmt::{Debug, Formatter};
use std::ops::{Add, AddAssign};
use std::process::Output;
use std::rc::Rc;
use std::sync::Arc;
use std::thread;

fn main() {
    arc();
    //ref_counter();
    //struct_lifetime();
    //lifetime();
    //borrowing();
    //ownership();
    //static_dispatch();
    //operator_overloading();
    //trait_drop();
    //trait_intro();
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

struct Anim {
    name: Arc<String>,
}

impl Anim {
    fn new(name: Arc<String>) -> Anim {
        Anim { name }
    }
    fn greeting(&self) {
        println!("Hi, My name is {}", self.name);
    }
}

fn arc() {
    let name = Arc::new("Jerry".to_string());
    let person = Anim::new(name.clone());
    let t = thread::spawn(move || {
        person.greeting();
    });

    println!("Name = {}", name);
    t.join().unwrap();
}

struct Ani {
    name: Rc<String>,
}

impl Ani {
    fn new(name: Rc<String>) -> Ani {
        Ani { name }
    }
    fn greeting(&self) {
        println!("Hi, My name is {}", self.name);
    }
}

fn ref_counter() {
    let name = Rc::new("Tom".to_string());
    println!("name = {}, has strong point count: {}", name, Rc::strong_count(&name));

    {
        let a = Ani::new(name.clone());
        println!("name = {}, has strong point count: {}", name, Rc::strong_count(&name));
        a.greeting();
    }
    println!("{}", name.clone());
    println!("name = {}, has strong point count: {}", name, Rc::strong_count(&name));
}

struct P<'a> {
    name: &'a str,
}

impl<'l> P<'l> {
    //需要显式的声明生命周期
    fn talk(&self)
    {
        println!("Hi my name is {}.", self.name);
    }
}

fn struct_lifetime() {
    let p = P { name: "Dmitri" };
    p.talk();
}

struct Company<'z> {
    name: String,
    ceo: &'z Person,
}

fn lifetime() {
    let boss = Person { name: "Jack".to_string() };
    let com = Company { name: "Ali".to_string(), ceo: &boss };
}

fn borrowing() {
    let print_vector = |x: &Vec<i32>|
        {
            println!("x[0] = {}", x[0]);
        };
    let v = vec![3, 2, 1];
    print_vector(&v);
    println!("v[0] = {}", v[0]);

    let mut a = 40;
    let b = &mut a;
    *b += 2;
    println!("b = {}", b);
    println!("a = {}", a);

    let mut z = vec![3, 2, 1];
    for i in &z {
        println!("{}", i);
    }
}

fn ownership() {
    let v = vec![1, 2, 3];
    //let v2 = v;//move for point type;
    //print!("{:?}", v); value borrowed here after move

    let mut a = 1;
    let b = a;
    a = 9;
    println!("{}", b);
    println!("{}", a);//primitive type always copy

    let print_vector = |x: Vec<i32>| -> Vec<i32>{
        println!("{:?}", x);
        x
    };
    let vv = print_vector(v);
    println!("{}", vv[0]);
    //println!("{}", v[0]);// after called v is moved
}

trait Printable {
    fn format(&self) -> String;
}

impl Printable for i32 {
    fn format(&self) -> String {
        format!("i32:{}", self)
    }
}

impl Printable for f32 {
    fn format(&self) -> String {
        format!("f32:{}", self)
    }
}

fn print_it<T: Printable>(z: T) {
    println!("{}", z.format());
}

fn print_it_dynamic(z: &dyn Printable) {
    println!("{}", z.format())
}

fn static_dispatch() {
    let a = 10;
    //print_it(a);
    print_it_dynamic(&a);
    let f: f32 = 12.3;
    //print_it(f);
    print_it_dynamic(&f);
}

#[derive(Debug)]
struct Complex<T>
{
    re: T,
    im: T,
}

impl<T> Complex<T> {
    fn new(re: T, im: T) -> Complex<T> {
        Complex::<T> { re, im }
    }
}

impl<T> Add for Complex<T>
    where T: Add<Output=T> {
    type Output = Complex<T>;

    fn add(self, rhs: Self) -> Self::Output {
        Complex::<T> {
            re: self.re + rhs.re,
            im: self.im + rhs.im,
        }
    }
}

impl<T> AddAssign for Complex<T>
    where T: AddAssign<T> {
    fn add_assign(&mut self, rhs: Self) {
        self.re += rhs.re;
        self.im += rhs.im;
    }
}

impl<T> PartialEq for Complex<T>
    where T: PartialEq {
    fn eq(&self, rhs: &Self) -> bool {
        self.re == rhs.re && self.im == rhs.im
    }
}

fn operator_overloading() {
    let mut a = Complex::new(1, 2);
    let mut b = Complex::new(3, 4);
    let mut c = Complex::new(1, 2);
    // let c = a + b;
    // println!("{:?}", c);

    // a += Complex::new(4, 5);
    // println!("{:?}", a);

    println!("{:?}", a == c);
}


struct Creature {
    name: String,
}

impl Creature {
    fn new(name: &str) -> Creature {
        println!("{} enters the game", name);
        Creature { name: name.into() }
    }
}

impl Drop for Creature {
    fn drop(&mut self) {
        println!("{} is DEAD", self.name);
    }
}

fn trait_drop() {
    let goblin = Creature::new("Jeff");
    println!("game proceeds");
}

struct Person {
    name: String,
}

impl Person {
    // fn new(name: &str) -> Person {
    //     Person { name: name.to_string() }
    // }
    fn new<S>(name: S) -> Person where S: Into<String> {
        Person { name: name.into() }
    }
}

fn trait_intro() {
    let john = Person::new("John");
    let name = "Jane".to_string();
    let jane = Person::new(name);
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
