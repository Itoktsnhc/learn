use std::mem;
use std::f64::consts::PI;

fn main() {
    scope_and_shadowing();
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

#[allow(unused)]
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

#[allow(unused)]
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
