#![allow(unused)]
#![allow(unused_variables)]

use std::fmt::Debug;
use std::ops::Add;
type File = String;

fn report<T>(item: T)
where
    T: Debug,
{
    println!("{:?}", item);
}

fn open(f: &mut File) -> bool {
    true
}
fn close(f: &mut File) -> bool {
    true
}

#[allow(dead_code)]
fn read(f: &mut File, save_to: &mut Vec<u8>) -> bool {
    unimplemented!()
}

fn main() {
    main_3_1();
    //main_2_3();
    //main_2_4();
    //main_2_5_7();
    //main_2_22();
}
fn main_3_1() {
    let mut f1 = File::from("f1.txt");
    open(&mut f1);

    close(&mut f1);
}
fn main_2_27() {
    let one = [1, 2, 3];
    let two: [u8; 3] = [1, 2, 3];
    let blank1 = [0; 3];
    let blank2: [u8; 3] = [0; 3];
    let arrays = [one, two, blank1, blank2];
    for a in &arrays {
        print!("{:?}: ", a);
        for n in a.iter() {
            print!("\t{} + 10 = {}", n, n + 10);
        }
        let mut sum = 0;
        for i in 0..a.len() {
            sum += a[i];
        }
        println!("\t(Σ{:?} = {})", a, sum);
    }
}

fn main_2_22() {
    let res = add_with_generic(1, 3);
    println!("{}", res);
    let res = add_with_generic(1.0, 2.1);
    println!("{}", res);
}

fn add_with_generic<T: Add<Output = T>>(i: T, j: T) -> T {
    i + j
}

fn main_2_5_7() {
    let haystack = [1, 1, 2, 5, 14, 42, 132, 429, 1430, 4862];
    for item in &haystack {
        let res = match *item {
            42 | 132 => "hit",
            _ => "miss",
        };
        if res == "hit" {
            println!("{}: {}", item, res);
        }
    }
}

fn main_2_4() {
    let needle = 0o52;
    let haystack = [1, 1, 2, 5, 14, 42, 132, 429, 1430, 4862];
    let haystack2 = vec![1, 3, 4, 42, 4, 5, 43, 554];
    for item in &haystack {
        if *item == needle {
            println!("{}", item);
        }
    }

    for item in haystack2.into_iter() {
        if item == needle {
            println!("{}", item);
        }
    }

    loop {
        println!("are we ok here?");
    }
}

fn main_2_3() {
    let a: i32 = 101;
    let b: u16 = 100;
    if a < b.try_into().unwrap() {
        println!("true");
    } else {
        println!("false");
    }
    let absolute_difference: f64 = (0.3f64 - (0.1 + 0.2)).abs();
    assert!(absolute_difference <= f64::EPSILON);
    assert_eq!(f32::NAN == f32::NAN, false); //NAN != NAN
}

fn is_even_2(a: i32) -> bool {
    if a % 2 == 0 {
        true
    } else {
        false
    }
}
