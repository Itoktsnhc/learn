use regex::Regex;
use std::env;
use std::fs;

struct ConfigParsed<'a> {
    query: &'a str,
    filename: &'a str,
}
fn main() {
    let args: Vec<String> = env::args().collect();
    let config_o = parse_config(&args);
    if config_o.is_none() {
        panic!("need two args for query and filename");
    }
    let config = config_o.unwrap();
    let contexts = fs::read_to_string(config.filename).expect("cannot read the file");
    let re = Regex::new(config.query).unwrap();
    let matchRes = re.is_match(&contexts);
    if matchRes {
        println!("FOUND!")
    } else {
        println!("NOT FOUND")
    }
}

fn parse_config(args: &[String]) -> Option<ConfigParsed> {
    if args.len() < 3 {
        None
    } else {
        Some(ConfigParsed {
            query: &args[1],
            filename: &args[2],
        })
    }
}


