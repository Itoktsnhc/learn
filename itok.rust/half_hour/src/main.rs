use std::{
    collections::{BTreeSet, HashMap, HashSet},
    fmt::{Debug, Display},
};
use std::borrow::Cow;
use std::cell::{Cell, RefCell};
use std::fmt::{format, Formatter};
use std::ptr::null;
use std::rc::Rc;
use std::sync::Mutex;

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
        f.debug_struct("Cat")
            .field("name", &self.name)
            .field("age", &self.age)
            .finish()
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

#[derive(Debug)]
struct Wizard {
    health: i32,
}

#[derive(Debug)]
struct Ranger {
    health: i32,
}

trait FightClose: Debug {
    fn attack_with_sword(&self, opponent: &mut Monster) {
        opponent.take_damage(2);
        println!(
            "You attack with your sword. Your opponent now has {} health left. You are now at: {:?}", // We can now print self with {:?} because we have Debug
            opponent.health, &self
        );
    }
    fn attack_with_hand(&self, opponent: &mut Monster) {
        opponent.take_damage(2);
        println!(
            "You attack with your hand. Your opponent now has {} health left. You are now at: {:?}", // We can now print self with {:?} because we have Debug
            opponent.health, &self
        );
    }
}

impl FightClose for Wizard {}

impl FightClose for Ranger {}

trait FightFromDistance: Debug {
    fn attack_with_bow<T: MonsterBehavior>(&self, opponent: &mut T, distance: u32) {
        println!("You attack with a rock!");
        if distance < 10 {
            opponent.take_damage(10);
        } else {
            println!("Too far away!");
        }
        opponent.display_self();
    }
    fn attack_with_rock<T: MonsterBehavior>(&self, opponent: &mut T, distance: u32) {
        println!("You attack with a rock!");
        if distance < 3 {
            opponent.take_damage(4);
        } else {
            println!("Too far away!");
        }
        opponent.display_self();
    }
}

impl FightFromDistance for Ranger {}

trait MonsterBehavior: Debug {
    fn take_damage(&mut self, damage: i32);
    fn display_self(&self) {
        println!("The monster is now: {self:?}");
    }
}

#[derive(Debug)]
struct Monster {
    health: i32,
}

impl MonsterBehavior for Monster {
    fn take_damage(&mut self, damage: i32) {
        self.health -= damage;
    }
}

#[test]
fn test_complex_trait() {
    let radagast = Wizard { health: 60 };
    let aragorn = Ranger { health: 80 };
    let mut uruk_hai = Monster { health: 90 };
    radagast.attack_with_hand(&mut uruk_hai);
    aragorn.attack_with_bow(&mut uruk_hai, 8);

    let new_vec = (1..).take(10).collect::<Vec<i32>>();
    // Or you can write it like this:
    // let new_vec: Vec<i32> = (1..).take(10).collect();
    println!("{new_vec:?}");
}

#[derive(Debug, Clone)]
struct BookCollection(Vec<String>);

#[derive(Debug)]
struct Library {
    name: String,
    books: BookCollection,
}

impl Library {
    fn add_book(&mut self, book: &str) {
        self.books.0.push(book.to_string());
    }

    fn new(name: &str) -> Self {
        Self {
            name: name.to_string(),
            books: BookCollection(Vec::new()),
        }
    }
    fn get_books(&self) -> BookCollection {
        self.books.clone()
    }
}

impl Iterator for BookCollection {
    type Item = String;

    fn next(&mut self) -> Option<Self::Item> {
        match self.0.pop() {
            Some(book) => {
                println!("Accessing book: {book}");
                Some(book)
            }
            None => None,
        }
    }
}

#[test]
fn test_iterator_func() {
    let mut my_library = Library::new("Calgary");
    my_library.add_book("The Doom of the Darksword");
    my_library.add_book("Demian - die Geschichte einer Jugend");
    my_library.add_book("구운몽");
    my_library.add_book("吾輩は猫である");
    for item in my_library.get_books() {
        println!("{}", item);
    }
}

//--- Closures
#[test]
fn closures_main() {
    let num_vec = vec![2, 4, 6];
    let double_vec = num_vec
        .iter()
        .map(|num| num * 2)
        .collect::<Vec<i32>>();
    println!("{:?}", double_vec);
    let some_keys = vec![0, 1, 2, 3, 4, 5]; // a Vec<i32>
    let some_values = vec!["zero", "one", "two", "three", "four", "five"]; // a Vec<&str>
    let number_word_hashmap = some_keys
        .into_iter() // now it is an iter
        .zip(some_values.into_iter())
        .collect::<HashMap<_, _>>();
    println!(
        "For key {} we get {}.",
        2,
        number_word_hashmap.get(&2).unwrap()
    );
}

