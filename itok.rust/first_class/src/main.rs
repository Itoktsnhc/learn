use core::fmt;
use regex::Regex;
use std::{collections::BTreeMap, io::Write, net::SocketAddr, ops::Add, str::FromStr};

fn main() {
    let cat = Cat;
    print!("name of obj : {}", name(cat))
}

struct Cat;
struct Dog;

trait Animal {
    fn name(&self) -> &'static str;
}

impl Animal for Dog {
    fn name(&self) -> &'static str {
        "Dog"
    }
}

impl Animal for Cat {
    fn name(&self) -> &'static str {
        "Cat"
    }
}

fn name<T: Animal>(animal: T) -> &'static str {
    animal.name()
}

#[derive(Debug)]
struct Complex {
    real: f64,
    imagine: f64,
}

impl Complex {
    pub fn new(real: f64, imagine: f64) -> Self {
        Self {
            real: real,
            imagine: imagine,
        }
    }
}
impl Add for Complex {
    type Output = Self;
    fn add(self, rhs: Self) -> Self::Output {
        let real = self.real + rhs.real;
        let imagine = self.imagine + rhs.imagine;
        Self::new(real, imagine)
    }
}

impl Add for &Complex {
    type Output = Complex;

    fn add(self, rhs: Self) -> Self::Output {
        let real = self.real + rhs.real;
        let imagine = self.imagine + rhs.imagine;
        Complex::new(real, imagine)
    }
}

impl Add<f64> for &Complex {
    type Output = Complex;

    fn add(self, rhs: f64) -> Self::Output {
        let real = self.real + rhs;
        Complex::new(real, self.imagine)
    }
}

pub trait Parse {
    type Error;
    fn parse(s: &str) -> Result<Self, Self::Error>
    where
        Self: Sized;
}

impl<T> Parse for T
where
    T: FromStr + Default,
{
    type Error = String;
    fn parse(s: &str) -> Result<Self, Self::Error> {
        let re = Regex::new(r"^[0-9]+(\.[0-9]+)?").unwrap();
        if let Some(captures) = re.captures(s) {
            captures
                .get(0)
                .map_or(Err("Failed to capture".to_string()), |s| {
                    s.as_str()
                        .parse()
                        .map_err(|_err| "failed to parse captured string".to_string())
                })
        } else {
            Err("failed to parse string".to_string())
        }
    }
}

struct BufBuilder {
    buf: Vec<u8>,
}

impl BufBuilder {
    pub fn new() -> Self {
        Self {
            buf: Vec::with_capacity(1024),
        }
    }
}

impl fmt::Debug for BufBuilder {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        write!(f, "{}", String::from_utf8_lossy(&self.buf))
    }
}

impl Write for BufBuilder {
    fn write(&mut self, buf: &[u8]) -> std::io::Result<usize> {
        self.buf.extend_from_slice(buf);
        Ok(buf.len())
    }

    fn flush(&mut self) -> std::io::Result<()> {
        Ok(())
    }
}

#[allow(dead_code)]
fn type_infer_3() {
    let addr = "127.0.0.1:8080".parse::<SocketAddr>().unwrap();
    println!("addr: {:?}, port: {:?}", addr.ip(), addr.port());
}

#[allow(dead_code)]
fn type_infer_2() {
    let numbers = vec![1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    // let even_numbers: Vec<_> = numbers.into_iter().filter(|n| n % 2 == 0).collect();
    let even_numbers = numbers
        .into_iter()
        .filter(|n| n % 2 == 0)
        .collect::<Vec<_>>();
    println!("{:?}", even_numbers);
}

#[allow(dead_code)]
fn type_infer() {
    let mut map = BTreeMap::new();
    map.insert("hello", "world");
    map.insert("goodbye", "world");
    println!("map:{:?}", map);
}
