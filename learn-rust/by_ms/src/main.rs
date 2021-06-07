struct Person {
    name: String,
    age: u8,
    likes_oranges: bool,
}

// A tuple struct
struct Point2D(u32, u32);

// A unit struct
struct Unit;

fn main() {
    let tuple = ("hello", 5, 'c');
    assert_eq!(tuple.0, "hello");
    assert_eq!(tuple.1, 5);
    assert_eq!(tuple.2, 'c');
    println!("{}", tuple.0);
}
