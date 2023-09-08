use std::{
    collections::{BTreeSet, HashMap, HashSet},
    fmt::{Display, Debug},
};

fn collection_map() {
    let book_collection = vec!["L'Allemagne Moderne", "Lep", "Lep", "Lep", "Lep"];
    let mut book_hashmap = HashMap::new();
    for book in book_collection {
        let return_val = book_hashmap.entry(book).or_insert(0);
        *return_val += 1;
    }
    for (book, number) in book_hashmap {
        println!("{book}, {number}");
    }
}

fn compare_and_display<T: Display, U: Display + PartialOrd>(statement: T, x: U, y: U) {
    println!("{statement}! Is {x} greater than {y}? {}", x > y);
}

fn take_fifth(value: Vec<i32>) -> Option<i32> {
    if value.len() < 5 {
        None
    } else {
        Some(value[4])
    }
}

#[allow(unused)]
fn main() {
    collection_map();
    let my_mood = Mood::Sleepy;
    my_mood.check();
    let my_mood = Mood::Bad;
    my_mood.check();
    let my_mood = Mood::Good;
    my_mood.check();
    compare_and_display("Listen UP", 9, 8);

    let new_vec = vec![1, 3];
    let bigger_vec = vec![1, 2, 3, 4, 6];
    println!(
        "{:?}, {:?}",
        take_fifth(new_vec).unwrap_or_default(),
        take_fifth(bigger_vec).unwrap_or_default()
    );
}

enum Mood {
    Good,
    Bad,
    Sleepy,
}

impl Mood {
    fn check(&self) {
        match self {
            Mood::Good => println!("good"),
            Mood::Bad => println!("bad"),
            Mood::Sleepy => println!("sleepy"),
        }
    }
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
fn test_collection_2() {
    let data = vec![("male", 9), ("female", 5), ("male", 0), ("female", 20)];
    let mut survey_hash = HashMap::new();
    for item in data {
        survey_hash.entry(item.0).or_insert(Vec::new()).push(item.1);
    }
    for (male_or_female, numbers) in survey_hash {
        println!("{male_or_female}: {numbers:?}");
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

#[test]
#[allow(unused)]
fn hash_set_and_b_tree_set() {
    let many_numbers = vec![
        94, 42, 59, 64, 32, 22, 38, 5, 59, 49, 15, 89, 74, 29, 14, 68, 82, 80, 56, 41, 36, 81, 66,
        51, 58, 34, 59, 44, 19, 93, 28, 33, 18, 46, 61, 76, 14, 87, 84, 73, 71, 29, 94, 10, 35, 20,
        35, 80, 8, 43, 79, 25, 60, 26, 11, 37, 94, 32, 90, 51, 11, 28, 76, 16, 63, 95, 13, 60, 59,
        96, 95, 55, 92, 28, 3, 17, 91, 36, 20, 24, 0, 86, 82, 58, 93, 68, 54, 80, 56, 22, 67, 82,
        58, 64, 80, 16, 61, 57, 14, 11,
    ];
    let mut unique_numbers = BTreeSet::new();
    let length = many_numbers.len();
    for num in many_numbers {
        unique_numbers.insert(num);
    }
    let hashset_length = unique_numbers.len(); // The length tells us how many numbers are in it
    println!(
        "There are {} unique numbers, so we are missing {}.",
        hashset_length,
        length - hashset_length
    );
    // Let's see what numbers we are missing
    let mut missing_vec = vec![];
    for number in 0..100 {
        if unique_numbers.get(&number).is_none() {
            // If .get() returns None,
            missing_vec.push(number);
        }
    }
    print!("It does not contain: ");
    for number in missing_vec {
        print!("{number} ");
    }
}

//traits
#[derive(Debug)]
struct Animal {
    name: String,
}

trait Dog {
    fn bark(&self);

    fn run(&self);
}

impl Dog for Animal {
    fn bark(&self) {
        println!("{} is barking", self.name);
    }

    fn run(&self) {
        println!("{} is running", self.name)
    }
}

#[test]
fn trait_basics() {
    let rover = Animal {
        name: "Rover".to_string(),
    };
    rover.bark();
    rover.run();
}

struct Cat {
    name: String,
    age: u8,
}

impl Debug for Cat {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        f.debug_struct("Cat").field("name", &self.name).field("age", &self.age).finish()
    }
}

#[test]
fn trait_basics_2() {
    let mr_mantle = Cat {
        name: "Reggie Mantle".to_string(),
        age: 4,
    };
    println!("Mr . Mantle is  a {mr_mantle:?}");
}