//---LifeTimes and interior mutability
#[test]
fn str_main() {
    let my_str = String::from("I am a string");
    prints_str(&my_str);
}

fn prints_str(mystr: &str) {
    println!("{mystr}")
}


//10.2 Lifetime annotations
#[test]
fn life_time_main() {
    let my_city = CityLifeTime {
        name: "Ichinomiya",
        date_founded: 2024,
    };
}


#[allow(unused)]
#[derive(Debug)]
struct CityLifeTime<'a> {
    name: &'a str,
    date_founded: u32,
}

struct Adventurer<'a> {
    name: &'a str,
    hit_points: u32,
}

impl Adventurer<'_> {
    fn take_damage(&mut self) {
        self.hit_points -= 20;
        println!("{} has {} hit points left!", self.name, self.hit_points);
    }
}

impl std::fmt::Display for Adventurer<'_> {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        write!(f, "{} has {} hit points.", self.name, self.hit_points)
    }
}

struct PhoneModel {
    company_name: String,
    model_name: String,
    screen_size: f32,
    memory: usize,
    date_issued: u32,
    on_sale: Cell<bool>,
}

#[derive(Debug)]
struct User {
    id: u32,
    year_registered: u32,
    username: String,
    active: RefCell<bool>,
}


//10.3 interior mutations
#[test]
fn interior_mutations_main() {
    let super_phone_3000 = PhoneModel {
        company_name: "YY Electronics".to_string(),
        model_name: "Super Phone 3000".to_string(),
        screen_size: 7.5,
        memory: 4000000,
        date_issued: 2023,
        on_sale: Cell::new(true),
    };
    super_phone_3000.on_sale.set(false);
    println!("{:?}", super_phone_3000.on_sale);

    let u1 = User {
        id: 1,
        year_registered: 2020,
        username: "User 1".to_string(),
        active: RefCell::new(true),
    };
    println!("{:?}", u1.active);
    let borrow_one = u1.active.borrow_mut();
    //let borrow_two = u1.active.borrow_mut();
}

#[test]
fn mutex_main() {
    let my_mutex = Mutex::new(0);
    for _ in 0..100 {
        *my_mutex.lock().unwrap() += 1; // locks and unlocks 100 times
    }
    println!("{:?}", my_mutex);
}


// 11

/// 11.4 Cow
#[derive(Debug)]
enum LocalError {
    TooBig,
    ToSmall,
}

#[derive(Debug)]
struct ErrorInfo {
    error: LocalError,
    message: String,
}

fn generate_message(message: &'static str, error_info: Option<ErrorInfo>) -> Cow<'static, str> {
    match error_info {
        None => message.into(),
        Some(info) => format!("{message}: {info:?}").into(),
    }
}

#[test]
fn test_cow() {
    let msg1 = generate_message("Every Thing is Fine", None);
    let msg2 = generate_message("Got an Error", Some(ErrorInfo {
        error: LocalError::TooBig,
        message: "It was too big".to_string(),
    }));

    for msg in [msg1, msg2] {
        match msg {
            Cow::Borrowed(msg) => {
                println!("Borrowed message, did not need an allocation:\n {msg}");
            }
            Cow::Owned(msg) => {
                println!("Owned message because we needed an allocation:\n {msg}")
            }
        }
    }
}

/// 11.5 Rc
#[derive(Debug)]
struct City {
    name: Rc<String>,
    population: u32,
    city_history: Rc<String>,
}

#[derive(Debug)]
struct CityData {
    names: Vec<Rc<String>>,
    histories: Vec<Rc<String>>,
}

#[test]
fn test_rc_in_pic() {
    let calgary_name = Rc::new("Calgary".to_string());
    let calgary_history = Rc::new("Calgary began as a fort called Fort Calgary that...".to_string());
    let calgary = City {
        name: Rc::clone(&calgary_name),
        population: 1_200_200,
        city_history: Rc::clone(&calgary_history),
    };
    let canada_cities = CityData {
        names: vec![Rc::clone(&calgary.name)],
        histories: vec![Rc::clone(&calgary.city_history)],
    };
    println!("Calgary's history is: {}", calgary.city_history);
    println!("{}", Rc::strong_count(&calgary.city_history));
}







