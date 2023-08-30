#[allow(unused)]
fn main() {
    let my_vec: Vec<_> = [9, 0, 10].into();
}

enum Number {
    U32(u32),
    I32(i32),
}

fn get_number(input: i32) -> Number{
    let number = match input.is_positive() {
        true => Number::U32(input as u32),
        false => todo!(),
    };
    number
}

trait Signed {
    fn is_strictly_negative(self) -> bool;
}

struct Number {
    odd: bool,
    value: i32,
}

impl Signed for Number {
    fn is_strictly_negative(self) -> bool {
        self.value < 0
    }
}

impl Signed for i32 {
    fn is_strictly_negative(self) -> bool {
        self < 0
    }
}

impl std::ops::Neg for Number {
    type Output = Number;

    fn neg(self) -> Self::Output {
        Number {
            value: -self.value,
            odd: self.odd,
        }
    }
}

#[test]
#[allow(unused)]
fn basic_tests() {
    let (first, second) = ('a', 42);
    assert_eq!(first, 'a');
    assert_eq!(second, 42);

    let x = vec![1, 2, 3, 4, 5, 6, 7, 8]
        .iter()
        .map(|x| x + 3)
        .fold(0, |x, y| x + y);
    assert_eq!(x, 60);
    let v1 = Vec2 { x: 1.0, y: 3.0 };
    let v2 = Vec2 { y: 2.0, x: 4.0 };
    let v3 = Vec2 { ..v2 };
}

#[allow(dead_code)]
struct Vec2 {
    x: f64,
    y: f64,
}
